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
            return Db.Persons.Include(p => p.Commitments).Include("Commitments.Disaster").First(); // TODO: Use the authenticated user to get the real user record.
        }

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
