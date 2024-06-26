﻿using Microsoft.EntityFrameworkCore;
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
using CoinControl;
using System.Printing;
using System.Data.Entity.Migrations.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Threading;


namespace CoinControl
{
    public partial class MainDashboard : Window
    {
        public string connectionString = "Data Source=LAPTOP-A9L7U7HJ\\SQL2022TRAINING;Initial Catalog=CoinControl;Integrated Security=True;";
        private readonly DatabaseContext _context = new DatabaseContext();
        int loggedInUserId = AuthenticationManager.LoggedInUserId;
        public int reminderCount;

        public MainDashboard()
        {
            InitializeComponent();
            LoadInformation();
            LoadReminders();

            var budgetInfo = _context.Budgeting.Where(income => income.User_ID == loggedInUserId);
            reminderCount = budgetInfo.Count(); ;
        }
        
        public void LoadInformation()
        {
            var user = _context.User.FirstOrDefault(u => u.User_ID == loggedInUserId);
            var balance = GetBalance(loggedInUserId);
            if (user != null)
            {
                userName.Text = $"{user.Name}";
                emailUser.Text = $"{user.Email}";
                balanceText.Text = balance.ToString("₱#,##0.00");

                util.Text = $"{countOccurences(loggedInUserId, "Utilities")}";
                transpo.Text = $"{countOccurences(loggedInUserId, "Transportation")}";
                food.Text = $"{countOccurences(loggedInUserId, "Food")}";
                rent.Text = $"{countOccurences(loggedInUserId, "Rent")}";
                entertainment.Text = $"{countOccurences(loggedInUserId, "Entertainment")}";
                health.Text = $"{countOccurences(loggedInUserId, "Health")}";
                misc.Text = $"{countOccurences(loggedInUserId, "Miscellaneous")}";
            }

            var allIncomes = _context.Income.Where(income => income.User_ID == loggedInUserId).ToList();
            decimal totalIncome = allIncomes.Sum(income => income.Amount);
            incomeText.Text = totalIncome.ToString("₱#,##0.00");

            var allExpenses = _context.Expense.Where(expense => expense.User_ID == loggedInUserId).ToList();
            decimal totalExpenses = allExpenses.Sum(expense => expense.Amount);
            expenseText.Text = totalExpenses.ToString("₱#,##0.00");

            decimal profit = totalIncome - totalExpenses;

            if (profit > 0)
            {
                upProfit.Opacity = 1;
                equalProfit.Opacity = 0;
            }
            else if(profit < 0)
            {
                downProfit.Opacity = 1;
                equalProfit.Opacity = 0;
            }
            else { }

            profit = Math.Abs(profit);
            profitText.Text = profit.ToString("₱#,##0.00");
        }

        private decimal GetBalance(long userId)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ReadBalanceByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.Add("@Balance", SqlDbType.Money).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Retrieve the balance from the output parameter
                        balance = Convert.ToDecimal(command.Parameters["@Balance"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching balance: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return balance;
        }

        public int countOccurences(int userId, string item)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("CountOccurrencesByCategory", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@User_ID", userId);
                command.Parameters.AddWithValue("@CategoryName", item);
                command.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Get the count from the output parameter
                    int count = Convert.ToInt32(command.Parameters["@Count"].Value);

                    // Set the count to the Text property of util TextBox
                    return count;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public void LoadReminders()
        {
            var expenseReminder = _context.Budgeting
                .Where(e => e.User_ID == loggedInUserId)
                .ToList();

            expenseReminders.ItemsSource = expenseReminder;
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainDashboard mainDashboard = new MainDashboard();
            mainDashboard.Show();
            this.Close();
        }

        private void NavigateToExpenses(object sender, RoutedEventArgs e)
        {
            Expenses expenses = new Expenses();
            expenses.Show();
            this.Close();
        }
        private void NavigateToSavings(object sender, RoutedEventArgs e)
        {

            Savings savings = new Savings();
            savings.Show();
            this.Close();
        }
        private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        {
            Analytics analytics = new Analytics();
            analytics.Show();
            this.Close();
        }
        private void NavigateToReports(object sender, RoutedEventArgs e)
        {
            Reports reports = new Reports();
            reports.Show();
            this.Close();
        }

        private void LogoutButton(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void deactivateUser(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to deactivate your account?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int userID = AuthenticationManager.LoggedInUserId;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("DeactivateUser", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserID", userID);

                        connection.Open();
                        command.ExecuteNonQuery();

                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                    }
                    catch (SqlException ex)
                    {
                        // Handle SQL exception
                        MessageBox.Show("An error occurred while deactivating the user: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions
                        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }  
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        private void addBalance_Click(object sender, RoutedEventArgs e)
        {
            addToBalance add = new addToBalance();
            add.Show();
        }
        private void addReminder(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(reminderCount.ToString());

            if (reminderCount >= 5)
            {
                MessageBox.Show("Only a limit of 5 reminders are allowed!");
            }
            else
            {
                addReminder reminder = new addReminder();
                reminder.Show();
            }
        }

        private void removeReminder(object sender, RoutedEventArgs e)
        {
            long budget_id = (expenseReminders.SelectedItem as BudgetingDB).Budget_ID;
            BudgetingDB budget = (from r in _context.Budgeting where r.Budget_ID == budget_id select r).SingleOrDefault();
            _context.Budgeting.Remove(budget);
            _context.SaveChanges();
            expenseReminders.ItemsSource = _context.Budgeting.ToList();
            reminderCount--;
        }
    }
}
