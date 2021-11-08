﻿using Chat_App_Library.Interfaces;
using Chat_App_Library.Models;
using Chat_App_Library.Singletons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat_App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        IDatabaseSingleton _databaseSingleton;
        IRepository _repo;
        public CredentialsController(IDatabaseSingleton databaseSingleton)
        {
            _databaseSingleton = databaseSingleton;
            _repo = databaseSingleton.GetRepository();
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