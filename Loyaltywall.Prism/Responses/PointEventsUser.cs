using System;
using System.Collections.Generic;
using System.Text;

namespace Loyaltywall.Prism.Responses
{
    public class PointEventsUser
    {
        public int totalEvents { get; set; }
        public List<UserEvent> user_events { get; set; }
    }    

    public class UserEvent
    {      

        public int points { get; set; }
        public DateTime registration_date { get; set; }
        public DateTime expiration_date { get; set; }
        public string @event { get; set; }
    }
}
