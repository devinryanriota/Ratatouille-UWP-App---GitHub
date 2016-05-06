using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class StatusPage : Page
    {
        public StatusPage()
        {
            this.InitializeComponent();
            loadList();
            txtLogin.Text = MainPage.firstName;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoginListBoxItem.IsSelected) ;
            else if (HomeListBoxItem.IsSelected) this.Frame.Navigate(typeof(MainPage));
            else if (ReservationListBoxItem.IsSelected) this.Frame.Navigate(typeof(ReservationPage));
            else if (DeliveryListBoxItem.IsSelected) this.Frame.Navigate(typeof(DeliveryPage));
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

        int userid = LoginPage.useridDB;
        String connString = @"Server=ap-cdbr-azure-southeast-b.cloudapp.net;Database=ratatouille;Uid=b137f1b83a0315;Pwd=e179ee0c;SslMode=None;charset=utf8";

        public async void loadList()
        {

            String name = "";
            String date = "";
            int guest = 0;
            String time = "";
            String status = "";

            ObservableCollection<Reservation> dataList = new ObservableCollection<Reservation>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT ReservationName, ReservationDate, ReservationGuest, ReservationTime, ReservationStatus FROM reservation WHERE userid='" + userid + "'";

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding.GetEncoding("windows-1252");

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                            date = reader.GetString(1);
                            guest = reader.GetInt32(2);
                            time = reader.GetString(3);
                            status = reader.GetString(4);

                            Reservation res = new Reservation() { Name = name, Date = date, Guest = guest, Time = time, Status = status };
                            dataList.Add(res);
                            
                        }
                    }
                }

               
            }
            catch (MySqlException ex)
            {
                MessageDialog dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }
            lvwReservation.ItemsSource = dataList;


            int food = 0;
            int qty = 0;
            String statusOrd = "";

            ObservableCollection<Order> list = new ObservableCollection<Order>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT FoodID, OrderQty, OrderStatus FROM delivery WHERE userid='" + userid + "'";

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding.GetEncoding("windows-1252");

                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            food = reader.GetInt32(0);
                            qty = reader.GetInt32(1);
                            statusOrd = reader.GetString(2);

                            String fd = "";
                            if(food.Equals(1))
                            {
                                fd = "Chocolate Cake";
                            }
                            else if(food.Equals(2))
                            {
                                fd = "Red Velvet";
                            }
                            else if (food.Equals(3))
                            {
                                fd = "Black Forest";
                            }
                            else if (food.Equals(4))
                            {
                                fd = "Cheese Cake";
                            }

                            Order ord = new Order() { Food = fd, Qty = qty, Status = statusOrd};
                            list.Add(ord);

                        }
                    }
                }


            }
            catch (MySqlException ex)
            {
                MessageDialog dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }
            lvwOrder.ItemsSource = list;



        }

       
            
           

            


        }
    }

