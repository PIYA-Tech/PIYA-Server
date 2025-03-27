using PIYA_API.Model;
namespace PIYA_API.Service.Interface;

public interface IUserService
{
    public Task<User> Authenticate(string username, string password);
    public Task<User> GetById(int id);
    public Task<User> Create(User user, string password);
    public Task Update(User user, string? password = null);
    public Task Delete(int id);
}
