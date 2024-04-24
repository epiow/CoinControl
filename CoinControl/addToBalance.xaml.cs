using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for addToBalance.xaml
    /// </summary>
    public partial class addToBalance : Window
    {
        public addToBalance()
        {
            InitializeComponent();
        }

        private void Confirm_Btn(object sender, RoutedEventArgs e)
        {
            long currentUserId = AuthenticationManager.LoggedInUserId;
            decimal amountToAdd = decimal.Parse(AmountTextBox.Text);

            AddBalance(currentUserId, amountToAdd);

            this.Close();

            if (Application.Current.Windows.OfType<MainDashboard>().Any())
            {
                MainDashboard mainDash = Application.Current.Windows.OfType<MainDashboard>().FirstOrDefault();
                mainDash?.LoadInformation();
            }
        }

        private void AddBalance(long userId, decimal amountToAdd)
        {
            try
            {
                MainDashboard conn = new MainDashboard();
                using (SqlConnection connection = new SqlConnection(conn.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("AddBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@User_ID", userId);
                        command.Parameters.AddWithValue("@Amount", amountToAdd);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
