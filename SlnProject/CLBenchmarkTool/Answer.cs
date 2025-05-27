using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace BenchmarkToolLibrary.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int YearreportId { get; set; }
        public string Value { get; set; }

        private static string connectionString = ConfigurationManager.ConnectionStrings["BenchmarkDB"].ConnectionString;

        // Lege constructor
        public Answer()
        {
            Value = string.Empty;
        }

        // Constructor voor ophalen uit database
        public Answer(int id, int questionId, int yearreportId, string value)
        {
            Id = id;
            QuestionId = questionId;
            YearreportId = yearreportId;
            Value = value;
        }

        public void Update()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Answers SET value=@value, question_id=@questionId, yearreport_id=@yearreportId WHERE id=@id", conn);

                cmd.Parameters.AddWithValue("@value", Value);
                cmd.Parameters.AddWithValue("@questionId", QuestionId);
                cmd.Parameters.AddWithValue("@yearreportId", YearreportId);
                cmd.Parameters.AddWithValue("@id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Answers WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Answer> GetAll()
        {
            List<Answer> answers = new List<Answer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Answers", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Answer a = new Answer(
                        (int)reader["id"],
                        (int)reader["question_id"],
                        (int)reader["yearreport_id"],
                        (string)reader["value"]
                    );
                    answers.Add(a);
                }
            }

            return answers;
        }

        public static Answer? GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Answers WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Answer(
                        (int)reader["id"],
                        (int)reader["question_id"],
                        (int)reader["yearreport_id"],
                        (string)reader["value"]
                    );
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"Answer {Id}: Q{QuestionId}, Report {YearreportId}, Value = {Value}";
        }
    }
}
