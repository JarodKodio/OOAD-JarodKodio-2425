using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace BenchmarkToolLibrary.Models
{
    public class Costtype
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string TextFr { get; set; }
        public string TextEn { get; set; }

        private static string connectionString = ConfigurationManager.ConnectionStrings["BenchmarkDB"].ConnectionString;

        // Lege constructor voor databinding
        public Costtype()
        {
            Type = string.Empty;
            Text = string.Empty;
            TextFr = string.Empty;
            TextEn = string.Empty;
        }

        // Constructor voor ophalen uit database
        public Costtype(string type, string text, string textFr, string textEn)
        {
            Type = type;
            Text = text;
            TextFr = textFr;
            TextEn = textEn;
        }

        public void Update()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Costtypes SET text=@text, textFr=@textFr, textEn=@textEn WHERE type=@type", conn);

                cmd.Parameters.AddWithValue("@text", Text);
                cmd.Parameters.AddWithValue("@textFr", TextFr);
                cmd.Parameters.AddWithValue("@textEn", TextEn);
                cmd.Parameters.AddWithValue("@type", Type);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(string type)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Costtypes WHERE type = @type", conn);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Costtype> GetAll()
        {
            List<Costtype> list = new List<Costtype>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Costtypes", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Costtype ct = new Costtype(
                        (string)reader["type"],
                        (string)reader["text"],
                        (string)reader["textFr"],
                        (string)reader["textEn"]
                    );
                    list.Add(ct);
                }
            }

            return list;
        }

        public static Costtype? GetByType(string type)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Costtypes WHERE type = @type", conn);
                cmd.Parameters.AddWithValue("@type", type);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Costtype(
                        (string)reader["type"],
                        (string)reader["text"],
                        (string)reader["textFr"],
                        (string)reader["textEn"]
                    );
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"{Type} - {Text}";
        }
    }
}
