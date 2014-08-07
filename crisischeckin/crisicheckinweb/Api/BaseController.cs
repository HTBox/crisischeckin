using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services.Interfaces;

namespace crisicheckinweb.Api
{
    public class BaseController : ApiController
    {
        private IApiService _apiService;

        protected IApiService ApiService
        {
            get { return _apiService; }
        }
        
        public BaseController(IApiService apiService)
        {
            _apiService = apiService;
        }
    }
}
