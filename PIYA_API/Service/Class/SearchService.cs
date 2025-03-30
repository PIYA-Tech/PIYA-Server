using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class SearchService(IPharmacyService pharmacyService, ICoordinatesService coordinatesService, PharmacyApiDbContext dbContext) : ISearchService
{
    private readonly IPharmacyService _pharmacyService = pharmacyService;
    private readonly ICoordinatesService _coordinatesService = coordinatesService;
    private readonly PharmacyApiDbContext _dbContext = dbContext;

    public Task<List<Pharmacy>> SearchByCity(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }

    public Task<List<Pharmacy>> SearchByCountry(Coordinates coordinates)
    {
        throw new NotImplementedException();
    }

    public Task<List<Pharmacy>> SearchByRadius(Coordinates coordinates, int radius)
    {
        throw new NotImplementedException();
    }
}
