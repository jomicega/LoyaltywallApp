using System;
using System.Collections.Generic;

namespace Loyaltywall.Prism.Responses
{
    public class EventConsumed
    {
        public string name { get; set; }
    }

    public class PointConsumedUser
    {
        public int totalConsumed { get; set; }
        public List<UserConsumed> user_consumed { get; set; }
    }

    public class UserConsumed
    {
        public int id_user_points { get; set; }
        public string product { get; set; }
        public int idProduct { get; set; }
        public double points { get; set; }
        public DateTime system_date { get; set; }
        public EventConsumed @event { get; set; }
    }
}
