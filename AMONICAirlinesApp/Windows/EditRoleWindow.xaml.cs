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
    /// Логика взаимодействия для EditRoleWindow.xaml
    /// </summary>
    public partial class EditRoleWindow : Window
    {
        private Users edituser;
        public EditRoleWindow(Users _user)
        {
            InitializeComponent();
            edituser = _user;
            TextBoxEmail.Text = _user.Email;
            TextBoxFirstName.Text = _user.FirstName;
            TextBoxLastName.Text = _user.LastName;
            ComboBoxOffice.ItemsSource = 
                AMONICEntities.GetContext().Offices.ToList();
            ComboBoxOffice.SelectedItem = AMONICEntities.GetContext()
                .Offices.FirstOrDefault(o => o.ID == _user.OfficeID);
            if (_user.RoleID == 1)
            {
                RadioButtonRoleAdmin.IsChecked = true;
                RadioButtonRoleUser.IsChecked = false;
            }
            else
            {
                RadioButtonRoleAdmin.IsChecked = false;
                RadioButtonRoleUser.IsChecked = true;
            }

        }

        private void ButtonApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            if (RadioButtonRoleAdmin.IsChecked == true)
                edituser.RoleID = 1;  
            else
                edituser.RoleID = 2;

            AMONICEntities.GetContext().SaveChanges();
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
