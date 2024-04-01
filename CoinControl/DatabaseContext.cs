using Microsoft.EntityFrameworkCore;
using System;

namespace CoinControl
{

    public class DatabaseContext : DbContext
    {
        //change data source here!!!
        private string ConnectionString = "Data Source=EPIOW\\SQLEXPRESS01;Initial Catalog=CoinControl;Integrated Security=True;Trust Server Certificate=True";

        public DbSet<ExpenseDB> Expense { get; set; }
        public DbSet<SavingsDB> Savings { get; set; }
        public DbSet<LogDB> Log { get; set; }   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseDB>()
            .HasKey(e => e.Payment_ID);
            modelBuilder.Entity<SavingsDB>()
            .HasKey(e => e.Income_ID);
            modelBuilder.Entity<LogDB>()
            .HasKey(e => e.Trans_Category);
        }
    }

    public class ExpenseDB
    {
        public long Payment_ID { get; set; }
        public long User_ID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public string Payment_Method { get; set; }
        // Add other properties of the Expense table here
    }
    public class SavingsDB
    {
        public long Income_ID { get; set; }
        public long User_ID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
    }
    public class LogDB
    {
        public string Trans_Category { get; set; }
        public long Income_ID { get; set; }
        public long User_ID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
    }
}