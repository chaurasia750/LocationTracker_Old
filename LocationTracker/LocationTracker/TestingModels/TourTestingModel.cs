using LocationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LocationTracker.TestingModels
{
    public class TourTestingModel
    {
        public Tour MyTour { get; set; }
        public TourTestingModel()
        {
           
            MyTour = new Tour()
            {
                SampleNumber = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("SampleNumber"),
                SampleVillage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("SampleVillage"),
                Population = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Population"),
                Scheme = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Scheme"),
                Tehsil = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Tehsil"),
                District = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("District"),

                OnwardFieldStartDate = JsonConvert.SerializeObject(DateTime.Now),
                OnwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + TimeSpan.FromMinutes(10))),
                OnwardFieldEndDate = JsonConvert.SerializeObject(DateTime.Today.AddDays(1)),
                OnwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + TimeSpan.FromMinutes(10))),
                OnwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("OnwardJourneyFrom"),
                OnwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("'example"),
                OnwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("OnwardModeOfJourney"),
                OnwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("OnwardDistance"),
                OnwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("OnwardFare"),


                //BackWard
                BackwardFieldStartDate = JsonConvert.SerializeObject(new DateTime()),
                BackwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + TimeSpan.FromMinutes(10))),
                BackwardFieldEndDate = JsonConvert.SerializeObject(DateTime.Today.AddDays(1)),
                BackwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + TimeSpan.FromMinutes(10))),
                BackwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("BackwardJourneyFrom"),
                BackwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("BackwardJourneyTo"),
                BackwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("BackwardModeOfJourney"),
                BackwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("BackwardDistance"),
                BackwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("BackwardFare"),


                CampDetails = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("CampDetails"),
                JSOFIName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("JSOFIName"),
                InspectionDak = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("InspectionDak"),
                Remark = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Remark"),
                ROSRO = Application.Current.Properties["Section"].ToString(),
                TourOwnerType = Application.Current.Properties["Designation"].ToString(),
                Status = "Pending",
                EditableBy = "superviser"
            };
        }
    }
}
