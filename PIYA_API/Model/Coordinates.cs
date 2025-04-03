namespace PIYA_API.Model;

public class Coordinates
{
    public Guid Id { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}
