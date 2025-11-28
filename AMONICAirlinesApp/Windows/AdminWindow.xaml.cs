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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataGridUsers.ItemsSource = AMONICEntities.GetContext().
                Users.ToList();
            
            var offices = AMONICEntities.GetContext().Offices.Select(
                x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                }).ToList();

            offices.Insert(0, new
            {
                ID = 0,
                Title = "All offices"
            });

            ComboboxOfficeFilter.ItemsSource = offices;
            ComboboxOfficeFilter.SelectedIndex = 0;
        }

        private void ButtonChangeRole_Click(object sender, RoutedEventArgs e)
        {
            var selecteduser = DataGridUsers.SelectedItem as Users;
            if (selecteduser == null)
                MessageBox.Show("Select an user, please", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new EditRoleWindow(selecteduser).ShowDialog();
                ComboboxOfficeFilter.SelectedIndex++;
                ComboboxOfficeFilter.SelectedIndex--;
            }
        }

        private void ButtonChangeLogin_Click(object sender, RoutedEventArgs e)
        {
            var selecteduser = DataGridUsers.SelectedItem as Users;
            if (selecteduser == null)
                MessageBox.Show("Select an user, please", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    if (selecteduser.Active == true)
                        selecteduser.Active = false;
                    else
                        selecteduser.Active = true;
                    AMONICEntities.GetContext().SaveChanges();
                    
                    MessageBox.Show("User activity changed successfully", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    ComboboxOfficeFilter.SelectedIndex++;
                    ComboboxOfficeFilter.SelectedIndex--;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Error",
                        MessageBoxButton.OK,MessageBoxImage.Error);
                }
                
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show
                ("Are you sure that you want to exit?",
                "Exit", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                Account.acc = null;
                new LoginWindow().Show();
                Close();
            }
            else
                return;
        }

        private void MenuItemAddUser_Click(object sender, RoutedEventArgs e)
        {
            new AddUserWindow().ShowDialog();
            ComboboxOfficeFilter.SelectedIndex++;
            ComboboxOfficeFilter.SelectedIndex--;
        }

        private void ComboboxOfficeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecteditem = ComboboxOfficeFilter.SelectedItem as dynamic;
            int officeID = selecteditem.ID;
            if (selecteditem.ID == 0)
            {
                DataGridUsers.ItemsSource = AMONICEntities.GetContext().
                Users.ToList();
            }
            else
            {
                DataGridUsers.ItemsSource = AMONICEntities.GetContext().
                Users.Where(u => u.OfficeID == officeID).ToList();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Account.acc = null;
            Account.activity.LogoutTime = DateTime.Now.TimeOfDay;
            AMONICEntities.GetContext().
                UserActivity.Add(Account.activity);
            AMONICEntities.GetContext().SaveChanges();
        }

        private void ButtonSchedule_Click(object sender, RoutedEventArgs e)
        {
            new ScheduleWindow().ShowDialog();

        }

        private void ButtonFlights_Click(object sender, RoutedEventArgs e)
        {
            new FlightsWindow().ShowDialog();
        }
    }
}
