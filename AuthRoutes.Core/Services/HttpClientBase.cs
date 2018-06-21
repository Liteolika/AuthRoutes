using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core.Services
{
    public abstract class HttpClientBase
    {
        private readonly HttpClient _client;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public HttpClientBase(ITokenService tokenService, IHttpContextAccessor httpContextAccessor, Uri baseUri)
        {
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                FloatParseHandling = FloatParseHandling.Double
            };

            _client = new HttpClient { BaseAddress = baseUri };

        }

        protected async Task<ApiResult<T>> PutAsync<T>(string url, object data) where T : class
        {
            try
            {
                _client.SetBearerToken(await _tokenService.GetBearerToken());
                var bikeUserContent = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
                var buffer = System.Text.Encoding.UTF8.GetBytes(bikeUserContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                AddUserToRequest(byteContent);

                var response = await _client.PutAsync(url, byteContent);
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(stringResult, _jsonSerializerSettings);

                return ApiResult<T>.AsHappyDandy(result);
            }
            catch (Exception exception)
            {
                // TODO: Implement some logging to get errors.
                return null;
            }
        }

        protected async Task<ApiResult<T>> PostAsync<T>(string url, object data) where T : class
        {
            try
            {
                _client.SetBearerToken(await _tokenService.GetBearerToken());
                var bikeUserContent = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
                var buffer = System.Text.Encoding.UTF8.GetBytes(bikeUserContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                AddUserToRequest(byteContent);

                var response = await _client.PostAsync(url, byteContent);

                var stringResult = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ApiResult<T>.AsDeathAndSorrow(stringResult);
                }

                response.EnsureSuccessStatusCode();

                var result = JsonConvert.DeserializeObject<T>(stringResult, _jsonSerializerSettings);
                return ApiResult<T>.AsHappyDandy(result);
            }
            catch (Exception exception)
            {
                // TODO: Implement some logging to get errors.
                return ApiResult<T>.AsDeathAndSorrow("Unhandled error");
            }
        }

        protected async Task<ApiResult> PostAsync(string url, object data)
        {
            try
            {
                _client.SetBearerToken(await _tokenService.GetBearerToken());
                var bikeUserContent = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
                var buffer = System.Text.Encoding.UTF8.GetBytes(bikeUserContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                AddUserToRequest(byteContent);

                var response = await _client.PostAsync(url, byteContent);

                var stringResult = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ApiResult.AsDeathAndSorrow(stringResult);
                }

                response.EnsureSuccessStatusCode();

                return ApiResult.AsHappyDandy();
            }
            catch (Exception exception)
            {
                // TODO: Implement some logging to get errors.
                return ApiResult.AsDeathAndSorrow("Unhandled error");
            }
        }

        protected async Task<ApiResult<T>> GetAsync<T>(string url) where T : class
        {
            HttpResponseMessage response = null;
            try
            {
                _client.SetBearerToken(await _tokenService.GetBearerToken());
                response = await _client.GetAsync(url);
                var stringResult = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return ApiResult<T>.AsDeathAndSorrow(stringResult);
                }

                response.EnsureSuccessStatusCode();

                var deserializedResult = JsonConvert.DeserializeObject<T>(stringResult, _jsonSerializerSettings);
                return ApiResult<T>.AsHappyDandy(deserializedResult);

            }
            catch (Exception exception)
            {
                // TODO: Implement some logging to get errors.
                return ApiResult<T>.AsDeathAndSorrow("Unhandled error");
            }

        }

        protected async Task<T> DeleteAsync<T>(string url) where T : class
        {
            HttpResponseMessage response = null;
            try
            {
                _client.SetBearerToken(await _tokenService.GetBearerToken());

                var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

                AddUserToRequest(requestMessage);

                response = await _client.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var deserializedResult = JsonConvert.DeserializeObject<T>(stringResult, _jsonSerializerSettings);
                return deserializedResult;

            }
            catch (Exception exception)
            {
                // TODO: Implement some logging to get errors.
                return null;
            }
        }

        private void AddUserToRequest(HttpRequestMessage requestMessage)
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            requestMessage.Headers.Add(ApiBoundaryUserHandler.HeaderName, user);
        }

        private void AddUserToRequest(ByteArrayContent byteContent)
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            byteContent.Headers.Add(ApiBoundaryUserHandler.HeaderName, user);
        }
    }
}
