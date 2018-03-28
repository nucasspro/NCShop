using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using NCShop.Data;
using NCShop.Data.Infrastructure;
using NCShop.Data.Repositories;
using NCShop.Service;
using Owin;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(NCShop.WebApplication.App_Start.Startup))]

namespace NCShop.WebApplication.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigAutofac(app);
        }

        private void ConfigAutofac(IAppBuilder app)
        {
            var buider = new ContainerBuilder();
            buider.RegisterControllers(Assembly.GetExecutingAssembly());

            //Register Web Api controller
            buider.RegisterApiControllers(Assembly.GetExecutingAssembly());

            buider.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            buider.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            buider.RegisterType<NCShopDbContext>().AsSelf().InstancePerRequest();

            //Register Repositories
            buider.RegisterAssemblyTypes(typeof(PostCategoryRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerRequest();

            //Register Service
            buider.RegisterAssemblyTypes(typeof(PostCategoryService).Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();

            Autofac.IContainer container = buider.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}