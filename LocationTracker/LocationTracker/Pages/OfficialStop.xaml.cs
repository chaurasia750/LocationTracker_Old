using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
	public partial class OfficialStop : ContentPage
	{
        public byte[] upfilebytes { get; set; }
        public string ImageUrl { get; set; }
        public OfficialStop ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            upfilebytes = null;
        }

        private async void OfficialStopButton_Clicked(object sender, EventArgs e)
        {
            var plac = String.Empty;
            if(!String.IsNullOrEmpty(PlaceEntry.Text))
            {
                plac = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PlaceEntry.Text);
            }
            else
            {
                await DisplayAlert("Error", "Enter purpose of stop!", "Ok");
                return;
            }
            if (upfilebytes == null)
            {
                await DisplayAlert("Error", "Click Selfie!", "Ok");
                return;
            }
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Adding Official Stop";
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

                                    using (var httpClient = new HttpClient())
                                    {
                                        try
                                        {
                                            var fname = $"{Guid.NewGuid().ToString()}.jpg";
                                            string IUrl = DifferentUrls.UploadBillUrl;
                                            MultipartFormDataContent content = new MultipartFormDataContent();
                                            ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                                            content.Add(baContent, "file", fname);
                                            var response = await httpClient.PostAsync(IUrl, content);
                                            if (!response.IsSuccessStatusCode)
                                            {
                                                Device.BeginInvokeOnMainThread(async () =>
                                                {
                                                    var Message = "Server Is Down. Try Again After Some Time";
                                                    await DisplayAlert("Error", Message, "OK");
                                                    MainFrame.IsVisible = true;
                                                    LoadingOverlay.IsVisible = false;
                                                    return;
                                                });
                                                return;
                                            }
                                            var responsestr = response.Content.ReadAsStringAsync().Result;
                                            if (responsestr == "False")
                                            {
                                                Device.BeginInvokeOnMainThread(async () =>
                                                {
                                                    var Message = "Unable to upload Selfie. Try After Some time!!!";
                                                    await DisplayAlert("Error", Message, "OK");
                                                    MainFrame.IsVisible = true;
                                                    LoadingOverlay.IsVisible = false;
                                                    return;
                                                });
                                                return;
                                            }
                                            else
                                            {
                                                ImageUrl = responsestr;
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
                                            return;
                                        }
                                    }

                                    string url = DifferentUrls.AddStopUrl;
                                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("Addresss", PlaceDetails), new KeyValuePair<string, string>("Timestamp", statime), new KeyValuePair<string, string>("JID", Application.Current.Properties["JID"].ToString()), new KeyValuePair<string, string>("UserPlace", plac), new KeyValuePair<string, string>("BillLink", ImageUrl) });
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
                                                        var Message = $"Added Official  Stop at {statime}!!";
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

        private async void ClickPicButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported )
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Bills",
                SaveToAlbum = false,
                CompressionQuality = 30,
                CustomPhotoSize = 30,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 600,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return;

            using (MemoryStream ms = new MemoryStream())
            {
                var stream = file.GetStream();
                stream.CopyTo(ms);
                upfilebytes = ms.ToArray();
            }

            imageView.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            ClickPicButton.IsVisible = false;
        }
    }
}