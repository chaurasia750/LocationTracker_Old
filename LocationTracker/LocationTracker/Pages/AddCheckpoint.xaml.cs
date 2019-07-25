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
	public partial class AddCheckpoint : ContentPage
	{
        public double Amount { get; set; }
        public double Distance { get; set; }

        public AddCheckpoint ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            Amount = 0.0;
            Distance = 0.0;
        }

        private async void AmountEntry_Completed(object sender, EventArgs e)
        {
            var amtEntry = (Entry)sender;
            if(String.IsNullOrEmpty(amtEntry.Text) || amtEntry.Text=="")
            {
                amtEntry.Text = String.Empty;
                amtEntry.Placeholder = "0.0";
                Amount = 0.0;
                return;
            }
            else
            {
                if (double.TryParse(amtEntry.Text, out double temp) && temp > 0)
                {
                    Amount = temp;
                    return;
                }
                else
                {
                    amtEntry.Text = String.Empty;
                    amtEntry.Placeholder = "0.0";
                    Amount = 0.0;
                    await DisplayAlert("Error", "Enter valid amount or no amount!", "Ok");
                    return;
                }
            }
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            var amtEntry = AmountEntry;
            
            if (String.IsNullOrEmpty(amtEntry.Text) || amtEntry.Text == "")
            {
                amtEntry.Text = String.Empty;
                amtEntry.Placeholder = "0.0";
                Amount = 0.0;
            }
            else
            {
                if (double.TryParse(amtEntry.Text, out double temp) && temp > 0)
                {
                    Amount = temp;
                }
                else
                {
                    amtEntry.Text = String.Empty;
                    amtEntry.Placeholder = "0.0";
                    Amount = 0.0;
                    await DisplayAlert("Error", "Enter valid amount or no amount!", "Ok");
                    return;
                }
            }
            if (String.IsNullOrEmpty(DistanceEntry.Text) || DistanceEntry.Text == "")
            {
                DistanceEntry.Text = String.Empty;
                DistanceEntry.Placeholder = "0.0";
                Distance = 0.0;
            }
            else
            {
                if (double.TryParse(DistanceEntry.Text, out double temp) && temp > 0)
                {
                    Distance = temp;
                }
                else
                {
                    DistanceEntry.Text = String.Empty;
                    DistanceEntry.Placeholder = "0.0";
                    Distance = 0.0;
                    await DisplayAlert("Error", "Enter valid distance or no distance!", "Ok");
                    return;
                }
            }
            var plac = String.Empty;
            if(!String.IsNullOrEmpty(PlaceEntry.Text))
            {
                plac = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PlaceEntry.Text);
            }
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Ending Transistion";
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
                                var Lati = lat;
                                var Longi = lon;
                                var PlaceDetails = "";
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
                                var statime = $"{DateTime.Now.ToLongDateString()}  {String.Format("{0:hh:mm tt}", DateTime.Now)}";
                                await Task.Run(async () =>
                                {
                                    string url = DifferentUrls.AddTransportUrl;
                                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("Timestamp", Application.Current.Properties["TransistTime"].ToString()), new KeyValuePair<string, string>("TimestampOut", statime), new KeyValuePair<string, string>("JID", Application.Current.Properties["JID"].ToString()), new KeyValuePair<string, string>("TransType", Application.Current.Properties["TransistType"].ToString()), new KeyValuePair<string, string>("TransFrom", Application.Current.Properties["TransistPlace"].ToString()), new KeyValuePair<string, string>("TransTo", PlaceDetails), new KeyValuePair<string, string>("TransAmt", Amount.ToString()), new KeyValuePair<string, string>("TransDist", Distance.ToString()), new KeyValuePair<string, string>("UserFrom", Application.Current.Properties["TransistUser"].ToString()), new KeyValuePair<string, string>("UserTo", plac) });
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
                                                        Application.Current.Properties["TransistTime"] = String.Empty;
                                                        Application.Current.Properties["TransistUser"] = String.Empty;
                                                        Application.Current.Properties["TransistPlace"] = String.Empty;
                                                        Application.Current.Properties["TransistType"] = String.Empty;
                                                        await Application.Current.SavePropertiesAsync();
                                                        var Message = $"Ended Transistion at {statime}!!";
                                                        await DisplayAlert("Success", Message, "OK");
                                                        MainFrame.IsVisible = true;
                                                        LoadingOverlay.IsVisible = false;
                                                        await Navigation.PopAsync();
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

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void DistanceEntry_Completed(object sender, EventArgs e)
        {
            var amtEntry = (Entry)sender;
            if (String.IsNullOrEmpty(amtEntry.Text) || amtEntry.Text == "")
            {
                amtEntry.Text = String.Empty;
                amtEntry.Placeholder = "0.0";
                Distance = 0.0;
                return;
            }
            else
            {
                if (double.TryParse(amtEntry.Text, out double temp) && temp > 0)
                {
                    Distance = temp;
                    return;
                }
                else
                {
                    amtEntry.Text = String.Empty;
                    amtEntry.Placeholder = "0.0";
                    Distance = 0.0;
                    await DisplayAlert("Error", "Enter valid distance or no distance!", "Ok");
                    return;
                }
            }
        }
    }
}