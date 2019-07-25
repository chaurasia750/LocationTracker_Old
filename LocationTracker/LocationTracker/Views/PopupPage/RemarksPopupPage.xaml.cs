using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker.Views.PopupPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RemarksPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public RemarksPopupPage()
        {
            InitializeComponent();
        }
    }
}