using System.Collections.Generic;
using System.Web.Http;
using crisicheckinweb.Wrappers;
using Models;
using Services.Api.Dtos;
using System.Data.Entity;
using System.Linq;

namespace crisicheckinweb.Api
{
    public class PersonController : ApiController
    {
        private CrisisCheckin DbContext { get; set; }
        private IWebSecurityWrapper WebSecurity { get; set; }

        public PersonController(CrisisCheckin dbContext, IWebSecurityWrapper webSecurity)
        {
            DbContext = dbContext;
            WebSecurity = webSecurity;
        }

        [HttpGet]
        [Route("api/person/{personId}/commitments")]
        public IEnumerable<CommitmentDto> GetCommitments(int personId)
        {
            return DbContext.Commitments.Include(c => c.Disaster)
                .Where(c => c.PersonId == personId) //WebSecurity.CurrentUserId)
                .Select(c => new CommitmentDto
                {
                    PersonId = c.PersonId,
                    DisasterId = c.DisasterId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.Status,
                    Disaster = new DisasterDto
                    {
                        Id = c.Disaster.Id,
                        Name = c.Disaster.Name,
                        IsActive = c.Disaster.IsActive
                    }

                }).ToList();
        }

        [HttpGet]
        [Route("api/person/{personId}/requests")]
        public IEnumerable<RequestDto> GetRequests(int personId)
        {
            return DbContext.Requests.Where(c => c.AssigneeId == personId)
                .Select(c => new RequestDto
                {
                    RequestId = c.RequestId,
                    CreatedDate = c.CreatedDate,
                    EndDate = c.EndDate,
                    Description = c.Description,
                    CreatorId = c.CreatorId,
                    AssigneeId = c.AssigneeId,
                    OrganizationId = c.OrganizationId,
                    Completed =c.Completed,
                    Location = c.Location                  
                }).ToList();
        }

    }
}
