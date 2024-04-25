using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CoinControl
{
    /// <summary>
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
    /// 

    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Btn(object sender, RoutedEventArgs e)
        {
            // Get the values from the input fields
            string amountText = AmountTextBox.Text.Trim();
            string note = NoteBox.Text.Trim();

            // Check if all input fields are empty
            if (string.IsNullOrWhiteSpace(amountText) || string.IsNullOrWhiteSpace(note) || selectedCategory.SelectedItem == null || selectedPayment.SelectedItem == null)
            {
                MessageBox.Show("Please enter the necessary details.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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

            ComboBoxItem CategorySelection = (ComboBoxItem)selectedCategory.SelectedItem;
            string categoryName = CategorySelection?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Please select a category.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ComboBoxItem PaymentSelection = (ComboBoxItem)selectedPayment.SelectedItem;
            string paymentMethod = PaymentSelection?.Content?.ToString();

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                MessageBox.Show("Please select a payment method.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime transactionDate = DateTime.Now;

            using (DatabaseContext dbContext = new DatabaseContext())
            {
                
                ExpenseDB expense = new ExpenseDB
                {
                    User_ID = AuthenticationManager.LoggedInUserId,
                    Amount = amount,
                    Note = note,
                    Payment_Method = paymentMethod,
                    Trans_Datetime = transactionDate,
                    Category_Name = categoryName
                };

                dbContext.Expense.Add(expense);

                dbContext.SaveChanges();

                var userIDParameter = new SqlParameter("@UserID", AuthenticationManager.LoggedInUserId);
                var expenseAmountParameter = new SqlParameter("@ExpenseAmount", amount);
                dbContext.Database.ExecuteSqlRaw("EXEC DeductExpenseFromBalance @UserID, @ExpenseAmount", userIDParameter, expenseAmountParameter);
            }

            this.Close();

            if (Application.Current.Windows.OfType<Expenses>().Any())
            {
                Expenses expensesWindow = Application.Current.Windows.OfType<Expenses>().FirstOrDefault();
                expensesWindow?.LoadExpenses(); 
            }
        }

        private void exitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
