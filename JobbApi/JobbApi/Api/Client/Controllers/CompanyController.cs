using AutoMapper;
using JobbApi.Api.Client.DTOs;
using JobbApi.Data;
using JobbApi.Data.Entities;
using JobbApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CompanyController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CompanyCreateDto createDto)
        {
            if (createDto.File != null)
            {
                #region CheckPhotoLength
                if (createDto.File.Length > 4 * (1024 * 1024))
                {
                    return StatusCode(409, "File cannot be more than 4MB");
                }
                #endregion
                #region CheckPhotoContentType
                if (createDto.File.ContentType != "image/png" && createDto.File.ContentType != "image/jpeg")
                {
                    return StatusCode(409, "File only jpeg and png files accepted");
                }
                #endregion

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/companies", createDto.File);

                createDto.Photo = filename;
            }
            Company company = new Company
            {
                Name = createDto.Name,
                Email = createDto.Email,
                Address = createDto.Address,
                Phone = createDto.Phone,
                Category = createDto.Category,
                Desc = createDto.Desc,
                Photo = createDto.Photo,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                ModifiedAt = DateTime.UtcNow.AddHours(4),
            };

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return StatusCode(201, company.Id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckAuthorNotFound
            if (company == null)
                return NotFound();
            #endregion

            CompanyGetDto companyGetDto = _mapper.Map<CompanyGetDto>(company);

            return Ok(companyGetDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Company> companies = await _context.Companies.Skip((page - 1) * 8).Take(8).ToListAsync();


            CompanyListDto companyDto = new CompanyListDto
            {
                Companies = _mapper.Map<List<CompanyItemDto>>(companies),
                TotalCount = await _context.Companies.CountAsync()
            };


            return Ok(companyDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Company> companies = await _context.Companies.ToListAsync();

            return Ok(_mapper.Map<List<CompanyItemDto>>(companies));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] CompanyCreateDto editDto)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
                return NotFound();


            if (editDto.File != null)
            {
                #region CheckPhotoLength
                if (editDto.File.Length > 4 * (1024 * 1024))
                {
                    return StatusCode(409, "File cannot be more than 4MB");
                }
                #endregion
                #region CheckPhotoContentType
                if (editDto.File.ContentType != "image/png" && editDto.File.ContentType != "image/jpeg")
                {
                    return StatusCode(409, "File only jpeg and png files accepted");
                }
                #endregion

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/companies", editDto.File);
                if (!string.IsNullOrWhiteSpace(company.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/companies", company.Photo);
                }
                company.Photo = filename;
            }

            company.Name = editDto.Name;
            company.Email = editDto.Email;
            company.Address = editDto.Address;
            company.Phone = editDto.Phone;
            company.Category = editDto.Category;
            company.Desc = editDto.Desc;
            company.ModifiedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Company company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCompanyNotFound
            if (company == null)
                return NotFound();
            #endregion

            _context.Companies.Remove(company);
            _context.SaveChanges();

            return NoContent();

        }
    }
}