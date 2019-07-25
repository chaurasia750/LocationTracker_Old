using LocationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ApprovedAllTours : ContentPage
	{
        public List<Tour> AllTours { get; set; }
        public List<Tour> AllToursond { get; set; }
        public ApprovedAllTours (List<Tour> cTours)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AllTours = cTours;
            AllToursond = new List<Tour>();
            for (int i = 0; i < cTours.Count; i++)
            {
                AllToursond.Add(new Tour()
                {
                    ApprovedByRO = AllTours[i].ApprovedByRO,
                    DistanceFromHome = AllTours[i].DistanceFromHome,
                    DistanceFromOffice = AllTours[i].DistanceFromOffice,
                    District = AllTours[i].District,
                    EditableBy = String.Empty,
                    Fare = AllTours[i].Fare,
                    OnwardFieldEndDate = AllTours[i].OnwardFieldEndDate,
                    OnwardFieldEndTime = AllTours[i].OnwardFieldEndTime,
                    OnwardFieldStartDate = AllTours[i].OnwardFieldStartDate,
                    SampleNumber = AllTours[i].SampleNumber,
                    OnwardFieldStartTime = AllTours[i].OnwardFieldStartTime,
                    ID = AllTours[i].ID,
                    InspectionDak = AllTours[i].InspectionDak,
                    JSOFIName = AllTours[i].JSOFIName,
                    ModeOfTravel = AllTours[i].ModeOfTravel,
                    Population = AllTours[i].Population,
                    RecommadationByIncharge = AllTours[i].RecommadationByIncharge,
                    Remark = AllTours[i].Remark,
                    ROSRO = AllTours[i].ROSRO,
                    SampleVillage = AllTours[i].SampleVillage,
                    Scheme = AllTours[i].Scheme,
                    Status = AllTours[i].Status,
                    Tehsil = AllTours[i].Tehsil,
                    TourOwnerType = AllTours[i].TourOwnerType
                });
                AllTours[i].OnwardFieldStartDate = JsonConvert.DeserializeObject<DateTime>(AllTours[i].OnwardFieldStartDate).ToLongDateString();
                AllTours[i].OnwardFieldEndDate = JsonConvert.DeserializeObject<DateTime>(AllTours[i].OnwardFieldEndDate).ToLongDateString();
            }
            MainListView.ItemsSource = AllTours;
        }

        private async void MainListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MainFrame.IsVisible = false;
            LoadingOverlay.IsVisible = true;
            LoadingIndicatorText.Text = "Please Wait";
            var STour = (Tour)e.Item;
            var dtour = AllToursond.SingleOrDefault(m => m.ID == STour.ID);
            if (dtour == null)
            {
                await DisplayAlert("Error", "Something Went Wrong.", "Ok");
                MainFrame.IsVisible = true;
                LoadingOverlay.IsVisible = false;
                await Navigation.PopToRootAsync();
                Application.Current.MainPage = new NavigationPage(new HomePage());
                return;
            }
            await Navigation.PushAsync(new DisplayTour(dtour), true);
            MainFrame.IsVisible = true;
            LoadingOverlay.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopToRootAsync();
            Application.Current.MainPage = new NavigationPage(new HomePage());
            return true;
        }
    }
}