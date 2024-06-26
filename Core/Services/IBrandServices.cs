﻿using Core.Domain;
using Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IBrandServices
    {
        Task<IActionResult> AddBrand(Brand brand);
        IActionResult UpdateBrand(Brand brand);
        IActionResult DeleteBrand(int Id);
        IActionResult GetAllBrands(int Page, int PageSize);
    }
}
