using FluentNewsApp.ViewModels;
using FluentNewsApp.Views;
using FluentNewsApp.WebCalls;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;

namespace FluentNewsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IServiceCollection _services = new ServiceCollection();
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureServices();
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();
        }

        private void ConfigureServices()
        {
            var userApiKey = ConfigurationManager.AppSettings["userApiKey"];
            if (string.IsNullOrEmpty(userApiKey))
            {
                throw new ConfigurationErrorsException("The 'userApiKey' setting is missing or empty in the application configuration.");
            }   //TODO maybe ask for user api key if not set

            _services.AddHttpClient("newsAPI", httpClient =>
            {
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", userApiKey); 
                httpClient.DefaultRequestHeaders.Add("user-agent", "FluentNewsApp/0.1");
            });
            _services.AddSingleton<INewsApiClient, NewsApiClient>();
            _services.AddSingleton<MainWindowViewModel>();
            _serviceProvider = _services.BuildServiceProvider();
        }
    }

}
