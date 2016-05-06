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
using Windows.Security.Cryptography;

using Windows.UI.Popups;
using Windows.Security.Cryptography.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rata
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoginListBoxItem.IsSelected) ;
            else if (HomeListBoxItem.IsSelected) this.Frame.Navigate(typeof(MainGuestPage), useridDB);
        }

        MySqlCommand comm = null;
        StringBuilder stringBuilder = null;
        IDataReader iDataReader = null;

        public static int useridDB = 0;
        public static String usernameDB = "";

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            String username = txtUsername.Text;
            String password = passwordBox.Password;
            
            String saltDB = "";
            String passwordDB = "";


            String connString = @"Server=ap-cdbr-azure-southeast-b.cloudapp.net;Database=ratatouille;Uid=b137f1b83a0315;Pwd=e179ee0c;SslMode=None;charset=utf8";

            txtNotif.Text = password;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT userid, email, salt, password FROM user WHERE email='" + username + "'";

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding.GetEncoding("windows-1252");

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            useridDB = reader.GetInt32(0);
                            usernameDB = reader.GetString(1);
                            saltDB = reader.GetString(2);
                            passwordDB = reader.GetString(3);
                        }
                    }
                    
                    String pw = saltDB + password;

                    //txtNotif.Text = useridDB.ToString();

                    if (passwordDB.Equals(pw))
                    {
                        MessageDialog dialog = new MessageDialog("Login success! Welcome " + username);
                        await dialog.ShowAsync();
                        this.Frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        txtNotif.Text = "Wrong username & password combination!";
                    }

                    
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


