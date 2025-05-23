﻿namespace PIYA_API.Service.Interface;

public interface IJwtService
{
    public string GenerateSecurityToken(string username);
    public string ValidateToken(string token);
    public Guid GetId(string token);
}