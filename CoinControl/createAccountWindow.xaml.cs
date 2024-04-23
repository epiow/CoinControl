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
using System.Text.RegularExpressions;

namespace CoinControl
{
    /// <summary>
    /// Interaction logic for createAccountWindow.xaml
    /// </summary>
    public partial class createAccountWindow : Window
    {
        string connectionString = "Data Source=LAPTOP-A9L7U7HJ\\SQL2022TRAINING;Initial Catalog=CoinControl;Integrated Security=True";
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
        private void passTextChanged(object sender, RoutedEventArgs e)
        {
            if (passText.Password.Length > 0)
            {
                txtPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtPass.Visibility = Visibility.Visible;
            }
        }
        private void confirmPassTextChanged(object sender, RoutedEventArgs e)
        {
            if (confirmPassText.Password.Length > 0)
            {
                txtConfirm.Visibility = Visibility.Collapsed;
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
            string password = passText.Password; 
            string confirmPass = confirmPassText.Password;

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

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.");
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
        private bool IsValidEmail(string email)
        {
            // This is a simple regex pattern to check for basic email format
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
