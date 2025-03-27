using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

public interface IPharmacyService
{
    public Task<Pharmacy> GetById(Guid id);
    public Task<List<Pharmacy>> GetByCompany(int company);
    public Task<Pharmacy> Create(Pharmacy pharmacy);
    public Task Update(Pharmacy pharmacy);
    public Task Delete(int id);
}