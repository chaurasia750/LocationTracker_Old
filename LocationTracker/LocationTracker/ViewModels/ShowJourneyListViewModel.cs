using LocationTracker.ApiServices;
using LocationTracker.Enum;
using LocationTracker.Models;
using LocationTracker.Pages;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocationTracker.ViewModels
{
    public class ShowJourneyListViewModel : ViewModel
    {
        private TourService tourService;
        private ObservableRangeCollection<Tour> _tourList;
        private string errorMessage;
        public ShowJourneyListViewModel()
        {
            tourService = new TourService();
            
            Task.Run(async() =>
            {
                IsBusy = true;
                await GetAllActiveTours();
                IsBusy = false;
            });
        }
        public string Message
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }
        public ObservableRangeCollection<Tour> TourList
        {
            get => _tourList;
            set
            {
                SetProperty(ref _tourList, value);
                OnPropertyChanged();
            }
        }

        Tour activeTour;
        public Tour ActiveTour {
            get => activeTour;
            set
            {
                if (value!=null)
                {
                    Device.BeginInvokeOnMainThread(async() => { await PushModalAsync(new StartJourneryType(value)); });
                }
            }


        }
        
        private async Task GetAllActiveTours()
        {

            try
            {
                var tours = await tourService.GetAllTours(ReportType.Date);
                if (tours.Item1)
                {
                    TourList = new ObservableRangeCollection<Tour>(tours.Item3);
                }
                else
                {
                    Message = tours.Item2;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

    }
}
