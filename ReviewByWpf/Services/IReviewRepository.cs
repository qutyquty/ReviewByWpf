using ReviewByWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewByWpf.Services
{
    public interface IReviewRepository
    {
        List<Review> GetReviews();
        Review GetReviewById(int id);
        void UpdateReview(int id, string content, string posterUrl, string title);
        void AddReview(string title, string contetn, string posterUrl, int categoryId);
        void DeleteReview(int id);
    }
}
