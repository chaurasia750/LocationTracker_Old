using LocationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
	public partial class EditTour : ContentPage
	{
        public Tour MyTour { get; set; }

        public EditTour (Tour cTour)
		{
            InitializeComponent();
            MyTour = cTour;
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            QuotEntry.Text = MyTour.SampleNumber;
            VillageEntry.Text = MyTour.SampleVillage;
            PopulationEntry.Text = MyTour.Population;
            SchemeEntry.Text = MyTour.Scheme;
            TehsilEntry.Text = MyTour.Tehsil;
            DistrictEntry.Text = MyTour.District;
            //ownward
            startDatePicker.Date = JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldStartDate);
            startTimePicker.Time = JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldStartTime).TimeOfDay;
            endDatePicker.Date = JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldEndDate);
            endTimePicker.Time = JsonConvert.DeserializeObject<DateTime>(MyTour.OnwardFieldEndTime).TimeOfDay;
            journeyFromEntry.Text = cTour.OnwardJourneyFrom;
            journeyToEntry.Text = cTour.OnwardJourneyTo;
            modeOfJourneyEntry.Text = cTour.OnwardModeOfJourney;
            distanceEntry.Text = cTour.OnwardDistance;
            fareEntry.Text = cTour.OnwardFare;

            backWardJourneyFromEntry.Text = cTour.BackwardJourneyFrom;
            backWardJourneyToEntry.Text = cTour.BackwardJourneyTo;
            backWardModeOfJourneyEntry.Text = cTour.BackwardModeOfJourney;
            backWardDistanceEntry.Text = cTour.BackwardDistance;
            backWardFareEntry.Text = cTour.BackwardFare;
            backWardStartDatePicker.Date = JsonConvert.DeserializeObject<DateTime>(MyTour.BackwardFieldStartDate);
            backWardStartTimePicker.Time = JsonConvert.DeserializeObject<DateTime>(MyTour.BackwardFieldStartTime).TimeOfDay;
            backWardEndDatePicker.Date = JsonConvert.DeserializeObject<DateTime>(MyTour.BackwardFieldEndDate);
            backWardEndTimePicker.Time = JsonConvert.DeserializeObject<DateTime>(MyTour.BackwardFieldEndTime).TimeOfDay;

            JSOEntry.Text = MyTour.JSOFIName;
            ssoEntry.Text = MyTour.InspectionDak;
            campAddressEntry.Text = MyTour.CampDetails;
            RemarksEntry.Text = MyTour.Remark;
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {

            Tour newTour = new Tour()
            {
                SampleNumber = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(QuotEntry.Text),
                SampleVillage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(VillageEntry.Text),
                Population = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PopulationEntry.Text),
                Scheme = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SchemeEntry.Text),
                Tehsil = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TehsilEntry.Text),
                District = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DistrictEntry.Text),

                OnwardFieldStartDate = JsonConvert.SerializeObject(startDatePicker.Date),
                OnwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + startTimePicker.Time)),
                OnwardFieldEndDate = JsonConvert.SerializeObject((endDatePicker.Date)),
                OnwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + endTimePicker.Time)),
                OnwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(journeyFromEntry.Text),
                OnwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(journeyToEntry.Text),
                OnwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modeOfJourneyEntry.Text),
                OnwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(distanceEntry.Text),
                OnwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fareEntry.Text),


                //BackWard
                BackwardFieldStartDate = JsonConvert.SerializeObject(backWardStartDatePicker.Date),
                BackwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + backWardStartTimePicker.Time)),

                BackwardFieldEndDate = JsonConvert.SerializeObject(backWardEndDatePicker.Date),
                BackwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + backWardEndTimePicker.Time)),



                BackwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardJourneyFromEntry.Text),
                BackwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardJourneyToEntry.Text),
                BackwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardModeOfJourneyEntry.Text),
                BackwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardDistanceEntry.Text),
                BackwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardFareEntry.Text),


                CampDetails = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(campAddressEntry.Text),
                JSOFIName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(JSOEntry.Text),
                InspectionDak = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ssoEntry.Text),
                Remark = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(RemarksEntry.Text),
                ROSRO = Application.Current.Properties["Section"].ToString(),
                TourOwnerType = Application.Current.Properties["Designation"].ToString(),
                Status = "",
                EditableBy = ""
            };
            
            if (Application.Current.Properties["Designation"].ToString() == "SSO")
            {
                newTour.InspectionDak = String.Empty;
            }
            else
            {
                newTour.InspectionDak = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ssoEntry.Text);
            }

            newTour.ROSRO = Application.Current.Properties["Section"].ToString();
            newTour.TourOwnerType = Application.Current.Properties["Designation"].ToString();
            newTour.Status = String.Empty;
            newTour.ID = MyTour.ID;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Uploading Tour Program";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.EditTour;
                    newTour.Status = "Pending";
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("TourInfo", JsonConvert.SerializeObject(newTour)) });
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

            Tour newTour = new Tour()
            {
                SampleNumber = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(QuotEntry.Text),
                SampleVillage = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(VillageEntry.Text),
                Population = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PopulationEntry.Text),
                Scheme = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SchemeEntry.Text),
                Tehsil = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TehsilEntry.Text),
                District = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DistrictEntry.Text),

                OnwardFieldStartDate = JsonConvert.SerializeObject(startDatePicker.Date),
                OnwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + startTimePicker.Time)),
                OnwardFieldEndDate = JsonConvert.SerializeObject((endDatePicker.Date)),
                OnwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + endTimePicker.Time)),
                OnwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(journeyFromEntry.Text),
                OnwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(journeyToEntry.Text),
                OnwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modeOfJourneyEntry.Text),
                OnwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(distanceEntry.Text),
                OnwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fareEntry.Text),


                //BackWard
                BackwardFieldStartDate = JsonConvert.SerializeObject(backWardStartDatePicker.Date),
                BackwardFieldStartTime = JsonConvert.SerializeObject((DateTime.Today + backWardStartTimePicker.Time)),

                BackwardFieldEndDate = JsonConvert.SerializeObject(backWardEndDatePicker.Date),
                BackwardFieldEndTime = JsonConvert.SerializeObject((DateTime.Today + backWardEndTimePicker.Time)),



                BackwardJourneyFrom = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardJourneyFromEntry.Text),
                BackwardJourneyTo = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardJourneyToEntry.Text),
                BackwardModeOfJourney = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardModeOfJourneyEntry.Text),
                BackwardDistance = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardDistanceEntry.Text),
                BackwardFare = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(backWardFareEntry.Text),


                CampDetails = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(campAddressEntry.Text),
                JSOFIName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(JSOEntry.Text),
                InspectionDak = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ssoEntry.Text),
                Remark = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(RemarksEntry.Text),
                ROSRO = Application.Current.Properties["Section"].ToString(),
                TourOwnerType = Application.Current.Properties["Designation"].ToString(),
                Status = "",
                EditableBy = ""
            };

            if (Application.Current.Properties["Designation"].ToString() == "SSO")
            {
                newTour.InspectionDak = String.Empty;
            }
            else
            {
                newTour.InspectionDak = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ssoEntry.Text);
            }

            newTour.ROSRO = Application.Current.Properties["Section"].ToString();
            newTour.TourOwnerType = Application.Current.Properties["Designation"].ToString();
            newTour.Status = String.Empty;
            newTour.ID = MyTour.ID;


            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                MainFrame.IsVisible = false;
                LoadingOverlay.IsVisible = true;
                LoadingIndicatorText.Text = "Saving Tour Program";
                await Task.Run(async () =>
                {
                    string url = DifferentUrls.EditTour;
                    HttpContent q1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()), new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString()), new KeyValuePair<string, string>("TourInfo", JsonConvert.SerializeObject(newTour)) });
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