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
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public JobController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(JobCreateDto createDto)
        {
            //404
            #region CheckCategoryNotFound
            if (!await _context.Categories.AnyAsync(x => x.Id == createDto.CategoryId))
                return NotFound($"Category not found by id: {createDto.CategoryId}");
            #endregion

            //404
            #region CheckCategoryNotFound
            if (!await _context.Cities.AnyAsync(x => x.Id == createDto.CityId))
                return NotFound($"City not found by id: {createDto.CityId}");
            #endregion

            //404
            #region CheckCategoryNotFound
            if (!await _context.Countries.AnyAsync(x => x.Id == createDto.CountryId))
                return NotFound($"Country not found by id: {createDto.CountryId}");
            #endregion

            //404
            #region CheckCategoryNotFound
            if (!await _context.Companies.AnyAsync(x => x.Id == createDto.CompanyId))
                return NotFound($"Company not found by id: {createDto.CompanyId}");
            #endregion

            Job job = _mapper.Map<Job>(createDto);
            job.CreatedAt = DateTime.UtcNow.AddHours(4);
            job.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            return StatusCode(201, job.Id);
        }
        #endregion

        #region Get
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Job job = await _context.Jobs
                .Include(x => x.Category).Include(x => x.City)
                .Include(x => x.Company).Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckJobNotFound
            if (job == null)
                return NotFound();
            #endregion

            JobGetDto jobDto = _mapper.Map<JobGetDto>(job);

            return Ok(jobDto);
        }
        #endregion

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Job> jobs = await _context.Jobs
                 .Include(x => x.Category).Include(x => x.City)
                .Include(x => x.Company).Include(x => x.Country)
                .Skip((page - 1) * 10).Take(10).ToListAsync();

            JobListDto jobDtos = new JobListDto
            {
                Jobs = _mapper.Map<List<JobItemDto>>(jobs),
                TotalCount = await _context.Jobs.CountAsync()
            };

            return Ok(jobDtos);
        }
        #endregion

        #region GetBrowse
        [HttpGet("getbrowse")]
        public async Task<IActionResult> GetAllBrowse(int page = 1)
        {
            List<Job> jobs = await _context.Jobs
                .Include(x => x.City).Include(x => x.Company)
                .Where(x => x.IsActive == true)
                .Skip((page - 1) * 8).Take(8).ToListAsync();

            JobGetListDto jobDtos = new JobGetListDto
            {
                Jobs = _mapper.Map<List<JobGetItemDto>>(jobs),
                TotalCount = jobs.Count()
            };

            return Ok(jobDtos);
        }
        #endregion

        #region GetRecent
        [HttpGet("getrecent")]
        public async Task<IActionResult> GetAllRecent()
        {
            List<Job> jobs = await _context.Jobs
                .Include(x => x.City).Include(x => x.Company)
                .Where(x => x.IsActive == true)
                .OrderByDescending(x => x.CreatedAt).Take(8).ToListAsync();

            JobGetListDto jobDtos = new JobGetListDto
            {
                Jobs = _mapper.Map<List<JobGetItemDto>>(jobs),
                TotalCount = jobs.Count()
            };

            return Ok(jobDtos);
        }
        #endregion

        #region Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, JobCreateDto editDto)
        {
            Job job = await _context.Jobs
                .Include(x => x.Category).Include(x => x.City)
                .Include(x => x.Company).Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckCategoryNotFound
            if (!await _context.Categories.AnyAsync(x => x.Id == editDto.CategoryId))
                return NotFound($"Category not found by id: {editDto.CategoryId}");
            #endregion

            //404
            #region CheckCategoryNotFound
            if (!await _context.Cities.AnyAsync(x => x.Id == editDto.CityId))
                return NotFound($"City not found by id: {editDto.CityId}");
            #endregion

            //404
            #region CheckCategoryNotFound
            if (!await _context.Countries.AnyAsync(x => x.Id == editDto.CountryId))
                return NotFound($"Country not found by id: {editDto.CountryId}");
            #endregion

            //404
            #region CheckCategoryNotFound
            if (!await _context.Companies.AnyAsync(x => x.Id == editDto.CompanyId))
                return NotFound($"Company not found by id: {editDto.CompanyId}");
            #endregion

            if (job == null)
                return NotFound();

            //job = _mapper.Map<Job>(editDto);
            job.Title = editDto.Title;
            job.Address = editDto.Address;
            job.Deadline = editDto.Deadline;
            job.Experience = editDto.Experience;
            job.Desc = editDto.Desc;
            job.Gender = editDto.Gender;
            job.JobType = editDto.JobType;
            job.Qualification = editDto.Qualification;
            job.Salary = editDto.Salary;
            job.IsActive = editDto.IsActive;
            job.CompanyId = editDto.CompanyId;
            job.CategoryId = editDto.CategoryId;
            job.CityId = editDto.CityId;
            job.CountryId = editDto.CountryId;
            job.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Job job = await _context.Jobs
                .Include(x => x.Category).Include(x => x.City)
                .Include(x => x.Company).Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckJobNotFound
            if (job == null)
                return NotFound();
            #endregion

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return NoContent();

        }
        #endregion

        #region Filter
        [HttpPost("filter")]
        public async Task<IActionResult> GetFilter(JobFilterItemDto jobFilter, int page = 1)
        {
            List<Job> jobs = await _context.Jobs
                .Include(x => x.Category).Include(x => x.Country)
                .Where(x => x.Title.Contains(jobFilter.Title)
                || x.Qualification == jobFilter.Qualification
                || x.JobType == jobFilter.JobType || x.Experience == jobFilter.Experience || x.Gender == jobFilter.Gender
                || (x.Salary > jobFilter.MinSalary && x.Salary < jobFilter.MaxSalary)
                || x.CategoryId == jobFilter.CategoryId || x.CountryId == jobFilter.CountryId).Skip((page - 1) * 8).Take(8).ToListAsync();

            JobFilterListDto jobDtos = new JobFilterListDto
            {
                Jobs = _mapper.Map<List<JobFilterItemDto>>(jobs),
                TotalCount = jobs.Count()
            };

            return Ok(jobDtos);
        }
        #endregion

        #region Search
        [HttpPost("search")]
        public async Task<IActionResult> GetSearch(JobSearchItemDto jobSearch, int page = 1)
        {
            List<Job> jobs = await _context.Jobs
                .Include(x => x.Company).Include(x => x.Country)
                .Where(x => x.Title.Contains(jobSearch.Title)
                || x.Company.Name.Contains(jobSearch.CompanyName) || x.CountryId == jobSearch.CountryId)
                .Skip((page - 1) * 8).Take(8).ToListAsync();

            JobSearchListDto jobDtos = new JobSearchListDto
            {
                Jobs = _mapper.Map<List<JobSearchItemDto>>(jobs),
                TotalCount = jobs.Count()

            };

            return Ok(jobDtos);
        }
        #endregion

        #region Apply
        [HttpPut("apply/{id}")]
        public async Task<IActionResult> Apply(int id, JobApplyDto jobApplyDto)
        {
            Job job = await _context.Jobs.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            if (job.IsApplied != jobApplyDto.IsApplied)
            {
                job.IsApplied = jobApplyDto.IsApplied;
            }
            

            await _context.SaveChangesAsync();

            return StatusCode(200);

        }

        [HttpGet("getapply")]
        public async Task<IActionResult> GetApply(int page = 1)
        {
            List<Job> jobs = await _context.Jobs
               .Include(x => x.Company).Where(x => x.IsApplied == true)
               .Skip((page - 1) * 8).Take(8).ToListAsync();

            JobApplyListDto jobDtos = new JobApplyListDto
            {
                Jobs = _mapper.Map<List<JobApplyItemDto>>(jobs),
                TotalCount = jobs.Count()
            };

            return Ok(jobDtos);
        }
        #endregion

        #region Bookmark
        [HttpPut("bookmark/{id}")]
        public async Task<IActionResult> Bookmark(int id, JobBookmarkDto jobBookmarkDto)
        {
            Job job = await _context.Jobs.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            if (job.IsBookmarked != jobBookmarkDto.IsBookmarked)
            {
                job.IsBookmarked = jobBookmarkDto.IsBookmarked;
            }
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }
        #endregion


    }
}
