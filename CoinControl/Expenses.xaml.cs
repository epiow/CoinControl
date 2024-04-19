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

        private void LoadExpenses()
        {
            //var expenses = _context.Expense.Where(e => e.User_ID == loggedInUserId).ToList();

            /*var displayExpenses = expenses.Select(e => new {
                e.Amount,
                e.Note,
                e.Payment_Method,
                e.Trans_Datetime
            }).ToList();

            expensesDataGrid.ItemsSource = displayExpenses;
            */

            var expensesWithCategoryNames = _context.Expense
                .Where(e => e.User_ID == loggedInUserId)
                .Select(e => new {
                    CategoryName = _context.Categories
                        .Where(c => c.Category_ID == e.Category_ID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault(),
                    e.Amount,
                    e.Note,
                    e.Payment_Method,
                    e.Trans_Datetime
                    })
                .ToList();

            
            expensesDataGrid.AutoGeneratingColumn += (sender, e) =>
            {
                if (e.PropertyName == "CategoryName")
                    e.Column.Header = "Expenditures";
                else if (e.PropertyName == "Amount")
                    e.Column.Header = "Expense Amount";
                else if (e.PropertyName == "Note")
                    e.Column.Header = "Note";
                else if (e.PropertyName == "Payment_Method")
                    e.Column.Header = "Payment Method";
                else if (e.PropertyName == "Trans_Datetime")
                    e.Column.Header = "Transaction Date & Time";
            };
            

            expensesDataGrid.ItemsSource = expensesWithCategoryNames;
            

            /*
            var expenses = _context.Expense


            var expenses = _context.Expense.Where(e => e.User_ID == loggedInUserId).ToList();
            expensesDataGrid.ItemsSource = expenses;
            
            var expenses = _context.Expense

            .Where(e => e.User_ID == loggedInUserId)
            .Select(e => new
            {
                Date = e.Trans_Datetime,
                Description = e.Note,
                Amount = e.Amount
            })
            .ToList();
            */

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

        private void LogoutButton(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
        private void AddTran_Btn(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow addExpenseWindow = new AddExpenseWindow();
            addExpenseWindow.Show();
            Close();
        }

        private void DeleteTran_Btn(object sender, RoutedEventArgs e)
        {
            ExpenseDB selectedExpense = expensesDataGrid.SelectedItem as ExpenseDB;
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
                    MessageBox.Show("Error deleting expense: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an expense to delete.");
            }
        }

    }
}