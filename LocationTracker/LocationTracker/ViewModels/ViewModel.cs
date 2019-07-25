using MvvmHelpers;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocationTracker.ViewModels
{
    public class ViewModel : BaseViewModel
    {
        public async Task PushModalAsync(Page navPage)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(navPage);
        }

        public async Task DisplayAlert(string title,string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }

        public async Task OpenPopUpPage(PopupPage popupPage)
        {
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
