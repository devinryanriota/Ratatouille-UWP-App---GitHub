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
    public sealed partial class DeliveryPage : Page
    {
        public DeliveryPage()
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
            else if (ReservationListBoxItem.IsSelected) this.Frame.Navigate(typeof(ReservationPage));
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

        int orderChocoCake = 0;
        int orderRedVelvet = 0;
        int orderCheeseCake = 0;
        int orderBlackForest = 0;
        int userID = LoginPage.useridDB;
        

        private async void btnDeliver_Click(object sender, RoutedEventArgs e)
        {
            // 25 30 25 25
            orderChocoCake = Int32.Parse(txtChocoCake.Text);
            orderRedVelvet = Int32.Parse(txtRedVelvet.Text);
            orderCheeseCake = Int32.Parse(txtCheeseCake.Text);
            orderBlackForest = Int32.Parse(txtBlackForest.Text);

            int totalChoco = orderChocoCake * 25;
            int totalVelvet = orderRedVelvet * 30;
            int totalCheese = orderCheeseCake * 25;
            int totalForest = orderBlackForest * 25;

            String details = "";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    if(orderChocoCake != 0)
                    {
                        MySqlCommand getCommand = connection.CreateCommand();
                        getCommand.CommandText = "INSERT INTO delivery(UserID, FoodID, OrderQty, OrderTotal, OrderStatus) VALUES ('"+ userID +"', 00001, '" + orderChocoCake +"', '"+ totalChoco +"', 'Waiting')";

                        details += "Chocolate Cake = " + orderChocoCake + "\n";

                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        Encoding.GetEncoding("windows-1252");

                        getCommand.ExecuteNonQuery();
                    }

                    if (orderRedVelvet != 0)
                    {
                        MySqlCommand getCommand = connection.CreateCommand();
                        getCommand.CommandText = "INSERT INTO delivery(UserID, FoodID, OrderQty, OrderTotal, OrderStatus) VALUES ('" + userID + "', 00002, '" + orderRedVelvet + "', '" + totalVelvet + "', 'Waiting')";

                        details += "Red Velvet = " + orderRedVelvet + "\n";

                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        Encoding.GetEncoding("windows-1252");

                        getCommand.ExecuteNonQuery();
                    }

                    if (orderCheeseCake != 0)
                    {
                        MySqlCommand getCommand = connection.CreateCommand();
                        getCommand.CommandText = "INSERT INTO delivery(UserID, FoodID, OrderQty, OrderTotal, OrderStatus) VALUES ('" + userID + "', 00004, '" + orderCheeseCake + "', '" + totalCheese + "', 'Waiting')";

                        details += "Cheese Cake = " + orderCheeseCake + "\n";

                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        Encoding.GetEncoding("windows-1252");

                        getCommand.ExecuteNonQuery();
                    }

                    if (orderBlackForest != 0)
                    {
                        MySqlCommand getCommand = connection.CreateCommand();
                        getCommand.CommandText = "INSERT INTO delivery(UserID, FoodID, OrderQty, OrderTotal, OrderStatus) VALUES ('" + userID + "', 00003, '" + orderBlackForest + "', '" + totalForest + "', 'Waiting')";

                        details += "Black Forest = " + orderBlackForest + "\n";

                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        Encoding.GetEncoding("windows-1252");

                        getCommand.ExecuteNonQuery();
                    }

                    int grandTotal = totalChoco + totalVelvet + totalCheese + totalForest;
                    

                    MessageDialog dialog = new MessageDialog("Order Successful! \n\n " + details + "To be paid: $" + grandTotal);
                    await dialog.ShowAsync();
                    this.Frame.Navigate(typeof(MainPage));
                    connection.Close();

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
