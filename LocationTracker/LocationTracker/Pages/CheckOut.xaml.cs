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
    public partial class CheckOut : ContentPage
    {
        public double Amount { get; set; }
        public byte[] upfilebytes { get; set; }
        public string ImageUrl { get; set; }

        public CheckOut()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            upfilebytes = null;
        }

        private async void AmountEntry_Completed(object sender, EventArgs e)
        {
            var amtEntry = (Entry)sender;
            if (String.IsNullOrEmpty(amtEntry.Text) || amtEntry.Text == "")
            {
                amtEntry.Text = String.Empty;
                amtEntry.Placeholder = "0.0";
                Amount = 0.0;
                return;
            }
            if (double.TryParse(amtEntry.Text, out double temp) && temp > 1)
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

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(HotelNameEntry.Text))
            {
                await DisplayAlert("Error", "Enter Hotel Name!", "Ok");
                return;
            }
            var HotelName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(HotelNameEntry.Text);
            var Registrat = String.Empty;
            var Contact = String.Empty;
            var Remar = String.Empty;

            if (!String.IsNullOrEmpty(GSTEntry.Text))
            {
                Registrat = GSTEntry.Text;
            }
            if (String.IsNullOrEmpty(ContactEntry.Text))
            {
                await DisplayAlert("Error", "Enter Contact Number!", "Ok");
                return;
            }
            else
            {
                Contact = ContactEntry.Text;
            }
            if (!String.IsNullOrEmpty(RemarksEntry.Text))
            {
                Remar = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(RemarksEntry.Text);

            }
            var amtEntry = AmountEntry;
            if (String.IsNullOrEmpty(amtEntry.Text) || amtEntry.Text == "")
            {
                amtEntry.Text = String.Empty;
                amtEntry.Placeholder = "0.0";
                Amount = 0.0;
            }
            else
            {
                if (double.TryParse(amtEntry.Text, out double temp) && temp > 1)
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
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Checking Out";
                var ckoutPlace = String.Empty;
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
                                ckoutPlace = PlaceDetails;
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
                try
                {
                    var statime = $"{DateTime.Now.ToLongDateString()}  {String.Format("{0:hh:mm tt}", DateTime.Now)}";
                    await Task.Run(async () =>
                    {
                        if(upfilebytes != null)
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
                                            var Message = "Unable to upload Bill. Try After Some time!!!";
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
                        }
                        else
                        {
                            ImageUrl = String.Empty;
                        }
                        string url = DifferentUrls.AddHotelUrl;
                        HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("InLocation", Application.Current.Properties["CINPlace"].ToString()), new KeyValuePair<string, string>("OutLocation", ckoutPlace), new KeyValuePair<string, string>("Timestamp", Application.Current.Properties["CINTime"].ToString()), new KeyValuePair<string, string>("JID", Application.Current.Properties["JID"].ToString()), new KeyValuePair<string, string>("TimestampOut", statime), new KeyValuePair<string, string>("HotelName", HotelName), new KeyValuePair<string, string>("HotelGST", Registrat), new KeyValuePair<string, string>("HotelContact", Contact), new KeyValuePair<string, string>("HotelRemark", Remar), new KeyValuePair<string, string>("BillLink", ImageUrl), new KeyValuePair<string, string>("BillAmt", Amount.ToString()) });
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
                                            Application.Current.Properties["CINTime"] = string.Empty;
                                            Application.Current.Properties["CINPlace"] = string.Empty;
                                            await Application.Current.SavePropertiesAsync();
                                            var Message = $"Checked Out at {statime}!!";
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
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
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
                DefaultCamera = CameraDevice.Rear
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