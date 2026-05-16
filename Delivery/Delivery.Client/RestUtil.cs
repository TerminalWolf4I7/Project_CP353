using System.Net.Http.Json;

namespace Delivery.Client
{
    /// <summary>
    /// RestUtil — wrapper class สำหรับ HttpClient
    /// ใช้สำหรับติดต่อกับ Application Server (REST API)
    /// ตาม 3-Tier Architecture ที่อาจารย์กำหนด
    /// </summary>
    public static class RestUtil
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/api/")
        };

        /// <summary>
        /// GET request and deserialize JSON response
        /// </summary>
        public static async Task<T?> GetAsync<T>(string endpoint)
        {
            return await _client.GetFromJsonAsync<T>(endpoint);
        }

        /// <summary>
        /// GET request and return raw HttpResponseMessage
        /// </summary>
        public static async Task<HttpResponseMessage> GetResponseAsync(string endpoint)
        {
            return await _client.GetAsync(endpoint);
        }

        /// <summary>
        /// POST request with JSON body and deserialize response
        /// </summary>
        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var response = await _client.PostAsJsonAsync(endpoint, body);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        /// <summary>
        /// POST request with JSON body and return HttpResponseMessage
        /// </summary>
        public static async Task<HttpResponseMessage> PostResponseAsync<TRequest>(string endpoint, TRequest body)
        {
            return await _client.PostAsJsonAsync(endpoint, body);
        }

        /// <summary>
        /// POST request without body
        /// </summary>
        public static async Task<HttpResponseMessage> PostAsync(string endpoint)
        {
            return await _client.PostAsync(endpoint, null);
        }

        /// <summary>
        /// PUT request with JSON body
        /// </summary>
        public static async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T body)
        {
            return await _client.PutAsJsonAsync(endpoint, body);
        }

        /// <summary>
        /// PATCH request with JSON body
        /// </summary>
        public static async Task<HttpResponseMessage> PatchAsync<T>(string endpoint, T body)
        {
            return await _client.PatchAsJsonAsync(endpoint, body);
        }

        /// <summary>
        /// PATCH request without body
        /// </summary>
        public static async Task<HttpResponseMessage> PatchAsync(string endpoint)
        {
            return await _client.PatchAsync(endpoint, null);
        }

        /// <summary>
        /// DELETE request
        /// </summary>
        public static async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _client.DeleteAsync(endpoint);
        }

        /// <summary>
        /// Read response body as deserialized JSON
        /// </summary>
        public static async Task<T?> ReadAsAsync<T>(HttpResponseMessage response)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
