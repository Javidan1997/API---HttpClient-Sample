using AutoMapper;
using JobbApi.Api.Client.DTOs;
using JobbApi.Data;
using JobbApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.Controllers
{
    [Authorize(Roles = "Member")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll(int page = 1)
        //{
        //    List<Category> categories = await _context.Categories.Skip((page - 1) * 8).Take(8).ToListAsync();

        //    CategoryListDto categoriesDto = new CategoryListDto
        //    {
        //        Categories = _mapper.Map<List<CategoryItemDto>>(categories),
        //        TotalCount = await _context.Categories.CountAsync()
        //    };

        //    return Ok(categoriesDto);
        //}

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = await _context.Categories.ToListAsync();

            return Ok(_mapper.Map<List<CategoryItemDto>>(categories));
        }
    }
}
