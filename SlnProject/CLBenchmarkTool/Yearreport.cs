using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
namespace BenchmarkToolLibrary.Models
{
    public class Yearreport
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int CompanyId { get; set; }
        
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        // Lege constructor
        public Yearreport()
        {
            Year = DateTime.Now.Year;
        }

        // Constructor voor ophalen uit database
        public Yearreport(int id, int year, int companyId)
        {
            Id = id;
            Year = year;
            CompanyId = companyId;
        }

        public void Update()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Yearreports SET year=@year, company_id=@companyId WHERE id=@id", conn);

                cmd.Parameters.AddWithValue("@year", Year);
                cmd.Parameters.AddWithValue("@companyId", CompanyId);
                cmd.Parameters.AddWithValue("@id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Yearreports WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Yearreport> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            List<Yearreport> list = new List<Yearreport>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Yearreports", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Yearreport yr = new Yearreport(
                        (int)reader["id"],
                        (int)reader["year"],
                        (int)reader["company_id"]
                    );
                    list.Add(yr);
                }
            }

            return list;
        }

        public static Yearreport? GetById(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Yearreports WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Yearreport(
                        (int)reader["id"],
                        (int)reader["year"],
                        (int)reader["company_id"]
                    );
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"Year {Year} - Company {CompanyId}";
        }
    }
}
