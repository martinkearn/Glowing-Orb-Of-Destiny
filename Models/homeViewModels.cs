using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sentimentR_frontend.Models
{
    public class homeViewModels
    {
        public string Background { get; set; }
        public string Box { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public bool ShowScore { get; set; }
        public int RefreshFrequency { get; set; }
        public bool ShowDots { get; set; }
    }
}