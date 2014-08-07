using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services.Api.Dtos;
using Services.Interfaces;

namespace crisicheckinweb.Api
{
    public class ClustersController : BaseApiController
    {
        public ClustersController(IDataService dataService, IApiService apiService) : base(dataService, apiService) { }

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                var clusters = DataService.Clusters.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Result = clusters.Select(c => DtoFactory.Create(c)),
                    Succeeded = true
                });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Succeeded = true,
                    Error = "There was an error when retrieving clusters."
                });
            }
        }
    }
}
