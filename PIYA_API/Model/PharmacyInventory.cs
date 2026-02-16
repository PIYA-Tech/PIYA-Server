namespace PIYA_API.Model;

/// <summary>
/// Real-time pharmacy inventory tracking
/// </summary>
public class PharmacyInventory
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Pharmacy that has this medication
    /// </summary>
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;
    
    /// <summary>
    /// Medication in stock
    /// </summary>
    public Guid MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;
    
    /// <summary>
    /// Current quantity in stock
    /// </summary>
    public int QuantityInStock { get; set; }
    
    /// <summary>
    /// Minimum stock level (for low stock alerts)
    /// </summary>
    public int MinimumStockLevel { get; set; } = 10;
    
    /// <summary>
    /// Price per unit
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Currency (AZN, USD, etc.)
    /// </summary>
    public string Currency { get; set; } = "AZN";
    
    /// <summary>
    /// Batch/lot number
    /// </summary>
    public string? BatchNumber { get; set; }
    
    /// <summary>
    /// Expiration date of this batch
    /// </summary>
    public DateTime? ExpirationDate { get; set; }
    
    /// <summary>
    /// Whether this medication is currently available
    /// </summary>
    public bool IsAvailable { get; set; } = true;
    
    /// <summary>
    /// Last time stock was updated
    /// </summary>
    public DateTime LastRestockedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
