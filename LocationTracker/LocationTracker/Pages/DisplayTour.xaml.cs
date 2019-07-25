using LocationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DisplayTour : ContentPage
	{
        public Tour NewTour { get; set; }
        public DisplayTour ( Tour MyTour)
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            QuotEntry.Text = MyTour.SampleNumber;
            VillageEntry.Text = MyTour.SampleVillage;
            PopulationEntry.Text = MyTour.Population;
            SchemeEntry.Text = MyTour.Scheme;
            TehsilEntry.Text = (MyTour.Tehsil);
            DistrictEntry.Text = (MyTour.District);
            startDateTimeEntry.Text = $"{(JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldStartDate).ToLongDateString())} {JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldStartTime).ToLongTimeString()}";
            endDateTimeEntry.Text = $"{(JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldEndDate).ToLongDateString())} {JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldEndTime).ToLongTimeString()}";
            DAInchargeEntry.Text = MyTour.RecommadationByIncharge;
            DAApprovedeEntry.Text = MyTour.ApprovedByRO;
            JSOEntry.Text = (MyTour.JSOFIName);
            DistOffiEntry.Text = MyTour.DistanceFromOffice;
            DistResiEntry.Text = MyTour.DistanceFromHome;
            TravelDescEntry.Text = (MyTour.ModeOfTravel);
            TravelFareEntry.Text = MyTour.Fare;
            ssoEntry.Text = (MyTour.InspectionDak);
            RemarksEntry.Text = (MyTour.Remark);
            statusLabel.Text = MyTour.Status;
            NewTour = MyTour;
            if(MyTour.Status == "Rejected")
            {
                editLabel.IsVisible = true;
                SaveButton.IsVisible = true;
                statusLabel.TextColor = Color.DarkRed;  
            }
            if(MyTour.EditableBy == "Orange")
            {
                statusLabel.TextColor = Color.DarkOrange;
            }
            else
            {
                statusLabel.TextColor = Color.DarkGreen;
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            NewTour.Status = String.Empty;
            NewTour.EditableBy = String.Empty;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Making Tour Program Editable";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.EditTour;
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("TourInfo", JsonConvert.SerializeObject(NewTour)) });
                    using (var httpClient = new HttpClient())
                    {
                        try
                        {
                            Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                            HttpResponseMessage response = await getResponse;
                            if (response.IsSuccessStatusCode)
                            {
                                var myContent = await response.Content.ReadAsStringAsync();
                                if (myContent == "False")
                                {
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        var Message = "Something Went Wrong.Sign In Again!!!";
                                        await DisplayAlert("Error", Message, "OK");
                                        MainFrame.IsVisible = true;
                                        LoadingOverlay.IsVisible = false;
                                        Application.Current.Properties["Username"] = string.Empty;
                                        await Application.Current.SavePropertiesAsync();
                                        await Navigation.PopToRootAsync();
                                        Application.Current.MainPage = new NavigationPage(new MainPage());
                                        return;
                                    });
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        var Message = "You can now edit this tour from Edit Tour List!";
                                        await DisplayAlert("Success", Message, "OK");
                                        MainFrame.IsVisible = true;
                                        LoadingOverlay.IsVisible = false;
                                        await Navigation.PopToRootAsync();
                                        Application.Current.MainPage = new NavigationPage(new HomePage());
                                        return;
                                    });
                                }
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    var Message = "Server Is Down. Try Again After Some Time";
                                    DisplayAlert("Error", Message, "OK");
                                    MainFrame.IsVisible = true;
                                    LoadingOverlay.IsVisible = false;
                                    return;
                                });
                            }
                        }
                        catch (Exception)
                        {

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                var Message = "Check Your Internet Connection and Try Again";
                                DisplayAlert("Error", Message, "OK");
                                MainFrame.IsVisible = true;
                                LoadingOverlay.IsVisible = false;
                                return;
                            });
                        }
                    }
                });
            }
            else
            {
                LoadingOverlay.IsVisible = false;
                MainFrame.IsVisible = true;
                await DisplayAlert("Error", "No Internet Connection", "OK");
                return;
            }
        }
    }
}