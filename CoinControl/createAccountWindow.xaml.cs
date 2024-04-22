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

namespace CoinControl
{
    /// <summary>
    /// Interaction logic for createAccountWindow.xaml
    /// </summary>
    public partial class createAccountWindow : Window
    {
        public createAccountWindow()
        {
            InitializeComponent();
        }

        private void returnButton(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
        private void userTextChanged(object sender, TextChangedEventArgs e)
        {
            if (userText.Text != "")
            {
                txtUser.Visibility = Visibility.Hidden;
            }
            else
            {
                txtUser.Visibility = Visibility.Visible;
            }
        }
        private void emailTextChanged(object sender, TextChangedEventArgs e)
        {
            if (emailText.Text != "")
            {
                txtEmail.Visibility = Visibility.Hidden;
            }
            else
            {
                txtEmail.Visibility = Visibility.Visible;
            }
        }
        private void passTextChanged(object sender, TextChangedEventArgs e)
        {
            if (passText.Text != "")
            {
                txtPass.Visibility = Visibility.Hidden;
            }
            else
            {
                txtPass.Visibility = Visibility.Visible;
            }
        }
        private void confirmPassTextChanged(object sender, TextChangedEventArgs e)
        {
            if (confirmPassText.Text != "")
            {
                txtConfirm.Visibility = Visibility.Hidden;
            }
            else
            {
                txtConfirm.Visibility = Visibility.Visible;
            }
        }
        private void balanceTextChanged(object sender, TextChangedEventArgs e)
        {
            if (balanceText.Text != "")
            {
                txtBalance.Visibility = Visibility.Hidden;
            }
            else
            {
                txtBalance.Visibility = Visibility.Visible;
            }
        }
    }
}
