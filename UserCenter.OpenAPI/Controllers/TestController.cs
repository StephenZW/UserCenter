using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UserCenter.IServices;

namespace UserCenter.OpenAPI.Controllers
{
    public class TestController : ApiController
    {
        public IUserService UserService { get; set; }

        public async Task<IEnumerable<string>> Get()
        {
            bool exists = await UserService.UserExistsAsync("134");
            return new[] { exists.ToString(), DateTime.Now.ToString() };
        }
    }
}
