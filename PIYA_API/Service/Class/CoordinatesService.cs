using Microsoft.EntityFrameworkCore;
using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class CoordinatesService(PharmacyApiDbContext dbContext) : ICoordinatesService
{
    private readonly PharmacyApiDbContext _dbContext = dbContext;

    public async Task<Coordinates> GetById(Guid id)
    {
        // Since Coordinates is not a separate table, we search for it in Pharmacy entities
        var pharmacy = await _dbContext.Pharmacies
            .Include(p => p.Coordinates)
            .FirstOrDefaultAsync(p => p.Coordinates.Id == id);

        if (pharmacy?.Coordinates == null)
        {
            throw new KeyNotFoundException($"Coordinates with ID {id} not found");
        }

        return pharmacy.Coordinates;
    }

    public Task<int> GetCountry(Coordinates coordinates)
    {
        // This would typically call a reverse geocoding API (Google Maps, OpenStreetMap, etc.)
        // For now, returning a placeholder
        // In production, implement actual geocoding service
        throw new NotImplementedException("Geocoding service integration required - use Google Maps Geocoding API");
    }

    public Task<int> GetCity(Coordinates coordinates)
    {
        // This would typically call a reverse geocoding API
        // For now, returning a placeholder
        throw new NotImplementedException("Geocoding service integration required - use Google Maps Geocoding API");
    }

    public Task<int> CalculateDistance(Coordinates coordinates1, Coordinates coordinates2)
    {
        // Haversine formula to calculate distance between two points on Earth
        const double earthRadiusKm = 6371;

        var lat1Rad = DegreesToRadians(coordinates1.Latitude);
        var lat2Rad = DegreesToRadians(coordinates2.Latitude);
        var deltaLat = DegreesToRadians(coordinates2.Latitude - coordinates1.Latitude);
        var deltaLon = DegreesToRadians(coordinates2.Longitude - coordinates1.Longitude);

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distanceKm = earthRadiusKm * c;

        return Task.FromResult((int)Math.Round(distanceKm * 1000)); // Return distance in meters
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    public async Task<Coordinates> Create(Coordinates coordinates)
    {
        // Coordinates are created as part of Pharmacy entity
        // This method is here for interface completeness
        coordinates.Id = Guid.NewGuid();
        return await Task.FromResult(coordinates);
    }

    public async Task Delete(int id)
    {
        // Coordinates are owned by Pharmacy, cannot be deleted independently
        await Task.CompletedTask;
        throw new InvalidOperationException("Coordinates cannot be deleted independently - delete the associated Pharmacy instead");
    }

    public async Task Update(Coordinates coordinates)
    {
        // Find the pharmacy that owns these coordinates
        var pharmacy = await _dbContext.Pharmacies
            .Include(p => p.Coordinates)
            .FirstOrDefaultAsync(p => p.Coordinates.Id == coordinates.Id);

        if (pharmacy?.Coordinates == null)
        {
            throw new KeyNotFoundException($"Coordinates with ID {coordinates.Id} not found");
        }

        pharmacy.Coordinates.Latitude = coordinates.Latitude;
        pharmacy.Coordinates.Longitude = coordinates.Longitude;

        _dbContext.Pharmacies.Update(pharmacy);
        await _dbContext.SaveChangesAsync();
    }
}
