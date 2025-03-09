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

                SqlCommand cmd = new SqlCommand("IF EXISTS (SELECT * FROM sysobjects WHERE name='SharkAttacks' AND xtype='U') " +
                    "Drop table SharkAttacks", conn);
                cmd.ExecuteNonQuery();

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
                cmd = new SqlCommand(query, conn);
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
                            
                            string[,] replaceables =
                            {
                                { "Fall", "Sep" }, { "Summer", "Jun" },
                                { "Winter", "Dec" }, { "Between", "" },
                                { "Before", "" }, { " ", "" } 
                            };

                            for(int i = 0; i<replaceables.GetLength(0); i++)
                            {
                                dateString = dateString.Replace(replaceables[i, 0], replaceables[i, 1]);
                            }

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

                            replaceables = new string[,]{
                                { "Ap-", "Apr-" },
                                { "Early", ""},
                                { "Late", ""},
                                { "Mid", ""},
                                { "Ca.", ""},
                                { "No date, Before", ""},
                                { "of" , "" }
                            };

                            for (int i = 0; i < replaceables.GetLength(0); i++)
                            {
                                dateString = dateString.Replace(replaceables[i, 0], replaceables[i, 1]);
                            }

                            if (dateString.Length > 11)
                            {
                                dateString = dateString.Substring(0, 11);
                            }

                            string ageString = line[9];
                            ageString = ageString.Replace("s", "");
                            if (ageString.Split("&").Length > 1)
                            {
                                ageString = ageString.Split("&")[0];
                            }else if (ageString.Split("or").Length > 1)
                            {
                                ageString = ageString.Split("or")[0];
                            }
                            else if (ageString.Split("to").Length > 1)
                            {
                                ageString = ageString.Split("to")[0];
                            }
                            else if (ageString.Equals("young"))
                            {
                                ageString = "15";
                            }else if (ageString.Equals("M"))
                            {
                                ageString = "";
                            }
                            ageString = ageString.Replace("\"", "");
                            ageString = ageString.Replace("\'", "");
                            ageString = ageString.Replace("?", "");
                            ageString = ageString.Replace("!", "");
                            ageString = ageString.Replace("mid-", "");
                            ageString = ageString.Replace("s", "");
                            ageString = ageString.Replace("Ca.", "");
                            ageString = ageString.Replace(" ", "");
                            ageString = ageString.Replace("Teen", "15");
                            ageString = ageString.Replace("teen", "15");
                            ageString = ageString.Replace("adult", "30");
                            ageString = ageString.Replace("Adult", "30");
                            ageString = ageString.Replace("a minor", "10");



                            DateTime time = DateTime.Parse(dateString);
                            cmd.Parameters.AddWithValue("@Date", time);
                            cmd.Parameters.AddWithValue("@Year", line[1].Length < 1 ? time.Year : int.Parse(line[1]));
                            cmd.Parameters.AddWithValue("@Type", line[2].Trim());
                            cmd.Parameters.AddWithValue("@Country", line[3].Trim());
                            cmd.Parameters.AddWithValue("@Area", line[4].Trim());
                            cmd.Parameters.AddWithValue("@Location", line[5].Trim());
                            cmd.Parameters.AddWithValue("@Activity", line[6].Trim());
                            cmd.Parameters.AddWithValue("@Name", line[7].Trim());
                            cmd.Parameters.AddWithValue("@Sex", line[8].Length > 1 ? line[8][0] : "?");
                            cmd.Parameters.AddWithValue("@Age", ageString);
                            cmd.Parameters.AddWithValue("@Injury", line[10].Trim());
                            cmd.Parameters.AddWithValue("@Fatal", line[11].Length > 1 ? line[11][0] : "?");
                            cmd.Parameters.AddWithValue("@Time", line[12].Trim());
                            cmd.Parameters.AddWithValue("@Species", line[13].Trim());
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
