using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ToDoList.ViewsModels;

namespace ToDoList
{

    public partial class App : Application
    {
        private ServiceProvider diContainer;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            diContainer = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<ToDoListDBContext>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = diContainer.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
