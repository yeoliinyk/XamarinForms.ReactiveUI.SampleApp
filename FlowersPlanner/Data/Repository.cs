using System;
using System.Collections.Generic;
using System.Reactive;
using FlowersPlanner.Data.Net;
using FlowersPlanner.Domain;
using FlowersPlanner.Domain.Models;
using FlowersPlanner.Presentation.Domain.Models;
using Splat;
using System.Reactive.Linq;
using FlowersPlanner.Data.Cache;
using FlowersPlanner.Data.Dto;

namespace FlowersPlanner.Data
{
    public class Repository : IRepository
    {
        private readonly IApiService _apiService;
        private readonly Settings _settings;

        public Repository(IApiService apiService = null, Settings settings = null)
        {
            _apiService = apiService ?? Locator.Current.GetService<IApiService>();
            _settings = settings ?? Locator.Current.GetService<Settings>();
        }

        public IObservable<string> SignIn(LoginData data)
        {
            var dict = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"username", data.Username},
                {"password", data.Password}
            };
            return _apiService
                .UserInitiated.SignIn(dict)
                              .Select(dto => dto.Token)
                              .Do(token => _settings.ApiToken = token);
        }

        public IObservable<string> SignUp(RegistrationData data)
        {
            return _apiService.UserInitiated.SignUp(data);
        }

        public IObservable<string> GetReferenceBook()
        {
            return _apiService
                .Background.GetReferenceBook();
        }
    }
}
