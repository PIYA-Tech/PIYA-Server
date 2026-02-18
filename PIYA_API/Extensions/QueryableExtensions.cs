using Microsoft.EntityFrameworkCore;
using PIYA_API.DTOs;
using System.Linq.Expressions;

namespace PIYA_API.Extensions;

/// <summary>
/// Extension methods for IQueryable to support pagination, filtering, and sorting
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Apply pagination to a queryable
    /// </summary>
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<T>(items, pageNumber, pageSize, totalCount);
    }

    /// <summary>
    /// Apply pagination parameters to a queryable
    /// </summary>
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams)
    {
        return query
            .Skip(paginationParams.Skip)
            .Take(paginationParams.Take);
    }

    /// <summary>
    /// Apply sorting to a queryable
    /// </summary>
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortBy,
        bool descending = false)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortBy);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), property.Type },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(resultExpression);
    }

    /// <summary>
    /// Apply search filter to a queryable
    /// </summary>
    public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> query,
        string? searchTerm,
        params Expression<Func<T, string>>[] searchProperties)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchProperties.Length == 0)
            return query;

        var parameter = searchProperties[0].Parameters[0];
        Expression? combinedExpression = null;

        foreach (var propertyExpression in searchProperties)
        {
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            var searchValue = Expression.Constant(searchTerm.ToLower());
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;

            var propertyAccess = Expression.Invoke(propertyExpression, parameter);
            var toLowerCall = Expression.Call(propertyAccess, toLowerMethod);
            var containsCall = Expression.Call(toLowerCall, containsMethod, searchValue);

            combinedExpression = combinedExpression == null
                ? containsCall
                : Expression.OrElse(combinedExpression, containsCall);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression!, parameter);
        return query.Where(lambda);
    }

    /// <summary>
    /// Apply all query parameters (pagination, sorting, search)
    /// </summary>
    public static IQueryable<T> ApplyQueryParams<T>(
        this IQueryable<T> query,
        QueryParams queryParams,
        params Expression<Func<T, string>>[] searchProperties)
    {
        // Apply search
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm) && searchProperties.Length > 0)
        {
            query = query.ApplySearch(queryParams.SearchTerm, searchProperties);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
        {
            query = query.ApplySorting(queryParams.SortBy, queryParams.SortDescending);
        }

        return query;
    }
}
