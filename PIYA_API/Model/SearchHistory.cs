namespace PIYA_API.Model;

/// <summary>
/// Tracks user search history for pharmacies and medications
/// </summary>
public class SearchHistory
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// User who performed the search
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Type of search performed
    /// </summary>
    public SearchType SearchType { get; set; }
    
    /// <summary>
    /// Search query text
    /// </summary>
    public string? SearchQuery { get; set; }
    
    /// <summary>
    /// Filters applied (JSON format)
    /// Example: {"city": "Baku", "radius": 5, "24hours": true}
    /// </summary>
    public string? Filters { get; set; }
    
    /// <summary>
    /// Number of results returned
    /// </summary>
    public int ResultCount { get; set; }
    
    /// <summary>
    /// User's location at time of search (optional)
    /// </summary>
    public Guid? CoordinatesId { get; set; }
    public Coordinates? Coordinates { get; set; }
    
    /// <summary>
    /// Which result the user selected (if any)
    /// </summary>
    public Guid? SelectedResultId { get; set; }
    
    /// <summary>
    /// Type of selected result (Pharmacy, Medication, Doctor)
    /// </summary>
    public string? SelectedResultType { get; set; }
    
    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Types of searches tracked
/// </summary>
public enum SearchType
{
    Pharmacy,
    Medication,
    Doctor,
    Hospital,
    PharmacyByMedication,
    PharmacyByLocation
}
