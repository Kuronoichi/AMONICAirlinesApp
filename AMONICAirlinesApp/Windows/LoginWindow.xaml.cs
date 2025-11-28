using AMONICAirlinesApp.Classes;
using AMONICAirlinesApp.Database;
using AMONICAirlinesApp.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace AMONICAirlinesApp.Windows
{
    public partial class LoginWindow : Window
    {
        private static int counterofinput = 0;
        private static DispatcherTimer cooldown;
        private static int cooldowntime = 10;
        public LoginWindow()
        {
            InitializeComponent();
            TextBlockCooldown.Visibility = Visibility.Hidden;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            counterofinput ++;
            
            if (counterofinput > 2)
            {
                TextBlockCooldown.Visibility = Visibility.Visible;
                ButtonLogin.IsEnabled = false;
                cooldown = new DispatcherTimer();
                cooldown.Interval = new TimeSpan(0, 0, 1);
                cooldown.Tick += Cooldown_Tick;
                cooldown.Start();
            }

            try
            {
                var user = AMONICEntities.GetContext().Users.
                    FirstOrDefault(u => u.Email == TextBoxUsername.Text &&
                    u.Password == TextBoxPassword.Text);

                if (user != null)
                {
                    counterofinput = 0;
                    if (user.Active == false)
                    {
                        MessageBox.Show($"User {user.Email} has been blocked",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Account.acc = user;
                    Account.activity = new UserActivity()
                    {
                        LoginTime = DateTime.Now.TimeOfDay,
                        Date = DateTime.Now.Date,
                        Error = null,
                        UserID = user.ID
                    };
                    if (user.RoleID == 1)
                    {
                        new AdminWindow().Show();
                        Close();
                    }
                    else if (user.RoleID == 2)
                    {
                        new UserWindow().Show();
                        Close();
                    }
                }
                else
                    MessageBox.Show("Incorrect username or password",
                        "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message,"Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show
                ("Are you sure that you want to exit?",
                "Exit",MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                Close();
            else
                return;
        }

        private void Cooldown_Tick(object sender, EventArgs e)
        {
            if (cooldowntime != 0)
            {
                TextBlockCooldown.Text = $"Block time: {cooldowntime} seconds";
                cooldowntime --;
            }
            else
            {
                cooldown.Stop();
                MessageBox.Show("Login button unlocked", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                TextBlockCooldown.Visibility = Visibility.Hidden;
                ButtonLogin.IsEnabled = true;
                counterofinput = 0;
                cooldowntime = 0;
            }

        }
    }
}
