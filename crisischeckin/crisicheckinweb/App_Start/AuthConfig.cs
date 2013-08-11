using System.Web.Security;
using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using WebMatrix.Data;
using WebMatrix.WebData;

namespace crisicheckinweb
{
    public class AuthConfig
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static void Register()
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                System.Data.Entity.Database.SetInitializer<CrisisCheckin>(null);

                try
                {
                    using (var context = new CrisisCheckin())
                    {
                        
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("CrisisCheckin", "User", "Id", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }

        public static void VerifyRolesAndDefaultAdminAccount()
        {
            if (!Roles.RoleExists(Constants.RoleAdmin))
            {
                Roles.CreateRole(Constants.RoleAdmin);
            }

            if (!Roles.RoleExists(Constants.RoleVolunteer))
            {
                Roles.CreateRole(Constants.RoleVolunteer);
            }

            if (!WebSecurity.UserExists(Constants.DefaultAdministratorUserName))
            {
                WebSecurity.CreateUserAndAccount(Constants.DefaultAdministratorUserName, Constants.DefaultAdministratorPassword, null, false);
            }
            if (WebSecurity.UserExists(Constants.DefaultAdministratorUserName) &&
                !Roles.IsUserInRole(Constants.DefaultAdministratorUserName))
            {
               Roles.AddUserToRole(Constants.DefaultAdministratorUserName, Constants.RoleAdmin); 
            }
        }
    }
}