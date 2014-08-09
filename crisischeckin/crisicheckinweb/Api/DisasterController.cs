using Services.Interfaces;

namespace crisicheckinweb.Api
{
    public class DisasterController : BaseApiController
    {
        public DisasterController(IDataService dataService, IApiService apiService) : base(dataService, apiService)
        {

        }
    }
}
