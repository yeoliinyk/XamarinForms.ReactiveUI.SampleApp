using System;
using Refit;
using FlowersPlanner.Presentation.Domain.Models;
using System.Reactive;
using System.Collections.Generic;
using FlowersPlanner.Data.Dto;

namespace FlowersPlanner.Data.Net
{
    [Headers("Content-Type: application/json", "Accept-Language: ru")]
    public interface IRestApiClient
    {
        [Post("/api/Account/Register")]
        IObservable<string> SignUp([Body]RegistrationData data);

        [Post("/token")]
        IObservable<ApiTokenDto> SignIn([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);

        [Get("/api/ReferenceBook")]
        [Headers("Authorization: Bearer")]
        IObservable<string> GetReferenceBook();
    }
}
