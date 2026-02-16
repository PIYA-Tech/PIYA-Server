using Microsoft.EntityFrameworkCore;
using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class SearchService(IPharmacyService pharmacyService, ICoordinatesService coordinatesService, PharmacyApiDbContext dbContext) : ISearchService
{
    private readonly IPharmacyService _pharmacyService = pharmacyService;
    private readonly ICoordinatesService _coordinatesService = coordinatesService;
    private readonly PharmacyApiDbContext _dbContext = dbContext;

    public async Task<List<Pharmacy>> SearchByCity(Coordinates coordinates)
    {
        // This would typically use reverse geocoding to get city name
        // Then search pharmacies in that city
        // For now, we'll search by proximity (10km radius)
        var allPharmacies = await _dbContext.Pharmacies
            .Include(p => p.Coordinates)
            .Include(p => p.Company)
            .ToListAsync();

        var pharmaciesInCity = new List<Pharmacy>();

        foreach (var pharmacy in allPharmacies)
        {
            var distance = await _coordinatesService.CalculateDistance(coordinates, pharmacy.Coordinates);
            // Consider pharmacies within 10km as same city
            if (distance <= 10000) // 10km in meters
            {
                pharmaciesInCity.Add(pharmacy);
            }
        }

        return pharmaciesInCity;
    }

    public async Task<List<Pharmacy>> SearchByCountry(Coordinates coordinates)
    {
        // This would typically use reverse geocoding to get country name
        // For demonstration, we'll return all pharmacies in the same country string
        // In production, implement proper geocoding
        var allPharmacies = await _dbContext.Pharmacies
            .Include(p => p.Coordinates)
            .Include(p => p.Company)
            .ToListAsync();

        // Simple proximity-based country search (within 1000km)
        var pharmaciesInCountry = new List<Pharmacy>();

        foreach (var pharmacy in allPharmacies)
        {
            var distance = await _coordinatesService.CalculateDistance(coordinates, pharmacy.Coordinates);
            // Consider pharmacies within 1000km as same country (rough approximation)
            if (distance <= 1000000) // 1000km in meters
            {
                pharmaciesInCountry.Add(pharmacy);
            }
        }

        return pharmaciesInCountry;
    }

    public async Task<List<Pharmacy>> SearchByRadius(Coordinates coordinates, int radius)
    {
        var allPharmacies = await _dbContext.Pharmacies
            .Include(p => p.Coordinates)
            .Include(p => p.Company)
            .ToListAsync();

        var pharmaciesInRadius = new List<Pharmacy>();

        foreach (var pharmacy in allPharmacies)
        {
            var distance = await _coordinatesService.CalculateDistance(coordinates, pharmacy.Coordinates);
            if (distance <= radius)
            {
                pharmaciesInRadius.Add(pharmacy);
            }
        }

        // Sort by distance (closest first)
        return pharmaciesInRadius
            .OrderBy(p => _coordinatesService.CalculateDistance(coordinates, p.Coordinates).Result)
            .ToList();
    }
}
