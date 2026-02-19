namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for Google Maps API integration (Geocoding and Distance Matrix)
/// </summary>
public interface IGoogleMapsService
{
    /// <summary>
    /// Convert address to coordinates (geocoding)
    /// </summary>
    Task<GeocodeResult?> GeocodeAddressAsync(string address);
    
    /// <summary>
    /// Convert coordinates to address (reverse geocoding)
    /// </summary>
    Task<string?> ReverseGeocodeAsync(double latitude, double longitude);
    
    /// <summary>
    /// Get country name from coordinates
    /// </summary>
    Task<string?> GetCountryFromCoordinatesAsync(double latitude, double longitude);
    
    /// <summary>
    /// Get city name from coordinates
    /// </summary>
    Task<string?> GetCityFromCoordinatesAsync(double latitude, double longitude);
    
    /// <summary>
    /// Calculate distance and duration between two locations
    /// </summary>
    Task<DistanceResult?> CalculateDistanceAsync(
        double originLat, double originLng,
        double destLat, double destLng,
        string mode = "driving");
    
    /// <summary>
    /// Calculate distances from origin to multiple destinations
    /// </summary>
    Task<List<DistanceResult>> CalculateDistancesAsync(
        double originLat, double originLng,
        List<(double lat, double lng)> destinations,
        string mode = "driving");
}

/// <summary>
/// Geocoding result
/// </summary>
public class GeocodeResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string FormattedAddress { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}

/// <summary>
/// Distance calculation result
/// </summary>
public class DistanceResult
{
    public double DistanceMeters { get; set; }
    public double DistanceKilometers => DistanceMeters / 1000;
    public int DurationSeconds { get; set; }
    public int DurationMinutes => DurationSeconds / 60;
    public string DistanceText { get; set; } = string.Empty;
    public string DurationText { get; set; } = string.Empty;
}
