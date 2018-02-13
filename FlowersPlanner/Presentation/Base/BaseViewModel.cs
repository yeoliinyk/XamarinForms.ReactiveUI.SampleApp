using System;
using ReactiveUI;
using Splat;

namespace FlowersPlanner.Presentation.Base
{
    public class BaseViewModel : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public BaseViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }

        public string UrlPathSegment
        {
            get;
            protected set;
        }

        public IScreen HostScreen
        {
            get;
            protected set;
        }

        public ViewModelActivator Activator => ViewModelActivator;

        protected readonly ViewModelActivator ViewModelActivator = new ViewModelActivator();
    }
}
