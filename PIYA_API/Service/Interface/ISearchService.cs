using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

public interface ISearchService
{
    public Task<List<Pharmacy>> SearchByCountry(Coordinates coordinates);
    public Task<List<Pharmacy>> SearchByCity(Coordinates coordinates);
    public Task<List<Pharmacy>> SearchByRadius(Coordinates coordinates, int radius);
}
