using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class PharmacyService : IPharmacyService
{
    public Task<List<Pharmacy>> GetByCompany(int company)
    {
        throw new NotImplementedException();
    }

    public Task<Pharmacy> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Pharmacy> Create(Pharmacy pharmacy)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public Task Update(Pharmacy pharmacy)
    {
        throw new NotImplementedException();
    }
}
