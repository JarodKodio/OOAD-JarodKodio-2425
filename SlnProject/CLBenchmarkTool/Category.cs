using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmarkTool
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Configuration;

    namespace BenchmarkToolLibrary.Models
    {
        public class Category
        {
            public int Nr { get; set; }
            public string Text { get; set; }
            public string TextFr { get; set; }
            public string TextEn { get; set; }
            public string Tooltip { get; set; }
            public string TooltipFr { get; set; }
            public string TooltipEn { get; set; }
            public string RelevantCostTypes { get; set; }
            public int? ParentNr { get; set; }

            private static string connectionString = ConfigurationManager.ConnectionStrings["BenchmarkDB"].ConnectionString;

            // Lege constructor voor databinding
            public Category()
            {
                Text = string.Empty;
                TextFr = string.Empty;
                TextEn = string.Empty;
                Tooltip = string.Empty;
                TooltipFr = string.Empty;
                TooltipEn = string.Empty;
                RelevantCostTypes = string.Empty;
            }

            // Constructor voor ophalen uit database
            public Category(int nr, string text, string textFr, string textEn, string tooltip, string tooltipFr, string tooltipEn, string relevantCostTypes, int? parentNr)
            {
                Nr = nr;
                Text = text;
                TextFr = textFr;
                TextEn = textEn;
                Tooltip = tooltip;
                TooltipFr = tooltipFr;
                TooltipEn = tooltipEn;
                RelevantCostTypes = relevantCostTypes;
                ParentNr = parentNr;
            }

            public void Update()
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Categories SET text=@text, textFr=@textFr, textEn=@textEn, tooltip=@tooltip, tooltipFr=@tooltipFr, tooltipEn=@tooltipEn, relevantCostTypes=@relevantCostTypes, parent_nr=@parentNr WHERE nr=@nr", conn);

                    cmd.Parameters.AddWithValue("@text", Text);
                    cmd.Parameters.AddWithValue("@textFr", TextFr);
                    cmd.Parameters.AddWithValue("@textEn", TextEn);
                    cmd.Parameters.AddWithValue("@tooltip", Tooltip);
                    cmd.Parameters.AddWithValue("@tooltipFr", TooltipFr);
                    cmd.Parameters.AddWithValue("@tooltipEn", TooltipEn);
                    cmd.Parameters.AddWithValue("@relevantCostTypes", RelevantCostTypes);
                    cmd.Parameters.AddWithValue("@parentNr", (object?)ParentNr ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nr", Nr);

                    cmd.ExecuteNonQuery();
                }
            }

            public static void Delete(int nr)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE nr = @nr", conn);
                    cmd.Parameters.AddWithValue("@nr", nr);
                    cmd.ExecuteNonQuery();
                }
            }

            public static List<Category> GetAll()
            {
                List<Category> categories = new List<Category>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Category c = new Category(
                            (int)reader["nr"],
                            (string)reader["text"],
                            (string)reader["textFr"],
                            (string)reader["textEn"],
                            (string)reader["tooltip"],
                            (string)reader["tooltipFr"],
                            (string)reader["tooltipEn"],
                            (string)reader["relevantCostTypes"],
                            reader["parent_nr"] == DBNull.Value ? null : (int?)reader["parent_nr"]
                        );
                        categories.Add(c);
                    }
                }

                return categories;
            }

            public static Category? GetByNr(int nr)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Categories WHERE nr = @nr", conn);
                    cmd.Parameters.AddWithValue("@nr", nr);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Category(
                            (int)reader["nr"],
                            (string)reader["text"],
                            (string)reader["textFr"],
                            (string)reader["textEn"],
                            (string)reader["tooltip"],
                            (string)reader["tooltipFr"],
                            (string)reader["tooltipEn"],
                            (string)reader["relevantCostTypes"],
                            reader["parent_nr"] == DBNull.Value ? null : (int?)reader["parent_nr"]
                        );
                    }
                }

                return null;
            }

            public override string ToString()
            {
                return $"{Nr} - {Text}";
            }
        }
    }

}
