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

using Windows.Storage.Streams;
using MySql.Data.MySqlClient;
using Windows.Security.Cryptography;
using System.Text;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Rata
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        int userid = LoginPage.useridDB;
        public static String firstName = "Guest";
        String lastName = "";

        String connString = @"Server=ap-cdbr-azure-southeast-b.cloudapp.net;Database=ratatouille;Uid=b137f1b83a0315;Pwd=e179ee0c;SslMode=None;charset=utf8";

        public MainPage()
        {
            this.InitializeComponent();
            init();
            txtLogin.Text = firstName;
        }

        private async void init()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT firstname, lastname FROM user WHERE userid=" + userid;

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding.GetEncoding("windows-1252");

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            firstName = reader.GetString(0);
                            lastName = reader.GetString(1);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageDialog dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //userid = e.Parameter as string;
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

            }
            else if (ReservationListBoxItem.IsSelected)
            {
                this.Frame.Navigate(typeof(ReservationPage));
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
    }
}
