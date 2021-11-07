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
    public class ChatController : ControllerBase
    {
        IDatabaseSingleton _databaseSingleton;
        IRepository _repo;
        public ChatController(IDatabaseSingleton databaseSingleton)
        {
            _databaseSingleton = databaseSingleton;
            _repo = databaseSingleton.GetRepository();
        }
        [HttpGet("api/getmessages")]
        public IEnumerable<Message> GetMessages()
        {
            return _repo.GetMessages();
        }
        [HttpGet("api/getmessagesbyuserid/{id}")]
        public IEnumerable<Message> GetMessagesByuserId(int id)
        {
            return _repo.GetMessagesByUserId(id);
        }
        [HttpPut("api/deletemessagegroup/{id}")]
        public void DeleteMessageGroup(int id, [FromBody] GroupChat input)
        {
            _repo.DeleteMessageGroup(input, id);
        }
        [HttpPut("api/deletemessagegeneral/{id}")]
        public void DeleteMessageGeneral(int id, [FromBody] GroupChat input)
        {
            _repo.DeleteMessageGeneral(input, id);
        }
        [HttpPut("api/deletemessagesingleuser/{id}")]
        public void DeleteMessageSingleUser(int id, [FromBody] SingleUserChat input)
        {
            _repo.DeleteMessageSingleUser(input, id);
        }
        [HttpPut("api/updatemessagetogroupchat/{id}")]
        public void UpdateMessageToGroupChat(int id, [FromBody] Message input)
        {
            _repo.UpdateMessageToGroupChat(input, id);
        }
        [HttpPut("api/updatemessagetosingleuserchat/{id}")]
        public void UpdateMessageToSingleUserChat(int id, [FromBody] Message input)
        {
            _repo.UpdateMessageToSingleUserChat(input, id);
        }
        [HttpPut("api/updatemessagetogeneralchat/{id}")]
        public void UpdateMessageToGeneralChat(int id, [FromBody] Message input)
        {
            _repo.UpdateMessageToSingleUserChat(input, id);
        }

        [HttpPut("api/addmessagetogroupchat/{id}")]
        public void AddMessageToGroupChat(int id, [FromBody] Message input)
        {
            _repo.AddMessageToGroupChat(input, id);
        }

        [HttpPut("api/addmessagetosingleuserchat/{id}")]
        public void AddMessageToSingleUserChat(int id, [FromBody] Message input)
        {
            _repo.AddMessageToSingleUserChat(input, id);
        }
        [HttpPut("api/addmessagetogeneralchat/{id}")]
        public void AddMessageToGeneralChat(int id, [FromBody] Message input)
        {
            _repo.AddMessageToGeneralChat(input, id);
        }
    }
}
