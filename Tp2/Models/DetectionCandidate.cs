using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2.Models
{
    public sealed class DetectionCandidate
    {
        public string LanguageCode { get; init; } = "";
        public string LanguageName { get; init; } = "";
        public float Confidence { get; init; }
        public bool IsReliable { get; init; }
    }
}
