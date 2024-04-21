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
            // Get the values from the input fields
            decimal amount = decimal.Parse(AmountTextBox.Text);
            DateTime transactionDate = DateTime.Now;
            string note = NoteBox.Text;

            ComboBoxItem IncomeSelection = (ComboBoxItem)selectedIncome.SelectedItem;
            string incomeName = IncomeSelection.Content.ToString();

            // Create an instance of DatabaseContext
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
                this.Close();
            }

            if (Application.Current.Windows.OfType<MainDashboard>().Any())
            {
                MainDashboard mainDash = Application.Current.Windows.OfType<MainDashboard>().FirstOrDefault();
                mainDash?.LoadReminders();
            }

            AmountTextBox.Clear();
            NoteBox.Clear();
        }
    }
}
