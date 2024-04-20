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
            string description = DescriptionTextBox.Text;
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
                    Category_Name = categoryName,
                    Amount = amount,
                    Note = note,
                    Payment_Method = paymentMethod,
                    Trans_Datetime = transactionDate
                };

                dbContext.Expense.Add(expense);

                dbContext.SaveChanges();
                Expenses expenses = new Expenses();
                expenses.Show();
                this.Close();
            }

            DescriptionTextBox.Clear();
            AmountTextBox.Clear();
            NoteBox.Clear();
        }

        /*
        using (var context = new BloggingContext())
        {
            var blog = new Blog { Url = "http://example.com" };
            context.Blogs.Add(blog);
            context.SaveChanges();
        }
        */
    }
}
