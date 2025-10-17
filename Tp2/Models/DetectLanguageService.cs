using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Models
{
    public sealed class DetectLanguageService : IDisposable
    {
        private const string BaseUrl = "https://ws.detectlanguage.com/0.2";

        private readonly ApiClient _client;

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

            var dto = JsonConvert.DeserializeObject<DetectResponseDto>(json)
                      ?? throw new Exception("Réponse invalide de /detect.");

            var results = dto.data.detections.Select(d => new DetectionCandidate
            {
                LanguageCode = d.language,
                LanguageName = GetLanguageName(d.language), 
                Confidence = d.confidence,
                IsReliable = d.isReliable
            }).ToList();

            return results;
        }

       
        public async Task<StatusResponseDto> GetStatusAsync()
        {
            EnsureToken();

            var json = await _client.GetAsync("/user/status");
            return JsonConvert.DeserializeObject<StatusResponseDto>(json)
                ?? throw new Exception("Réponse invalide de /user/status.");
        }

        private static string GetLanguageName(string code)
        {
            try
            {
                return new CultureInfo(code).EnglishName;
            }
            catch
            {
                return code?.ToUpperInvariant() ?? string.Empty;
            }
        }

        public void Dispose() => _client.Dispose();
    }
}
