﻿using Microsoft.EntityFrameworkCore;
using System;

namespace CoinControl
{

    public class DatabaseContext : DbContext
    {
        //change data source here!!!
        private string ConnectionString = "Data Source=EPIOW\\SQLEXPRESS;Initial Catalog=CoinControl;Integrated Security=True;Trust Server Certificate=True";

        public DbSet<ExpenseDB> Expense { get; set; }
        public DbSet<IncomeDB> Income { get; set; }
        public DbSet<UserDB> User { get; set; }
        public DbSet<CategoriesDB> Categories { get; set; }
        public DbSet<BudgetingDB> Budgeting { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseDB>()
            .HasKey(e => e.Payment_ID);
            modelBuilder.Entity<IncomeDB>()
            .HasKey(e => e.Income_ID);
            modelBuilder.Entity<UserDB>()
            .HasKey(e => e.User_ID);
            modelBuilder.Entity<CategoriesDB>()
            .HasKey(e => e.Category_ID);
            modelBuilder.Entity<BudgetingDB>()
            .HasKey(e => e.Budget_ID);
        }
    }

    public class ExpenseDB
    {
        public long Payment_ID { get; set; }
        public long User_ID { get; set; }
        public long Category_ID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public string Payment_Method { get; set; }
        public string Trans_Datetime { get; set; }
        // Add other properties of the Expense table here
    }
    public class IncomeDB
    {
        public long Income_ID { get; set; }
        public long User_ID { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public string Source { get; set; }
        public string Trans_Datetime { get; set; }
    }
    public class UserDB
    {
        public long User_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class CategoriesDB
    {
        public long Category_ID { get; set; }
        public long User_ID { get; set; }
        public string CategoryName { get; set; }
    }
    public class BudgetingDB
    {
        public long Budget_ID { get; set; }
        public long User_ID { get; set; }
        public long Category_ID { get; set; }
        public decimal Amount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}