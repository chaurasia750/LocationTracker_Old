using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocationTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountDetails : ContentPage
	{
		public AccountDetails ()
		{
			InitializeComponent ();
            UsernameLabel.Text = UsernameLabel.Text + Application.Current.Properties["Username"].ToString();
            FullNameLabel.Text = FullNameLabel.Text + Application.Current.Properties["FullName"].ToString();
            DesignationLabel.Text = DesignationLabel.Text + Application.Current.Properties["Designation"].ToString();
            if(Application.Current.Properties["ECode"] != null)
            {
                ECodeLabel.Text = ECodeLabel.Text + Application.Current.Properties["ECode"].ToString();
            }
            if(Application.Current.Properties["Section"] != null)
            {
                SectionLabel.Text = SectionLabel.Text + Application.Current.Properties["Section"].ToString();
            }
            OfficeLabel.Text = OfficeLabel.Text + Application.Current.Properties["FullDepartmentInfo"].ToString();
            MobileLabel.Text = MobileLabel.Text + Application.Current.Properties["MobileNumber"].ToString();
        }

        private async void PasswordButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePassword(), true);
        }
    }
}