using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PharmacyCompaniesController(IPharmacyCompanyService pharmacyCompanyService) : ControllerBase
{
    private readonly IPharmacyCompanyService _pharmacyCompanyService = pharmacyCompanyService;
}
