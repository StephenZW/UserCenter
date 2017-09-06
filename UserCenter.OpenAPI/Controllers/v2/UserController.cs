using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UserCenter.OpenAPI.Controllers.v2
{
    public class UserController : ApiController
    {
        [HttpGet]
        public string Test()
        {
            return DateTime.Now.ToString() + "---v2---";
        }
    }
}
