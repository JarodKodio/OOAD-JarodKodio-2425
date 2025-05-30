using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
namespace BenchmarkToolLibrary.Models
{
    public class Cost
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string CosttypeType { get; set; }
        public int CategoryNr { get; set; }
        public int YearreportId { get; set; }

        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        // Lege constructor voor databinding
        public Cost()
        {
            CosttypeType = string.Empty;
            Value = 0;
        }

        // Constructor voor ophalen uit database
        public Cost(int id, decimal value, string costtypeType, int categoryNr, int yearreportId)
        {
            Id = id;
            Value = value;
            CosttypeType = costtypeType;
            CategoryNr = categoryNr;
            YearreportId = yearreportId;
        }
        public static void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Costs WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Cost> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            List<Cost> costs = new List<Cost>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Costs", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cost cost = new Cost(
                        (int)reader["id"],
                        (decimal)reader["value"],
                        (string)reader["costtype_type"],
                        (int)reader["category_nr"],
                        (int)reader["yearreport_id"]);
                    costs.Add(cost);
                }
            }

            return costs;
        }

        public static Cost? GetById(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Costs WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Cost(
                        (int)reader["id"],
                        (decimal)reader["value"],
                        (string)reader["costtype_type"],
                        (int)reader["category_nr"],
                        (int)reader["yearreport_id"]);
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
                    "UPDATE Costs SET value=@value, costtype_type=@costtypeType, category_nr=@categoryNr, yearreport_id=@yearreportId WHERE id=@id", conn);

                cmd.Parameters.AddWithValue("@value", Value);
                cmd.Parameters.AddWithValue("@costtypeType", CosttypeType);
                cmd.Parameters.AddWithValue("@categoryNr", CategoryNr);
                cmd.Parameters.AddWithValue("@yearreportId", YearreportId);
                cmd.Parameters.AddWithValue("@id", Id);

                cmd.ExecuteNonQuery();
            }
        } 
        public override string ToString()
        {
            return $"Cost {Id}: {Value} [{CosttypeType}]";
        }
    }
}
