﻿using AutoMapper;
using JobbApi.Api.Manage.DTOs;
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

namespace JobbApi.Api.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/manage/[controller]")]
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

        [HttpPost("create")]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreate)
        {
            #region CheckCategoryExist
            if (await _context.Categories.AnyAsync(x => x.Name.ToLower() == categoryCreate.Name.Trim().ToLower()))
            {
                return Conflict($"Category already exist by name: {categoryCreate.Name}");
            }
            #endregion

            Category category = _mapper.Map<Category>(categoryCreate);
            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            category.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return StatusCode(201, category.Id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Category> categories = await _context.Categories.Skip((page - 1) * 8).Take(8).ToListAsync();

            CategoryListDto categoriesDto = new CategoryListDto
            {
                Categories = _mapper.Map<List<CategoryItemDto>>(categories),
                TotalCount = await _context.Categories.CountAsync()
            };

            return Ok(categoriesDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Category> categories = await _context.Categories.ToListAsync();

            return Ok(_mapper.Map<List<CategoryItemDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCategoryNotFound
            if (category == null)
                return NotFound();
            #endregion

            CategoryGetDto categoryDto = _mapper.Map<CategoryGetDto>(category);

            return Ok(categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CategoryCreateDto editDto)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            category.Name = editDto.Name;
            category.Icon = editDto.Icon;
            category.ModifiedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCategoryNotFound
            if (category == null)
                return NotFound();
            #endregion

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
