using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

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

        private static string connectionString = ConfigurationManager.ConnectionStrings["BenchmarkDB"].ConnectionString;

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
        public Company(int id, string name, string contact, string address, string zip, string city, string country,
                       string phone, string email, string btw, string login, string password, DateTime regDate,
                       DateTime? acceptDate, DateTime? lastModified, string status, string language, byte[] logo, string nacecodeCode)
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

        public bool CheckPassword(string plainText)
        {
            return HashPassword(plainText) == Password;
        }

        private string HashPassword(string plain)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
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

        public static void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Companies WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Login}) - Status: {Status}";
        }
    }
}
