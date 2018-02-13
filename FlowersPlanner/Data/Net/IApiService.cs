using System;
namespace FlowersPlanner.Data.Net
{
    public interface IApiService
    {
        IRestApiClient Speculative { get; }
        IRestApiClient UserInitiated { get; }
        IRestApiClient Background { get; }
    }
}
