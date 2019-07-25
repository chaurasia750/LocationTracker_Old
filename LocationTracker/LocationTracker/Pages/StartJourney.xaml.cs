using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartJourney : ContentPage
    {
        public double Lati { get; set; }
        public double Longi { get; set; }
        public string PlaceDetails { get; set; }
        public string plac { get; set; } = String.Empty;

        public StartJourney()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            Lati = 0.0;
            Longi = 0.0;
            PlaceDetails = String.Empty;
            LatitudeLabel.IsVisible = false;
            LongitudeLabel.IsVisible = false;
            LocationDetailsLabel.IsVisible = false;
            ConfirmButton.IsVisible = false;
        }

        private async void GetLocationButton_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Getting Location";
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.High,TimeSpan.FromSeconds(10));
                    var location =await Geolocation.GetLocationAsync(request);
                    if(location==null)
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
                                //var geocodeAddress =
                                //    $"AdminArea:       {placemark.AdminArea}\n" +
                                //    $"CountryCode:     {placemark.CountryCode}\n" +
                                //    $"CountryName:     {placemark.CountryName}\n" +
                                //    $"FeatureName:     {placemark.FeatureName}\n" +
                                //    $"Locality:        {placemark.Locality}\n" +
                                //    $"PostalCode:      {placemark.PostalCode}\n" +
                                //    $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                                //    $"SubLocality:     {placemark.SubLocality}\n" +
                                //    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                                //    $"Thoroughfare:    {placemark.Thoroughfare}\n";

                                Lati = lat;
                                Longi = lon;
                                if(!String.IsNullOrEmpty(placemark.SubThoroughfare))
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
                                LatitudeLabel.IsVisible = true;
                                LongitudeLabel.IsVisible = true;
                                LocationDetailsLabel.IsVisible = true;
                                LatitudeLabel.Text = $"Current Location Latitute: {Lati}";
                                LongitudeLabel.Text = $"Current Location Longitude: {Longi}";
                                LocationDetailsLabel.Text = $"Current Location Details: {PlaceDetails}";
                                
                                GetLocationLabel.IsVisible = false;
                                GetLocationButton.IsVisible = false;
                                ConfirmButton.IsVisible = true;
                                LoadingOverlay.IsVisible = false;
                                MainFrame.IsVisible = true;
                                return;
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

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            MainFrame.IsVisible = false;
            LoadingOverlay.IsVisible = true;
            LoadingIndicatorText.Text = "Starting New Journey";
            var statime = $"{DateTime.Now.ToLongDateString()}  {String.Format("{0:hh:mm tt}", DateTime.Now)}";
            await Task.Run(async () =>
            {
                string url = DifferentUrls.StartJourneyUrl;
                HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("Addresss", PlaceDetails), new KeyValuePair<string, string>("Timestamp", statime), new KeyValuePair<string, string>("UserPlace", plac) });
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
                                    Application.Current.Properties["JID"] = String.Empty;
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
                                    Application.Current.Properties["JID"] = myContent;
                                    await Application.Current.SavePropertiesAsync();
                                    var Message = $"Started new journey at {statime}!!";
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

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(PlaceEntry.Text))
            {
                plac = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PlaceEntry.Text);
            }
            PlaceLabel.IsVisible = false;
            PlaceEntry.IsVisible = false;
            NextButton.IsVisible = false;
            GetLocationLabel.IsVisible = true;
            GetLocationButton.IsVisible = true;
        }
    }
}