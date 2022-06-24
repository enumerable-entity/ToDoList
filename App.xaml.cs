using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ToDoList.Views;
using ToDoList.ViewsModels;

/// Główny namespace projektu
namespace ToDoList
{
    /// <summary>
    /// Klasa zawierająca globalne ustawienia aplikacji.
    /// W danej klasi jest inicjalizowany dependency injection container
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private ServiceProvider diContainer;
        /// <summary>
        /// Konstruktor klasy inicjalizujący kontainer
        /// </summary>
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            diContainer = services.BuildServiceProvider();
        }
        /// <summary>
        /// Metoda służąca do dodawania servisów do listy servisów, sterowanych DI kontenerem
        /// </summary>
        /// <param name="services">Kolecja dla umieszczanie servisów, pod opieke DI kontenera</param>
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<ToDoListDBContext>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
        }
        /// <summary>
        /// Metoda uruchamiana jendorazowa przy uruchamianiu aplikacji.
        /// Z DI contenera jest otrzymywany reference na główne okno programu, po czym, dane okno jest wyświetlane
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = diContainer.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
