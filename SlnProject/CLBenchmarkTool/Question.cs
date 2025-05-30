using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
namespace BenchmarkToolLibrary.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string TextFr { get; set; }
        public string TextEn { get; set; }
        public bool Active { get; set; }
        public int CategoryNr { get; set; }
        public string CosttypeType { get; set; }
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        // Lege constructor  
        public Question()
        {
            Text = string.Empty;
            TextFr = string.Empty;
            TextEn = string.Empty;
            CosttypeType = string.Empty;
        }

        // Constructor voor ophalen uit database  
        public Question(int id, string text, string textFr, string textEn, bool active, int categoryNr, string costtypeType)
        {
            Id = id;
            Text = text;
            TextFr = textFr;
            TextEn = textEn;
            Active = active;
            CategoryNr = categoryNr;
            CosttypeType = costtypeType;
        }
        public static void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Questions WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Question> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            List<Question> list = new List<Question>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Questions", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Question q = new Question(
                        (int)reader["id"],
                        (string)reader["text"],
                        (string)reader["textFr"],
                        (string)reader["textEn"],
                        (bool)reader["active"],
                        (int)reader["category_nr"],
                        (string)reader["costtype_type"]);
                    list.Add(q);
                }
            }

            return list;
        }

        public static Question? GetById(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Questions WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Question(
                        (int)reader["id"],
                        (string)reader["text"],
                        (string)reader["textFr"],
                        (string)reader["textEn"],
                        (bool)reader["active"],
                        (int)reader["category_nr"],
                        (string)reader["costtype_type"]);
                }
            }

            return null;
        }
        public void Update()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Questions SET text=@text, textFr=@textFr, textEn=@textEn, active=@active, category_nr=@categoryNr, costtype_type=@costtypeType WHERE id=@id", conn);

                cmd.Parameters.AddWithValue("@text", Text);
                cmd.Parameters.AddWithValue("@textFr", TextFr);
                cmd.Parameters.AddWithValue("@textEn", TextEn);
                cmd.Parameters.AddWithValue("@active", Active);
                cmd.Parameters.AddWithValue("@categoryNr", CategoryNr);
                cmd.Parameters.AddWithValue("@costtypeType", CosttypeType);
                cmd.Parameters.AddWithValue("@id", Id);

                cmd.ExecuteNonQuery();
            }
        }
        public override string ToString()
        {
            return $"Q{Id}: {Text} (Active: {Active})";
        }
    }
}
