using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace LocationTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignIn(), true);
        }

        //private async void LocationB_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var request = new GeolocationRequest(GeolocationAccuracy.Best);
        //        var location = await Geolocation.GetLocationAsync();

        //        if (location != null)
        //        {
        //            // = ($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        //            try
        //            {
        //                var lat = location.Latitude;
        //                var lon = location.Longitude;

        //                var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);

        //                var placemark = placemarks?.FirstOrDefault();
        //                if (placemark != null)
        //                {
        //                    var geocodeAddress =
        //                        $"AdminArea:       {placemark.AdminArea}\n" +
        //                        $"CountryCode:     {placemark.CountryCode}\n" +
        //                        $"CountryName:     {placemark.CountryName}\n" +
        //                        $"FeatureName:     {placemark.FeatureName}\n" +
        //                        $"Locality:        {placemark.Locality}\n" +
        //                        $"PostalCode:      {placemark.PostalCode}\n" +
        //                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
        //                        $"SubLocality:     {placemark.SubLocality}\n" +
        //                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
        //                        $"Thoroughfare:    {placemark.Thoroughfare}\n";

        //                    output.Text = (geocodeAddress);
        //                }
        //            }
        //            catch (FeatureNotSupportedException fnsEx)
        //            {
        //                // Feature not supported on device
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle exception that may have occurred in geocoding
        //            }
        //        }
        //    }
        //    catch (FeatureNotSupportedException fnsEx)
        //    {
        //        // Handle not supported on device exception
        //    }
        //    catch (FeatureNotEnabledException fneEx)
        //    {
        //        // Handle not enabled on device exception
        //    }
        //    catch (PermissionException pEx)
        //    {
        //        // Handle permission exception
        //    }
        //    catch (Exception ex)
        //    {
        //        // Unable to get location
        //    }
        //}
    }
}
