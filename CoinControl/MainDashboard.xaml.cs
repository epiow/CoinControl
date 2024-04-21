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
    public partial class MainDashboard : Window
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        int loggedInUserId = AuthenticationManager.LoggedInUserId;
        public MainDashboard()
        {
            InitializeComponent();
            LoadInformation();
            LoadReminders();
        }

        public void LoadInformation()
        {
            var allIncomes = _context.Income.Where(income => income.User_ID == loggedInUserId).ToList();
            decimal totalIncome = allIncomes.Sum(income => income.Amount);
            incomeText.Text = totalIncome.ToString("0.00");

            var allExpenses = _context.Expense.Where(expense => expense.User_ID == loggedInUserId).ToList();
            decimal totalExpenses = allExpenses.Sum(expense => expense.Amount);
            expenseText.Text = totalExpenses.ToString("0.00");

            decimal profit = totalIncome - totalExpenses;

            profitText.Text = profit.ToString("0.00");
        }
        
        public void LoadReminders()
        {
            var expenseReminder = _context.Budgeting
                .Where(e => e.User_ID == loggedInUserId)
                .ToList();
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

        private void addReminder(object sender, RoutedEventArgs e)
        {
            var expenseReminder = _context.Budgeting
                .Where(e => e.User_ID == loggedInUserId);

            if(expenseReminder.Count() > 5)
            {
                MessageBox.Show("Only a limit of 5 reminders are allowed!");
            }
            else
            {
                addReminder reminder = new addReminder();
                reminder.Show();
            }
        }
    }
}
