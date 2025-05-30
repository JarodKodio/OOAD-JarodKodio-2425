using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
namespace BenchmarkToolLibrary.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Btw { get; set; }
        public string Login { get; set; }
        public string Password { get; private set; }
        public DateTime RegDate { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? LastModified { get; set; }
        public string Status { get; private set; }
        public string Language { get; set; }
        public byte[] Logo { get; set; }
        public string NacecodeCode { get; set; }
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        // Lege constructor
        public Company()
        {
            Name = string.Empty;
            Contact = string.Empty;
            Address = string.Empty;
            Zip = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Btw = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            Status = "pending";
            Language = "nl";
            Logo = Array.Empty<byte>();
            RegDate = DateTime.Now;
        }

        // Constructor voor ophalen uit database
        public Company(int id, string name, string contact, string address, string zip, string city, string country, string phone, string email, string btw, string login, string password, DateTime regDate, DateTime? acceptDate, DateTime? lastModified, string status, string language, byte[] logo, string nacecodeCode)
        {
            Id = id;
            Name = name;
            Contact = contact;
            Address = address;
            Zip = zip;
            City = city;
            Country = country;
            Phone = phone;
            Email = email;
            Btw = btw;
            Login = login;
            Password = password;
            RegDate = regDate;
            AcceptDate = acceptDate;
            LastModified = lastModified;
            Status = status;
            Language = language;
            Logo = logo;
            NacecodeCode = nacecodeCode;
        }
        public static List<Company> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            List<Company> companies = new List<Company>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Companies", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Company c = new Company(
    (int)reader["id"],
    (string)reader["name"],
    (string)reader["contact"],
    (string)reader["address"],
    (string)reader["zip"],
    (string)reader["city"],
    (string)reader["country"],
    (string)reader["phone"],
    (string)reader["email"],
    (string)reader["btw"],
    (string)reader["login"],
    (string)reader["password"],
    (DateTime)reader["regdate"],
    reader["acceptdate"] == DBNull.Value ? null : (DateTime?)reader["acceptdate"],
    reader["lastmodified"] == DBNull.Value ? null : (DateTime?)reader["lastmodified"],
    (string)reader["status"],
    (string)reader["language"],
    reader["logo"] == DBNull.Value ? Array.Empty<byte>() : (byte[])reader["logo"],
    (string)reader["nacecode_code"]);
                    companies.Add(c);
                }
            }
            return companies;
        }
        public static Company? GetByLogin(string login)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Companies WHERE login = @login", conn);
                cmd.Parameters.AddWithValue("@login", login);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Company(
                        (int)reader["id"],
                        (string)reader["name"],
                        (string)reader["contact"],
                        (string)reader["address"],
                        (string)reader["zip"],
                        (string)reader["city"],
                        (string)reader["country"],
                        (string)reader["phone"],
                        (string)reader["email"],
                        (string)reader["btw"],
                        (string)reader["login"],
                        (string)reader["password"],
                        (DateTime)reader["regdate"],
                        reader["acceptdate"] == DBNull.Value ? null : (DateTime?)reader["acceptdate"],
                        reader["lastmodified"] == DBNull.Value ? null : (DateTime?)reader["lastmodified"],
                        (string)reader["status"],
                        (string)reader["language"],
                        reader["logo"] == DBNull.Value ? Array.Empty<byte>() : (byte[])reader["logo"],
                        (string)reader["nacecode_code"]);
                }
            }
            return null;
        }
        public static void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Companies WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        public bool CheckPassword(string plainText)
        {
            return HashPassword(plainText) == Password;
        }

        public string HashPassword(string plain)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2")); // hoofdletters
                }
                return sb.ToString();
            }
        }

        public void ChangeStatus(string newStatus)
        {
            if (newStatus == "active" || newStatus == "pending" || newStatus == "suspended" || newStatus == "rejected")
            {
                Status = newStatus;
            }
        }
        public void Update()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Companies SET name=@name, contact=@contact, address=@address, zip=@zip, city=@city, country=@country, phone=@phone, email=@email, btw=@btw, login=@login, password=@password, regdate=@regdate, acceptdate=@acceptdate, lastmodified=@lastmodified, status=@status, language=@language, logo=@logo, nacecode_code=@nace WHERE id=@id", conn);

                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@contact", Contact);
                cmd.Parameters.AddWithValue("@address", Address);
                cmd.Parameters.AddWithValue("@zip", Zip);
                cmd.Parameters.AddWithValue("@city", City);
                cmd.Parameters.AddWithValue("@country", Country);
                cmd.Parameters.AddWithValue("@phone", Phone);
                cmd.Parameters.AddWithValue("@email", Email);
                cmd.Parameters.AddWithValue("@btw", Btw);
                cmd.Parameters.AddWithValue("@login", Login);
                cmd.Parameters.AddWithValue("@password", Password);
                cmd.Parameters.AddWithValue("@regdate", RegDate);
                cmd.Parameters.AddWithValue("@acceptdate", (object?)AcceptDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lastmodified", (object?)LastModified ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", Status);
                cmd.Parameters.AddWithValue("@language", Language);
                cmd.Parameters.AddWithValue("@logo", Logo);
                cmd.Parameters.AddWithValue("@nace", NacecodeCode);
                cmd.Parameters.AddWithValue("@id", Id);

                cmd.ExecuteNonQuery();
            }
        }
        public override string ToString()
        {
            return $"{Name} ({Login}) - Status: {Status}";
        }
    }
}
