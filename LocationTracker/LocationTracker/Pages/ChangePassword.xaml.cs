using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePassword : ContentPage
	{
		public ChangePassword ()
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            if (Application.Current.Properties["PWDChange"].ToString() == "Yes")
            {
                //hideLabel.IsVisible = true;
                WarningLabel.IsVisible = true;
                LogOutBtn.IsVisible = true;
            }
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            var oldPassword = OldPasswordEntry.Text;
            var newPassword = NewPasswordEntry.Text;
            var cnfPassword = ConfirmPasswordEntry.Text;
            var regexItem = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$");
            if (String.IsNullOrEmpty(oldPassword) || oldPassword.Length < 6 || oldPassword.Length > 15)
            {
                var ErrorMessage = "Old password should contain at least six characters and at max fifteen characters";
                await DisplayAlert("Error", ErrorMessage, "Ok");
                return;
            }
            if (String.IsNullOrEmpty(newPassword) || (!regexItem.IsMatch(newPassword)))
            {
                NewPasswordEntry.Text = String.Empty;
                ConfirmPasswordEntry.Text = String.Empty;
                var ErrorMessage = ("New password must contain at least six characters, at max fifteen characters, at least one digit, atleast one special character, atleast one lowercase letter and atleast one uppercase letter");
                await DisplayAlert("Error", ErrorMessage, "Ok");
                return;
            }
            if(newPassword.Contains("#"))
            {
                NewPasswordEntry.Text = String.Empty;
                ConfirmPasswordEntry.Text = String.Empty;
                var ErrorMessage = ("New password must not contain #");
                await DisplayAlert("Error", ErrorMessage, "Ok");
                return;
            }
            if (String.IsNullOrEmpty(cnfPassword) || (!regexItem.IsMatch(cnfPassword)))
            {
                ConfirmPasswordEntry.Text = String.Empty;
                var ErrorMessage = ("New password must contain at least six characters, at max fifteen characters, at least one digit, atleast one special character, atleast one lowercase letter and atleast one uppercase letter");
                await DisplayAlert("Error", ErrorMessage, "Ok");
                return;
            }
            if (cnfPassword != newPassword)
            {
                NewPasswordEntry.Text = String.Empty;
                ConfirmPasswordEntry.Text = String.Empty;
                var ErrorMessage = ("Passwords do not match");
                await DisplayAlert("Error", ErrorMessage, "Ok");
                return;
            }

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Changing Password";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.ChangePasswordUrl;
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("OldPassword", oldPassword), new KeyValuePair<string, string>("NewPassword", newPassword) });
                    using (var httpClient = new HttpClient())
                    {
                        try
                        {
                            Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, q1);
                            HttpResponseMessage response = await getResponse;
                            if (response.IsSuccessStatusCode)
                            {
                                var myContent = await response.Content.ReadAsStringAsync();
                                if (myContent == "Username")
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
                                else if (myContent == "Password")
                                {
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        var Message = $"Old Password was wrong. Try Again!!";
                                        await DisplayAlert("Failed", Message, "OK");
                                        MainFrame.IsVisible = true;
                                        LoadingOverlay.IsVisible = false;
                                        OldPasswordEntry.Text = String.Empty;
                                        NewPasswordEntry.Text = String.Empty;
                                        ConfirmPasswordEntry.Text = String.Empty;
                                        return;
                                    });
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        var Message = "Password Changed Successfully!!!";
                                        await DisplayAlert("Success", Message, "OK");
                                        Message = "You need to Sign In Again!";
                                        await DisplayAlert("Info", Message, "OK");
                                        MainFrame.IsVisible = true;
                                        LoadingOverlay.IsVisible = false;
                                        Application.Current.Properties["Username"] = string.Empty;
                                        Application.Current.Properties["PWDChange"] = String.Empty;
                                        await Application.Current.SavePropertiesAsync();
                                        await Navigation.PopToRootAsync();
                                        Application.Current.MainPage = new NavigationPage(new MainPage());
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

        private async void LogOutBtn_Clicked(object sender, EventArgs e)
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
    }
}