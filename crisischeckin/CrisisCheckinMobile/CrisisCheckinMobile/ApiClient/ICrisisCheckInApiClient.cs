using System.Collections.Generic;
using System.Threading.Tasks;
using CrisisCheckinMobile.Models;

namespace CrisisCheckinMobile.ApiClient
{
    public interface ICrisisCheckInApiClient
    {
        Task<IEnumerable<CommitmentDto>> GetCommitmentsList(int personId);

        Task<IEnumerable<RequestDto>> GetRequests(int personId);
        Task CompleteRequest(int requestId);
    }
}