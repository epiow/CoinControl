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
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace CoinControl
{
    /// <summary>
    /// Interaction logic for AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        public AddIncomeWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Btn(object sender, RoutedEventArgs e)
        {
            decimal amount;
            DateTime transactionDate = DateTime.Now;
            string note = NoteBox.Text;

            if (string.IsNullOrWhiteSpace(AmountTextBox.Text) && string.IsNullOrWhiteSpace(note))
            {
                MessageBox.Show("Please enter the necessary details.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out amount))
            {
                MessageBox.Show("Please enter a valid decimal amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (amount < 0)
            {
                MessageBox.Show("Please enter a non-negative amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ComboBoxItem IncomeSelection = (ComboBoxItem)selectedIncome.SelectedItem;
            string incomeName = IncomeSelection?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(incomeName))
            {
                MessageBox.Show("Please select an income source.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (DatabaseContext dbContext = new DatabaseContext())
            {

                IncomeDB income = new IncomeDB
                {
                    User_ID = AuthenticationManager.LoggedInUserId,
                    Source = incomeName,
                    Amount = amount,
                    Note = note,
                    Trans_Datetime = transactionDate
                };

                dbContext.Income.Add(income);

                dbContext.SaveChanges();

                var userIDParameter = new SqlParameter("@UserID", AuthenticationManager.LoggedInUserId);
                var incomeAmountParameter = new SqlParameter("@IncomeAmount", amount);
                dbContext.Database.ExecuteSqlRaw("AddIncomeToBalance @UserID, @IncomeAmount", userIDParameter, incomeAmountParameter);
            }

            this.Close();

            if (Application.Current.Windows.OfType<Savings>().Any())
            {
                Savings income = Application.Current.Windows.OfType<Savings>().FirstOrDefault();
                income?.LoadIncome();
            }

            AmountTextBox.Clear();
            NoteBox.Clear();
        }

        private void exitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
