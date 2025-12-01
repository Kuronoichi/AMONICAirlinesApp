using AMONICAirlinesApp.Classes;
using AMONICAirlinesApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AMONICAirlinesApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для FlightsWindow.xaml
    /// </summary>
    public partial class FlightsWindow : Window
    {
        public FlightsWindow()
        {
            InitializeComponent();
            var Airports = AMONICEntities.
                GetContext().Airports.Select(airport => new
                {
                    airport.ID,
                    airport.IATACode
                }

                ).ToList();

            ComboBoxFrom.ItemsSource = Airports;
            ComboBoxTo.ItemsSource = Airports;

            ComboBoxFrom.SelectedIndex = 0;
            ComboBoxTo.SelectedIndex = 1;

            ComboBoxCabinType.ItemsSource =
                AMONICEntities.GetContext().
                CabinTypes.ToList();

            ComboBoxCabinType.SelectedIndex = 0;

            RadioButtonReturn.IsChecked = true;

            DatePickerOutbound.SelectedDate = new DateTime(2017, 12, 04);
            DatePickerReturn.SelectedDate = new DateTime(2017, 12, 16);

            UpdateData();
        }

        private void CheckBoxReturn_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void CheckBoxOutbound_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void RadioButtonReturn_Checked(object sender, RoutedEventArgs e)
        {
            DatePickerReturn.IsEnabled = true;
        }

        private void RadioButtonOneWay_Checked(object sender, RoutedEventArgs e)
        {
            DatePickerReturn.IsEnabled = false;
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int passengersnumber = int.Parse(TextBoxPassanger.Text);

                var FlightOut = DataGridOutboundFlights.SelectedItem as ScheduleEntity;

                var FlightReturn = DataGridReturnFlights.SelectedItem as ScheduleEntity;

                if (FlightOut != null && FlightReturn != null)
                {
                    new BookingWindow(FlightOut, FlightReturn, passengersnumber).ShowDialog();
                }
                else if (FlightOut != null)
                    new BookingWindow(FlightOut, null, passengersnumber).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void UpdateData()
        {
            var schedule = AMONICEntities.GetContext().
                Schedules.AsQueryable();
            var cabin = ComboBoxCabinType.SelectedItem as CabinTypes;
            List<ScheduleEntity> data =
                new List<ScheduleEntity>();
            foreach (Schedules f in schedule)
            {
                var item = new ScheduleEntity()
                {
                    ID = f.ID,
                    Date = f.Date,
                    Time = f.Time,
                    From = f.DepartureAirport,
                    To = f.ArrivalAirport,
                    Flight = f.FlightNumber,
                    Stops = 0
                };
                if (ComboBoxCabinType.SelectedItem != null)
                {
                    if (cabin.Name == "Economy")
                    {
                        item.CabinPrice = (int)f.EconomyPrice;
                        item.CabinTypeID = 1;
                    }
                    if (cabin.Name == "Business")
                    {
                        item.CabinTypeID = 2;
                        item.CabinPrice = (int)f.BussinesPrice;
                    }
                    if (cabin.Name == "First Class")
                    {
                        item.CabinPrice = (int)f.FirstClassPrice;
                        item.CabinTypeID = 3;
                    }
                        
                }
                data.Add(item);
            }

            var outbounddata = data.AsQueryable();
            var arrivaldata = data.AsQueryable();

            string departureName = (ComboBoxFrom.SelectedItem as dynamic)?.IATACode;
            string arrivalName = (ComboBoxTo.SelectedItem as dynamic)?.IATACode;

            if (!string.IsNullOrEmpty(departureName))
            {
                outbounddata = outbounddata.Where(i => i.From == departureName);
                arrivaldata = arrivaldata.Where(i => i.To == departureName);
            }

            if (!string.IsNullOrEmpty(arrivalName))
            { 
                outbounddata = outbounddata.Where(i => i.To == arrivalName);
                arrivaldata = arrivaldata.Where(i => i.From == arrivalName);
            }


            var outbounddate = DatePickerOutbound.SelectedDate.Value;

            var arrivaldate = DatePickerReturn.SelectedDate.Value;

            if (DatePickerReturn.SelectedDate.HasValue)
            {
                if (CheckBoxReturn.IsChecked == true)
                    arrivaldata = arrivaldata.Where(i =>
                    i.Date <= arrivaldate.AddDays(3) &&
                    i.Date >= arrivaldate.AddDays(-3));
                else
                    arrivaldata = arrivaldata.Where(i => i.Date ==
                    arrivaldate);
            }

            if (DatePickerOutbound.SelectedDate.HasValue)
            {
                if (CheckBoxOutbound.IsChecked == true)
                    outbounddata = outbounddata.Where(i =>
                    i.Date <= outbounddate.AddDays(3) &&
                    i.Date >= outbounddate.AddDays(-3));
                else
                    outbounddata = outbounddata.Where(i => i.Date ==
                outbounddate);
            }

            if (RadioButtonReturn.IsChecked == false)
            {
                arrivaldata = null;
            }

            DataGridOutboundFlights.ItemsSource = outbounddata.ToList();
            if(arrivaldata != null)
            DataGridReturnFlights.ItemsSource = arrivaldata.ToList();
            else
            DataGridReturnFlights.Visibility = Visibility.Collapsed;
        }
    }
}
