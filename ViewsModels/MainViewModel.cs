using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ToDoList.Models;

namespace ToDoList.ViewsModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ToDoListDBContext DBcontext;
        private User admin;
        

        public Category NewCategory { get; set; }
        public TasksList NewTaskList { get; set; }
        public Task NewTask { get; set; }

        #region Categories
        public ICommand AddCategoryCommand { get; set; }


        private ObservableCollection<Task> _selectetTaskListItems;
        public ObservableCollection<Task> SelectedTaskListItems
        {
            get
            {
                return _selectetTaskListItems;
            }
            set
            {

                if (_selectetTaskListItems != null)
                {
                    foreach (var item in _selectetTaskListItems)
                    {
                        item.PropertyChanged -= PropertyChanged;
                    }
                }

                if (value != null)
                {
                    foreach (var item in value)
                    {
                        item.PropertyChanged += PropertyChanged;
                    }
                }
                _selectetTaskListItems = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get
           {
                return _categories;
            }
            set
            {
                if (_categories != null)
                {
                    foreach (var item in _categories)
                    {
                        item.PropertyChanged -= PropertyChanged;
                    }
                }

                if (value != null)
                {
                    foreach (var item in value)
                    {
                        item.PropertyChanged += PropertyChanged;
                    }
                }
                _categories = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            #region default user
            admin = new User()
            {
                LoginName = "admin",
                Password = "admin"
            };
            UserSettings defaultSettings = new UserSettings()
            {
                WindowHeight = 900,
                WindowWidth = 1200,
                DarkMode = true,
                GridSplitterPosition = 200,
                User = admin
            };

            Category defaultCategory = new Category()
            {
                Title = "Default Category",
                User = admin
            };
            TasksList defaultTaskList = new TasksList()
            {
                Name = "Default task list",
                Category = defaultCategory,
            };
            Task defaultTask = new Task()
            {
                Content = "This is a default task",
                CompleteDate = new System.DateTime(2025, 11, 04, 04, 00, 00),
                IsCompleted = false,
                TaskList = defaultTaskList
            };


            DBcontext.Users.Add(admin);
            DBcontext.UserSettings.Add(defaultSettings);
            DBcontext.Categories.Add(defaultCategory);
            DBcontext.TasksLists.Add(defaultTaskList);
            DBcontext.Tasks.Add(defaultTask);


            //_context.Categories.Where(x => x.UserId == 1).ToList();

            //var a = (from Category in _context.Categories
            //        where Category.UserId == 1
            //        select Category.Title).ToList();

            DBcontext.SaveChanges();
            #endregion
        }

        public MainViewModel(ToDoListDBContext dBcontext)
        {
            Categories = new ObservableCollection<Category>(dBcontext.Categories.Include(c => c.TaskLists).ToList());
            SelectedTaskListItems = new ObservableCollection<Task>(dBcontext.Tasks.ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            Categories.Add(new Category()
            {
                Title =
                NewCategory.Title,
                User = admin
            });

            OnPropertyChanged();
            
        }
    }
}
