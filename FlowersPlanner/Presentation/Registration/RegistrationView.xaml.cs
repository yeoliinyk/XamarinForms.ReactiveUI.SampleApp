using System;
using System.Collections.Generic;
using FlowersPlanner.Presentation.Base;
using Xamarin.Forms;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using FluentValidation.Results;
using FlowersPlanner.Localization;

namespace FlowersPlanner.Presentation.Registration
{
    public partial class RegistrationView : BaseContentPage<RegistrationViewModel>
    {
        public RegistrationView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            InitRootMargins();

            Func<ValidationResult, string> selector = (result) => result.Errors.Any() ? result.Errors.First().ErrorMessage : null;

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.Name, v => v.NameEntry.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.PhoneNumber, v => v.PhoneEntry.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.Email, v => v.EmailEntry.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.Password, v => v.PasswordEntry.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.ConfirmPassword, v => v.ConfirmPasswordEntry.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.Login, v => v.LoginEmailEntry.Text)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.LoginPassword, v => v.LoginPasswordEntry.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.NameValidationResult, v => v.NameEntry.ErrorText, selector)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.PhoneNumberValidationResult, v => v.PhoneEntry.ErrorText, selector)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.EmailValidationResult, v => v.EmailEntry.ErrorText, selector)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.PasswordValidationResult, v => v.PasswordEntry.ErrorText, selector)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.ConfirmPasswordValidationResult, v => v.ConfirmPasswordEntry.ErrorText, selector)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.State, v => v.FirstStepStack.IsVisible, state => state == 0)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.State, v => v.SecondStepStack.IsVisible, state => state == 1)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.State, v => v.BackButton.IsVisible, state => state != 0)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.State, v => v.LoginLink.IsVisible, state => state == 0)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.State, v => v.LoginStack.IsVisible, state => state == 2);

                this.BindCommand(ViewModel, vm => vm.GoToPasswordEntryCommand, v => v.RegisterButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.BackCommand, v => v.BackButton)
                    .DisposeWith(disposables);

                this.WhenAnyValue(v => v.ViewModel.State)
                    .DistinctUntilChanged()
                    .Subscribe(state => 
                    {
                        string text = AppResources.Register;
                                                
                        if (state == 2)
                            text = AppResources.SignIn;

                        RegisterButton.Text = text;
                    })
                    .DisposeWith(disposables);

                this.PhoneEntry
                    .Events()
                    .Focused
                    .Select(x => true)
                    .BindTo(this, v => v.ViewModel.PhoneEntryFocused)
                    .DisposeWith(disposables);

                this.PhoneEntry
                    .Events()
                    .Unfocused
                    .Select(x => false)
                    .BindTo(this, v => v.ViewModel.PhoneEntryFocused)
                    .DisposeWith(disposables);

                this.PhoneEntry
                    .Events()
                    .TextChanged
                    .Subscribe(args =>
                    {
                        if (args.NewTextValue == null)
                            return;

                        var newLength = args.NewTextValue.Length;
                        if (newLength < 4 || newLength > 19)
                        {
                            PhoneEntry.Text = args.OldTextValue;
                        }
                    })
                    .DisposeWith(disposables);

                this.PhoneEntry
                    .Events()
                    .TextChanged
                    .Select(args =>
                    {
                        if (args.NewTextValue == null)
                            return false;

                        var newLength = args.NewTextValue.Length;

                        return newLength == 13 || newLength >= 19;
                    })
                    .DistinctUntilChanged()
                    .InvokeCommand(ViewModel.FormatPhoneNumberCommand)
                    .DisposeWith(disposables);

                this.GenderSelector
                    .Events()
                    .Selected
                    .Select(x => (int)x.SelectedPosition)
                    .BindTo(this, v => v.ViewModel.Gender)
                    .DisposeWith(disposables);

            });
        }

        private void OnLoginTapped(object sender, EventArgs args)
        {
            Observable.Return(Unit.Default)
                      .InvokeCommand(this, v => v.ViewModel.GoToLoginCommand);
        }

        private void InitRootMargins()
        {
            if (Device.RuntimePlatform == Device.iOS)
                RootLayout.Margin = new Thickness(0, 20, 0, 0);
        }
    }
}
