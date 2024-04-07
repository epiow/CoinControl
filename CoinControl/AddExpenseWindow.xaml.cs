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
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
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
            string categoryName = CategoryBox.Text;
            string paymentMethod = PaymentMethodBox.Text;

            // Create an instance of DatabaseContext
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                // Check if the category exists
                CategoriesDB existingCategory = dbContext.Categories
                    .FirstOrDefault(c => c.CategoryName == categoryName && c.User_ID == AuthenticationManager.LoggedInUserId);

                long categoryId;

                if (existingCategory == null)
                {
                    // Insert a new category
                    CategoriesDB newCategory = new CategoriesDB
                    {
                        User_ID = AuthenticationManager.LoggedInUserId,
                        CategoryName = categoryName
                    };
                    dbContext.Categories.Add(newCategory);
                    dbContext.SaveChanges();

                    // Use the newly inserted Category_ID
                    categoryId = newCategory.Category_ID;
                }
                else
                {
                    // Use the existing Category_ID
                    categoryId = existingCategory.Category_ID;
                }

                // Create a new ExpenseDB instance
                ExpenseDB expense = new ExpenseDB
                {
                    User_ID = AuthenticationManager.LoggedInUserId,
                    Category_ID = categoryId,
                    Amount = amount,
                    Note = note,
                    Payment_Method = paymentMethod,
                    Trans_Datetime = transactionDate
                };

                // Add the expense to the context
                dbContext.Expense.Add(expense);

                // Save the changes to the database
                dbContext.SaveChanges();
                Expenses expenses = new Expenses();
                expenses.Show();
                this.Close();
            }

            // Clear the input fields or perform any other necessary actions
            DescriptionTextBox.Clear();
            AmountTextBox.Clear();
            NoteBox.Clear();
            CategoryBox.Clear();
            PaymentMethodBox.Clear();
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
