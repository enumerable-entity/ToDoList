﻿using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.ViewsModels.Commands;

namespace ToDoList.ViewsModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ToDoListDBContext _DBcontext;
        private User AuthenticatedUser;
        private UserSettings UserSettings { get; set; }

        #region Settings
        private bool _isDarkModeEanbled;
        public bool IsDarkModeEnabled
        {
            get { return _isDarkModeEanbled; }
            set
            {
                _isDarkModeEanbled = value;
                SwitchTheme();
                UserSettings.DarkMode = IsDarkModeEnabled;
                _DBcontext.SaveChanges();
            }
        }

        public void SwitchTheme()
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            if (_isDarkModeEanbled)
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }
            paletteHelper.SetTheme(theme);
        }


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
                SelectedTaskListItems.Clear();
                var load = _DBcontext.Tasks.Where(t => t.TaskListId == _selectedTasksListId).ToList();

                foreach (var task in load)
                {
                    SelectedTaskListItems.Add(task);
                }
                SelectedTasksView.Refresh();
            }
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


        public ICollectionView SelectedTasksView { get; }




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

        private ICommand _deleteTaskListCommand;
        public ICommand DeleteTaskListCommand
        {
            get
            {
                if (_deleteTaskListCommand == null)
                    _deleteTaskListCommand = new RelayCommand<Task>(selectedItem =>
                    {
                        var taskListToDelete = _DBcontext.TasksLists.FirstOrDefault(tl => tl.Id == _selectedTasksListId);
                        _DBcontext.TasksLists.Remove(taskListToDelete);
                        _DBcontext.SaveChanges();
                        Category category = TreeViewCategories.FirstOrDefault(cat => cat.Id == taskListToDelete.CategoryId);
                        category.TaskLists.Remove(taskListToDelete);


                    });
                return _deleteTaskCommand;
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

        private Visibility _textBlockSelectedTaskVisibility;
        private Visibility _textBoxSelectedTaskVisibility;

        public Visibility TextBlockSelectedTaskVisibility
        {
            get
            {
                return _textBlockSelectedTaskVisibility;
            }
            set
            {
                _textBlockSelectedTaskVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility TextBoxSelectedTaskVisibility
        {
            get
            {
                return _textBoxSelectedTaskVisibility;
            }
            set
            {
                _textBoxSelectedTaskVisibility = value;
                OnPropertyChanged();
            }
        }


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
        private string _editedTaskContent;
        public string EditedTaskContent
        {
            get
            {
                return _editedTaskContent;
            }
            set
            {
                _editedTaskContent = value;
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
            var selectedTask = (Task)sender;
            selectedTask.IsCompleted = !selectedTask.IsCompleted;
            _DBcontext.SaveChanges();
            SelectedTasksView.Refresh();
        }

        private ICommand _deleteTaskCommand;
        public ICommand DeleteTaskCommand
        {
            get
            {
                if (_deleteTaskCommand == null)
                    _deleteTaskCommand = new RelayCommand<Task>(selectedItem =>
                    {
                        _DBcontext.Tasks.Remove(SelectedTask);
                        _DBcontext.SaveChanges();
                        SelectedTaskListItems.Remove(SelectedTask);
                        SelectedTasksView.Refresh();
                    });
                return _deleteTaskCommand;
            }
        }

        private ICommand _renameTaskCommand;
        public ICommand RenameTaskCommand
        {
            get
            {
                if (_renameTaskCommand == null)
                    _renameTaskCommand = new RelayCommand<Task>(selectedItem =>
                    {
                        var selectedTask = (Task)selectedItem;
                        selectedTask.IsInEditMode = true;
                        SelectedTasksView.Refresh();
                    });
                return _renameTaskCommand;
            }
        }
        private ICommand _finishRenameTaskCommand;
        public ICommand FinishRenameTaskCommand
        {
            get
            {
                if (_finishRenameTaskCommand == null)
                    _finishRenameTaskCommand = new RelayCommand<Task>(selectedItem =>
                    {

                        selectedItem.Content = EditedTaskContent;
                        SelectedTask.IsInEditMode = false;

                        SelectedTasksView.Refresh();
                        _DBcontext.SaveChanges();
                        EditedTaskContent = null;


                    });
                return _finishRenameTaskCommand;
            }
        }
        private bool _showOnlyInProgres;
        public bool ShowOnlyInProgres
        {
            get
            {
                return _showOnlyInProgres;
            }
            set
            {
                if (value != _showOnlyInProgres)
                {
                    _showOnlyInProgres = value;
                    OnPropertyChanged();
                    SelectedTasksView.Refresh();
                }

            }
        }



        private string _taskFilterSubString = String.Empty;
        public string TaskFilterSubString
        {
            get { return _taskFilterSubString; }
            set
            {
                if (value != _taskFilterSubString)
                {
                    _taskFilterSubString = value;
                    OnPropertyChanged();
                    SelectedTasksView.Refresh();
                }
            }
        }
        private bool FilterTasks(object obj)
        {
            if (obj is Task task)
            {

                if (!ShowOnlyInProgres)
                {
                    return task.Content.Contains(TaskFilterSubString, StringComparison.InvariantCultureIgnoreCase);
                }
                else
                {
                    return task.Content.Contains(TaskFilterSubString, StringComparison.InvariantCultureIgnoreCase) && task.IsCompleted == !ShowOnlyInProgres;
                }

            }
            return false;
        }

        private string _taskListSelectedSorting;
        public string TaskListSelectedSorting
        {
            get
            {
                return _taskListSelectedSorting;
            }
            set
            {
                if (value != _taskListSelectedSorting)
                {
                    _taskListSelectedSorting = value;
                    OnPropertyChanged();
                    SelectedTasksView.SortDescriptions.Remove(new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Ascending));
                    SelectedTasksView.SortDescriptions.Remove(new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Descending));
                    SelectedTasksView.SortDescriptions.Remove(new SortDescription(nameof(Task.IsCompleted), ListSortDirection.Ascending));

                    SelectedTasksView.SortDescriptions.Add(sortingConverter(TaskListSelectedSorting));
                    SelectedTasksView.Refresh();
                }
            }
        }

        class SortingConverter
        {
            public ListSortDirection SortingDirection { get; set; }
            public SortDescription SortDescription { get; set; }
        }

        private SortDescription sortingConverter(string sortFromView)
        {
            //SortingConverter sortingConverter = new SortingConverter();

            if (sortFromView == "System.Windows.Controls.ComboBoxItem: Date ascending")
            {
                return new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Ascending);
            }
            else if (sortFromView == "System.Windows.Controls.ComboBoxItem: Date descending")
            { return new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Descending); }
            else if (sortFromView == "System.Windows.Controls.ComboBoxItem: Status")
            {
                return new SortDescription(nameof(Task.IsCompleted), ListSortDirection.Ascending);
            }
            return new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Ascending);

        }


        #endregion


        public ICommand ShowMyDayTasksCommand { get; }

        private void OnShowMyDayTasksCommandExecuted(object sender)
        {
            _selectedTasksListId = 2;
            SelectedTaskListItems.Clear();
            var load = _DBcontext.Tasks.Where(t => t.TaskListId == _selectedTasksListId).ToList();

            foreach (var task in load)
            {
                SelectedTaskListItems.Add(task);
            }
            SelectedTasksView.Refresh();
            OnPropertyChanged();
        }

        public MainViewModel(ToDoListDBContext dBcontext)
        {
            _DBcontext = dBcontext;

            dBcontext.Database.Migrate();
            _defaultCategory = dBcontext.Categories.First(c => c.Id == 1);
            AuthenticatedUser = dBcontext.Users.First<User>(u => u.IsAuthenticated == true);

            UserSettings = dBcontext.UserSettings.First<UserSettings>(us => us.User == AuthenticatedUser);
            IsDarkModeEnabled = UserSettings.DarkMode;
            _selectedTasksListId = 2;
            SelectedTaskListItems = new ObservableCollection<Task>(dBcontext.Tasks.Where(t => t.TaskListId == 2).ToList());

            TreeViewCategories = new ObservableCollection<Category>(dBcontext.Categories.Include(c => c.TaskLists).Where(c => c.UserId == AuthenticatedUser.Id).ToList());
            AddNewCategoryCommand = new LambdaCommand(OnAddNewCategoryCommandExecuted, CanAddNewCategoryCommandExecute);
            AddNewTaskListCommand = new LambdaCommand(OnAddNewTaskListCommandExecuted, CanAddNewTaskListCommandExecute);
            AddNewTaskCommand = new LambdaCommand(OnAddNewTaskCommandExecuted, CanAddNewTaskCommandExecute);
            ShowMyDayTasksCommand = new LambdaCommand(OnShowMyDayTasksCommandExecuted);




            WindowWidth = _DBcontext.UserSettings.First(u => u.UserId == AuthenticatedUser.Id).WindowWidth;

            SelectedTasksView = CollectionViewSource.GetDefaultView(SelectedTaskListItems);
            SelectedTasksView.Filter = FilterTasks;
            SelectedTasksView.SortDescriptions.Add(new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Descending));

            TextBlockSelectedTaskVisibility = Visibility.Visible;
            TextBoxSelectedTaskVisibility = Visibility.Collapsed;
        }


    }
}
