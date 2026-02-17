using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

public interface IPharmacyCompanyService
{
    Task<PharmacyCompany?> GetByIdAsync(Guid id);
    Task<List<PharmacyCompany>> GetAllAsync();
    Task<PharmacyCompany> CreateAsync(PharmacyCompany company);
    Task<PharmacyCompany> UpdateAsync(PharmacyCompany company);
    Task DeleteAsync(Guid id);
    Task<List<Pharmacy>> GetCompanyPharmaciesAsync(Guid companyId);
    Task<int> GetPharmacyCountAsync(Guid companyId);
}
