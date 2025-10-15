using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2.Models
{
    public class DetectResponseDto
    {
        public DetectData data { get; set; } = new();
    }

    public class DetectData
    {
        public List<DetectItem> detections { get; set; } = new();
    }

    public class DetectItem
    {
        public string language { get; set; } = "";   // "fr"
        public float confidence { get; set; }        // ex: 8.0
        public bool isReliable { get; set; }         // true / false
    }

    public class StatusResponseDto
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

    public class LanguageDto
    {
        public string code { get; set; } = "";
        public string name { get; set; } = "";
    }
}
