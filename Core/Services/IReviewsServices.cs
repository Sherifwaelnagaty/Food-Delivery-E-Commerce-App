﻿using Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IReviewsServices
    {

        Task<IActionResult> AddReview(ReviewsDTO Review);
        Task<IActionResult> UpdateReview(int id, ReviewsDTO ReviewsDTO);
        IActionResult DeleteReview(int Id);
    }
}
