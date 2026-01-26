using MySql.Data.MySqlClient;
using ReviewByWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ReviewByWpf.Services
{
    public class MySqlReviewRepository : IReviewRepository
    {
        private readonly string connectionString;

        public MySqlReviewRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Review> GetReviews()
        {
            var reviews = new List<Review>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT " +
                    "id, title, content, poster_path, " + 
                    "created_time, updated_time, category_id, first_year, tmdb_id " +
                    "FROM review " +
                    "ORDER BY id DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reviews.Add(new Review
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.IsDBNull(reader.GetOrdinal("title")) ? "" : reader.GetString("title"),
                            Content = reader.IsDBNull(reader.GetOrdinal("content")) ? "" : reader.GetString("content"),
                            PosterPath = reader.IsDBNull(reader.GetOrdinal("poster_path")) ? "" : reader.GetString("poster_path"),
                            CreatedTime = reader.IsDBNull(reader.GetOrdinal("created_time")) ? DateTime.MinValue : reader.GetDateTime("created_time"),
                            UpdatedTime = reader.IsDBNull(reader.GetOrdinal("updated_time")) ? DateTime.MinValue : reader.GetDateTime("updated_time"),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("category_id")) ? 0 : reader.GetInt32("category_id"),
                            FirstYear = reader.IsDBNull(reader.GetOrdinal("first_year")) ? "" : reader.GetString("first_year"),
                            TmdbId = reader.IsDBNull(reader.GetOrdinal("tmdb_id")) ? 0 : reader.GetInt32("tmdb_id")
                        }); ;
                    }
                }                
            }
            return reviews;
        }

        public Review GetReviewById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT " +
                    "id, title, content, created_time " +
                    "FROM review " + 
                    "WHERE id=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Review
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.IsDBNull(reader.GetOrdinal("title")) ? "" : reader.GetString("title"),
                                Content = reader.IsDBNull(reader.GetOrdinal("content")) ? "" : reader.GetString("content"),
                                CreatedTime = reader.IsDBNull(reader.GetOrdinal("created_time")) ? DateTime.MinValue : reader.GetDateTime("created_time")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateReview(int id, string content, string posterUrl, string title, string firstYear, int? tmdbId)
        {
            string posterPath = posterUrl.Replace("https://image.tmdb.org/t/p/w200", "");
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE review SET " +
                    "content=@content, " +
                    "poster_path=@posterPath, " +
                    "title=@title, " +
                    "first_year=@firstYear, " +
                    "tmdb_id=@TmdbId, " +
                    "updated_time=NOW() " +
                    "WHERE id=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@posterPath", posterPath);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@firstYear", firstYear);
                    cmd.Parameters.AddWithValue("@TmdbId", tmdbId);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddReview(string title, string content, string posterUrl, int categoryId, string firstYear, int? tmdbId)
        {
            string posterPath = posterUrl.Replace("https://image.tmdb.org/t/p/w200", "");
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO review " +
                    "(title, content, poster_path, category_id, " + 
                    " created_time, first_year, tmdb_id) " +
                    "values (" +
                    "@title, @content, @posterPath, @categoryId, " +
                    "NOW(), @firstYear, @tmdbId)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@posterPath", posterPath);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@firstYear", firstYear);
                    cmd.Parameters.AddWithValue("@tmdbId", tmdbId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteReview(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM review " +
                    "WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
