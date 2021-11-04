﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App_Library.Models
{
    public class GroupChat
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Message> Messages { get; set; }
        public List<User> Users { get; set; }
        public User GroupOwner { get; set; }
    }
}
