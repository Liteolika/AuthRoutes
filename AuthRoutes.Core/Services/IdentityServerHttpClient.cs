using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core.Services
{

    public interface IIdentityServerHttpClient
    {
        Task<ApiResult<string[]>> GetValues();
    }

    public class IdentityServerHttpClient : HttpClientBase, IIdentityServerHttpClient
    {
        public IdentityServerHttpClient(IHttpContextAccessor httpContextAccessor) : base(
                new TokenService(
                    SystemConfig.IdentityServerUrl,
                    "webapi_backend",
                    "webapi_backend",
                    IdentityConfig.Scopes.IdServer),
                httpContextAccessor,
                new Uri(SystemConfig.IdentityServerUrl))
        {

        }

        public Task<ApiResult<string[]>> GetValues()
        {
            return base.GetAsync<string[]>("/api/values");
        }
    }

}
