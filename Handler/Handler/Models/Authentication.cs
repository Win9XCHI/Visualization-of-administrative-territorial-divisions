﻿using Microsoft.AspNetCore.Http;

namespace Handler.Models
{
    public class LogIn
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PIB { get; set; }
        public string Rights { get; set; }
    }

    public class Register
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PIB { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public byte[] Diploma { get; set; }
        public IFormFile DiplomaF { get; set; }
    }
}