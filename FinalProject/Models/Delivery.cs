using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Delivery
    {
        public int DeliveryID { get; set; }
        public int OrderID { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
    }
}