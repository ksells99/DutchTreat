﻿using DutchTreat.Data.Entities;
using DutchTreat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager, UserManager<StoreUser> userManager, IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Login()
        {
            // Redirect user to homepage if already logged in
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Login using credentials from form
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // If url contains return string, redirect to there, otherwise just go to Shop
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    } else
                    {
                       return RedirectToAction("Shop", "App");
                    }
                
                }
            }

            ModelState.AddModelError("", "Login failure");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Call signout method & redirect to homepage
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user exists with that username (from body)
                var user = await _userManager.FindByNameAsync(model.Username);

                // If so, check if the password given is correct
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    // Create JWT
                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        // Get key from config.json file & create new key
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

                        // Convert key to sign in credentials
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        // Create JWT & return it (201)
                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"], 
                            _config["Tokens:Audience"], 
                            claims,
                            signingCredentials: creds,
                            expires: DateTime.UtcNow.AddMinutes(20));

                        return Created("", new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }


                }
    
            }

            return BadRequest();
            


        }
    }
}