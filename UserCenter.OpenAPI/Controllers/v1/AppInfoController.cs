using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UserCenter.Common;
using UserCenter.DTO;
using UserCenter.IServices;

namespace UserCenter.OpenAPI.Controllers.v1
{
    /// <summary>
    /// appkey 管理
    /// </summary>
    public class AppInfoController : ApiController
    {
        public IAppInfoService AppInfoService { get; set; }

        /// <summary>
        /// 签名 参数
        /// </summary>
        /// <param name="appKey">key</param>
        /// <param name="params">参数</param>
        /// <returns></returns>
        public async Task<string> Sign(string appKey, string @params)
        {
            @params = @params.Trim('?', ' ');
            string result = string.Join("&", @params.Split('&').OrderBy(s => s));
            var appInfo = await AppInfoService.GetByAppKeyAsync(appKey);

            if (appInfo == null)
            {
                return "AppKey错误";
            }
            return Algorithm.ToMD5(result + appInfo.AppSecret);

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="name">key 名字描述</param>
        /// <param name="appKey">key</param>
        /// <returns></returns>
        public async Task<long> AddNew(string name, string appKey)
        {
            return await AppInfoService.AddNewAsync(name, appKey);
        }
        /// <summary>
        /// 获取key 信息
        /// </summary>
        /// <param name="appKey">key</param>
        /// <returns></returns>
        public async Task<AppInfoDTO> GetByAppKey(string appKey)
        {
            return await AppInfoService.GetByAppKeyAsync(appKey);
        }



    }
}
