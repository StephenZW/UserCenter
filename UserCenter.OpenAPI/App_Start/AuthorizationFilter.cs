using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserCenter.Common;
using UserCenter.IServices;

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
        public bool AllowMultiple => true;

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ControllerContext.ControllerDescriptor.ControllerName.Equals("AppInfoController"))
            {
                return await continuation();
            }
            var headers = actionContext.Request.Headers;
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
}