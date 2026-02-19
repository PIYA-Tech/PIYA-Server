namespace PIYA_API.Model;

/// <summary>
/// Represents a user's rating and review of a pharmacy
/// </summary>
public class PharmacyRating
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// The pharmacy being rated
    /// </summary>
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;
    
    /// <summary>
    /// The user who left the rating
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Rating value (1-5 stars)
    /// </summary>
    public int Rating { get; set; }
    
    /// <summary>
    /// Optional review text
    /// </summary>
    public string? ReviewText { get; set; }
    
    /// <summary>
    /// Categories rated (optional)
    /// </summary>
    public PharmacyRatingCategories? Categories { get; set; }
    
    /// <summary>
    /// Whether the user would recommend this pharmacy
    /// </summary>
    public bool? WouldRecommend { get; set; }
    
    /// <summary>
    /// Was the rating verified (user made a purchase/prescription)
    /// </summary>
    public bool IsVerified { get; set; } = false;
    
    /// <summary>
    /// Related prescription (if verified)
    /// </summary>
    public Guid? PrescriptionId { get; set; }
    public Prescription? Prescription { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Detailed rating categories
/// </summary>
public class PharmacyRatingCategories
{
    /// <summary>
    /// Service quality (1-5)
    /// </summary>
    public int? ServiceQuality { get; set; }
    
    /// <summary>
    /// Staff friendliness (1-5)
    /// </summary>
    public int? StaffFriendliness { get; set; }
    
    /// <summary>
    /// Wait time rating (1-5)
    /// </summary>
    public int? WaitTime { get; set; }
    
    /// <summary>
    /// Product availability (1-5)
    /// </summary>
    public int? ProductAvailability { get; set; }
    
    /// <summary>
    /// Cleanliness (1-5)
    /// </summary>
    public int? Cleanliness { get; set; }
    
    /// <summary>
    /// Price competitiveness (1-5)
    /// </summary>
    public int? PriceValue { get; set; }
}
