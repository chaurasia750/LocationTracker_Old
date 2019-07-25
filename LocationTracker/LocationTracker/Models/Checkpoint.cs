using System;
using System.Collections.Generic;
using System.Text;

namespace LocationTracker.Models
{
    public class Checkpoint
    {
        public string Addresss { get; set; } = String.Empty;
        public string Timestamp { get; set; } = String.Empty;

        public bool IfTransport { get; set; } = false;
        public string TransportType { get; set; } = String.Empty;
        public string TranUserFrom { get; set; } = String.Empty;
        public string TranFrom { get; set; } = String.Empty;
        public string TranUserTo { get; set; } = String.Empty;
        public string TranTo { get; set; } = String.Empty;
        public double TransAmt { get; set; } = 0.0;
        public double TransDist { get; set; } = 0.0;

        public bool IfHotel { get; set; } = false;
        public string InLocation { get; set; }
        public string OutLocation { get; set; }
        public string InTime { get; set; } = String.Empty;
        public string OutTime { get; set; } = String.Empty;
        public string HotelName { get; set; } = String.Empty;
        public string HotelGST { get; set; } = String.Empty;
        public string HotelContact { get; set; } = String.Empty;
        public string HotelRemark { get; set; } = String.Empty;
        public double HotelAmount { get; set; } = 0.0;
        public string HotelBillLink { get; set; } = String.Empty;

        public bool IfFoodBill { get; set; } = false;
        public string FoodBillLink { get; set; } = String.Empty;
        public double FoodBillAmt { get; set; } = 0.0;
    }
}
