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
    /// Логика взаимодействия для ScheduleEditWindow.xaml
    /// </summary>
    public partial class ScheduleEditWindow : Window
    {
        private Schedules _schedule;
        public ScheduleEditWindow(Schedules schedule)
        {
            InitializeComponent();
            _schedule = schedule;
            TextBlockFrom.Text = schedule.DepartureAirport;
            TextBlockTo.Text = schedule.ArrivalAirport;
            TextBlockAircraft.Text = schedule.Aircrafts.Name;

            DatePickerDate.SelectedDate = schedule.Date;
            TextBoxTime.Text = schedule.Time.ToString(@"hh\:mm");
            TextBoxPrice.Text = ((int)schedule.EconomyPrice).ToString();
        }

        private void ButtonUpdateSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _schedule.Date = DatePickerDate.SelectedDate.Value;
                _schedule.Time = TimeSpan.Parse(TextBoxTime.Text);
                _schedule.EconomyPrice = int.Parse(TextBoxPrice.Text);
                AMONICEntities.GetContext().SaveChanges();
                MessageBox.Show("Schedule changed succesfully",
                    "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
                MessageBoxResult result = MessageBox.Show
                    ("Are you sure that you want to exit?",
                    "Exit", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                    Close();
                else
                    return;
        }
    }
}
