using System.Collections.Generic;

namespace Tp2.Models
{
    public sealed class DetectResponseDto
    {
        public DetectData data { get; set; } = new();
    }

    public sealed class DetectData
    {
        public List<Detection> detections { get; set; } = new();
    }

    public sealed class Detection
    {
        public string language { get; set; } = "";
        public float confidence { get; set; }
        public bool isReliable { get; set; }
    }

    public sealed class StatusResponseDto
    {
        public string date { get; set; } = "";
        public int requests_today { get; set; }
        public int bytes_today { get; set; }
        public string plan { get; set; } = "";
        public string plan_expires { get; set; } = "";
        public int daily_requests_limit { get; set; }
        public int daily_bytes_limit { get; set; }
        public string status { get; set; } = "";
    }

    public sealed class DetectionCandidate
    {
        public string LanguageCode { get; init; } = "";
        public string LanguageName { get; init; } = "";
        public float Confidence { get; init; }
        public bool IsReliable { get; init; }
    }
}
