using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

public interface ICoordinatesService
{
    public Task<Coordinates> GetById(Guid id);
    public Task<int> GetCountry(Coordinates coordinates);

    public Task<int> GetCity(Coordinates coordinates);
    public Task<int> CalculateDistance(Coordinates coordinates1, Coordinates coordinates2);
    public Task<Coordinates> Create(Coordinates coordinates);
    public Task Update(Coordinates coordinates);
    public Task Delete(int id);
}