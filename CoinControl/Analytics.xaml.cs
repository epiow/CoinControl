using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for Analytics.xaml
    /// </summary>
    public partial class Analytics : Window
    {
        public Analytics()
        {
            InitializeComponent();
            DataContext = new AnalyticsViewModel();
        }
        public class AnalyticsViewModel
        {
            public List<string> ChartLabels { get; set; }
            public List<ColumnSeries> ChartSeries { get; set; }

            public AnalyticsViewModel()
            {
                using (var context = new DatabaseContext())
                {
                    // Retrieve data from the database
                    var expensesByMonth = context.Expense
                        .GroupBy(expense => expense.Trans_Datetime.Month)
                        .Select(group => new { Month = group.Key, TotalAmount = group.Sum(expense => expense.Amount) })
                        .OrderBy(item => item.Month)
                        .ToList();

                    // Populate chart data
                    ChartLabels = expensesByMonth.Select(item => new DateTime(2024, item.Month, 1).ToString("MMMM")).ToList();
                    ChartSeries = new List<ColumnSeries>
                {
                    new ColumnSeries
                    {
                        Title = "Total Expenses",
                        Values = new ChartValues<double>(expensesByMonth.Select(item => (double)item.TotalAmount))
                    }
                };
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close(); // Close the window if the user clicks Yes
            }
        }

        private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        {

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
    }
}
