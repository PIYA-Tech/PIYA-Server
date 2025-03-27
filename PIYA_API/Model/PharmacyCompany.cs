namespace PIYA_API.Model
{
    public class PharmacyCompany
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<Pharmacy>? Pharmacies { get; set; }
        public List<User>? Staff { get; set; }
    }
}
