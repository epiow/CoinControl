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
            decimal amount = decimal.Parse(AmountTextBox.Text);
            DateTime transactionDate = DateTime.Now;
            string note = NoteBox.Text;

            ComboBoxItem CategorySelection= (ComboBoxItem)selectedCategory.SelectedItem;
            string categoryName = CategorySelection.Content.ToString();

            ComboBoxItem PaymentSelection = (ComboBoxItem)selectedPayment.SelectedItem;
            string paymentMethod = PaymentSelection.Content.ToString();

            // Create an instance of DatabaseContext
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

            AmountTextBox.Clear();
            NoteBox.Clear();
        }

    }
}
