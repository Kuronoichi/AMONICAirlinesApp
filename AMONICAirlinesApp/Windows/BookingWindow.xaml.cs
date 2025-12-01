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
    /// Логика взаимодействия для BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        private List<Passenger> passengers = new List<Passenger>();
        ScheduleEntity _outbound;
        ScheduleEntity _return;
        int _passnum;
        public BookingWindow(ScheduleEntity _out, ScheduleEntity _ret, int passnum)
        {
            InitializeComponent();
            _outbound = _out;
            _return = _ret;
            _passnum = passnum;
            ComboBoxPassportCountry.ItemsSource =
                AMONICEntities.GetContext().Countries.ToList();
            ComboBoxPassportCountry.SelectedIndex = 0;

            TextBlockOutCabinType.Text = AMONICEntities.GetContext().
                CabinTypes.FirstOrDefault(cb => cb.ID == _out.CabinTypeID).Name;
            TextBlockOutDate.Text = _out.Date.ToString();
            TextBlockOutFlightNumber.Text = _out.Flight;
            TextBlockOutFrom.Text = _out.From;
            TextBlockOutTo.Text = _out.To;
            if (_ret != null)
            {
                TextBlockRetCabinType.Text = AMONICEntities.GetContext().
                    CabinTypes.FirstOrDefault(cb => cb.ID == _ret.CabinTypeID).Name;
                TextBlockRetDate.Text = _ret.Date.ToString();
                TextBlockRetFlightNumber.Text = _ret.Flight;
                TextBlockRetFrom.Text = _ret.From;
                TextBlockRetTo.Text = _ret.To;
            }
            else
                GroupBoxReturn.Visibility = Visibility.Collapsed;
        }

        private void Load()
        {
            DataGridPassengers.ItemsSource = passengers.ToList();
        }

        private void ButtonAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            var selectedcountry = ComboBoxPassportCountry.SelectedItem as Countries;
            Passenger newpass = new Passenger()
            {
                FirstName = TextBoxFisrtname.Text,
                LastName = TextBoxLastname.Text,
                BirthDate = DatePickerBirthDate.Text,
                PassportNumber = TextBoxPassport.Text,
                CountryID = selectedcountry.ID,
                Phone = TextBoxPhone.Text,
            };
            passengers.Add(newpass);
            Load();
        }

        private void ButtonRemovePassenger_Click(object sender, RoutedEventArgs e)
        {
            var selectedpass = DataGridPassengers.SelectedItem as Passenger;
            if(selectedpass != null)
                passengers.Remove(selectedpass);
            else
                MessageBox.Show("Please select a passenger","Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            Load();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
                if (passengers.Count != _passnum)
                {
                    MessageBox.Show("Incorrect number of passengers",
                        "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                    return;
                }

                List<Tickets> tickets = new List<Tickets>();

                foreach (var passenger in passengers)
                {
                    Tickets outticket = new Tickets()
                    {
                        UserID = Account.acc.ID,
                        ScheduleID = _outbound.ID,
                        CabinTypeID = _outbound.CabinTypeID,
                        Firstname = passenger.FirstName,
                        Lastname = passenger.LastName,
                        Phone = passenger.Phone,
                        PassportNumber = passenger.PassportNumber,
                        PassportCountryID = passenger.CountryID,
                        Value = _outbound.CabinPrice,
                        BookingReference = passenger.BookingReference,
                        Confirmed = false
                    };
                    tickets.Add(outticket);
                    if (_return != null)
                    {
                        Tickets retticket = new Tickets()
                        {
                            UserID = Account.acc.ID,
                            ScheduleID = _return.ID,
                            CabinTypeID = _return.CabinTypeID,
                            Firstname = passenger.FirstName,
                            Lastname = passenger.LastName,
                            Phone = passenger.Phone,
                            PassportNumber = passenger.PassportNumber,
                            PassportCountryID = passenger.CountryID,
                            Value = _return.CabinPrice,
                            BookingReference = passenger.BookingReference,
                            Confirmed = false
                        };
                        tickets.Add(retticket);
                    }

                    new BookingConfirmWindow(tickets).ShowDialog();
                }
            }
        }

        public class Passenger
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string BirthDate { get; set; }
            public string Phone { get; set; }
            public string PassportNumber { get; set; }
            public int CountryID { get; set; }
        public string BookingReference =>
FirstName?[0] + LastName?.Substring(1, Math.Min(4, (LastName?.Length ?? 0) - 1)) ?? "";
        public string PassportCountry => AMONICEntities.GetContext().
                Countries.FirstOrDefault(c => c.ID == CountryID).Name;
        }
    }

