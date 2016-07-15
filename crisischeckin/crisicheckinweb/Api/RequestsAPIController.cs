using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Api
{
    public class RequestsAPIController : ApiController
    {
        private CrisisCheckin db = new CrisisCheckin();
        private readonly IRequest _requestSvc;

        public RequestsAPIController(IRequest requestSvc)
        {
            _requestSvc = requestSvc;
        }

        [HttpPost]
        [Route("api/request/{requestId}/complete")]
        public async Task<IHttpActionResult> CompleteRequest(int requestId)
        {
            await _requestSvc.CompleteRequestAsync(requestId);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RequestExists(int id)
        {
            return db.Requests.Count(e => e.RequestId == id) > 0;
        }
    }
}
