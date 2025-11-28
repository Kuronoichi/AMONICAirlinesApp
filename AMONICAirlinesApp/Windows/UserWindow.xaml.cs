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
using System.Windows.Threading;

namespace AMONICAirlinesApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private DispatcherTimer timer;
        public UserWindow()
        {
            InitializeComponent();
            DataGridActivity.ItemsSource = AMONICEntities.GetContext().
                UserActivity.Where(ua => ua.UserID ==
                Account.acc.ID).ToList();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
            TextBlockEntry.Text = $"Hi, {Account.acc.FirstName} " +
                $"{Account.acc.LastName}, Welcome to AMONIC " +
                $"Airlines Automation System";
            TextBlockSpentTime.Text = "00:00:00";
            int d = AMONICEntities.GetContext().
                    UserActivity.Count(e => e.UserID == Account.acc.ID &&
                    e.Error != null);
            string numberoferrors = d.ToString();
            TextBlockCrushCount.Text = numberoferrors;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show
                ("Are you sure that you want to exit?",
                "Exit", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                Close();
            else
                return;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Account.acc = null;
            Account.activity.LogoutTime = DateTime.Now.TimeOfDay;
            AMONICEntities.GetContext().UserActivity.Add(Account.activity);
            AMONICEntities.GetContext().SaveChanges();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var AccountTime = DateTime.Now.TimeOfDay - Account.activity.LoginTime;
            TextBlockSpentTime.Text = AccountTime.ToString(@"hh\:mm\:ss");
        }
    }
}
