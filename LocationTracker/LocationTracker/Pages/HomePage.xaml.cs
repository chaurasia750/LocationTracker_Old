using LocationTracker.Models;
using LocationTracker.Pages;
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
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            if (Application.Current.Properties["JID"].ToString() != string.Empty)
            {
                TourLabel.IsVisible = false;
                TourButton.IsVisible = false;
                StartNewJourneyLabel.IsVisible = false;
                NewJourneyButton.IsVisible = false;
                LogOutLabel.IsVisible = false;
                LogOutButton.IsVisible = false;
            }
            else
            {
                AddCheckpointLabel.IsVisible = false;
                AddCheckpointButton.IsVisible = false;
                AddBillLabel.IsVisible = false;
                AddBillButton.IsVisible = false;
                EndJourneyLabel.IsVisible = false;
                EndJourneyButton.IsVisible = false;
            }
        }

        private async void NewJourneyButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowJourneyList(), true);
        }

        //CheckIn
        private async void AddCheckpointButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CheckInSelector(), true);
        }

        //Transport
        private async void AddBillButton_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("TransistTime") && Application.Current.Properties["TransistTime"].ToString() != String.Empty)
            {
                await Navigation.PushAsync(new AddCheckpoint(), true);
            }
            else
            {
                await Navigation.PushAsync(new StartTransport(), true);
            }
        }

        private async void EndJourneyButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EndJourney(), true);
        }

        private async void LogOutButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["Username"] = string.Empty;
            Application.Current.Properties["JID"] = string.Empty;
            Application.Current.Properties["CINTime"] = string.Empty;
            Application.Current.Properties["CINPlace"] = string.Empty;
            Application.Current.Properties["TransistTime"] = String.Empty;
            Application.Current.Properties["TransistUser"] = String.Empty;
            Application.Current.Properties["TransistPlace"] = String.Empty;
            Application.Current.Properties["TransistType"] = String.Empty;
            Application.Current.Properties["PWDChange"] = String.Empty;
            await Application.Current.SavePropertiesAsync();
            await Navigation.PopToRootAsync();
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void AccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountDetails(), true);
        }

        private async void TourButton_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Getting Tours Info";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.GetAllTours;
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()),
                        new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()) });
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
                                        var AllTours = JsonConvert.DeserializeObject<List<Tour>>(myContent);
                                        AllTours.Reverse();
                                        await Navigation.PushAsync(new TourSelector(AllTours), true);
                                        MainFrame.IsVisible = true;
                                        LoadingOverlay.IsVisible = false;
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