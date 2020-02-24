using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DemoRandomUser.Data;
//using AutoMapper;
//using TheCodeCamp.Data;

namespace DemoRandomUser
{
  public class AutofacConfig
  {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterWebApiFilterProvider(config);
            RegisterServices(builder);

            builder.RegisterWebApiModelBinderProvider();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            //var config = new MapperConfiguration(cfg =>
            //{
            //  cfg.AddProfile(new CampMappingProfile());
            //});

            //bldr.RegisterInstance(config.CreateMapper())
            //  .As<IMapper>()
            //  .SingleInstance();

            //bldr.RegisterType<CampContext>()
            //  .InstancePerRequest();

            //bldr.RegisterType<CampRepository>()
            //  .As<ICampRepository>()
            //  .InstancePerRequest();
            builder.RegisterType<SqlDb>().As<IDbContext>().InstancePerRequest();

            builder.RegisterType<Repository.UserRepository>()
                .As<Repository.UserRepository>()
                .InstancePerRequest();
        }
  }
}
