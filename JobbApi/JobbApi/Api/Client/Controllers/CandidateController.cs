﻿using AutoMapper;
using JobbApi.Api.Client.DTOs;
using JobbApi.Data;
using JobbApi.Data.Entities;
using JobbApi.Services;
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
    [Authorize(Roles ="Member")]
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public CandidateController(AppDbContext context, IMapper mapper,IMailService mailService)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CandidateCreateDto createDto)
        {
            
            #region CheckJobNotFound
            if (!await _context.Jobs.AnyAsync(x => x.Id == createDto.JobId))
                return NotFound($"Job not found by id: {createDto.JobId}");
            #endregion

            Candidate candidate = _mapper.Map<Candidate>(createDto);

            candidate.ModifiedAt = DateTime.UtcNow.AddHours(4);
            candidate.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Candidate candidate = await _context.Candidates
                .Include(x => x.AppUser).Include(x => x.Job)
                .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckJobNotFound
            if (candidate == null)
                return NotFound();
            #endregion

            CandidateGetDto candidateDto = _mapper.Map<CandidateGetDto>(candidate);

            return Ok(candidateDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1)
        {
            List<Candidate> candidates = await _context.Candidates
                .Include(x => x.AppUser).Include(x => x.Job)
                .Skip((page - 1) * 10).Take(10).ToListAsync();

            CandidateListDto candidateDtos = new CandidateListDto
            {
                Candidates = _mapper.Map<List<CandidateItemDto>>(candidates),
                TotalCount = await _context.Candidates.CountAsync()
            };

            return Ok(candidateDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CandidateCreateDto editDto)
        {
            Candidate candidate = await _context.Candidates
                .Include(x => x.Job).ThenInclude(c=>c.Company).Include(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            //404
            #region CheckJobNotFound
            if (!await _context.Jobs.AnyAsync(x => x.Id == editDto.JobId))
                return NotFound($"Job not found by id: {editDto.JobId}");
            #endregion

            if (candidate == null)
                return NotFound();

            candidate.Status = editDto.Status;
            candidate.ModifiedAt = DateTime.UtcNow.AddHours(4);

            if (editDto.Status == Data.Enums.CandidateStatus.Accepted)
            {
                await _mailService.SendEmailAsync(candidate.AppUser.Email, "Congratulations!!!You got the job", $"As a {candidate.Job.Company.Name} we are pleased to inform that you are our new employee");
            }
            else if (editDto.Status == Data.Enums.CandidateStatus.Rejected)
            {
                await _mailService.SendEmailAsync(candidate.AppUser.Email, "We are sorry", $"As a {candidate.Job.Company.Name} we are grateful for your spent time on application.Unfortunately we cannot offer you a job.Good Luck!!!");
            }

            await _context.SaveChangesAsync();

            return StatusCode(200);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            Candidate candidate = await _context.Candidates
                .Include(x => x.Job).Include(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (candidate == null)
                return NotFound();

             _context.Candidates.Remove(candidate);

            await _context.SaveChangesAsync();

            return StatusCode(204);
        }
    }
}

