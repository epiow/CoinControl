using Microsoft.EntityFrameworkCore;
using System;

namespace CoinControl
{
    public class ExpenseContext : DbContext
    {
        public DbSet<Expense> Expense { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //change nalang here yung data source based sa server na ginagamit nyo locally
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-A9L7U7HJ\\SQL2022TRAINING;Initial Catalog=CoinControl;Integrated Security=True;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .HasKey(e => e.Payment_ID);
        }
    }

    public class Expense
    {
        public long Payment_ID { get; set; }
        public long User_ID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public string Payment_Method { get; set; }
        // Add other properties of the Expense table here
    }
}