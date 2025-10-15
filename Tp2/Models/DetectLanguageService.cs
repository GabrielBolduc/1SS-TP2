using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Models
{
    public sealed class DetectLanguageService : IDisposable
    {
        private const string BaseUrl = "https://api.detectlanguage.com/v2";
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

        // ⬇️ Doivent être PUBLICS
        public async Task<IReadOnlyList<DetectionCandidate>> DetectAsync(string text)
        {
            EnsureToken();
            var body = JsonConvert.SerializeObject(new { q = text });
            var json = await _client.PostJsonAsync("/detect", body);
            var dto = JsonConvert.DeserializeObject<DetectResponseDto>(json)
                      ?? throw new Exception("Réponse invalide de /detect.");

            if (_langMap.Count == 0)
                await WarmLanguagesAsync();

            var list = new List<DetectionCandidate>();
            foreach (var d in dto.data.detections)
            {
                _langMap.TryGetValue(d.language, out var longName);
                list.Add(new DetectionCandidate
                {
                    LanguageCode = d.language,
                    LanguageName = longName ?? d.language.ToUpperInvariant(),
                    Confidence = d.confidence,
                    IsReliable = d.isReliable
                });
            }
            return list;
        }

        public async Task<StatusResponseDto> GetStatusAsync()
        {
            EnsureToken();
            var json = await _client.GetAsync("/user/status");
            return JsonConvert.DeserializeObject<StatusResponseDto>(json)
                ?? throw new Exception("Réponse invalide de /user/status.");
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
