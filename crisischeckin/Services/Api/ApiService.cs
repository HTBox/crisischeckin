using Services.Interfaces;
using Services.Api.Dtos;
using System.Linq;

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
