using System;

namespace Loyaltywall.Prism.Responses
{
    public class PointsUserResponse
    {
        public int id { get; set; }
        public int total_points { get; set; }
        public int current_points { get; set; }
        public int expired_points { get; set; }
        public int redeemed_points { get; set; }
        public DateTime system_date { get; set; }
    }
}
