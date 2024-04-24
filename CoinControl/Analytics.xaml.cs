using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace CoinControl
{
    public partial class Analytics : Window
    {
        public Analytics()
        {
            InitializeComponent();
            DataContext = new AnalyticsViewModel();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close(); // Close the window if the user clicks Yes
            }
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainDashboard mainDashboard = new MainDashboard();
            mainDashboard.Show();
            this.Close();
        }
        private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        {

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

    public class AnalyticsViewModel
    {
        public List<ColumnSeries> IncomeChartSeries { get; set; }
        public List<ColumnSeries> ExpenseChartSeries { get; set; }
        public List<LineSeries> LineChartSeries { get; set; }
        public List<string> ChartLabels { get; set; }

        public AnalyticsViewModel()
        {
            IncomeChartSeries = new List<ColumnSeries>();
            ExpenseChartSeries = new List<ColumnSeries>();
            LineChartSeries = new List<LineSeries>();
            ChartLabels = new List<string>();

            // Retrieve income data by month from the database
            using (var context = new DatabaseContext())
            {
                var incomeByMonth = context.Income
                    .GroupBy(income => income.Trans_Datetime.Month)
                    .Select(group => new { Month = group.Key, TotalAmount = group.Sum(income => income.Amount) })
                    .OrderBy(item => item.Month)
                    .ToList();

                foreach (var item in incomeByMonth)
                {
                    IncomeChartSeries.Add(new ColumnSeries
                    {
                        Title = "Income",
                        Values = new ChartValues<double>(new[] { (double)item.TotalAmount }),
                        Fill = Brushes.LightGreen
                    });
                    ChartLabels.Add(new DateTime(2024, 04, 23).ToString("MMMM"));
                }

                var expensesByDate = context.Expense
                    .OrderBy(expense => expense.Trans_Datetime)
                    .ToList();

                var lineSeries = new LineSeries
                {
                    Title = "Expenses over Time",
                    Values = new ChartValues<double>(),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10
                };

                foreach (var expense in expensesByDate)
                {
                    lineSeries.Values.Add((double)expense.Amount);
                    ChartLabels.Add(expense.Trans_Datetime.ToString("MMMM dd, yyyy"));
                }

                LineChartSeries.Add(lineSeries);
            }
        }
    }
}
