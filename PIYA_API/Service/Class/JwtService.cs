using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class JwtService : IJwtService
{
    private readonly PharmacyApiDbContext _dbContext;
    public JwtService(PharmacyApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Guid GetId(string token)
    {
        var tokenObj = _dbContext.Tokens.FirstOrDefault(t => t.AccessToken == token);
        return tokenObj == null ? new Guid() : tokenObj.Id;
    }

    public string GenerateSecurityToken(string username)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return null;
        }
        var token = new Token();
        token.Id = Guid.NewGuid();

        _dbContext.Tokens.Add(token);
        _dbContext.SaveChanges();
        return token.AccessToken;
    }

    public string ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}
