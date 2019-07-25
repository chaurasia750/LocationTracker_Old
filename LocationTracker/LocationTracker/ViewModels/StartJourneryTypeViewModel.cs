using LocationTracker.Enum;
using LocationTracker.Models;
using LocationTracker.Pages;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LocationTracker.ViewModels
{
    public class StartJourneryTypeViewModel : BaseViewModel
    {
        public StartJourneryTypeViewModel()
        {

        }
        public Tour SelectedTour { get; set; }

        public Command JourneyTypeCommand {
            get
            {
                return new Command((parameter) =>
                {
                    int type = Convert.ToInt32(parameter);
                    if (type==(int) JourneryStartType.DailyUpDown)
                    {
                        App.Current.MainPage.Navigation.PushModalAsync(new DailyUpDownPage(SelectedTour));
                    }
                    else if (type == (int)JourneryStartType.Camp)
                    {

                    }
                });
            }
        }
        
    }
}
