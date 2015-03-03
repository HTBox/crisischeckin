using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using crisicheckinweb.Filters;
using crisicheckinweb.Wrappers;

namespace crisicheckinweb.Api
{
    /// <summary>
    /// Provides entity view models to the web application.
    /// </summary>
    [CrisisCheckInApiAuthorize]
    [BreezeController]
    public class EntitiesController : ApiController
    {
        private readonly CrisisCheckinContextProvider _contextProvider;
        private readonly IWebSecurityWrapper _webSecurity;

        CrisisCheckin Context { get { return _contextProvider.Context; } }

        public EntitiesController(CrisisCheckin ctx, IWebSecurityWrapper webSecurity)
        {
            _contextProvider = new CrisisCheckinContextProvider(ctx);
            _webSecurity = webSecurity;
        }

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        /// <summary>
        /// Gets the basic info for the logged in user.
        /// </summary>
        [HttpGet]
        public IQueryable<Person> Persons()
        {
            return Context.Persons.Include(p => p.Commitments).Include("Commitments.Disaster")
                .Where(p => p.UserId == _webSecurity.CurrentUserId);
        }

        /// <summary>
        /// Gets the entire collection of clusters
        /// </summary>
        [HttpGet]
        public IQueryable<Cluster> Clusters()
        {
            return Context.Clusters;
        }

        /// <summary>
        /// Gets the entire collection of disasters
        /// </summary>
        [HttpGet]
        public IQueryable<Disaster> Disasters()
        {
            return Context.Disasters;
        }

        /// <summary>
        /// Creates a commitment between a person and a disaster for the given start/end dates
        /// </summary>
        /// <param name="personId">ID of the person volunteering</param>
        /// <param name="disasterId">disaster ID</param>
        /// <param name="startDate">start of the commitment</param>
        /// <param name="endDate">end of the commitment</param>
        [HttpPost]
        public void Volunteer(int personId, int disasterId, DateTime startDate, DateTime endDate)
        {
            Commitment commitment = new Commitment()
            {
                PersonId = personId, // TODO - figure out personId from logged-in user
                DisasterId = disasterId,
                StartDate = startDate,
                EndDate = endDate
            };
            _contextProvider.CreateEntityInfo(new Commitment(), Breeze.ContextProvider.EntityState.Added);
            Context.SaveChanges();
        }

        /// <summary>
        /// Returns all commitments for the given person
        /// </summary>
        /// <param name="personId">ID of the person for which to return commitments</param>
        [HttpGet]
        public IQueryable<Commitment> Commitments(int personId)
        {
            return Context.Commitments.Where(c => c.PersonId == personId);
        }

        /// <summary>
        /// Checks the user in to the given commitment
        /// </summary>
        /// <param name="commitmentId">ID of the commitment for which to check in user</param>
        [HttpPost]
        public void CheckIn(int commitmentId)
        {
            Commitment commitment = Context.Commitments.Single(c => c.Id == commitmentId);
            commitment.PersonIsCheckedIn = true;
            Context.SaveChanges();
        }

        /// <summary>
        /// Checks the user out of the given commitment
        /// </summary>
        /// <param name="commitmentId">ID of the commitment for which to check out user</param>
        [HttpPost]
        public void CheckOut(int commitmentId)
        {
            Commitment commitment = Context.Commitments.Single(c => c.Id == commitmentId);
            commitment.PersonIsCheckedIn = false;
            Context.SaveChanges();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        public class CrisisCheckinContextProvider : EFContextProvider<CrisisCheckin>
        {
            readonly CrisisCheckin _context;

            public CrisisCheckinContextProvider(CrisisCheckin ctx)
            {
                _context = ctx;
            }

            protected override CrisisCheckin CreateContext()
            {
                return _context;
            }

            // TODO: override BeforeSaveEntities and/or BeforeSaveEntity to validate saves
        }
    }
}
