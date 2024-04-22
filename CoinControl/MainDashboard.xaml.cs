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
using System.Data.Entity.Migrations.Design;


namespace CoinControl
{
    public partial class MainDashboard : Window
    {
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
            if (user != null)
            {
                balanceText.Text = user.Balance.ToString("0.00");
            }

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
            //MessageBox.Show(reminderCount.ToString());
            
            if(reminderCount >= 5)
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
