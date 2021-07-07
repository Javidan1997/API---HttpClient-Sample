using JobbApi.Api.Manage.DTOs;
using JobbApi.Data;
using JobbApi.Data.Entities;
using JobbApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(AppDbContext context, UserManager<AppUser> userManager, IJwtService jwtService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

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

            return Ok(token);
        }

        

    }
}
