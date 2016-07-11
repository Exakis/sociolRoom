using System.ComponentModel.DataAnnotations;
using LinqToTwitter;

namespace SociOLRoom.Analytics.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }
}