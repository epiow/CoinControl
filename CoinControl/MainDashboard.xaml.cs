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
    public partial class MainDashboard : Window
    {
        public MainDashboard()
        {
            InitializeComponent();
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


    }
}
