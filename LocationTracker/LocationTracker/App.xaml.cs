using LocationTracker.Pages;
using LocationTracker.TestingModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("Username"))
            {
                if (Application.Current.Properties["Username"].ToString() != string.Empty)
                {
                    if (Application.Current.Properties["PWDChange"].ToString() == "Yes")
                    {
                        Application.Current.MainPage = new NavigationPage(new ChangePassword());
                    }
                    else
                    {
                        MainPage = new NavigationPage(new HomePage());
                        //MainPage = new NavigationPage(new StartJourneryType());
                        // MainPage = new NavigationPage(new ConfirmTour(new TourTestingModel().MyTour));
                    }
                }
                else
                {
                    Application.Current.Properties["EID"] = string.Empty;
                    Application.Current.Properties["JID"] = string.Empty;
                    Application.Current.Properties["CINTime"] = string.Empty;
                    Application.Current.Properties["CINPlace"] = string.Empty;
                    Application.Current.Properties["Username"] = string.Empty;
                    Application.Current.Properties["Password"] = string.Empty;
                    Application.Current.Properties["FullName"] = string.Empty;
                    Application.Current.Properties["Designation"] = string.Empty;
                    Application.Current.Properties["ECode"] = string.Empty;
                    Application.Current.Properties["Section"] = string.Empty;
                    Application.Current.Properties["FullDepartmentInfo"] = string.Empty;
                    Application.Current.Properties["MobileNumber"] = string.Empty;
                    Application.Current.Properties["PWDChange"] = string.Empty;
                    Application.Current.Properties["TransistTime"] = string.Empty;
                    Application.Current.Properties["TransistUser"] = string.Empty;
                    Application.Current.Properties["TransistPlace"] = string.Empty;
                    Application.Current.Properties["TransistType"] = string.Empty;
                    Application.Current.SavePropertiesAsync();
                    MainPage = new NavigationPage(new MainPage());
                }
            }
            else
            {
                Application.Current.Properties["EID"] = string.Empty;
                Application.Current.Properties["JID"] = string.Empty;
                Application.Current.Properties["CINTime"] = string.Empty;
                Application.Current.Properties["CINPlace"] = string.Empty;
                Application.Current.Properties["Username"] = string.Empty;
                Application.Current.Properties["Password"] = string.Empty;
                Application.Current.Properties["FullName"] = string.Empty;
                Application.Current.Properties["Designation"] = string.Empty;
                Application.Current.Properties["ECode"] = string.Empty;
                Application.Current.Properties["Section"] = string.Empty;
                Application.Current.Properties["FullDepartmentInfo"] = string.Empty;
                Application.Current.Properties["MobileNumber"] = string.Empty;
                Application.Current.Properties["PWDChange"] = string.Empty;
                Application.Current.Properties["TransistTime"] = string.Empty;
                Application.Current.Properties["TransistUser"] = string.Empty;
                Application.Current.Properties["TransistPlace"] = string.Empty;
                Application.Current.Properties["TransistType"] = string.Empty;
                Application.Current.SavePropertiesAsync();
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
