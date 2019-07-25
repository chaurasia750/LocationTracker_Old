using System;
using System.Collections.Generic;
using System.Text;

namespace LocationTracker.Models
{
    public class Journey
    {
        public Guid ID { get; set; }

        public string UserFromPlace { get; set; } = String.Empty;
        public string UserToPlace { get; set; } = String.Empty;
        public string StartingTime { get; set; }
        public string StartingPlace { get; set; }
        public string EndingTime { get; set; }
        public string EndingPlace { get; set; }

        public double AutoDist { get; set; } = 0.0;
        public double CabACDist { get; set; } = 0.0;
        public double CabNCDist { get; set; } = 0.0;
        public double BusACDist { get; set; } = 0.0;
        public double BusNCDist { get; set; } = 0.0;
        public double TrainDist { get; set; } = 0.0;
        public double FlightDist { get; set; } = 0.0;
        public double WalkDist { get; set; } = 0.0;
        public double CarDist { get; set; } = 0.0;
        public double BikeDist { get; set; } = 0.0;
        public double OtherDist { get; set; } = 0.0;

        public double AutoAmt { get; set; } = 0.0;
        public double CabACAmt { get; set; } = 0.0;
        public double CabNCAmt { get; set; } = 0.0;
        public double BusACAmt { get; set; } = 0.0;
        public double BusNCAmt { get; set; } = 0.0;
        public double TrainAmt { get; set; } = 0.0;
        public double FlightAmt { get; set; } = 0.0;
        public double WalkAmt { get; set; } = 0.0;
        public double CarAmt { get; set; } = 0.0;
        public double BikeAmt { get; set; } = 0.0;
        public double OtherAmt { get; set; } = 0.0;

        public double HotelAmt { get; set; } = 0.0;
        public double FoodAmt { get; set; } = 0.0;

        public string Checkpoints { get; set; }
        public string Remarks { get; set; }
    }
}
