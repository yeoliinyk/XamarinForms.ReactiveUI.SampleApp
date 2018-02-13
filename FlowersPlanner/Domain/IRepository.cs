using System;
using System.Reactive;
using FlowersPlanner.Presentation.Domain.Models;
using FlowersPlanner.Domain.Models;

namespace FlowersPlanner.Domain
{
    public interface IRepository
    {
        IObservable<string> SignUp(RegistrationData data);
        IObservable<string> SignIn(LoginData data);

        IObservable<string> GetReferenceBook();

    }
}
