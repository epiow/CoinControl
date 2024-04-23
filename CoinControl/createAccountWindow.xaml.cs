using Microsoft.IdentityModel.Tokens;
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
using System.Data.SqlClient;

namespace CoinControl
{
    /// <summary>
    /// Interaction logic for createAccountWindow.xaml
    /// </summary>
    public partial class createAccountWindow : Window
    {
        string connectionString = "Data Source=EPIOW\\SQLEXPRESS;Initial Catalog=CoinControl;Integrated Security=True";
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

        private void createAcc(object sender, RoutedEventArgs e)
        {
            string username = userText.Text;
            string email = emailText.Text;
            string password = passText.Text; 
            string confirmPass = confirmPassText.Text;

            if (password != confirmPass)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Please complete the necessary credentials.");
                return;
            }

            int balance;
            if (!int.TryParse(balanceText.Text, out balance))
            {
                MessageBox.Show("Please enter a valid balance.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("CreateUser", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password); 
                command.Parameters.AddWithValue("@Balance", balance); 

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("User account created successfully!");

                    LoginWindow login = new LoginWindow();
                    login.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to create user account. Please try again. Error: " + ex.Message);
                }
            }
        }
    }
}
