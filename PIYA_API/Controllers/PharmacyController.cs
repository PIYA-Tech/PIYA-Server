using Microsoft.AspNetCore.Mvc;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PharmacyController(ISearchService searchService, IPharmacyService pharmacyService) : ControllerBase
{
    private readonly ISearchService _searchService = searchService;
    private readonly IPharmacyService _pharmacyService = pharmacyService;

    [HttpGet("getBtId")]
    public async Task<IActionResult> GetPharmacy([FromQuery] Guid id)
    {
        var pharmacy = await _pharmacyService.GetById(id);
        if (pharmacy == null)
        {
            return NotFound();
        }
        return Ok(pharmacy);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePharmacy([FromBody] Pharmacy pharmacy)
    {
        if (pharmacy == null)
        {
            return BadRequest("Pharmacy cannot be null.");
        }
        var createdPharmacy = await _pharmacyService.Create(pharmacy);
        return CreatedAtAction(nameof(GetPharmacy), new { id = createdPharmacy.Id }, createdPharmacy);
    }

    [HttpGet("searchByCountry")]
    public async Task<IActionResult> SearchByCountry([FromQuery] Coordinates coordinates)
    {
        var pharmacies = await _searchService.SearchByCountry(coordinates);
        if (pharmacies == null || pharmacies.Count == 0)
        {
            return NotFound("No pharmacies found in this country.");
        }
        return Ok(pharmacies);
    }
    [HttpGet("searchByCity")]
    public async Task<IActionResult> SearchByCity([FromQuery] Coordinates coordinates)
    {
        var pharmacies = await _searchService.SearchByCity(coordinates);
        if (pharmacies == null || pharmacies.Count == 0)
        {
            return NotFound("No pharmacies found in this city.");
        }
        return Ok(pharmacies);
    }
    [HttpGet("searchByRadius")]
    public async Task<IActionResult> SearchByRadius([FromQuery] Coordinates coordinates, [FromQuery] int radius)
    {
        var pharmacies = await _searchService.SearchByRadius(coordinates, radius);
        if (pharmacies == null || pharmacies.Count == 0)
        {
            return NotFound("No pharmacies found within this radius.");
        }
        return Ok(pharmacies);
    }
}
