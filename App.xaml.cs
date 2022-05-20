using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ToDoList.Views;
using ToDoList.ViewsModels;
using ToDoList.ViewsModels.Commands;

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
            services.AddSingleton<LambdaCommand>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = diContainer.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
