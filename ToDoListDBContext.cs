using Microsoft.EntityFrameworkCore;
using System;
using ToDoList.Models;

namespace ToDoList
{
    public class ToDoListDBContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TasksList> TasksLists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public string DbPath { get; }

        public ToDoListDBContext()
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
            modelBuilder.Entity<Task>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<TasksList>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<UserSettings>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasMany(pt => pt.Categories)
                .WithOne(p => p.User)
                .HasForeignKey(pt => pt.UserId);


            modelBuilder.Entity<Category>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Categories)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<TasksList>()
                .HasOne(tl => tl.Category)
                .WithMany(p => p.TaskLists)
                .HasForeignKey(tl => tl.CategoryId);

            modelBuilder.Entity<TasksList>()
                .HasMany(tl => tl.Tasks)
                .WithOne(p => p.TaskList)
                .HasForeignKey(tl => tl.Id);

            modelBuilder.Entity<Task>()
                .HasOne(pt => pt.TaskList)
                .WithMany(p => p.Tasks)
                .HasForeignKey(pt => pt.TaskListId);

            modelBuilder.Entity<UserSettings>()
                .HasOne(pt => pt.User)
                .WithOne(p => p.Settings)
                .HasForeignKey<User>(pt => pt.Id);

            

            //modelBuilder.Entity<Order>().Property(t => t.OrderDate).IsRequired();
        }

        
    }
}