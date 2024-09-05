using System;
using System.Collections.Generic;
using System.Text;

namespace Loyaltywall.Prism.Models
{
    public class UserInfo
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public int TokenExpiresIn { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; }

        public string Id { get; set; }

    }
}
