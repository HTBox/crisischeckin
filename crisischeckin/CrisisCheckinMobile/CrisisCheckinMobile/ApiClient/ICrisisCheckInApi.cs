using System.Collections.Generic;
using System.Threading.Tasks;
using CrisisCheckinMobile.Models;
using Refit;

namespace CrisisCheckinMobile.ApiClient
{
    [Headers("Accept: application/json")] 
    public interface ICrisisCheckInApi
    {
        // TODO: add authentication header with Auth0
        // Remove personId argument at that time; the server will use the bearer token to identify the logged-in user
        [Get("/person/{personId}/commitments")]
        Task<List<CommitmentDto>> GetCommitmentsWithDisasterInfo(int personId);
    }
}
