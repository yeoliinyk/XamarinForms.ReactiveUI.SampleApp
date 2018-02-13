using System;
using FlowersPlanner.Presentation.Main;
using ReactiveUI;
using Splat;
using Xamarin.Forms;
using RoutedViewHost = ReactiveUI.XamForms.RoutedViewHost;
using FlowersPlanner.Presentation.Registration;
using FlowersPlanner.Data;
using FlowersPlanner.Domain;
using FlowersPlanner.Data.Net;
using FlowersPlanner.Data.Cache;
using System.Threading.Tasks;

namespace FlowersPlanner
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper()
        {            
            Router = new RoutingState();
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
            Locator.CurrentMutable.RegisterConstant(new Settings());
            Locator.CurrentMutable.RegisterConstant(new ApiService(), typeof(IApiService));
            Locator.CurrentMutable.RegisterConstant(new Repository(), typeof(IRepository));

            Locator.CurrentMutable.Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
            Locator.CurrentMutable.Register(() => new RegistrationView(), typeof(IViewFor<RegistrationViewModel>));
            
            Router
                .NavigateAndReset
                .Execute(new RegistrationViewModel())
                .Subscribe();
        }

        public RoutingState Router { get; }
        
        public Page CreateMainPage()
        {
            return new RoutedViewHost();
        }

        //private Func<Task<string>> getToken = () =>
        //{
        //    var tcs = new TaskCompletionSource<string>();
        //    var token = cache.GetValueOrDefault(CacheKeys.ApiToken, "");
        //    tcs.SetResult(token);
        //    return tcs.Task;
        //};
    }
}