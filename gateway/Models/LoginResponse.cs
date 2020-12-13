using System;
using System.Collections.Generic;

#nullable disable

namespace Gateway.Models
{
    public partial class LoginResponse
    {
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public int RoleType { get; set; }
        public string UserCode { get; set; }
    }
}
