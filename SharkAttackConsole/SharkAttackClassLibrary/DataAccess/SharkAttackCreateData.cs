using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SharkAttackClassLibrary.DataAccess
{
    public static class SharkAttackCreateData
    {
        private static string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SharkAttacks;Integrated Security=True;Connect Timeout=30;";

        static SharkAttackCreateData()
        {
            CreateTableIfNotExists();
        }

        private static void CreateTableIfNotExists()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SharkAttacks' AND xtype='U')
                CREATE TABLE SharkAttacks (
                    Id INT IDENTITY PRIMARY KEY,
                    Date DATE,
                    Year INT,
                    Type VARCHAR(50),
                    Country VARCHAR(100),
                    Area VARCHAR(100),
                    Location VARCHAR(150),
                    Activity VARCHAR(150),
                    Name VARCHAR(100),
                    Sex CHAR(1),
                    Age VARCHAR(50),
                    Injury VARCHAR(255),
                    Fatal CHAR(1),
                    Time VARCHAR(50),
                    Species VARCHAR(500)
                )";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertFromCsv(string filePath)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    int numberOfErrors = 0;
                    reader.ReadLine(); // Skip header
                    while (!reader.EndOfStream)
                    {
                        string text = "";
                        string[] line;
                        try
                        {
                            text = reader.ReadLine();
                            line = text.Split(';');
                            if (line.Length < 150)
                            {
                                line = (text + reader.ReadLine()).Split(";");
                            }
                            string query = @"INSERT INTO SharkAttacks (Date, Year, Type, Country, Area, Location, Activity, Name, Sex, Age, Injury, Fatal, Time, Species)
                                     VALUES (@Date, @Year, @Type, @Country, @Area, @Location, @Activity, @Name, @Sex, @Age, @Injury, @Fatal, @Time, @Species)";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            String dateString = line[0].Replace("Reported ", "")
                                .Replace("--", "-").Replace("´", "").Replace("`", "");
                            if (!Char.IsDigit(dateString.Trim()[0]))
                            {
                                dateString = "01-" + dateString.Trim();
                            }

                            if (dateString.Trim().EndsWith("s"))
                            {
                                dateString = dateString.Trim().Substring(0, dateString.Trim().Length - 1);
                            }
                            dateString = dateString.Replace("Fall", "Sep");
                            dateString = dateString.Replace("Summer", "Jun");
                            dateString = dateString.Replace("Winter", "Dec");
                            dateString = dateString.Replace("Between", "");
                            dateString = dateString.Replace("Before", "");
                            dateString = dateString.Replace(" ", "");

                            if (dateString[dateString.Length - 1] == '.')
                            {
                                dateString = dateString.Substring(0, dateString.Length - 1);
                            }
                            if (dateString[dateString.Length - 1] == '-')
                            {
                                dateString = dateString.Substring(0, dateString.Length - 1);
                            }

                            if (dateString.Split("-").Length == 2 && dateString.Split("-")[0].Length == 4)
                            {
                                dateString = dateString.Split("-")[0];
                            }

                            if (dateString.Trim().Length == 4)
                            {
                                dateString = "01-Jan-" + dateString.Trim();
                            }
                            dateString = dateString.Replace("Ap-", "Apr-");
                            dateString = dateString.Replace("Early", "");
                            dateString = dateString.Replace("Late", "");
                            dateString = dateString.Replace("Mid", "");
                            dateString = dateString.Replace("Ca.", "");
                            dateString = dateString.Replace("No date, Before", "");

                            dateString = dateString.Replace("of", "");

                            if (dateString.Length > 11)
                            {
                                dateString = dateString.Substring(0, 11);
                            }

                            DateTime time = DateTime.Parse(dateString);
                            cmd.Parameters.AddWithValue("@Date", time);
                            cmd.Parameters.AddWithValue("@Year", line[1].Length < 1 ? time.Year : int.Parse(line[1]));
                            cmd.Parameters.AddWithValue("@Type", line[2]);
                            cmd.Parameters.AddWithValue("@Country", line[3]);
                            cmd.Parameters.AddWithValue("@Area", line[4]);
                            cmd.Parameters.AddWithValue("@Location", line[5]);
                            cmd.Parameters.AddWithValue("@Activity", line[6]);
                            cmd.Parameters.AddWithValue("@Name", line[7]);
                            cmd.Parameters.AddWithValue("@Sex", line[8].Length > 1 ? line[8][0] : "?");
                            cmd.Parameters.AddWithValue("@Age", line[9]);
                            cmd.Parameters.AddWithValue("@Injury", line[10]);
                            cmd.Parameters.AddWithValue("@Fatal", line[11].Length > 1 ? line[11][0] : "?");
                            cmd.Parameters.AddWithValue("@Time", line[12]);
                            cmd.Parameters.AddWithValue("@Species", line[13]);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex) 
                        {
                            numberOfErrors++;
                            Console.WriteLine($"{numberOfErrors}: {text}\n");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
