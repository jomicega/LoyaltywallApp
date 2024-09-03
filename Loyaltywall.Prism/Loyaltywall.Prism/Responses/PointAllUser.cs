using System;
using System.Collections.Generic;
using System.Text;

namespace Loyaltywall.Prism.Responses
{
    public class PointAllUser
    {
        public int total { get; set; }
        public List<TotalMovementPoint> total_movement_points { get; set; }
    }

    public class TotalMovementPoint
    {
        public double points { get; set; }
        public DateTime registration_date { get; set; }
        public DateTime? expiration_date { get; set; }
        public string name { get; set; }
    }
}
