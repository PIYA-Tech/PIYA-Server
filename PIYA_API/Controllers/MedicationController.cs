using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicationController(IMedicationService medicationService, ILogger<MedicationController> logger) : ControllerBase
{
    private readonly IMedicationService _medicationService = medicationService;
    private readonly ILogger<MedicationController> _logger = logger;

    /// <summary>
    /// Search medications by name (public access for pharmacy search)
    /// </summary>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Medication>>> Search([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                return BadRequest(new { error = "Search query must be at least 2 characters" });
            }

            var results = await _medicationService.SearchByNameAsync(query);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching medications");
            return StatusCode(500, new { error = "Failed to search medications" });
        }
    }

    /// <summary>
    /// Get all medications (paginated)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<Medication>>> GetAll()
    {
        try
        {
            var medications = await _medicationService.GetAllAsync();
            return Ok(medications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving medications");
            return StatusCode(500, new { error = "Failed to retrieve medications" });
        }
    }

    /// <summary>
    /// Get medication by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Medication>> GetById(Guid id)
    {
        try
        {
            var medication = await _medicationService.GetByIdAsync(id);
            if (medication == null)
            {
                return NotFound(new { error = "Medication not found" });
            }

            return Ok(medication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving medication {MedicationId}", id);
            return StatusCode(500, new { error = "Failed to retrieve medication" });
        }
    }

    /// <summary>
    /// Create new medication (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Medication>> Create([FromBody] Medication medication)
    {
        try
        {
            var created = await _medicationService.CreateAsync(medication);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating medication");
            return StatusCode(500, new { error = "Failed to create medication" });
        }
    }

    /// <summary>
    /// Search by active ingredient
    /// </summary>
    [HttpGet("ingredient/{ingredient}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Medication>>> SearchByIngredient(string ingredient)
    {
        try
        {
            var results = await _medicationService.SearchByIngredientAsync(ingredient);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching by ingredient");
            return StatusCode(500, new { error = "Failed to search by ingredient" });
        }
    }
}
