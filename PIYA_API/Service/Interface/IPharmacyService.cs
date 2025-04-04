﻿using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

public interface IPharmacyService
{
    public Task<Pharmacy> GetById(Guid id);
    public Task<List<Pharmacy>> GetByCompany(Guid companyId);
    public Task<Pharmacy> Create(Pharmacy pharmacy);
    public Task Delete(Guid id);
    public Task Update(Pharmacy pharmacy);
}