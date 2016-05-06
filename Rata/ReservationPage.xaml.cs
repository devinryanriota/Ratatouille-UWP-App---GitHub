using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.IO;
using System.Text;
using Windows.Storage.Streams;
using MySql.Data.MySqlClient;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rata
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReservationPage : Page
    {
        public ReservationPage()
        {
            this.InitializeComponent();
            txtLogin.Text = MainPage.firstName;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoginListBoxItem.IsSelected)
            {
                this.Frame.Navigate(typeof(StatusPage));
            }
            else if (HomeListBoxItem.IsSelected)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else if(DeliveryListBoxItem.IsSelected)
            {
                this.Frame.Navigate(typeof(DeliveryPage));
            }
            else if (LogoutListBoxItem.IsSelected)
            {
                MessageDialog dialog = new MessageDialog("Logged Out !");
                await dialog.ShowAsync();
                this.Frame.Navigate(typeof(LoginPage));
            }
        }

        MySqlCommand comm = null;
        StringBuilder stringBuilder = null;
        IDataReader iDataReader = null;

        String connString = @"Server=ap-cdbr-azure-southeast-b.cloudapp.net;Database=ratatouille;Uid=b137f1b83a0315;Pwd=e179ee0c;SslMode=None;charset=utf8";

        String resName = "";
        String resPhone = "";
        String resDate = "";
        String resGuest = "";
        String resTime = "";
        String resDuration = "";
        int userID = LoginPage.useridDB;

        private async void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            //validation


            resName = txtName.Text;
            resPhone = txtPhone.Text;
            resDate = dtpResDate.Date.ToString("yyyy-MM-dd");
            resGuest = txtGuest.Text;
            resTime = tpResTime.Time.Hours + ":" + tpResTime.Time.Minutes + ":" + tpResTime.Time.Seconds;
            resDuration = txtDuration.Text;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "INSERT INTO reservation(UserID, ReservationName, ReservationPhone, ReservationDate, ReservationGuest, ReservationTime, ReservationDuration, ReservationStatus) VALUES("+ userID +", '"+ resName +"', '" + resPhone +"', '"+ resDate +"', '"+ resGuest +"', '"+ resTime +"', '"+ resDuration +"', 'Waiting')";

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding.GetEncoding("windows-1252");

                    getCommand.ExecuteNonQuery();

                    MessageDialog dialog = new MessageDialog("Reservation Successful!");
                    await dialog.ShowAsync();
                    this.Frame.Navigate(typeof(MainPage));
                    
                }
            }
            catch (MySqlException ex)
            {
                MessageDialog dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }

        }
    }
}
