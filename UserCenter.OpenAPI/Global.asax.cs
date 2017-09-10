using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Routing;
using UserCenter.IServices;
using UserCenter.OpenAPI.App_Start;
using UserCenter.Services;

namespace UserCenter.OpenAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            #region 注册Autofac
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            //给控制器里的属性自动注入
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            var serviceTypes = Assembly.Load("UserCenter.Services")
                                .GetTypes()
                                .Where(t => !t.IsAbstract && typeof(IServiceTag).IsAssignableFrom(t))
                                .ToArray();

            builder.RegisterTypes(serviceTypes)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
              .InstancePerLifetimeScope();



            builder.RegisterType<UserCenterContext>().InstancePerLifetimeScope();

            //注册过滤器
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => !t.IsAbstract && typeof(IFilter).IsAssignableFrom(t)).PropertiesAutowired();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            #endregion

            //移除 XML格式化
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //EF初始化模式为空
            Database.SetInitializer<UserCenterContext>(null);


            GlobalConfiguration.Configure(WebApiConfig.Register);

          
        }
    }
}
