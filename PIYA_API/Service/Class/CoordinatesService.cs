using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class CoordinatesService() : ICoordinatesService
{
    public Task<Coordinates> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCountry(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCity(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }

    public Task<int> CalculateDistance(Coordinates coordinates1, Coordinates coordinates2)
    {
        throw new NotImplementedException();
    }

    public Task<Coordinates> Create(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }
}
