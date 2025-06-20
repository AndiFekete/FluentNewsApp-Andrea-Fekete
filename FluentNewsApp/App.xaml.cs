using FluentNewsApp.WebCalls;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Net.Http;
using System.Printing;
using System.Windows;

namespace FluentNewsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IServiceCollection _services = new ServiceCollection();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureServices();
            // Additional startup logic can go here
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
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue(userApiKey);
            });
            _services.AddSingleton<NewsApiClient>();
        }
    }

}
