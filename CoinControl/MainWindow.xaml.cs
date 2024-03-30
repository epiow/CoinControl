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

            User authenticatedUser = ValidateUser(username, password);

            if (authenticatedUser != null)
            {
                MessageBox.Show("Login successful!");
                AuthenticationManager.LoggedInUserId =(int)authenticatedUser.User_ID;
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

        private User ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=EPIOW\\SQLEXPRESS01;Initial Catalog=CoinControl;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT User_ID, COUNT(1) FROM [User] WHERE Name=@Name AND Password=@Password GROUP BY User_ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", username);
                    command.Parameters.AddWithValue("@Password", password);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        long userId = reader.GetInt64(0);
                        int count = reader.GetInt32(1);

                        if (count == 1)
                        {
                            return new User { User_ID = userId };
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
