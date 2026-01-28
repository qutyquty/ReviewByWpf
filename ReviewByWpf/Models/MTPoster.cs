using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewByWpf.Models
{
    public class MTPoster
    {
        public int TmdbId { get; set; }
        public string PosterUrl { get; set; } // tmdb이미지url+poster_path
        public string FirstYear { get; set; }
    }
}
