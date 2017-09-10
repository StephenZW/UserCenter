using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UserCenter.IServices;

namespace UserCenter.OpenAPI.Controllers.v1
{
    /// <summary>
    /// WebApi 版本 v1
    /// </summary>
    public class UserController : ApiController
    {
        public IUserService UserService { get; set; }

        /// <summary>
        /// 新增用户 -- v1
        /// </summary>
        /// <param name="phoneNum">手机号码</param>
        /// <param name="nickName">昵称</param>
        /// <param name="password">密码</param>
        /// <returns>id long</returns>
        [HttpPost]
        public async Task<long> AddNew(string phoneNum,string nickName,string password)
        {
            return await UserService.AddNewAsync(phoneNum, nickName, password);
        }
        /// <summary>
        /// test -- v1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Test()
        {
            return DateTime.Now.ToString() + "---v1---";
        }
    }
}
