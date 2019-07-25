using LocationTracker.Models;
using LocationTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyUpDownPage : ContentPage
    {
        public DailyUpDownPage(Tour selectedTour)
        {
            InitializeComponent();
            var vm = BindingContext as DailyUpDownPageViewModel;
            vm.ActiveTour = selectedTour;
        }
    }
}