using AutoMapper;
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
    public class CountryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CountryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CountryCreateDto countryCreate)
        {
            #region CheckCountryExist
            if (await _context.Countries.AnyAsync(x => x.Name.ToLower() == countryCreate.Name.Trim().ToLower()))
            {
                return Conflict($"Country already exist by name: {countryCreate.Name}");
            }
            #endregion

            Country country = _mapper.Map<Country>(countryCreate);
            country.CreatedAt = DateTime.UtcNow.AddHours(4);
            country.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();

            return StatusCode(201, country.Id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Country> countries = await _context.Countries.Skip((page - 1) * 8).Take(8).ToListAsync();

            CountryListDto countriesDto = new CountryListDto
            {
                Countries = _mapper.Map<List<CountryItemDto>>(countries),
                TotalCount = await _context.Countries.CountAsync()
            };

            return Ok(countriesDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<Country> countries = await _context.Countries.ToListAsync();

            return Ok(_mapper.Map<List<CountryItemDto>>(countries));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Country country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCountryNotFound
            if (country == null)
                return NotFound();
            #endregion

            CountryGetDto countryDto = _mapper.Map<CountryGetDto>(country);

            return Ok(countryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CountryCreateDto editDto)
        {
            Country country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
                return NotFound();

            country.Name = editDto.Name;
            country.ModifiedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Country country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCountryNotFound
            if (country == null)
                return NotFound();
            #endregion

            _context.Countries.Remove(country);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
