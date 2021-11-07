﻿using Chat_App_JWT_API.Configuration;
using Chat_App_Library.Interfaces;
using Chat_App_Library.Models;
using Chat_App_Library.Singletons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat_App__JWT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        IDatabaseSingleton _databaseSingleton;
        IRepository _repo;
        private readonly JwtConfig _jwtConfig;
        public CredentialsController(IDatabaseSingleton databaseSingleton, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _databaseSingleton = databaseSingleton;
            _repo = databaseSingleton.GetRepository();
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("api/register")]
        public IActionResult Register([FromBody] User input)
        {
            if (ModelState.IsValid)
            {
                var existingUsers = _databaseSingleton.GetRepository().GetUsers();

                if (existingUsers.Any(a => a == input))
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                        "Email already in use"
                        },
                        Success = false
                    });
                }
                var jwtToken = Chat_App_JWT_API.JWT.JWTTokens.GenerateJwtToken(input);
                _repo.AddUser(input);
                return Ok(new RegistrationResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() { 
                "Invalid payload"
                },
                Success = false
            });
        }


        [HttpPost("api/adduser")]
        public void AddUser([FromBody] User input)
        {
            _repo.AddUser(input);
        }
        [HttpGet("api/getusers")]
        public IEnumerable<User> GetUsers()
        {
            return _repo.GetUsers();
        }
        [HttpGet("api/getuserbyid/{id}")]
#nullable enable
        public User? GetUserById(int id)
        {
            return _repo.GetUserById(id);
        }
#nullable disable
        [HttpGet("api/getusersbyname/{id}")]
        public IEnumerable<User> GetUsersByName(string id)
        {
            return _repo.GetUserByName(id);
        }

        [HttpPost("api/updateuserdata")]
        public void UpdateUserData([FromBody] User input, string placeholder = "placeholder")
        {
            _repo.UpdateUserData(input);
        }

        [HttpPost("api/deleteuser")]
        public void DeleteUser(int id)
        {
            _repo.DeleteUser(id);
        }
    }
}
