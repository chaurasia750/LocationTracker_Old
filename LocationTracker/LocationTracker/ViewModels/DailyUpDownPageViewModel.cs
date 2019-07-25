using LocationTracker.Models;
using LocationTracker.Views.PopupPage;
using MvvmHelpers;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocationTracker.ViewModels
{
    public class DailyUpDownPageViewModel : ViewModel
    {
        Tour activeTour;
        public DailyUpDownPageViewModel()
        {

        }


        public Tour ActiveTour
        {
            get => activeTour;
            set
            {
                if (value != null)
                {
                    GenerateDilyButton(value);
                    OnPropertyChanged("TourDays");
                }
            }
        }
        public ObservableRangeCollection<TourDays> TourDays { get; set; }
        public Command DaysCommand {
            get
            {
                return new Command<TourDays>(async(tourDays) =>
                {
                    await DisplayAlert("Alert!", "Day ");
                });
            }
        }
        public Command RemakrsCommand
        {
            get
            {
                return new Command<TourDays>(async (tourDays) =>
                {
                    await OpenPopUpPage(new RemarksPopupPage());
                });
            }
        }
        public TourDays SelectedDay { get; set; }

        private void GenerateDilyButton(Tour activeTour)
        {
            var list = new List<TourDays>();
            var days = (int)(activeTour.OwnwardEndDate - activeTour.OwnwardStartDate).TotalDays;
            var startDate = new DateTime(activeTour.OwnwardStartDate.Year, activeTour.OwnwardStartDate.Month, activeTour.OwnwardStartDate.Day);
            for (int i = 0; i <= days; i++)
            {
                int dayId = i + 1;
                list.Add(new TourDays
                {
                    Text = "Day " + dayId,
                    Day = dayId,
                    GUID = activeTour.ID.ToString(),
                    IsActive = startDate.AddDays(i)==DateTime.Today
                });
            }

            list.ForEach(x => x.DaysCommand = DaysCommand);
            list.ForEach(x => x.RemakrsCommand = RemakrsCommand);
            TourDays = new ObservableRangeCollection<TourDays>(list);

        }
    }
}
