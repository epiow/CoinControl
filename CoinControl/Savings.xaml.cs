﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for Savings.xaml
    /// </summary>
    public partial class Savings : Window
    {
        private readonly DatabaseContext _context = new DatabaseContext();
        int loggedInUserId = AuthenticationManager.LoggedInUserId;
        public Savings()
        {
            InitializeComponent();
            LoadIncome();
        }

        public void LoadIncome()
        {
            var user = _context.User.FirstOrDefault(u => u.User_ID == loggedInUserId);
            if (user != null)
            {
                userName.Text = $"{user.Name}";
                emailUser.Text = $"{user.Email}";
            }

            var incomeData = _context.Income
                .Where(e => e.User_ID == loggedInUserId)
                .ToList();

            incomeDataGrid.ItemsSource = incomeData;
        }
        private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        {
            Analytics analytics = new Analytics();
            analytics.Show();
            this.Close();
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
            //lmao
        }

        private void NavigateToReports(object sender, RoutedEventArgs e)
        {
            Reports reports = new Reports();
            reports.Show();
            this.Close();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close(); // Close the window if the user clicks Yes
            }
        }

        private void LogoutButton(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private void AddTran_Btn(object sender, RoutedEventArgs e)
        {
            AddIncomeWindow addIncomeWindow = new AddIncomeWindow();
            addIncomeWindow.Show();
        }

        private void DeleteTran_Btn(object sender, RoutedEventArgs e)
        {
            IncomeDB selectedIncome = incomeDataGrid.SelectedItem as IncomeDB;
            if (selectedIncome != null)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    long incomeId = selectedIncome.Income_ID;

                    try
                    {
                        dbContext.Database.ExecuteSqlInterpolated($"EXEC DeleteIncome {incomeId}");
                        LoadIncome();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting income: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an income to delete.");
            }
        }
    }
}
