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
    /// Логика взаимодействия для BookingConfirmWindow.xaml
    /// </summary>
    public partial class BookingConfirmWindow : Window
    {
        List<Tickets> _tickets;
        public BookingConfirmWindow(List<Tickets> tickets)
        {
            InitializeComponent();
            _tickets = tickets;
            int total = 0;
            foreach (Tickets item in _tickets)
            {
                total += item.Value;
            }
            TextBlockTotalAmount.Text = total.ToString();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (RadioButtonCard.IsChecked == true)
                MessageBox.Show("Succsesful card pay","Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            else if (RadioButtonCash.IsChecked == true)
                MessageBox.Show("Succsesful cash pay", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            else if (RadioButtonVoucher.IsChecked == true)
                MessageBox.Show("Succsesful voucher pay", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (Tickets item in _tickets)
            {
                AMONICEntities.GetContext().Tickets.Add(item);
            }
            AMONICEntities.GetContext().SaveChanges();
            Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
