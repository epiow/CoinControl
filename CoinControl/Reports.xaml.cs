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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        private DatabaseContext dbContext;
        private readonly DatabaseContext _context = new DatabaseContext();
        private long currentUserId = AuthenticationManager.LoggedInUserId;

        public Reports()
        {
            InitializeComponent();
            dbContext = new DatabaseContext();
            LoadData();
        }

        private void LoadData()
        {
            var user = _context.User.FirstOrDefault(u => u.User_ID == currentUserId);
            if (user != null)
            {
                userName.Text = $"{user.Name}";
                emailUser.Text = $"{user.Email}";
            }

            List<Transaction> transactions = new List<Transaction>();
            List<ExpenseDB> expenses = dbContext.Expense.Where(e => e.User_ID == currentUserId).ToList();
            List<IncomeDB> income = dbContext.Income.Where(i => i.User_ID == currentUserId).ToList();

            foreach (var expense in expenses)
            {
                transactions.Add(new Transaction
                {
                    Type = "Expense",
                    Amount = expense.Amount,
                    Note = expense.Note,
                    Trans_Datetime = expense.Trans_Datetime
                });
            }

            foreach (var inc in income)
            {
                transactions.Add(new Transaction
                {
                    Type = "Income",
                    Amount = inc.Amount,
                    Note = inc.Note,
                    Trans_Datetime = inc.Trans_Datetime
                });
            }

            dataGridTransactions.ItemsSource = transactions;
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
        }
        private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        {
            Analytics analytics = new Analytics();
            analytics.Show();
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
            Close();
        }

    }

}
