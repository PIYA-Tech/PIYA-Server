namespace PIYA_API.Model;

/// <summary>
/// Individual medication item in a prescription
/// </summary>
public class PrescriptionItem
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Parent prescription
    /// </summary>
    public Guid PrescriptionId { get; set; }
    public Prescription Prescription { get; set; } = null!;
    
    /// <summary>
    /// Medication prescribed
    /// </summary>
    public Guid MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;
    
    /// <summary>
    /// Dosage (e.g., "500mg", "10ml")
    /// </summary>
    public required string Dosage { get; set; }
    
    /// <summary>
    /// Frequency (e.g., "Twice daily", "Every 8 hours")
    /// </summary>
    public required string Frequency { get; set; }
    
    /// <summary>
    /// Duration (e.g., "7 days", "2 weeks")
    /// </summary>
    public required string Duration { get; set; }
    
    /// <summary>
    /// Quantity to dispense
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Special instructions for this medication
    /// </summary>
    public string? Instructions { get; set; }
    
    /// <summary>
    /// Whether this item has been fulfilled
    /// </summary>
    public bool IsFulfilled { get; set; } = false;
    
    /// <summary>
    /// When this item was fulfilled
    /// </summary>
    public DateTime? FulfilledAt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
