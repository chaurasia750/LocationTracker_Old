using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LocationTracker.Models
{
    public class TourDays
    {
        public int Day { get; set; }
        public string GUID { get; set; }
        public Command DaysCommand { get; set; }
        public Command RemakrsCommand { get; set; }
        public bool IsActive { get; set; }
        
        public string Text { get; set; }
    }
}
