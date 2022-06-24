using Microsoft.EntityFrameworkCore;
using System;
using ToDoList.Models;

namespace ToDoList
{
    /// <summary>
    /// Klasa reprezentująca kontekst bazy danych.
    /// ORM maping jest realizowany za pomocą EntityFramework
    /// </summary>
    public class ToDoListDBContext : DbContext
    {
        /// <summary>
        /// Zbór, reprezentujący encje zadań w bazie danych
        /// </summary>
        public DbSet<Task> Tasks { get; set; }
        /// <summary>
        /// Zbór, reprezentujący encje kategorii w bazie danych
        /// </summary>
        public DbSet<Category> Categories { get; set; }
        /// <summary>
        /// Zbór, reprezentujący encje listów zadań w bazie danych
        /// </summary>
        public DbSet<TasksList> TasksLists { get; set; }
        /// <summary>
        /// Zbór, reprezentujący encje użytkowików w bazie danych
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Zbór, reprezentujący encje ustawień użytkowników w bazie danych
        /// </summary>
        public DbSet<UserSettings> UserSettings { get; set; }
        /// <summary>
        /// Miesce lokalizowania bazy danych aplikacji
        /// </summary>
        public string DbPath { get; }

        /// <summary>
        /// Konstruktor klasy DB contekstu.
        /// Inicjalizowane jest miejsce przechowywania bazy danych
        /// </summary>
        public ToDoListDBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "todolist.db");
        }

        /// <summary>
        /// Metoda inicjalizująca używanie jako servera bazy danych - biblioteki SQLite
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                                => options.UseSqlite($"Data Source={DbPath}");
        /// <summary>
        /// Mapowanie encji aplikacji na bazę danych
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                .HasForeignKey(tl => tl.CategoryId)
                .IsRequired(false);

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
                .HasForeignKey<UserSettings>(pt => pt.UserId);

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                LoginName = "admin",
                Password = "admin",
                IsAuthenticated = true

            });

            modelBuilder.Entity<UserSettings>().HasData(new UserSettings()
            {
                Id = 1,
                WindowHeight = 900,
                WindowWidth = 1200,
                DarkMode = true,
                GridSplitterPosition = 200,
                UserId = 1
            });

            modelBuilder.Entity<Category>().HasData(new Category()
            {
                Id = 1,
                Title = "Default category",
                UserId = 1
            });

            modelBuilder.Entity<TasksList>().HasData(new TasksList()
            {
                Id = 1,
                Title = "Default task list",
                CategoryId = 1

            });

            modelBuilder.Entity<TasksList>().HasData(new TasksList()
            {
                Id = 2,
                Title = "My day",

            });

            modelBuilder.Entity<Task>().HasData(new Task()
            {
                Id = 1,
                Content = "This is a default task",
                CompleteDate = new System.DateTime(2025, 11, 04, 04, 00, 00),
                IsCompleted = false,
                TaskListId = 1

            });
        }


    }
}