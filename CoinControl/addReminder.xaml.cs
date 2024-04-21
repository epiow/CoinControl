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
            string expenseCategory = chosenCategory.Content.ToString();

            decimal amount = decimal.Parse(AmountTextBox.Text);

            DateTime transactionDate = DateTime.Now;
            DateTime due_Date = (DateTime)datePicked.SelectedDate;

            if(due_Date <= transactionDate.AddDays(1)) {
                MessageBox.Show("Please select a due date that is after the current date.");
                return;
                //addReminder reminder = new addReminder();
                //reminder.Show();

                //this.Close();
            }
            // Create an instance of DatabaseContext
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                BudgetingDB budgeted = new BudgetingDB
                {
                    User_ID = AuthenticationManager.LoggedInUserId,
                    // Category_Name = expenseCategory,
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
    }
}
