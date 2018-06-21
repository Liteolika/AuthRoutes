using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthRoutes.Core.Services
{
    public interface IWebApiHttpClient
    {
        Task<ApiResult<string[]>> GetValues();
    }

    public class WebApiHttpClient : HttpClientBase, IWebApiHttpClient
    {
        public WebApiHttpClient(IHttpContextAccessor httpContextAccessor) : base(
                new TokenService(
                    SystemConfig.IdentityServerUrl,
                    "webapplication_backend",
                    "webapplication_backend", 
                    IdentityConfig.Scopes.WebApi), 
                httpContextAccessor, 
                new Uri(SystemConfig.WebApiUrl))
        {

        }

        public Task<ApiResult<string[]>> GetValues()
        {
            return base.GetAsync<string[]>("/api/values");
        }
    }
}
