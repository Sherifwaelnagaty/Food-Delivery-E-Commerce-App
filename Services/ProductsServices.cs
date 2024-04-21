﻿using AutoMapper;
using Core;
using Core.Domain;
using Core.DTO;
using Core.Enums;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductsServices : IProductsServices
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;


        public ProductsServices(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _unitOfWork = UnitOfWork;
            _mapper = mapper;

        }
        public async Task<IActionResult> AddProduct(ProductsDTO product)
        {
            Products products = _mapper.Map<Products>(product);
            try
            {
                var result = await _unitOfWork.Products.Add(products);
                if (result is OkResult)
                {
                    return new OkObjectResult(products);
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Products.Delete(products);
                return new BadRequestObjectResult($"There is a problem during adding user \n" +
                    $"{ex.Message}\n {ex.InnerException?.Message}");
            }
        }

        public IActionResult DeleteProduct(int Id)
        {
            throw new NotImplementedException();
        }

        public IActionResult GetAllProducts(int Page, int PageSize, string search)
        {
            try
            {
                Func<ProductsDTO, bool> criteria = null;

                if (!string.IsNullOrEmpty(search))
                    criteria = (d => d.Colors.Contains(search) || d.Brand.Contains(search) ||
                                d.Description.Contains(search) || d.Category.Contains(search) ||
                                d.Name.Contains(search)); 

                // get doctors
                var gettingDoctorsResult = _unitOfWork.Products.GetAllProducts(Page, PageSize, criteria);
                if (gettingDoctorsResult is not OkObjectResult doctorsResult)
                {
                    return gettingDoctorsResult;
                }
                List<ProductsDTO> doctorsInfoList = doctorsResult.Value as List<ProductsDTO>;

                if (doctorsInfoList == null || doctorsInfoList.Count == 0)
                {
                    return new NotFoundObjectResult("There is no doctor");
                }

                // Load doctor images
                var doctorsInfo = doctorsInfoList.Select(d => new
                {
                    Image = GetImage(d.Images),
                    d.Name,
                    d.Description,
                    d.Price,
                    d.Quantity,
                    d.Brand,
                    d.Category,
                    d.Colors,
                }).ToList();

                return new OkObjectResult(doctorsInfo);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred while Getting Doctors info \n: {ex.Message}" +
                    $"\n {ex.InnerException?.Message}")
                {
                    StatusCode = 500
                };
            }
        }
        protected Image GetImage(List<Image> images)
        {
            if (images == null || images.Count == 0)
            {
                return null;
            }

            // For simplicity, return the first image in the list
            return images[0];
        }
        public IActionResult GetProductById(int ProductId)
        {
            throw new NotImplementedException();
        }

        public IActionResult GetProductByName(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> UpdateProduct(int id, ProductsDTO productsDTO)
        {
            try
            {
                // Retrieve the product by id
                Products product = _unitOfWork.Products.GetById(id);
                if (product == null)
                {
                    return new NotFoundObjectResult($"There is no Product with id: {id}.");
                }

                // Update product properties based on productsDTO
                product.Brand = productsDTO.Brand;
                product.Price = productsDTO.Price;
                product.Category = productsDTO.Category;
                product.Description = productsDTO.Description;

                var result = _unitOfWork.Products.Update(product);
                _unitOfWork.Complete();

                if (result is not OkResult)
                {
                    return new OkObjectResult(product);
                }
                return new OkObjectResult(product);
            }
            catch (Exception ex)
            {
                // Return error message in case of exception
                return new ObjectResult($"An error occurred while Adding Doctor \n: {ex.Message}" +
                    $"\n {ex.InnerException?.Message}")
                {
                    StatusCode = 500
                };
            }
        }
    }
}
