using System;

using Xamarin.Forms;
using FlowersPlanner.Presentation.Base;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System.Reactive;
using FluentValidation;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using FluentValidation.Results;
using System.Linq;
using System.Diagnostics;
using FluentValidation.Validators;
using System.Collections.Generic;
using FlowersPlanner.Presentation.Domain.Models;
using FlowersPlanner.Domain;
using Splat;
using FlowersPlanner.Domain.Models;
using FlowersPlanner.Presentation.Main;

namespace FlowersPlanner.Presentation.Registration
{
    public class RegistrationViewModel : BaseViewModel
    {
        private IRepository _repository;

        private AbstractValidator<string> _nameValidator;
        private AbstractValidator<string> _phoneNumberValidator;
        private AbstractValidator<string> _emailValidator;
        private AbstractValidator<string> _passwordValidator;
        private AbstractValidator<ConfirmPasswordValidatorArgs> _confirmPasswordValidator;

        // states history
        private Stack<int> _stateStack;

        public RegistrationViewModel(IRepository repository = null)
        {
            _repository = repository ?? Locator.Current.GetService<IRepository>();

            _nameValidator = new NameValidator();
            _phoneNumberValidator = new PhoneNumberValidator();
            _emailValidator = new EmailValidator();
            _passwordValidator = new PasswordValidator();
            _confirmPasswordValidator = new ConfirmPasswordValidator();

            _stateStack = new Stack<int>();
            _stateStack.Push(0);

            NameValidationResult = new ValidationResult();
            PhoneNumberValidationResult = new ValidationResult();
            EmailValidationResult = new ValidationResult();
            PasswordValidationResult = new ValidationResult();
            ConfirmPasswordValidationResult = new ValidationResult();

            GoToPasswordEntryCommand = ReactiveCommand.Create(OnActionButtonClicked, outputScheduler: RxApp.MainThreadScheduler);
            GoToLoginCommand = ReactiveCommand.Create(ShowLogin, outputScheduler: RxApp.MainThreadScheduler);
            BackCommand = ReactiveCommand.Create(OnBackButton, outputScheduler: RxApp.MainThreadScheduler);
            FormatPhoneNumberCommand = ReactiveCommand.Create((bool needFormat) => FormatPhoneNumber(needFormat), outputScheduler: RxApp.MainThreadScheduler);

            SignUpCommand = ReactiveCommand.CreateFromObservable((RegistrationData data) => _repository.SignUp(data), outputScheduler: RxApp.MainThreadScheduler);
            SignInCommand = ReactiveCommand.CreateFromObservable((LoginData data) => _repository.SignIn(data), outputScheduler: RxApp.MainThreadScheduler);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                GoToPasswordEntryCommand
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe()
                    .DisposeWith(disposables);

                GoToPasswordEntryCommand
                    .ThrownExceptions
                    .Subscribe((ex) =>
                    {
                        Debug.WriteLine("Error occurs: " + ex.Message);
                    })
                    .DisposeWith(disposables);

                this.WhenAnyValue(vm => vm.PhoneEntryFocused)
                    .Subscribe(focused =>
                    {
                        if (focused && string.IsNullOrWhiteSpace(PhoneNumber))
                        {
                            PhoneNumber = "+380";
                        }
                        else if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length < 7)
                            PhoneNumber = null;
                    })
                    .DisposeWith(disposables);

                SignUpCommand
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe(unit =>
                    {
                        var loginData = new LoginData { Username = Email, Password = Password };
                        TryLogin(loginData);
                    })
                    .DisposeWith(disposables);

                SignUpCommand
                    .ThrownExceptions
                    .Subscribe(ex =>
                    {
                        Debug.WriteLine("Error occurs: " + ex);
                    })
                    .DisposeWith(disposables);

                SignInCommand
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .Subscribe(token =>
                    {
                        NavigateToMain();
                    })
                    .DisposeWith(disposables);

                SignInCommand
                    .ThrownExceptions
                    .Subscribe(ex =>
                    {
                        Debug.WriteLine("Error occurs: " + ex);
                    })
                    .DisposeWith(disposables);
            });
        }

        [Reactive] public string Name { get; set; } 
        [Reactive] public string PhoneNumber { get; set; } 
        [Reactive] public string Email { get; set; }
        [Reactive] public int Gender { get; set; }
        [Reactive] public string Password { get; set; }
        [Reactive] public string ConfirmPassword { get; set; }

        [Reactive] public string Login { get; set; }
        [Reactive] public string LoginPassword { get; set; }

        // 0 - first step registration, 
        // 1 - second step, 
        // 2 - login
        // 3 - forgot password
        [Reactive] public int State { get; set; }
        [Reactive] public bool PhoneEntryFocused { get; set; }

        [Reactive] public ValidationResult NameValidationResult { get; private set; }
        [Reactive] public ValidationResult PhoneNumberValidationResult { get; private set; }
        [Reactive] public ValidationResult EmailValidationResult { get; private set; }
        [Reactive] public ValidationResult PasswordValidationResult { get; private set; }
        [Reactive] public ValidationResult ConfirmPasswordValidationResult { get; private set; }

        public ReactiveCommand<Unit, Unit> GoToLoginCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToPasswordEntryCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public ReactiveCommand<bool, Unit> FormatPhoneNumberCommand { get; }

        public ReactiveCommand<RegistrationData, string> SignUpCommand { get; }
        public ReactiveCommand<LoginData, string> SignInCommand { get; }

        private void ShowLogin()
        {
            State = 2;
            _stateStack.Push(State);
        }

        private void OnActionButtonClicked()
        {
            switch (State)
            {
                case 0:
                    {
                        if (ValidateFirstStep())
                        {
                            State = 1;
                            _stateStack.Push(State);
                        }
                        break;
                    }
                case 1:
                    {
                        if (ValidatePasswords())
                            TryRegister();

                        break;
                    }
                case 2:
                    {
                        var loginData = new LoginData
                        {
                            Username = Login,
                            Password = LoginPassword
                        };
                        TryLogin(loginData);
                        break;
                    }
                default: throw new ArgumentOutOfRangeException("Wrong state -> " + State);
            }
        }

        private void TryRegister()
        {
            var data = new RegistrationData
            {
                UserName = Name,
                PhoneNumber = ClearPhoneNumberFormat(PhoneNumber),
                Email = Email,
                Gender = Gender ^ 1,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };

            Observable.Return(data)
                      .InvokeCommand(SignUpCommand);
        }

        private void TryLogin(LoginData data)
        {            
            Observable.Return(data)
                      .InvokeCommand(SignInCommand);
        }

        private void OnBackButton()
        {
            _stateStack.Pop();
            State = _stateStack.Last();
        }

        private void NavigateToMain()
        {
            HostScreen
                .Router
                .NavigateAndReset
                .Execute(new MainViewModel())
                .Subscribe();
        }

        private bool ValidateFirstStep()
        {
            NameValidationResult = _nameValidator.Validate(Name);
            PhoneNumberValidationResult = _phoneNumberValidator.Validate(PhoneNumber);
            EmailValidationResult = _emailValidator.Validate(Email);

            if (NameValidationResult.IsValid && PhoneNumberValidationResult.IsValid && EmailValidationResult.IsValid)
                return true;

            return false;
        }

        private bool ValidatePasswords()
        {
            PasswordValidationResult = _passwordValidator.Validate(Password);
            ConfirmPasswordValidationResult = _confirmPasswordValidator
            .Validate(new ConfirmPasswordValidatorArgs(Password, ConfirmPassword));

            if (PasswordValidationResult.IsValid && ConfirmPasswordValidationResult.IsValid)
                return true;

            return false;
        }

        private void FormatPhoneNumber(bool needFormat)
        {
            if (needFormat)                          
                PhoneNumber = MakePhoneNumberPretty(PhoneNumber);            
            else
                PhoneNumber = ClearPhoneNumberFormat(PhoneNumber);
        }

        private string MakePhoneNumberPretty(string phoneNumber)
        {
            var phone = UInt64.Parse(phoneNumber.Replace("+", ""));
            return String.Format("{0:+## (###) ###-##-##}", phone);
        }

        private string ClearPhoneNumberFormat(string prettyPhoneNumber)
        {
            return prettyPhoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");
        }
    }
}

