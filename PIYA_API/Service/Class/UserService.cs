using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class UserService : IUserService
{
    public Task<User> Authenticate(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<User> Create(User user, string password)
    {
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
