namespace PIYA_API.Model;

public class Pharmacy
{
    public Guid Id { get; set; }
    public required string Country { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public User? Manager { get; set; }
    public List<User>? Staff { get; set; }
    public required Coordinates Coordinates { get; set; }
    public required PharmacyCompany Company { get; set; }
}
