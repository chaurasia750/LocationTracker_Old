using LocationTracker.Models;
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
	public partial class TourSelector : ContentPage
	{
        public List<Tour> AllTours { get; set; }
        public TourSelector (List<Tour> cTours)
		{
			InitializeComponent ();
            AbsoluteLayout.SetLayoutFlags(LoadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(LoadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AllTours = cTours;
        }

        private async void NewTourButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTour(), true);
        }

        private async void EditTourButton_Clicked(object sender, EventArgs e)
        {
            var editTours = new List<Tour>();
            foreach (var tt in AllTours)
            {
                if(String.IsNullOrEmpty(tt.Status))
                {
                    editTours.Add(tt);
                }
            }
            await Navigation.PushAsync(new EditAllTours(editTours), true);
        }

        private async void PendingTourButton_Clicked(object sender, EventArgs e)
        {
            var editTours = new List<Tour>();
            foreach (var tt in AllTours)
            {
                if (tt.Status== "Pending")
                {
                    editTours.Add(tt);
                }
            }
            await Navigation.PushAsync(new PendingAllTours(editTours), true);
        }

        private async void RejectedTourButton_Clicked(object sender, EventArgs e)
        {
            var editTours = new List<Tour>();
            foreach (var tt in AllTours)
            {
                if (tt.Status == "Rejected")
                {
                    editTours.Add(tt);
                }
            }
            await Navigation.PushAsync(new RejectedAllTours(editTours), true);
        }

        private async void ApprovedTourButton_Clicked(object sender, EventArgs e)
        {
            var editTours = new List<Tour>();
            foreach (var tt in AllTours)
            {
                if (tt.Status == "Approved by Admin/R.O." || tt.Status == "Approved by Supervisor")
                {
                    editTours.Add(tt);
                }
            }
            await Navigation.PushAsync(new ApprovedAllTours(editTours), true);
        }
    }
}