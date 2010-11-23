using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NHibernate;
using Ninject;
using NHibernate.Cfg;

namespace LivingRoom
{
    public static class GuyWire
    {

        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            var kernel = new StandardKernel();

            var cfg = new Configuration().Configure();
            var sessionFactory = cfg.BuildSessionFactory();

            kernel.Bind<ISession>()
                .ToMethod(ctx => sessionFactory.OpenSession())
                .InRequestScope();

            ControllerBuilder.Current.SetControllerFactory(
                new NinjectControllerFactory(kernel));

            new Models.Mappings();

        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }


    }
}