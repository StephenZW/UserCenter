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
    public class UserController : ApiController
    {
        public IUserService UserService { get; set; }

        [HttpPost]
        public async Task<long> AddNew(string phoneNum,string nickName,string password)
        {
            return await UserService.AddNewAsync(phoneNum, nickName, password);
        }

        [HttpGet]
        public string Test()
        {
            return DateTime.Now.ToString() + "---v1---";
        }
    }
}
