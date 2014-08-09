using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services.Interfaces;
using Services.Api.Dtos;

namespace crisicheckinweb.Api
{
    public class BaseApiController : ApiController
    {
        private IDataService _dataService;
        private IApiService _apiService;
        private DtoFactory _dtoFactory;

        protected IDataService DataService
        {
            get { return _dataService; }
        }

        protected DtoFactory DtoFactory
        {
            get { return _dtoFactory; }
        }

        public IApiService ApiService 
        {
            get { return _apiService; }
        }

        public BaseApiController(IDataService dataService, IApiService apiService)
        {
            _dataService = dataService;
            _apiService = apiService;
            _dtoFactory = new DtoFactory();
        }
    }
}
