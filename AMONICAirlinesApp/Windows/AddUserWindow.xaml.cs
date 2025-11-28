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
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
            DatePickerBirthday.SelectedDate = DateTime.Now;
            ComboBoxOffices.ItemsSource = AMONICEntities.GetContext().
                Offices.ToList();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
 
                Offices selectedoffice = ComboBoxOffices.SelectedItem as Offices;
            Users newuser = new Users()
            {
                ID = AMONICEntities.GetContext().
                Users.ToList().Last().ID + 1,
                    Email = TextBoxEmail.Text,
                FirstName = TextBoxFirstName.Text,
                LastName = TextBoxLastName.Text,
                OfficeID = selectedoffice.ID,
                Birthdate = DatePickerBirthday.SelectedDate,
                Active = true,
                Password = TextBoxPassword.Text,
                RoleID = 2
            };
                AMONICEntities.GetContext().Users.Add(newuser);
                AMONICEntities.GetContext().SaveChanges();
                MessageBox.Show("New user added successfully","Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show
                ("Are you sure that you want to exit?",
                "Exit", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
            else
                return;
        }
    }
}
