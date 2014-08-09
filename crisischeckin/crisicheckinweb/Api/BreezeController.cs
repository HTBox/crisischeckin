using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace crisicheckinweb.Api
{
    /// <summary>
    /// Provides entity view models to the web application.
    /// </summary>
    [BreezeController]
    public class BreezeController : ApiController
    {
        readonly CrisisCheckinContextProvider _contextProvider;

        CrisisCheckin Db { get { return _contextProvider.Context; } }

        public BreezeController(CrisisCheckin db)
        {
             _contextProvider = new CrisisCheckinContextProvider(db);
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
        public Person Person()
        {
            //return Db.Persons.Include(p => p.Commitments).Include("Commitments.Disaster").Single(p => p.UserId == WebSecurity.CurrentUserId);
            return Db.Persons.Include(p => p.Commitments).Include("Commitments.Disaster").First(); // TODO - deal with user authentication
        }

        /// <summary>
        /// Gets the entire collection of clusters
        /// </summary>
        [HttpGet]
        public IQueryable<Cluster> Clusters()
        {
            return Db.Clusters;
        }

        /// <summary>
        /// Gets the entire collection of disasters
        /// </summary>
        [HttpGet]
        public IQueryable<Disaster> Disasters()
        {
            return Db.Disasters;
        }

        /// <summary>
        /// Creates a commitment between a person and a disaster for the given start/end dates
        /// </summary>
        /// <param name="personId">ID of the person volunteering</param>
        /// <param name="disasterId">disaster ID</param>
        /// <param name="startDate">start of the commitment</param>
        /// <param name="endDate">end of the commitment</param>
        /// <returns>True if succeeded, false otherwise</returns>
        [HttpPost]
        public bool Volunteer(int personId, int disasterId, DateTime startDate, DateTime endDate)
        {
            try
            {
                // TODO - should we allow more than one commitment per person per disaster?
                Commitment commitment = new Commitment()
                {
                    PersonId = personId, // TODO - figure out personId from logged-in user
                    DisasterId = disasterId,
                    StartDate = startDate,
                    EndDate = endDate
                };
                _contextProvider.CreateEntityInfo(new Commitment(), Breeze.ContextProvider.EntityState.Added);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns all commitments for the given person
        /// </summary>
        /// <param name="personId">ID of the person for which to return commitments</param>
        [HttpGet]
        public IQueryable<Commitment> Commitments(int personId)
        {
            return Db.Commitments.Where(c => c.PersonId == personId); // TODO - figure out personId from logged-in user
        }

        /// <summary>
        /// Checks the user in to the given disaster
        /// </summary>
        /// <param name="personId">ID of the person to check in</param>
        /// <param name="disasterId">ID of the disaster to which the person is committed</param>
        /// <returns>True if check in succeeded; false otherwise</returns>
        [HttpPost]
        public bool CheckIn(int personId, int disasterId)
        {
            // TODO - according to Jon's gist, this should also fail if user is already checked in
            //     to ANY commitment, not just the one associated with disasterId

            IEnumerable<Commitment> activeCommitments = Db.Commitments
                .Where(c => c.PersonId == personId)
                .Where(c => c.DisasterId == disasterId);
            if (activeCommitments.Count() != 1) { return false; }
            Commitment commitment = activeCommitments.First();
            if (commitment.PersonIsCheckedIn)
            {
                return false; // already checked in
            }
            else
            {
                return commitment.PersonIsCheckedIn = true;
            }
        }

        //[HttpPost]
        //public bool CheckOut(int personId)
        //{
        //    // TODO - get the disaster id

        //    IEnumerable<Commitment> activeCommitments = Db.Commitments
        //       .Where(c => c.PersonId == personId)
        //       .Where(c => c.DisasterId == disasterId);
        //    if (activeCommitments.Count() != 1) { return false; }
        //    Commitment commitment = activeCommitments.First();
        //}

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        class CrisisCheckinContextProvider : EFContextProvider<CrisisCheckin>
        {
            readonly CrisisCheckin _db;

            public CrisisCheckinContextProvider(CrisisCheckin db)
            {
                _db = db;
            }

            protected override CrisisCheckin CreateContext()
            {
                return _db;
            }

            // TODO: override BeforeSaveEntities and/or BeforeSaveEntity to validate saves
        }
    }
}
