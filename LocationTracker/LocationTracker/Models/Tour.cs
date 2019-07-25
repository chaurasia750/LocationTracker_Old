using System;
using System.Collections.Generic;
using System.Text;

namespace LocationTracker.Models
{
    public class Tour
    {
        public Guid ID { get; set; }

        public string SampleNumber { get; set; } = String.Empty;
        public string SampleVillage { get; set; } = String.Empty;
        public string Population { get; set; } = String.Empty;
        public string Scheme { get; set; } = String.Empty;
        public string Tehsil { get; set; } = String.Empty;
        public string District { get; set; } = String.Empty;
        public string OnwardFieldStartDate { get; set; } = String.Empty;
        public string OnwardFieldStartTime { get; set; } = String.Empty;
        public string OnwardFieldEndDate { get; set; } = String.Empty;
        public string OnwardFieldEndTime { get; set; } = String.Empty;
        public string RecommadationByIncharge { get; set; } = String.Empty;
        public string ApprovedByRO { get; set; } = String.Empty;
        public string JSOFIName { get; set; } = String.Empty;
        public string DistanceFromHome { get; set; } = String.Empty;
        public string DistanceFromOffice { get; set; } = String.Empty;
        public string ModeOfTravel { get; set; } = String.Empty;
        public string Fare { get; set; } = String.Empty;
        public string InspectionDak { get; set; } = String.Empty;
        public string Remark { get; set; } = String.Empty;

        public string ROSRO { get; set; } = String.Empty;
        public string TourOwnerType { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public string EditableBy { get; set; }

        //new Filed
        public string OnwardDistance { get; set; }
        public string OnwardModeOfJourney { get; set; } = String.Empty;
        public string OnwardFare { get; set; } = String.Empty;
        public string OnwardJourneyFrom { get; set; } = String.Empty;
        public string OnwardJourneyTo { get; set; } = String.Empty;

        public string BackwardFieldStartDate { get; set; } = String.Empty;
        public string BackwardFieldStartTime { get; set; } = String.Empty;
        public string BackwardJourneyFrom { get; set; } = String.Empty;
        public string BackwardFieldEndDate { get; set; } = String.Empty;
        public string BackwardFieldEndTime { get; set; } = String.Empty;

        public string BackwardModeOfJourney { get; set; } = String.Empty;

        public string BackwardDistance { get; set; } = String.Empty;
        public string BackwardFare { get; set; } = String.Empty;
        public string BackwardJourneyTo { get; set; } = String.Empty;
        public string CampDetails { get;  set; }

        //not used for Api
        public string OwnwardStartDateTime { get; set; }
        public string OwnwardEndDateTime { get; set; }

        public string BackWardStartDateTime { get; set; }
        public string BackWardEndDateTime { get; set; }

        public DateTime OwnwardStartDate { get; set; }
        public DateTime OwnwardEndDate { get; set; }
    }
}
