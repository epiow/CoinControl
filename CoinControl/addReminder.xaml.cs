using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    /// Interaction logic for addReminder.xaml
    /// </summary>
    public partial class addReminder : Window
    {
        public addReminder()
        {
            InitializeComponent();
        }

        private void saveReminder(object sender, RoutedEventArgs e)
        {
            ComboBoxItem chosenCategory = (ComboBoxItem)selectedCategory.SelectedItem;
            string expenseCategory = chosenCategory?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(expenseCategory) || string.IsNullOrWhiteSpace(AmountTextBox.Text) || !datePicked.SelectedDate.HasValue)
            {
                MessageBox.Show("Please enter all details before saving the reminder.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string amountText = AmountTextBox.Text;
            decimal amount;

            if (!decimal.TryParse(amountText, out amount))
            {
                MessageBox.Show("Please enter a valid decimal amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (amount < 0)
            {
                MessageBox.Show("Please enter a non-negative amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime transactionDate = DateTime.Now;
            DateTime due_Date = (DateTime)datePicked.SelectedDate;

            if(due_Date <= transactionDate.AddDays(0)) {
                MessageBox.Show("Please select a due date that is after the current date.");
                return;
            }

            using (DatabaseContext dbContext = new DatabaseContext())
            {
                BudgetingDB budgeted = new BudgetingDB
                {
                    User_ID = AuthenticationManager.LoggedInUserId,
                    Category_Name = expenseCategory,
                    Amount = amount,
                    StartDate = transactionDate,
                    EndDate = due_Date
                };

                dbContext.Budgeting.Add(budgeted);

                dbContext.SaveChanges();
                this.Close();
            }

            if (Application.Current.Windows.OfType<MainDashboard>().Any())
            {
                MainDashboard mainDash = Application.Current.Windows.OfType<MainDashboard>().FirstOrDefault();
                mainDash?.LoadReminders();
                mainDash.reminderCount++;
            }

            AmountTextBox.Clear();
        }

        private void exitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
