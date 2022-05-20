using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.ViewsModels.Commands;

namespace ToDoList.ViewsModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ToDoListDBContext _DBcontext;
        private User admin;


        #region Settings
        public bool IsDarkModeEnabled { get; set; }
        public int WindowWidth { get; set; }
        #endregion



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

        #region TaskLists

        private string _newTaskListTitle;
        public string NewTaskListTitle
        {
            get { return _newTaskListTitle; }
            set
            {
                _newTaskListTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNewTaskListCommand { get; }
        private void OnAddNewTaskListCommandExecuted(object sender)
        {
            TasksList newTaskList = new TasksList()
            {
                Title = NewTaskListTitle,
                Category = _defaultCategory
            };

            _DBcontext.TasksLists.Add(newTaskList);
            _DBcontext.SaveChanges();
            NewTaskListTitle = string.Empty;
        }
        private bool CanAddNewTaskListCommandExecute(object sender)
        {
            return string.IsNullOrEmpty(NewTaskListTitle) ? false : true;
        }

        private ObservableCollection<TasksList> _taskListsWithoutCategory;
        public ObservableCollection<TasksList> TaskListWithoutCategory
        {
            get
            {
                return _taskListsWithoutCategory;
            }
            set
            {
                _taskListsWithoutCategory = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Categories
        private Category _defaultCategory;
        private string _newCategoryTitile;
        public string NewCategoryTitle
        {
            get { return _newCategoryTitile; }
            set
            {
                _newCategoryTitile = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddNewCategoryCommand { get; }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> TreeViewCategories
        {
            get
            {
                return _categories;
            }
            set
            {
                //if (_categories != null)
                //{
                //    foreach (var item in _categories)
                //    {
                //        item.PropertyChanged -= PropertyChanged;
                //    }
                //}

                //if (value != null)
                //{
                //    foreach (var item in value)
                //    {
                //        item.PropertyChanged += PropertyChanged;
                //    }
                //}
                _categories = value;
                OnPropertyChanged();
            }
        }

        private void OnAddNewCategoryCommandExecuted(object sender)
        {
            Category newCategory = new Category()
            {
                Title = NewCategoryTitle,
                User = admin
            };

            _DBcontext.Categories.Add(newCategory);
            _DBcontext.SaveChanges();
            TreeViewCategories.Add(newCategory);
            NewCategoryTitle = string.Empty;
            OnPropertyChanged();
        }
        private bool CanAddNewCategoryCommandExecute(object sender)
        {
            return string.IsNullOrEmpty(NewCategoryTitle) ? false : true;
        }

        #endregion

        public MainViewModel(ToDoListDBContext dBcontext)
        {
            _DBcontext = dBcontext;

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

            _defaultCategory = new Category()
            {
                Title = "Default Category",
                User = admin
            };
            TasksList defaultTaskList = new TasksList()
            {
                Title = "Default task list",
                Category = _defaultCategory,
            };
            Task defaultTask = new Task()
            {
                Content = "This is a default task",
                CompleteDate = new System.DateTime(2025, 11, 04, 04, 00, 00),
                IsCompleted = false,
                TaskList = defaultTaskList
            };


            _DBcontext.Users.Add(admin);
            _DBcontext.UserSettings.Add(defaultSettings);
            _DBcontext.Categories.Add(_defaultCategory);
            _DBcontext.TasksLists.Add(defaultTaskList);
            _DBcontext.Tasks.Add(defaultTask);


            //_context.Categories.Where(x => x.UserId == 1).ToList();

            //var a = (from Category in _context.Categories
            //        where Category.UserId == 1
            //        select Category.Title).ToList();

            _DBcontext.SaveChanges();
            #endregion

            TreeViewCategories = new ObservableCollection<Category>(dBcontext.Categories.Include(c => c.TaskLists).Where(c => c.UserId == admin.Id).ToList());
            SelectedTaskListItems = new ObservableCollection<Task>(dBcontext.Tasks.ToList());
            AddNewCategoryCommand = new LambdaCommand(OnAddNewCategoryCommandExecuted, CanAddNewCategoryCommandExecute);
            AddNewTaskListCommand = new LambdaCommand(OnAddNewTaskListCommandExecuted, CanAddNewTaskListCommandExecute);
            //WindowWidth = _DBcontext.UserSettings.First(u=> u.UserId==admin.Id).WindowWidth;
            WindowWidth = 1200;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
