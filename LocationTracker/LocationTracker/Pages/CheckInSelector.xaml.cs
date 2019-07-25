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
    public partial class CheckInSelector : ContentPage
    {
        public CheckInSelector()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            if (Application.Current.Properties["CINTime"].ToString() != string.Empty)
            {
                ChechInButton.IsVisible = false;
                checkinLabel.IsVisible = false;
            }
            else
            {
                CheckOutButton.IsVisible = false;
                CheckOutLabel.IsVisible = false;
            }
        }

        private async void ChechInButton_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Checking In";
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.GetLocationAsync(request);
                    if (location == null)
                    {
                        location = await Geolocation.GetLastKnownLocationAsync();
                    }
                    if (location != null)
                    {
                        if (location.IsFromMockProvider)
                        {
                            LoadingOverlay.IsVisible = false;
                            MainFrame.IsVisible = true;
                            await DisplayAlert("Error", "Your device is set to use mock location.Please disable it and try again!", "OK");
                            return;
                        }
                        try
                        {
                            var lat = location.Latitude;
                            var lon = location.Longitude;
                            var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);
                            var placemark = placemarks?.FirstOrDefault();
                            if (placemark != null)
                            {
                                var PlaceDetails = String.Empty;
                                if (!String.IsNullOrEmpty(placemark.SubThoroughfare))
                                {
                                    PlaceDetails = placemark.SubThoroughfare;
                                }
                                if (!String.IsNullOrEmpty(placemark.Thoroughfare))
                                {
                                    PlaceDetails = $"{PlaceDetails}, {placemark.Thoroughfare}";
                                }
                                if (!String.IsNullOrEmpty(placemark.SubLocality))
                                {
                                    PlaceDetails = $"{PlaceDetails}, {placemark.SubLocality}";
                                }
                                if (!String.IsNullOrEmpty(placemark.Locality))
                                {
                                    PlaceDetails = $"{PlaceDetails}, {placemark.Locality}";
                                }
                                if (!String.IsNullOrEmpty(placemark.SubAdminArea))
                                {
                                    PlaceDetails = $"{PlaceDetails}, {placemark.SubAdminArea}";
                                }
                                if (!String.IsNullOrEmpty(placemark.AdminArea))
                                {
                                    PlaceDetails = $"{PlaceDetails}, {placemark.AdminArea}";
                                }
                                if (!String.IsNullOrEmpty(PlaceDetails))
                                {
                                    if (PlaceDetails.Length > 0)
                                    {
                                        if (PlaceDetails[0] == ',')
                                        {
                                            PlaceDetails = PlaceDetails.Remove(0, 1);
                                        }
                                    }
                                }
                                await Task.Run(async () =>
                                {
                                    string url = DifferentUrls.GetDateTime;
                                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()) });
                                    using (var httpClient = new HttpClient())
                                    {
                                        try
                                        {
                                            Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                                            HttpResponseMessage response = await getResponse;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                var myContent = await response.Content.ReadAsStringAsync();
                                                Device.BeginInvokeOnMainThread(async () =>
                                                {

                                                    Application.Current.Properties["CINTime"] = myContent;
                                                    Application.Current.Properties["CINPlace"] = PlaceDetails;
                                                    await Application.Current.SavePropertiesAsync();
                                                    ChechInButton.IsVisible = false;
                                                    checkinLabel.IsVisible = false;
                                                    CheckOutButton.IsVisible = true;
                                                    CheckOutLabel.IsVisible = true;
                                                    LoadingOverlay.IsVisible = false;
                                                    MainFrame.IsVisible = true;
                                                    return;
                                                });
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
                                await DisplayAlert("Error", "Can not connect to location server. Try after some time!", "OK");
                                return;
                            }
                        }
                        catch (FeatureNotSupportedException)
                        {
                            LoadingOverlay.IsVisible = false;
                            MainFrame.IsVisible = true;
                            await DisplayAlert("Error", "Location feature not supported on your device!", "OK");
                            return;
                        }
                        catch (Exception)
                        {
                            LoadingOverlay.IsVisible = false;
                            MainFrame.IsVisible = true;
                            await DisplayAlert("Error", "Can not connect to location server. Try after some time!", "OK");
                            return;
                        }
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    LoadingOverlay.IsVisible = false;
                    MainFrame.IsVisible = true;
                    await DisplayAlert("Error", "Location feature not supported on your device!", "OK");
                    return;
                }
                catch (FeatureNotEnabledException)
                {
                    LoadingOverlay.IsVisible = false;
                    MainFrame.IsVisible = true;
                    await DisplayAlert("Error", "Enable location access on your device!", "OK");
                    return;
                }
                catch (PermissionException)
                {
                    LoadingOverlay.IsVisible = false;
                    MainFrame.IsVisible = true;
                    await DisplayAlert("Error", "Location permission not granted. Go to app setting and grant location permission!", "OK");
                    return;
                }
                catch (Exception)
                {
                    LoadingOverlay.IsVisible = false;
                    MainFrame.IsVisible = true;
                    await DisplayAlert("Error", "Can not connect to location server. Try after some time!", "OK");
                    return;
                }
            }
            else
            {
                LoadingOverlay.IsVisible = false;
                MainFrame.IsVisible = true;
                await DisplayAlert("Error", "No Internet Connection", "OK");
                return;
            }
        }

        private async void CheckOutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CheckOut(), true);
        }

        private async void AddOtherBillButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBill(), true);
        }

        private async void OfficialStopButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OfficialStop(), true);
        }
    }
}