using System;
using System.Collections.Generic;

namespace Loyaltywall.Prism.Responses
{
    public class TotalpointsExpireResponse
    {
        public int total_points_to_expire { get; set; }
        public List<DateToExpire> date_to_expire { get; set; }
        
    }

    public class DateToExpire
    {
        public int id { get; set; }
        public int points { get; set; }
        public DateTime expiration_date { get; set; }
    }
}
