using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace UserCenter.OpenAPI.App_Start
{
    public class VersionControllerSelector : DefaultHttpControllerSelector
    {
        HttpConfiguration config;
        public VersionControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            this.config = configuration;
        }

        private static Dictionary<string, HttpControllerDescriptor> dic = new Dictionary<string, HttpControllerDescriptor>();



        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            if (dic.Count > 0)
            {
                return dic;
            }
            //获取当前程序集所有控制器
            var cTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract
                && typeof(ApiController)
                .IsAssignableFrom(t)
                && t.Name.EndsWith("Controller"));
            Regex versionRegex = new Regex(@"\w+\.(v[0-9]+)", RegexOptions.IgnoreCase);
            foreach (var type in cTypes)
            {
                int cIndex = type.Name.IndexOf("Controller");
                var cName = type.Name.Substring(0, cIndex).ToUpper();
                var match = versionRegex.Match(type.FullName);

                string key;
                if (match.Success)
                {
                    key = cName + match.Groups[1].Value.ToUpper();
                }
                else
                {
                    key = cName + "V1";
                }
                dic[key] = new HttpControllerDescriptor(config, type.Name, type);
            }

            return dic;
        }
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request.GetRouteData().Values.TryGetValue("controller", out var value))
            {
                var match = Regex.Match(request.RequestUri.AbsoluteUri, @"/(v[0-9]+)/", RegexOptions.IgnoreCase);
                string key;
                if (match.Success)
                {
                     key = value.ToString().ToUpper() + match.Groups[1].Value.ToUpper();
                }
                else
                {
                    key = value.ToString().ToUpper() + "V1";
                }
                if (dic.TryGetValue(key, out var cDescriptor))
                {
                    return cDescriptor;
                }
            } 
            return base.SelectController(request);
        }
    }
}