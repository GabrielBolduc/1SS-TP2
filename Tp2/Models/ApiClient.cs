using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tp2.Models
{
    public sealed class ApiClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _http = new HttpClient();
        }

        public void SetBearer(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = null;
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<string> GetAsync(string path)
        {
            var res = await _http.GetAsync(_baseUrl + path);
            var content = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} {res.ReasonPhrase}: {content}");
            return content;
        }

        public async Task<string> PostJsonAsync(string path, string jsonBody)
        {
            using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var res = await _http.PostAsync(_baseUrl + path, content);
            var str = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} {res.ReasonPhrase}: {str}");
            return str;
        }

        public async Task<string> PostFormAsync(string path, IEnumerable<KeyValuePair<string, string>> fields)
        {
            using var content = new FormUrlEncodedContent(fields);
            var res = await _http.PostAsync(_baseUrl + path, content);
            var str = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} {res.ReasonPhrase}: {str}");
            return str;
        }

        public void Dispose() => _http.Dispose();
    }
}
