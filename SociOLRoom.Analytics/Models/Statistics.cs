using System;
using SociOLRoom.Analytics.Core;

namespace SociOLRoom.Analytics.Models
{
    public class Statistics
    {
        public int Women { get; set; }
        public int Men { get; set; }
        public int Age { get; set; }
        public NamePercent Emotion { get; set; }

        public int Children { get; set; }
        public DateTime? From { get; set; }
        public NamePercent Category { get; set; }
    }
}