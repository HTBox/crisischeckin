using System.Collections.Generic;
using System.Threading.Tasks;
using CrisisCheckinMobile.Models;
using Refit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Polly;
using System;

namespace CrisisCheckinMobile.ApiClient
{
    public class CrisisCheckInApiClient : ICrisisCheckInApiClient
    {
        private const string EndpointUrl = "http://localhost:2077/api"; // TODO: Need System.Configuration to put this in app.config

        public CrisisCheckInApiClient()
        {
            JsonConvert.DefaultSettings =
                () => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = { new StringEnumConverter() }
                };
        }

        public async Task<IEnumerable<CommitmentDto>> GetCommitmentsList(int personId)
        {
            var apiClient = RestService.For<ICrisisCheckInApi>(EndpointUrl);

            var policy = Policy
                .Handle<System.Net.WebException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                });
            // TODO: add authentication header to hook up to Autho0
            var dtos = await policy.ExecuteAsync(() =>
                    apiClient.GetCommitmentsWithDisasterInfo(personId
                        /* [Header("Authorization")] string authorization = "bearer AUTH0-TOKEN" */)
                );

            return dtos;
            // check for connectivity
            // if connected, retrieve
            // otherwise, check cache. use that if data exist. 
            // Error/something if no data and can't connect
        }

        public async Task<IEnumerable<RequestDto>> GetRequests(int personId)
        {
            var apiClient = RestService.For<ICrisisCheckInApi>(EndpointUrl);

            var policy = Policy
                .Handle<System.Net.WebException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                });
            // TODO: add authentication header to hook up to Autho0
            var dtos = await policy.ExecuteAsync(() =>
                    apiClient.GetRequests(personId
                        /* [Header("Authorization")] string authorization = "bearer AUTH0-TOKEN" */)
                );

            return dtos;
            // check for connectivity
            // if connected, retrieve
            // otherwise, check cache. use that if data exist. 
            // Error/something if no data and can't connect
        }

    }
}
