using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tp2.Models
{
    public sealed class DetectV3Item
    {
        public string language { get; set; } = "";
        public float score { get; set; }
    }

    public sealed class StatusV3Dto
    {
        public string date { get; set; } = "";
        public int requests { get; set; }
        public int bytes { get; set; }
        public string plan { get; set; } = "";
        public string? plan_expires { get; set; }
        public int daily_requests_limit { get; set; }
        public int daily_bytes_limit { get; set; }
        public string status { get; set; } = "";
    }

    public sealed class LanguageDto
    {
        public string code { get; set; } = "";
        public string name { get; set; } = "";
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
