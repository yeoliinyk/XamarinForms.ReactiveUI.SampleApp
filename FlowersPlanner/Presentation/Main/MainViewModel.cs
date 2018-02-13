using System;
using FlowersPlanner.Presentation.Base;
using ReactiveUI;
using System.Reactive;
using System.Collections.Generic;
using FlowersPlanner.Domain;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Diagnostics;

namespace FlowersPlanner.Presentation.Main
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRepository _repository;

        public ReactiveCommand<Unit, string> GetReferenceBook;

        public MainViewModel(IRepository repository = null)
        {
            _repository = repository ?? Locator.Current.GetService<IRepository>();

            GetReferenceBook = ReactiveCommand.CreateFromObservable(() => _repository.GetReferenceBook(), outputScheduler: RxApp.MainThreadScheduler);

            this.WhenActivated((CompositeDisposable disposables) => 
            {
                GetReferenceBook
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe(rb =>
                    {

                    })
                    .DisposeWith(disposables);

                GetReferenceBook
                    .ThrownExceptions
                    .Subscribe(ex =>
                    {
                        Debug.WriteLine("Error occurs: " + ex);
                    })
                    .DisposeWith(disposables);
            });
        }
    }
}
