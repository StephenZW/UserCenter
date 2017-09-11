using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UserCenter.OpenAPI.App_Start
{
    public static class WebHelper
    {
        public const string jwtKey = "jwtKey";
        static JsonSerializerSettings settings = null;
        public static JsonSerializerSettings CreateSerializerSettings()
        {
            if (settings != null)
            {
                return settings;
            }
            settings = new JsonSerializerSettings();

            //将C#中的大写开头的属性转换为小写开头 
            DefaultContractResolver resolver1 = new DefaultContractResolver();
            resolver1.NamingStrategy = new CamelCaseNamingStrategy();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//json序列化是默认的时间格式
            settings.ContractResolver = resolver1;
            //忽略反序列化时不在Class中的对象
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.MaxDepth = 32;//递归层次32次
            settings.TypeNameHandling = TypeNameHandling.None;
          
            return settings;
        }

        public static string AppSetting(string key)
        {
            return ConfigurationManager.AppSettings["jwtKey"];
        }

    }
}