using AuthRoutes.IdServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthRoutes.IdServer.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<string>> Values()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
