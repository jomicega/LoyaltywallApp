using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loyaltywall.Prism.Responses
{ 
    public class All
    {
        public string name { get; set; }
        public DateTime date_point { get; set; }
        public double points { get; set; }
    }

    public class Consumed
    {
        public string name { get; set; }
        public DateTime date_point { get; set; }
        public double points { get; set; }
    }

    public class Data
    {
        public int current_points { get; set; }
        public int expired_points { get; set; }
        public string date_point_from { get; set; }
        public string date_point_to { get; set; }
        public HistoryPoints history_points { get; set; }
    }

    public class HistoryPoints
    {
        public List<Consumed> consumed { get; set; }
        public List<Purchased> purchased { get; set; }
        public List<All> All { get; set; }
    }

    public class Purchased
    {
        public string name { get; set; }
        public DateTime date_point { get; set; }
        public double points { get; set; }
    }

    public class PointsResponse
    {
        public string message { get; set; }
        public Data data { get; set; }
    }



}
