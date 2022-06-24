using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.ViewsModels.Commands;

/// ViewModele aplikacji, według wzorca MVVM
namespace ToDoList.ViewsModels
{
    /// <summary>
    /// ViewModel widoku MainWindow według wzorca MVVM
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Wydarzenie zmiany stanu ViewModel'u
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metoda wywołująca wydarzenie o zmianie stanu ViewModel'u
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        private ToDoListDBContext _DBcontext { get; set; }

        /// <summary>
        /// Zalogowany użytkownik
        /// </summary>
        private User AuthenticatedUser { get; set; }

        /// <summary>
        /// Ustawienia zalogowanego użytkownika
        /// </summary>
        public UserSettings UserSettings { get; set; }

        #region Settings

        private bool _isDarkModeEanbled;
        /// <summary>
        /// Property dla bindingu stylu programu
        /// </summary>
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
        /// <summary>
        /// Metoda zminiająca kolor programu
        /// </summary>
        private void SwitchTheme()
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

        private int _windowWidth;

        /// <summary>
        /// Szerokość głównego okna
        /// </summary>
        public int WindowWidth
        {
            get
            {
                return _windowWidth;
            }
            set
            {
                _windowWidth = value;
                UserSettings.WindowWidth = _windowWidth;
                _DBcontext.SaveChanges();
            }
        }

        private int _windowHeight;

        /// <summary>
        /// Wysokość głównego okna
        /// </summary>
        public int WindowHeight
        {
            get
            {
                return _windowHeight;
            }
            set
            {
                _windowHeight = value;
                UserSettings.WindowHeight = _windowHeight;
                _DBcontext.SaveChanges();
            }
        }


        private GridLength _spliterPosition;
        /// <summary>
        /// Położenie rozdzielacza
        /// </summary>
        public GridLength SpliterPosition
        {
            get
            {
                return _spliterPosition;
            }
            set
            {
                _spliterPosition = value;
                UserSettings.GridSplitterPosition = (int)SpliterPosition.Value;
                _DBcontext.SaveChanges();
            }
        }

        #endregion

        #region TaskLists

        /// <summary>
        /// Identyfikator wybranej listy zadań
        /// </summary>
        private int _selectedTasksListId;
        /// <summary>
        /// Wybrany element na TreeView
        /// </summary>
        public object SelectedTreeViewItem { get; set; }

        private ICommand _selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand
        {
            get
            {
                if (_selectedItemChangedCommand == null)
                    _selectedItemChangedCommand = new RelayCommand<object>(selectedItem =>
                    {
                        SelectedTreeViewItemLoadTasks(selectedItem);
                        SelectedTreeViewItem = selectedItem;
                    });
                return _selectedItemChangedCommand;
            }
        }
        /// <summary>
        /// Pobira zadania dla wybranej listy zadań
        /// </summary>
        /// <param name="selectedItem">Lista zadań, dla której należy pobrać zadania</param>
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

        private string _editedTasksListTitile;
        /// <summary>
        /// Nowa nazwa listy zadań
        /// </summary>
        public string EditedTasksListTitile
        {
            get
            {
                return _editedTasksListTitile;
            }
            set
            {
                _editedTasksListTitile = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Task> _selectetTaskListItems;
        /// <summary>
        /// Zadania dla wybranej listy zadań
        /// </summary>
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

        private ICommand _renameTreeViewItemCommand;

        /// <summary>
        /// Komenda edytowania kategorii lub listy zadań
        /// </summary>
        public ICommand RenameTreeViewItemCommand
        {
            get
            {
                if (_renameTreeViewItemCommand == null)
                    _renameTreeViewItemCommand = new RelayCommand<object>(selectedItem =>
                    {

                        if (selectedItem is Category)
                        {
                            Category category = (Category)selectedItem;
                            category.IsInEditMode = true;
                        }
                        else
                        {
                            TasksList taskList = (TasksList)selectedItem;
                            taskList.IsInEditMode = true;
                        }

                        TreeViewCategoriesView.Refresh();

                    });
                return _renameTreeViewItemCommand;
            }
        }

        private ICommand _finishRenameTreeViewItemCommand;

        /// <summary>
        /// Komenda akceptująca edytowanie kategorii lub listy zadań
        /// </summary>
        public ICommand FinishRenameTreeViewItemCommand
        {
            get
            {
                if (_finishRenameTreeViewItemCommand == null)
                    _finishRenameTreeViewItemCommand = new RelayCommand<object>(selectedItem =>
                    {
                        if (selectedItem is Category)
                        {
                            Category category = (Category)selectedItem;
                            category.Title = EditedCategoryTitile;
                            category.IsInEditMode = false;

                            EditedCategoryTitile = null;
                        }
                        else
                        {
                            TasksList taskList = (TasksList)selectedItem;
                            taskList.Title = EditedTasksListTitile;
                            taskList.IsInEditMode = false;
                            EditedTasksListTitile = null;

                        }
                        _DBcontext.SaveChanges();
                        TreeViewCategoriesView.Refresh();
                    });
                return _finishRenameTreeViewItemCommand;
            }
        }

        private string _newTaskListTitle;

        /// <summary>
        /// Nowa nazwa listy zadań
        /// </summary>
        public string NewTaskListTitle
        {
            get { return _newTaskListTitle; }
            set
            {
                _newTaskListTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Komenda dodawania nowej listy zadań
        /// </summary>
        public ICommand AddNewTaskListCommand { get; set; }
        private void OnAddNewTaskListCommandExecuted(object sender)
        {
            TasksList newTaskList;

            if (SelectedTreeViewItem is Category)
            {
                newTaskList = new TasksList()
                {
                    Title = NewTaskListTitle,
                    Category = (Category)SelectedTreeViewItem
                };
            }
            else if (SelectedTreeViewItem is TasksList)
            {
                newTaskList = new TasksList()
                {
                    Title = NewTaskListTitle,
                    Category = ((TasksList)SelectedTreeViewItem).Category
                };
            }
            else
            {
                newTaskList = new TasksList()
                {
                    Title = NewTaskListTitle,
                    Category = _defaultCategory
                };
            }

            _DBcontext.TasksLists.Add(newTaskList);
            _DBcontext.SaveChanges();
            NewTaskListTitle = string.Empty;
            TreeViewCategoriesView.Refresh();
        }
        private bool CanAddNewTaskListCommandExecute(object sender)
        {
            return string.IsNullOrEmpty(NewTaskListTitle) ? false : true;
        }

        private ICommand _deleteTreeViewItemCommand;

        /// <summary>
        /// Komenda usuwania kategorii lub listy zadań
        /// </summary>
        public ICommand DeleteTreeViewItemCommand
        {
            get
            {
                if (_deleteTreeViewItemCommand == null)
                    _deleteTreeViewItemCommand = new RelayCommand<object>(selectedItem =>
                    {
                        if (SelectedTreeViewItem is Category)
                        {
                            _DBcontext.Categories.Remove((Category)SelectedTreeViewItem);
                            TreeViewCategories.Remove((Category)SelectedTreeViewItem);
                        }
                        else
                        {
                            _DBcontext.TasksLists.Remove((TasksList)SelectedTreeViewItem);

                        }

                        _DBcontext.SaveChanges();
                        SelectedTaskListItems.Clear();
                        TreeViewCategoriesView.Refresh();
                        SelectedTasksView.Refresh();
                    });
                return _deleteTreeViewItemCommand;
            }
        }

        #endregion

        #region Categories

        /// <summary>
        /// Domyślna kategoria dla listów zadań
        /// </summary>
        private Category _defaultCategory;

        private string _editedCategoryTitile;

        /// <summary>
        /// Nowa nazwa edytowanej kategorii
        /// </summary>
        public string EditedCategoryTitile
        {
            get
            {
                return _editedCategoryTitile;
            }
            set
            {
                _editedCategoryTitile = value;
                OnPropertyChanged();
            }
        }

        private string _newCategoryTitile;

        /// <summary>
        /// Nazwa nowej kategorii
        /// </summary>
        public string NewCategoryTitle
        {
            get { return _newCategoryTitile; }
            set
            {
                _newCategoryTitile = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Category> _categories;
        /// <summary>
        /// Kategorie danego użytkownika
        /// </summary>
        public ObservableCollection<Category> TreeViewCategories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Object widoku dla manupulowania elementami na UI
        /// </summary>
        private ICollectionView TreeViewCategoriesView { get; set; }

        /// <summary>
        /// Komenda dodania nowej kategorii 
        /// </summary>
        public ICommand AddNewCategoryCommand { get; set; }
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
        /// <summary>
        /// Zawartość nowo tworzonego zadania
        /// </summary>
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

        private DateTime? _newTaskDate;

        /// <summary>
        /// Data nowo tworzonego zadania
        /// </summary>
        public DateTime? NewTaskDate
        {
            get { return _newTaskDate; }
            set
            {
                _newTaskDate = value;
                OnPropertyChanged();
            }
        }

        private string _editedTaskContent;

        /// <summary>
        /// Nowo wprowadzana zawatrość edytowanego zadania
        /// </summary>
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

        /// <summary>
        /// Wybrane zadanie na liście UI
        /// </summary>
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

        /// <summary>
        /// Widok dla manupulowania zadaniami na UI
        /// </summary>
        public ICollectionView SelectedTasksView { get; set; }

        /// <summary>
        /// Komenda dodania nowego zadania
        /// </summary>
        public ICommand AddNewTaskCommand { get; set; }
        private void OnAddNewTaskCommandExecuted(object sender)
        {
            Task newTask = new Task()
            {
                Content = NewTaskContent,
                IsCompleted = false,
                TaskListId = _selectedTasksListId,
                CompleteDate = NewTaskDate
            };
            NewTaskDate = null;
            _DBcontext.Tasks.Add(newTask);
            _DBcontext.SaveChanges();
            SelectedTaskListItems.Add(newTask);
            NewTaskContent = string.Empty;
            OnPropertyChanged();
        }
        private bool CanAddNewTaskCommandExecute(object sender)
        {
            return string.IsNullOrEmpty(NewTaskContent) || _selectedTasksListId == 2 ? false : true;
        }

        private ICommand _changeTaskStatusCommand;

        /// <summary>
        /// Komenda zmiany statusu zadania
        /// </summary>
        public ICommand ChangeTaskStatusCommand
        {
            get
            {
                if (_changeTaskStatusCommand == null)
                    _changeTaskStatusCommand = new RelayCommand<object>(selectedItem => OnChangeTaskStatusCommand(selectedItem));
                return _changeTaskStatusCommand;
            }
        }

        private void OnChangeTaskStatusCommand(object sender)
        {
            var selectedTask = (Task)sender;
            selectedTask.IsCompleted = !selectedTask.IsCompleted;
            _DBcontext.SaveChanges();
            SelectedTasksView.Refresh();
        }

        private ICommand _deleteTaskCommand;

        /// <summary>
        /// Komenda usuwania zadania
        /// </summary>
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

        /// <summary>
        /// Komenda zmiany zawartości zadania
        /// </summary>
        public ICommand RenameTaskCommand
        {
            get
            {
                if (_renameTaskCommand == null)
                    _renameTaskCommand = new RelayCommand<Task>(selectedItem =>
                    {
                        if (selectedItem != null)
                        {
                            var selectedTask = (Task)selectedItem;
                            if (!SelectedTaskListItems.Any(tl => tl.IsInEditMode == true)) selectedTask.IsInEditMode = true;
                            EditedTaskContent = selectedTask.Content;
                            SelectedTasksView.Refresh();
                        }
                    });
                return _renameTaskCommand;
            }
        }

        private ICommand _finishRenameTaskCommand;

        /// <summary>
        /// Komenda akceptująca zmiane zawartości zadania
        /// </summary>
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

        /// <summary>
        /// Property dla bindingu przełącznika 'Show only In progres'
        /// </summary>
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

        /// <summary>
        /// Property dla bindingu sub-stringa dla wyszukiwania zadań
        /// </summary>
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

        /// <summary>
        /// Medoda filtrująca. Zwraca true jeśli objekt spełnia wymagania
        /// </summary>
        /// <param name="obj">Zadanie które będzie filtrowane</param>
        /// <returns></returns>
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

        /// <summary>
        /// Property dla bindingu na selector sortowania
        /// </summary>
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

        /// <summary>
        /// Klasa konwertująca string z widoku na objekt SortDescription
        /// </summary>
        /// <param name="sortFromView">String zawierający potrzebne sortowanie</param>
        /// <returns>Zwraca obiekt SortDescription</returns>
        private SortDescription sortingConverter(string sortFromView)
        {
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

        /// <summary>
        /// Komenda pobierania zadań z dzisiajszą datą
        /// </summary>
        public ICommand ShowMyDayTasksCommand { get; set; }

        private void OnShowMyDayTasksCommandExecuted(object sender)
        {
            _selectedTasksListId = 2;
            SelectedTaskListItems.Clear();
            var load = _DBcontext.Tasks.Where(t => t.CompleteDate == DateTime.Today).ToList();

            foreach (var task in load)
            {
                SelectedTaskListItems.Add(task);
            }

            SelectedTasksView.Refresh();
            OnPropertyChanged();
        }
        #endregion

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dBcontext">Kontekst bazy danych, przekazywany kontenerem DI</param>
        public MainViewModel(ToDoListDBContext dBcontext)
        {
            _DBcontext = dBcontext;
            init();
        }

        /// <summary>
        /// Medoda inicjalizująca stan aplikacji przy urachamianiu
        /// </summary>
        private void init()
        {
            _DBcontext.Database.Migrate();

            _defaultCategory = _DBcontext.Categories.First(c => c.Id == 1);
            AuthenticatedUser = _DBcontext.Users.First<User>(u => u.IsAuthenticated == true);

            // Pobieranie ustawień aplikacji
            UserSettings = _DBcontext.UserSettings.First<UserSettings>(us => us.User == AuthenticatedUser);
            IsDarkModeEnabled = UserSettings.DarkMode;
            WindowWidth = _DBcontext.UserSettings.First(u => u.UserId == AuthenticatedUser.Id).WindowWidth;
            WindowHeight = _DBcontext.UserSettings.First(u => u.UserId == AuthenticatedUser.Id).WindowHeight;
            SpliterPosition = new GridLength(_DBcontext.UserSettings.First(u => u.UserId == AuthenticatedUser.Id).GridSplitterPosition);

            // Ładowanie listy zadań, dana zakończenia których jest dzisiaj
            _selectedTasksListId = 2;
            SelectedTaskListItems = new ObservableCollection<Task>(_DBcontext.Tasks.Where(t => t.CompleteDate == DateTime.Today).ToList());

            //Pobieranie danych dla TreeView
            TreeViewCategories = new ObservableCollection<Category>(_DBcontext.Categories.Include(c => c.TaskLists).Where(c => c.UserId == AuthenticatedUser.Id).ToList());

            // Inicjalizowanie komend
            AddNewCategoryCommand = new LambdaCommand(OnAddNewCategoryCommandExecuted, CanAddNewCategoryCommandExecute);
            AddNewTaskListCommand = new LambdaCommand(OnAddNewTaskListCommandExecuted, CanAddNewTaskListCommandExecute);
            AddNewTaskCommand = new LambdaCommand(OnAddNewTaskCommandExecuted, CanAddNewTaskCommandExecute);
            ShowMyDayTasksCommand = new LambdaCommand(OnShowMyDayTasksCommandExecuted);

            // Pobieranie widoku dla mapulowania elementami na UI
            TreeViewCategoriesView = CollectionViewSource.GetDefaultView(TreeViewCategories);
            SelectedTasksView = CollectionViewSource.GetDefaultView(SelectedTaskListItems);
            SelectedTasksView.Filter = FilterTasks;
            SelectedTasksView.SortDescriptions.Add(new SortDescription(nameof(Task.CompleteDate), ListSortDirection.Descending));

        }
    }
}
