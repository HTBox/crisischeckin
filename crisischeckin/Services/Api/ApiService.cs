using Services.Interfaces;
using Services.Api.Requests;
using Services.Api.Responses;

namespace Services.Api
{
    public class ApiService : IApiService
    {
        private IDataService _dataService;

        protected IDataService DataService
        {
            get { return _dataService; }
        }
        
        public ApiService(IDataService dataService)
        {
            _dataService = dataService;
        }


    }
}
