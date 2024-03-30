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
using System.Windows.Shapes;

namespace CoinControl
{
    public partial class Expenses : Window
    {
        public Expenses()
        {
            InitializeComponent();
            Loaded += Expenses_Loaded; // Attach the Loaded event handler
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainDashboard mainDashboard = new MainDashboard();
            mainDashboard.Show();
            Close();
        }

        private void NavigateToExpenses(object sender, RoutedEventArgs e)
        {
            // No need to create another instance of Expenses
            // Since we are already in the Expenses window
        }

        private void NavigateToSavings(object sender, RoutedEventArgs e)
        {
            Savings savings = new Savings();
            savings.Show();
            Close();
        }

        private void NavigateToReports(object sender, RoutedEventArgs e)
        {
            Reports reports = new Reports();
            reports.Show();
            Close();
        }

        // Event handler for the window Loaded event
        private void Expenses_Loaded(object sender, RoutedEventArgs e)
        {
            // Call the ShowUserExpenses method with the user's ID
            ShowUserExpenses(3); // Replace 1 with the actual user ID
        }

        // Define the ShowUserExpenses method
        private void ShowUserExpenses(int userId)
        {
            string connectionString = "Data Source=EPIOW\\SQLEXPRESS01;Initial Catalog=CoinControl;Integrated Security=True";
            string query = "SELECT * FROM Expense WHERE [User_ID] = @User_ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@User_ID", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    expensesDataGrid.Items.Clear();

                    while (reader.Read())
                    {
                        //retrieve expense details
                        long expenseId = reader.GetInt64(reader.GetOrdinal("Payment_ID"));
                        string description = reader.GetString(reader.GetOrdinal("Note"));
                        decimal amount = reader.GetDecimal(reader.GetOrdinal("Amount"));

                        //add expense details to the data grid
                        expensesDataGrid.Items.Add(new { ExpenseId = expenseId, Description = description, Amount = amount });
                    }


                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching expenses: " + ex.Message);
                }
            }
        }
    }
}