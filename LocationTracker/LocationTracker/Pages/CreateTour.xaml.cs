using LocationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTour : ContentPage
    {
        public CreateTour()
        {
            InitializeComponent();
            if (Application.Current.Properties["Designation"].ToString() == "SSO")
            {
                ssoLabel.IsVisible = false;
                ssoEntry.IsVisible = false;
                //inspectionDatePicker.IsVisible = false;
                //InspectionDatLabel.IsVisible = false;
            }
            JSOEntry.Text = Application.Current.Properties["FullName"].ToString();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Tour newTour = new Tour()
                {
                    SampleNumber = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(QuotEntry.Text),
                    SampleVillage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(VillageEntry.Text),
                    Population = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PopulationEntry.Text),
                    Scheme = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SchemeEntry.Text),
                    Tehsil = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TehsilEntry.Text),
                    District = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DistrictEntry.Text),

                    OnwardFieldStartDate = JsonConvert.SerializeObject(startDatePicker.Date),
                    OnwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + startTimePicker.Time)),
                    OnwardFieldEndDate = JsonConvert.SerializeObject((endDatePicker.Date)),
                    OnwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + endTimePicker.Time)),
                    OnwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(journeyFromEntry.Text),
                    OnwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(journeyToEntry.Text),
                    OnwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modeOfJourneyEntry.Text),
                    OnwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(distanceEntry.Text),
                    OnwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fareEntry.Text),


                    //BackWard
                    BackwardFieldStartDate = JsonConvert.SerializeObject(backWardStartDatePicker.Date),
                    BackwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + backWardStartTimePicker.Time)),

                    BackwardFieldEndDate = JsonConvert.SerializeObject(backWardEndDatePicker.Date),
                    BackwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + backWardEndTimePicker.Time)),



                    BackwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardJourneyFromEntry.Text),
                    BackwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardJourneyToEntry.Text),
                    BackwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardModeOfJourneyEntry.Text),
                    BackwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardDistanceEntry.Text),
                    BackwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardFareEntry.Text),


                    CampDetails = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(campAddressEntry.Text??""),
                    JSOFIName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(JSOEntry.Text??""),
                    InspectionDak = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ssoEntry.Text??""),
                    Remark = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(RemarksEntry.Text??""),
                    ROSRO = Application.Current.Properties["Section"].ToString(),
                    TourOwnerType = Application.Current.Properties["Designation"].ToString(),
                    Status = "",
                    EditableBy = ""
                };
                await Navigation.PushAsync(new ConfirmTour(newTour), true);
            }
            catch (Exception ex)
            {

               
            }
            

            
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopToRootAsync();
            Application.Current.MainPage = new NavigationPage(new HomePage());
            return true;
        }

    }
}