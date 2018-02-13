using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FlowersPlanner.Presentation.Base;
using FlowersPlanner.Presentation;
using ReactiveUI;
using System.Reactive.Disposables;

namespace FlowersPlanner.Presentation.Main
{
    public partial class MainView : BaseContentPage<MainViewModel>
	{
		public MainView()
		{
			InitializeComponent();

            this.WhenActivated((CompositeDisposable disposables) => 
            {
                this.BindCommand(ViewModel, vm => vm.GetReferenceBook, v => v.Button)
                    .DisposeWith(disposables);
            });
		}
	}
}
