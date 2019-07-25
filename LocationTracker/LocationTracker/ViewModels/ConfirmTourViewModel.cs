using LocationTracker.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationTracker.ViewModels
{
    public class ConfirmTourViewModel : BaseViewModel
    {
        public Tour Tour { get; set; }
        public ConfirmTourViewModel(Tour cTour)
        {
            Tour = cTour;

            Tour.OwnwardStartDateTime = $"{JsonConvert.DeserializeObject<DateTime>(Tour.OnwardFieldStartDate).ToLongDateString()} " +
                $"{JsonConvert.DeserializeObject<DateTime>(Tour.OnwardFieldStartTime).ToLongTimeString()}";

            Tour.OwnwardEndDateTime = $"{JsonConvert.DeserializeObject<DateTime>(Tour.OnwardFieldEndDate).ToLongDateString()} " +
                $"{JsonConvert.DeserializeObject<DateTime>(Tour.OnwardFieldEndTime).ToLongTimeString()}";

            Tour.BackWardStartDateTime = $"{JsonConvert.DeserializeObject<DateTime>(Tour.BackwardFieldStartDate).ToLongDateString()} " +
                    $"{JsonConvert.DeserializeObject<DateTime>(Tour.BackwardFieldStartTime).ToLongTimeString()}";

            Tour.BackWardEndDateTime = $"{JsonConvert.DeserializeObject<DateTime>(Tour.BackwardFieldEndDate).ToLongDateString()} " +
                $"{JsonConvert.DeserializeObject<DateTime>(Tour.BackwardFieldEndTime).ToLongTimeString()}";


        }
    }
}
