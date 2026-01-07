using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewByWpf.Models
{
    public class Review
    {
        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? PosterPath { get; set; }
        public string? FirstYear { get; set; }
        public int TmdbId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
