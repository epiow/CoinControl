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
            /*
            var expenses = _context.Expense.Where(e => e.User_ID == loggedInUserId).ToList();
            expensesDataGrid.ItemsSource = expenses;
            */
            var expenses = _context.Expense
            .Where(e => e.User_ID == loggedInUserId)
            .Select(e => new
            {
                Date = e.Trans_Datetime,
                Description = e.Note,
                Amount = e.Amount
            })
            .ToList();

                    expensesDataGrid.ItemsSource = expenses;
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

        private void DeleteTran_Btn(object sender, RoutedEventArgs e)
        {
            ExpenseDB selectedExpense = expensesDataGrid.SelectedItem as ExpenseDB;
            if (selectedExpense != null)
            {
                _context.Expense.Remove(selectedExpense);
                try
                {
                    _context.SaveChanges();

                    // Refresh the DataGrid after the deletion
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

        private void AddTran_Btn(object sender, RoutedEventArgs e)
        {
        }

    }
}