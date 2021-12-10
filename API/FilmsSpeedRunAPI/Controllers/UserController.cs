﻿using BLL.DTO;
using BLL.Services;
using DAL.Context;
using FilmsSpeedRunAPI.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmsSpeedRunAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("AllowOrigin")]
    public class UserController : Controller
    {
        UserService service;
        public UserController(FilmContext context)
        {
            service = new UserService(context);
        }
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] UserDTO user)
        {
            if (user == null)
                return BadRequest(new { error = "Invalid login or password" });
            if (!service.Login(user.Login, user.PasswordHash))
                return BadRequest(new { error = "Invalid login or password" });

            var claim = await GetClaimsIdentity(user.Login, user.PasswordHash);

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claim.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = accessToken,
                username = claim.Name,
                id= service.GetId(user.Login, user.PasswordHash)
            };
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] UserDTO user)
        {
            if (user == null)
                return BadRequest(new { error = "Invalid data" });
            if (!service.ValidateUserName(user.Login))
                return BadRequest(new { error = "Login is not valid" });
            if (user.PasswordHash == "" || user.PasswordHash == null)
                return BadRequest(new { error = "No password!" });
            user.RoleId = 1;
            await service.AddAsync(user);
            return Ok();
        }
        [NonAction]
        private async Task<ClaimsIdentity> GetClaimsIdentity(string login, string password)
        {
            var claims = new List<Claim>()
                {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);            
            return claimsIdentity;
        }
        [HttpGet]
        public async Task<IActionResult> User(string login)
        {
            return Json(service.GetAll().FirstOrDefault(x => x.Login == login));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserDTO user)
        {
            var u = service.GetAll().Where(x => x.Login == user.Login).FirstOrDefault();
            if (u == null)
                return NotFound(new { error = "User not found" });
            u.PasswordHash = user.PasswordHash;
            return Ok();
        }
    }
}
