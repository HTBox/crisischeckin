using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Http;
using crisicheckinweb.Infrastructure;
using Models;
using Services;
using Services.Mocks;
using Twilio;

[assembly: WebActivator.PreApplicationStartMethod(typeof(crisicheckinweb.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(crisicheckinweb.App_Start.NinjectWebCommon), "Stop")]

namespace crisicheckinweb.App_Start
{
    using System;
    using System.Web;
    using crisicheckinweb.Wrappers;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Services.Interfaces;
    using Services.Api;
    using WebApiContrib.IoC.Ninject;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDisaster>().To<DisasterService>().InRequestScope();
            kernel.Bind<IVolunteerService>().To<VolunteerService>().InRequestScope();
            kernel.Bind<IDataService>().To<DataService>().InRequestScope();
            kernel.Bind<IAdmin>().To<AdminService>().InRequestScope();
            kernel.Bind<ICluster>().To<ClusterService>().InRequestScope();
            kernel.Bind<IClusterGroup>().To<ClusterGroupService>().InRequestScope();
            kernel.Bind<CrisisCheckin>().ToSelf().InRequestScope();
            kernel.Bind<IWebSecurityWrapper>().To<WebSecurityWrapper>().InRequestScope();
            kernel.Bind<IVolunteerTypeService>().To<VolunteerTypesService>().InRequestScope();
            kernel.Bind<IMessageService>().To<MessageService>().InRequestScope();
            kernel.Bind<IMessageSender>().To<SmtpMessageSender>().InRequestScope();
            kernel.Bind<IMessageSender>().To<SMSMessageSender>().InRequestScope();
            kernel.Bind<MailAddress>()
                .ToConstant(new MailAddress(ConfigurationManager.AppSettings["smtp.fromaddress"], ConfigurationManager.AppSettings["smtp.fromname"]))
                .WhenInjectedInto<SmtpMessageSender>();
            kernel.Bind<IMessageCoordinator>().To<MessageCoordinator>().InRequestScope();
            kernel.Bind<IClusterCoordinatorService>().To<ClusterCoordinatorService>().InRequestScope();
            kernel.Bind<IApiService>().To<ApiService>().InRequestScope();
            kernel.Bind<IDisasterClusterService>().To<DisasterClusterService>().InRequestScope();
            kernel.Bind<Func<SmtpClient>>()
                .ToMethod(c => () => new SmtpClient
                {
#if DEBUG
                    // Emails go to "C:\Users\[USER]\AppData\Roaming" if not Release mode
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
#else
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = ConfigurationManager.AppSettings["smtp.host"],
                    Port = int.Parse(ConfigurationManager.AppSettings["smtp.port"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtp.username"], ConfigurationManager.AppSettings["smtp.password"]),
#endif
                })
                .InRequestScope();
            kernel.Bind<string>()
                .ToConstant(ConfigurationManager.AppSettings["SMS.fromphone"])
                .WhenInjectedInto<SMSMessageSender>();
            kernel.Bind<Func<TwilioRestClient>>()
                .ToMethod(c => () =>
                {
#if DEBUG
                    return new TwilioRestClientMock(ConfigurationManager.AppSettings["twilio.account.sid"], ConfigurationManager.AppSettings["twilio.auth.token"])
                    { SaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) };
#else
                    return new TwilioRestClient(ConfigurationManager.AppSettings["twilio.account.sid"], ConfigurationManager.AppSettings["twilio.auth.token"]);
#endif
                });
        }
    }
}
