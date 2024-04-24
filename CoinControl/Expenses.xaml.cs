using Microsoft.EntityFrameworkCore;
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

namespace CoinControl
{
    public partial class Expenses : Window
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        int loggedInUserId = AuthenticationManager.LoggedInUserId;
        public Expenses()
        {
            InitializeComponent();
            LoadExpenses();
        }

        public void LoadExpenses()
        {
            var user = _context.User.FirstOrDefault(u => u.User_ID == loggedInUserId);
            if (user != null)
            {
                userName.Text = $"{user.Name}";
                emailUser.Text = $"{user.Email}";
            }

            var expensesData = _context.Expense
                .Where(e => e.User_ID == loggedInUserId)
                .ToList();
           
            expenseDataGrid.ItemsSource = expensesData;
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainDashboard mainDashboard = new MainDashboard();
            mainDashboard.Show();
            Close();
        }

        private void NavigateToExpenses(object sender, RoutedEventArgs e)
        {
            // No action needed here since the user is already on the Expenses page
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close(); // Close the window if the user clicks Yes
            }
        }


        private void NavigateToSavings(object sender, RoutedEventArgs e)
        {
            Savings savings = new Savings();
            savings.Show();
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
            this.Close();
        }
        private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        {
            Analytics analytics = new Analytics();
            analytics.Show();
            this.Close();
        }
        private void AddTran_Btn(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow addExpenseWindow = new AddExpenseWindow();
            addExpenseWindow.Show();

            //this.Close();
        }

        private void DeleteTran_Btn(object sender, RoutedEventArgs e)
        {
            ExpenseDB selectedExpense = expenseDataGrid.SelectedItem as ExpenseDB;
            if (selectedExpense != null)
            {
                _context.Expense.Remove(selectedExpense);
                try
                {
                    _context.SaveChanges();
                    LoadExpenses();
                    MessageBox.Show("Expense deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an expense to delete.");
            }
        }

    }
}