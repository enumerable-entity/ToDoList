using Microsoft.EntityFrameworkCore;
using System;
using ToDoList.Models;

namespace ToDoList
{
    public class ToDoListContext : DbContext
    {
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemList> ItemLists { get; set; }
        public DbSet<User> Users { get; set; }
        public string DbPath { get; }

        public ToDoListContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "todolist.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                                => options.UseSqlite($"Data Source={DbPath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Category>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<ListItem>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<ItemList>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Category>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Categories)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<ItemList>()
                .HasOne(pt => pt.Category)
                .WithMany(p => p.Lists)
                .HasForeignKey(pt => pt.CategoryId);

            modelBuilder.Entity<ListItem>()
                .HasOne(pt => pt.ItemList)
                .WithMany(p => p.Items)
                .HasForeignKey(pt => pt.ItemListId);

            //modelBuilder.Entity<Order>().Property(t => t.OrderDate).IsRequired();
        }
    }
}