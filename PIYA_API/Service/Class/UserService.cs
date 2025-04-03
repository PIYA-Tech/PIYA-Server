using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class UserService(PharmacyApiDbContext dbContext) : IUserService
{
    private readonly PharmacyApiDbContext _dbContext = dbContext;
    public Task<User> Authenticate(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<User> Create(User user, string password)
    {
        // validation
        //if (string.IsNullOrWhiteSpace(password))
        //    throw new AppException("Password is required");
        throw new NotImplementedException();

    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(User user, string? password = null)
    {
        throw new NotImplementedException();
    }
}
