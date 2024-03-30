using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoinControl
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            if (ValidateUser(username, password))
            {
                MessageBox.Show("Login successful!");
                MainDashboard mainDashboard = new MainDashboard();
                mainDashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void createAcc_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Insert the new user into the database
            if (InsertUser(username, password))
            {
                MessageBox.Show("User account created successfully!");
            }
            else
            {
                MessageBox.Show("Failed to create user account. Please try again.");
            }
        }

        private bool InsertUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=EPIOW\\SQLEXPRESS01;Initial Catalog=CoinControl;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO [User] (Name, Password) VALUES (@Name, @Password)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }


        private bool ValidateUser(string username, string password)
        {

            using (SqlConnection connection = new SqlConnection("Data Source=EPIOW\\SQLEXPRESS01;Initial Catalog=CoinControl;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM [User] WHERE Name=@Name AND Password=@Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count == 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUsername.Text != ""){
                txtUser.Visibility = Visibility.Hidden;
            }
            else{
                txtUser.Visibility = Visibility.Visible;
            }
        }

        void PassChanged(Object sender, RoutedEventArgs args)
        {
            if (txtPassword.Password.Length > 0)
            {
                txtPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtPass.Visibility = Visibility.Visible;
            }
        }
        /*private void txtPassword_PasswordChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Password.Length > 0)
            {
                txtPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtPass.Visibility = Visibility.Visible;
            }
        }*/
    }
}
