using JWT;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserCenter.Common;
using UserCenter.DTO;
using UserCenter.IServices;
using UserCenter.OpenAPI.Controllers.v1;


namespace UserCenter.OpenAPI.App_Start
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public IAppInfoService MyProperty { get; set; }
        public IUserService MyProperty2 { get; set; }
        private IAppInfoService _appInfoService;
        public AuthorizationFilter(IAppInfoService appInfoService)
        {
            this._appInfoService = appInfoService;
        }

        Regex version = new Regex(@"\.v(\d+)$");

        public bool AllowMultiple => true;

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var attrs = actionContext.ActionDescriptor.GetCustomAttributes<SkipAuthAttribute>(true);
            if (attrs.Count == 1)
            {
                return await continuation();
            }
            attrs = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<SkipAuthAttribute>(true);
            if (attrs.Count == 1)
            {
                return await continuation();
            }
            var headers = actionContext.Request.Headers;

            var cNameSpace = actionContext.ControllerContext.ControllerDescriptor.ControllerType.Namespace;
            var match = version.Match(cNameSpace);
            if (match.Success
                && float.TryParse(match.Groups[1].Value, out var v)
                && v > 2)
            {
                if (!headers.TryGetValues("JWT", out var jwt))
                {
                    return Content(HttpStatusCode.Unauthorized, "JWT为空");
                }
                try
                {
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IDateTimeProvider provider = new UtcDateTimeProvider();
                    IJwtValidator validator = new JwtValidator(serializer, provider);
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                    var secret = WebHelper.AppSetting(WebHelper.jwtKey);
                    var data = decoder.DecodeToObject<Payload>(jwt.FirstOrDefault(), secret, true);
                    return await continuation();
                }
                catch (TokenExpiredException)
                {
                    return Content(HttpStatusCode.Unauthorized, "Token 已过期");
                }
                catch (SignatureVerificationException)
                {
                    return Content(HttpStatusCode.Unauthorized, "签名错误！");
                }
            }

            if (!headers.TryGetValues("AppKey", out var appKeys))
            {
                return Content(HttpStatusCode.Unauthorized, "AppKey为空");
            }
            if (!headers.TryGetValues("Sign", out var signs))
            {
                return Content(HttpStatusCode.Unauthorized, "Sign为空");
            }
            string appkey = appKeys.FirstOrDefault();
            string sign = signs.FirstOrDefault();
            var appInfo = await _appInfoService.GetByAppKeyAsync(appkey);

            if (appInfo == null)
            {
                return Content(HttpStatusCode.Unauthorized, "AppKey错误");
            }

            var paramArr = actionContext.Request
                  .GetQueryNameValuePairs()
                  .OrderBy(kv => kv.Key)
                  .Select(kv => kv.Key + "=" + kv.Value)
                  .ToArray();

            string sign2 = Algorithm.ToMD5(string.Join("&", paramArr) + appInfo.AppSecret);

            if (sign != sign2)
            {
                return Content(HttpStatusCode.Unauthorized, "签名错误");
            }
            return await continuation();
        }

        static HttpResponseMessage Content(HttpStatusCode statusCode, string content)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content)
            };
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SkipAuthAttribute : Attribute
    {

    }

    public class Payload : UserDTO
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public double exp { get; set; }
    }
}