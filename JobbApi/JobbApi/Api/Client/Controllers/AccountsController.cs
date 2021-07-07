using AutoMapper;
using JobbApi.Api.Client.DTOs;
using JobbApi.Data;
using JobbApi.Data.Entities;
using JobbApi.Helpers;
using JobbApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public AccountsController(AppDbContext context, UserManager<AppUser> userManager,
            IJwtService jwtService, RoleManager<IdentityRole> roleManager, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _mapper = mapper;
            _env = env;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(registerDto.Email);

            //409
            #region CheckUserAlreadyExist
            if (user != null)
                return StatusCode(409, $"User already exist by email {registerDto.Email}");
            #endregion

            user = new AppUser
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            //402
            #region CheckResultFailed
            if (!result.Succeeded)
            {
                return StatusCode(402, result.Errors.First().Description);
            }
            #endregion

            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201, user.Id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(MemberLoginDto loginDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginDto.Email);

            //404
            #region CheckUserNotFound
            if (user == null)
                return NotFound();
            #endregion

            //404
            #region CheckPasswordIncorrect
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return NotFound();
            #endregion

            #region JWT Generate
            var roleNames = await _userManager.GetRolesAsync(user);
            string token = _jwtService.Generate(user, roleNames);
            #endregion

            return Ok(new { user.FullName, Token = token });
        }

        [Authorize(Roles ="Member")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePassword(MemberUpdatePasswordDto passwordDto)
        {
            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return NotFound();

            var resultPass = await _userManager.ChangePasswordAsync(existUser, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!resultPass.Succeeded)
            {
                return StatusCode(402, resultPass.Errors.First().Description);
            }

            return StatusCode(201);
        }

        [Authorize(Roles = "Member")]
        [HttpPut("edit")]

        public async Task<IActionResult> EditProfile([FromForm]MemberEditDto editDto)
        {
            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return NotFound();

            //if (await _userManager.Users.AnyAsync(x=>x.Email==editDto.Email))
            //{
            //    return StatusCode(409, $"User already exist by email {editDto.Email}");
            //}

            //if (await _userManager.Users.AnyAsync(x => x.UserName == editDto.UserName))
            //{
            //    return StatusCode(409, $"User already exist by user name {editDto.UserName}");
            //}

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

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/users", editDto.File);
                if (!string.IsNullOrWhiteSpace(existUser.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/users", existUser.Photo);
                }
                existUser.Photo = filename;
            }

            existUser.UserName = editDto.UserName;
            existUser.Email = editDto.Email;
            existUser.PhoneNumber = editDto.PhoneNumber;
            existUser.Occupation = editDto.Occupation;
            existUser.Address = editDto.Address;
            existUser.Desc = editDto.Desc;
            existUser.ModifiedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return StatusCode(201, new { existUser.UserName,existUser.Id});


        }

    }
}
