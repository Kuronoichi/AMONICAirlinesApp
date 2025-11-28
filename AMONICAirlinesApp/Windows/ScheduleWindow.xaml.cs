using AMONICAirlinesApp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
    /// Логика взаимодействия для ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ScheduleWindow()
        {
            InitializeComponent();

            var Airports = AMONICEntities.
                GetContext().Airports.Select(airport => new
                {
                    airport.ID,
                    airport.Name
                }

                ).ToList();

            Airports.Insert(0, new
            {
                ID = 0,
                Name = "All"
            });

            ComboBoxFromAirport.ItemsSource = Airports;
            ComboBoxToAirport.ItemsSource = Airports;

            ComboBoxFromAirport.SelectedIndex = 0;
            ComboBoxToAirport.SelectedIndex = 0;

            var SortVariants = new List<string>()
            {
                "Date-Time",
                "Price",
                "Confirmed"
            };

            ComboBoxSortBy.ItemsSource = SortVariants;
            ComboBoxSortBy.SelectedIndex = 0;

            UpdateData();
        }

        private void ButtonCancelFlight_Click(object sender, RoutedEventArgs e)
        {
            var selectedflight = DataGridFlights.SelectedItem
                as Schedules;
            if (selectedflight != null)
            {
                if (selectedflight.Confirmed == true)
                    selectedflight.Confirmed = false;
                else
                    selectedflight.Confirmed = true;
                AMONICEntities.GetContext().SaveChanges();
                UpdateData();
            }

            else
                MessageBox.Show("Please, choose schedule", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ButtonEditFlight_Click(object sender, RoutedEventArgs e)
        {
            var selectedschedule = DataGridFlights.SelectedItem
                as Schedules;
            if (selectedschedule != null)
                new ScheduleEditWindow(selectedschedule).ShowDialog();
            else
                MessageBox.Show("Please, choose schedule", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            UpdateData();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            var _flights = AMONICEntities.GetContext().
                Schedules.
                AsQueryable();

            var departureairport =
                ComboBoxFromAirport.SelectedItem as Airports;

            if (ComboBoxFromAirport.SelectedIndex != 0)
                _flights = _flights.
                    Where(x => x.Routes.DepartureAirportID ==
                    ComboBoxFromAirport.SelectedIndex + 2);

            var arrivalairport =
                ComboBoxToAirport.SelectedItem as Airports;

            if (ComboBoxToAirport.SelectedIndex != 0)
                _flights = _flights.
                    Where(x => x.Routes.ArrivalAirportID ==
                    ComboBoxToAirport.SelectedIndex + 2);

            var date =
                DatePickerOutbound.SelectedDate;

            if (DatePickerOutbound.SelectedDate != null)
                _flights = _flights.
                    Where(x => x.Date == date);

            var flightNumber =
                TextBoxNumber.Text;

            if (!string.IsNullOrWhiteSpace(TextBoxNumber.Text))
                _flights = _flights.Where(x => x.FlightNumber == TextBoxNumber.Text.Trim());

            if (ComboBoxSortBy.SelectedIndex == 0)
                _flights = _flights.OrderBy(x => x.Date);
            else if (ComboBoxSortBy.SelectedIndex == 1)
                _flights = _flights.OrderBy(x => x.EconomyPrice);
            else if (ComboBoxSortBy.SelectedIndex == 2)
                _flights = _flights.OrderBy(x => x.Confirmed);

            DataGridFlights.ItemsSource = _flights.ToList();
        }

        private void DataGridFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedflight = DataGridFlights.SelectedItem as Schedules;
            if (selectedflight.Confirmed == true)
                ButtonCancelFlight.Content = "Cancel flight";
            else
                ButtonCancelFlight.Content = "Confirm flight";
        }

        private void ButtonImportChanges_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry, but this function" +
                " in progress", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
