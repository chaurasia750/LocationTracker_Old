using LocationTracker.ServiceModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LocationTracker.ViewModels
{
    public class RemarksPopupPageViewModel:ViewModel
    {
        public RemarksModel RemarksModel { get; set; }

        public Command SaveRemarksCommand
        {
            get
            {
                return new Command(() => {

                });
            }
            
        }
    }
}
