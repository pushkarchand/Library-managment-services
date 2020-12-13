using System;
using System.Collections.Generic;

#nullable disable

namespace Gateway.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string CardDetail { get; set; }
        public int RoleType { get; set; }
        public string UserName { get; set; }
    }
}
