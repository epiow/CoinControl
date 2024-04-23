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
        public string dataConnector = "Data Source=LAPTOP-A9L7U7HJ\\SQL2022TRAINING;Initial Catalog=CoinControl;Integrated Security=True";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            User authenticatedUser = ValidateUser(email, password);

            if (authenticatedUser != null)
            {
                MessageBox.Show("Login successful!");
                AuthenticationManager.LoggedInUserId = (int)authenticatedUser.User_ID;
                MainDashboard mainDashboard = new MainDashboard();
                mainDashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.");
                txtEmail.Clear();
                txtPassword.Clear();
            }
        }

        private void createAcc_Click(object sender, RoutedEventArgs e)
        {
            createAccountWindow createAcc = new createAccountWindow();
            createAcc.Show();
            Close();
        }

        private void exitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private User ValidateUser(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(dataConnector))

            {
                try
                {
                    connection.Open();
                    string query = "SELECT User_ID, COUNT(1) FROM [User] WHERE Email=@Email AND Password=@Password GROUP BY User_ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
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

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtEmail.Text != "")
            {
                txtEmailWater.Visibility = Visibility.Hidden;
            }
            else
            {
                txtEmailWater.Visibility = Visibility.Visible;
            }
        }

        private void PassChanged(object sender, RoutedEventArgs e)
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
    }
}