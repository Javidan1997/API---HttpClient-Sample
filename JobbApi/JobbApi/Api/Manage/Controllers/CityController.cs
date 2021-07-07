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
    public class CityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CityController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Create City
        /// </summary>
        /// <param name="cityCreate"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create(CityCreateDto cityCreate)
        {
            #region CheckCityExist
            if (await _context.Cities.AnyAsync(x => x.Name.ToLower() == cityCreate.Name.Trim().ToLower()))
            {
                return Conflict($"City already exist by name: {cityCreate.Name}");
            }
            #endregion

            City city = _mapper.Map<City>(cityCreate);
            city.CreatedAt = DateTime.UtcNow.AddHours(4);
            city.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return StatusCode(201, city.Id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<City> cities = await _context.Cities.Skip((page - 1) * 8).Take(8).ToListAsync();

            CityListDto citiesDto = new CityListDto
            {
                Cities = _mapper.Map<List<CityItemDto>>(cities),
                TotalCount = await _context.Cities.CountAsync()
            };

            return Ok(citiesDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            List<City> cities = await _context.Cities.ToListAsync();

            return Ok(_mapper.Map<List<CityItemDto>>(cities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCityNotFound
            if (city == null)
                return NotFound();
            #endregion

            CityGetDto cityDto = _mapper.Map<CityGetDto>(city);

            return Ok(cityDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CityCreateDto editDto)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);

            if (city == null)
                return NotFound();

            city.Name = editDto.Name;
            city.ModifiedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCityNotFound
            if (city == null)
                return NotFound();
            #endregion

            _context.Cities.Remove(city);
            _context.SaveChanges();

            return NoContent();

        }
    }
}
