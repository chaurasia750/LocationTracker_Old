using LocationTracker.Models;
using LocationTracker.ViewModels;
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
    public partial class ConfirmTour : ContentPage
    {
        public Tour MyTour { get; set; }

        public ConfirmTour(Tour cTour)
        {
            InitializeComponent();
            MyTour = cTour;
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            BindingContext =new  ConfirmTourViewModel(cTour);
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Uploading Tour Program";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.UploadTour;
                    
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()),
                        new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()),
                        new KeyValuePair<string, string>("TourInfo", JsonConvert.SerializeObject(MyTour)) });
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
                                        var Message = "Tour Program uploaded successfully!";
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
                                    MyTour.Status = String.Empty;
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
                                MyTour.Status = String.Empty;
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
                MyTour.Status = String.Empty;
                LoadingOverlay.IsVisible = false;
                MainFrame.IsVisible = true;
                await DisplayAlert("Error", "No Internet Connection", "OK");
                return;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Saving Tour Program";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.UploadTour;
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()),
                        new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()),
                        new KeyValuePair<string, string>("TourInfo", JsonConvert.SerializeObject(MyTour)) });
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
                                        var Message = "Tour Program saved successfully!";
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
                                    MyTour.Status = String.Empty;
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
                                MyTour.Status = String.Empty;
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
                MyTour.Status = String.Empty;
                LoadingOverlay.IsVisible = false;
                MainFrame.IsVisible = true;
                await DisplayAlert("Error", "No Internet Connection", "OK");
                return;
            }
        }
    }
}