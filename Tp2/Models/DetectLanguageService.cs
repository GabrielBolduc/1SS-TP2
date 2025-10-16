using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Models
{
    public sealed class DetectLanguageService : IDisposable
    {
        private const string BaseUrl = "https://ws.detectlanguage.com/v3";

        private readonly ApiClient _client;
        private Dictionary<string, string> _langMap = new(StringComparer.OrdinalIgnoreCase);

        public DetectLanguageService()
        {
            _client = new ApiClient(BaseUrl);
        }

        private void EnsureToken()
        {
            var token = Properties.Settings.Default.ApiToken;
            _client.SetBearer(token);
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Aucun jeton configuré.");
        }

        public async Task<IReadOnlyList<DetectionCandidate>> DetectAsync(string text)
        {
            EnsureToken();

            var form = new List<KeyValuePair<string, string>> { new("q", text) };
            var json = await _client.PostFormAsync("/detect", form);

            var items = JsonConvert.DeserializeObject<List<DetectV3Item>>(json)
                        ?? throw new Exception("Réponse invalide de /detect.");

            if (_langMap.Count == 0)
                await WarmLanguagesAsync();

            var list = new List<DetectionCandidate>();
            foreach (var it in items)
            {
                _langMap.TryGetValue(it.language, out var longName);

                list.Add(new DetectionCandidate
                {
                    LanguageCode = it.language,
                    LanguageName = longName ?? it.language.ToUpperInvariant(),
                    Confidence = it.score * 100f,     
                    IsReliable = it.score >= 0.50f
                });
            }

            return list;
        }

        public async Task<StatusResponseDto> GetStatusAsync()
        {
            EnsureToken();

            var json = await _client.GetAsync("/account/status");
            var v3 = JsonConvert.DeserializeObject<StatusV3Dto>(json)
                     ?? throw new Exception("Réponse invalide de /account/status.");

            return new StatusResponseDto
            {
                date = v3.date,
                requests_today = v3.requests,
                bytes_today = v3.bytes,
                plan = v3.plan,
                plan_expires = v3.plan_expires ?? "",
                daily_requests_limit = v3.daily_requests_limit,
                daily_bytes_limit = v3.daily_bytes_limit,
                status = v3.status
            };
        }

        public async Task WarmLanguagesAsync()
        {
            EnsureToken();

            var json = await _client.GetAsync("/languages");
            var list = JsonConvert.DeserializeObject<List<LanguageDto>>(json) ?? new();
            _langMap = list.ToDictionary(x => x.code, x => x.name, StringComparer.OrdinalIgnoreCase);
        }

        public void Dispose() => _client.Dispose();
    }
}
