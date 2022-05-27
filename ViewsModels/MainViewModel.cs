using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using System;
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
        private User AuthenticatedUser;
        private UserSettings UserSettings { get; set; }

        #region Settings
        public bool IsDarkModeEnabled { get; set; }
        public int WindowWidth { get; set; }
        #endregion

        #region TaskLists
        private int _selectedTasksListId;

        private ICommand _selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand
        {
            get
            {
                if (_selectedItemChangedCommand == null)
                    _selectedItemChangedCommand = new RelayCommand<object>(selectedItem => SelectedTreeViewItemLoadTasks(selectedItem));
                return _selectedItemChangedCommand;
            }
        }

        private void SelectedTreeViewItemLoadTasks(object selectedItem)
        {
            var taskListId = (selectedItem as TasksList)?.Id;
            if (taskListId != null)
            {
                _selectedTasksListId = (int)taskListId;
                SelectedTaskListItems = new ObservableCollection<Task>(_DBcontext.Tasks.Where(t => t.TaskListId == _selectedTasksListId).ToList());
            };
        }

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
                User = AuthenticatedUser
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

        #region Tasks
        private string _newTaskContent;
        public string NewTaskContent
        {
            get
            {
                return _newTaskContent;
            }
            set
            {
                _newTaskContent = value;
                OnPropertyChanged();
            }
        }
        private Task _selectedTask;
        public Task SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddNewTaskCommand { get; }
        private ICommand _changeTaskStatusCommand;
        public ICommand ChangeTaskStatusCommand
        {
            get
            {
                if (_changeTaskStatusCommand == null)
                    _changeTaskStatusCommand = new RelayCommand<object>(selectedItem => OnChangeTaskStatusCommand(selectedItem));
                return _changeTaskStatusCommand;
            }
        }
        private void OnAddNewTaskCommandExecuted(object sender)
        {
            Task newTask = new Task()
            {
                Content = NewTaskContent,
                IsCompleted = false,
                TaskListId = _selectedTasksListId,
                CompleteDate = DateTime.Now
            };

            _DBcontext.Tasks.Add(newTask);
            _DBcontext.SaveChanges();
            SelectedTaskListItems.Add(newTask);
            NewTaskContent = string.Empty;
            OnPropertyChanged();
        }
        private bool CanAddNewTaskCommandExecute(object sender)
        {
            return string.IsNullOrEmpty(NewTaskContent) ? false : true;
        }
        private void OnChangeTaskStatusCommand(object sender)
        {
            //SelectedTask.IsCompleted = !SelectedTask.IsCompleted;
            var selectedTask = (Task)sender;
            selectedTask.IsCompleted = !selectedTask.IsCompleted;
            _DBcontext.SaveChanges();
            OnPropertyChanged("SelectedTaskListItems");
        }
        private bool CanChangeTaskStatusCommandExecute(object sender) => true;

        #endregion


        public ICommand ShowMyDayTasksCommand { get; }

        private void OnShowMyDayTasksCommandExecuted(object sender)
        {
            _selectedTasksListId = Int32.MaxValue;
            SelectedTaskListItems = new ObservableCollection<Task>(_DBcontext.Tasks.Where(t => t.TaskListId == Int32.MaxValue).ToList());
            OnPropertyChanged();
        }



        public MainViewModel(ToDoListDBContext dBcontext)
        {
            _DBcontext = dBcontext;

            AuthenticatedUser = dBcontext.Users.First<User>(u => u.IsAuthenticated == true);

            UserSettings = dBcontext.UserSettings.First<UserSettings>(us => us.User == AuthenticatedUser);

            _selectedTasksListId = Int32.MaxValue;
            SelectedTaskListItems = new ObservableCollection<Task>(dBcontext.Tasks.Where(t => t.TaskListId == Int32.MaxValue).ToList());

            TreeViewCategories = new ObservableCollection<Category>(dBcontext.Categories.Include(c => c.TaskLists).Where(c => c.UserId == AuthenticatedUser.Id).ToList());
            AddNewCategoryCommand = new LambdaCommand(OnAddNewCategoryCommandExecuted, CanAddNewCategoryCommandExecute);
            AddNewTaskListCommand = new LambdaCommand(OnAddNewTaskListCommandExecuted, CanAddNewTaskListCommandExecute);
            AddNewTaskCommand = new LambdaCommand(OnAddNewTaskCommandExecuted, CanAddNewTaskCommandExecute);
            //ChangeTaskStatusCommand = new LambdaCommand(OnChangeTaskStatusCommand, CanChangeTaskStatusCommandExecute);
            ShowMyDayTasksCommand = new LambdaCommand(OnShowMyDayTasksCommandExecuted);
            WindowWidth = _DBcontext.UserSettings.First(u => u.UserId == AuthenticatedUser.Id).WindowWidth;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
