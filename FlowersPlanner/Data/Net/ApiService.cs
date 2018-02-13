using System;
using System.Net.Http;
using System.Threading.Tasks;
using FlowersPlanner.Data.Net.Tools;
using Fusillade;
using Plugin.Settings;
using Refit;
using FlowersPlanner.Data.Cache;
using Splat;

namespace FlowersPlanner.Data.Net
{
    public class ApiService : IApiService
    {
        private readonly Settings _settings;

        public const string ApiBaseAddress = "http://flowersplanner.azurewebsites.net";

        public IRestApiClient Background
        {
            get => background.Value; 
        }

        public IRestApiClient UserInitiated
        {
            get => userInitiated.Value; 
        }

        public IRestApiClient Speculative
        {
            get => speculative.Value; 
        }

        private readonly Lazy<IRestApiClient> background;
        private readonly Lazy<IRestApiClient> userInitiated;
        private readonly Lazy<IRestApiClient> speculative;

        public ApiService(Settings settings = null, string apiBaseAddress = null)
        {
            _settings = settings ?? Locator.Current.GetService<Settings>();

            Func<HttpMessageHandler, IRestApiClient> createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress),
                    Timeout = TimeSpan.FromSeconds(15)                        
                };

                return RestService.For<IRestApiClient>(client);
            };

            background = new Lazy<IRestApiClient>(() =>
            {
#if DEBUG
                return createClient(new RateLimitedHttpMessageHandler(new HttpLoggingHandler(new AuthenticatedHttpClientHandler(() => _settings.ApiToken)), Priority.Background));
#else
                return createClient(new RateLimitedHttpMessageHandler(new AuthenticatedHttpClientHandler(() => _settings.ApiToken), Priority.Background));
#endif
            });

            userInitiated = new Lazy<IRestApiClient>(() =>
            {
#if DEBUG
                return createClient(new RateLimitedHttpMessageHandler(new HttpLoggingHandler(new AuthenticatedHttpClientHandler(() => _settings.ApiToken)), Priority.UserInitiated));
#else
                return createClient(new RateLimitedHttpMessageHandler(new AuthenticatedHttpClientHandler(() => _settings.ApiToken), Priority.UserInitiated));
#endif
            });

            speculative = new Lazy<IRestApiClient>(() =>
            {
#if DEBUG
                return createClient(new RateLimitedHttpMessageHandler(new HttpLoggingHandler(new AuthenticatedHttpClientHandler(() => _settings.ApiToken)), Priority.Speculative));
#else
                return createClient(new RateLimitedHttpMessageHandler(new AuthenticatedHttpClientHandler(() => _settings.ApiToken), Priority.Speculative));
#endif
            });
        }
    }    
}
