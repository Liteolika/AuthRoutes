using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthRoutes.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthRoutes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        public readonly IIdentityServerHttpClient _identityServerHttpClient;

        public ValuesController(IIdentityServerHttpClient identityServerHttpClient)
        {
            _identityServerHttpClient = identityServerHttpClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var values = await _identityServerHttpClient.GetValues();
            return values.Result;
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
