using ReviewByWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ReviewByWpf.Services
{
    public class ReviewRepository
    {
        private string connectionString = "Server=localhost;Database=db_review;Uid=db_review_user;Pwd=1234;";

        public List<Review> GetReviews()
        {
            var reviews = new List<Review>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT " +
                    "id, title, content, created_time " +
                    "FROM review ";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        reviews.Add(new Review
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.IsDBNull(reader.GetOrdinal("title")) ? string.Empty : reader.GetString("title"),
                            Content = reader.IsDBNull(reader.GetOrdinal("content")) ? string.Empty : reader.GetString("content"),
                            CreatedTime = reader.IsDBNull(reader.GetOrdinal("created_time")) ? null : reader.GetDateTime("created_time")
                        }); ;
                    }
                }
                return reviews;
            }
        }
    }
}
