using Loyaltywall.Prism.Models;
using System;
using System.Collections.Generic;

namespace Loyaltywall.Prism.Responses
{
    public class PointsTotalUserResponse
    {
        public string message { get; set; }
        public int total_points { get; set; }
        public List<User> user { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
    }
}
