using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Services
{
    internal class SubjectEnrichmentService
    {
        /// <summary>
        /// If we are not in production, prepend the email subject with [STAGING] or [DEVELOPMENT], depending
        /// on the environment.
        /// </summary>
        internal static string Enrich(string subject)
        {
            var environment = ConfigurationManager.AppSettings["environment"];

            if (environment == Constants.Production)
            {
                return subject;               
            }

            return string.Format("[{0}] {1}", environment.ToUpper(), subject);
        }
    }
}
