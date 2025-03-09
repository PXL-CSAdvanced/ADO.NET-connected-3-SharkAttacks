using SharkAttackClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SharkAttackClassLibrary.DataAccess
{
    public static class SharkAttackData
    {
        private static string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SharkAttacks;Integrated Security=True;Connect Timeout=30;";

        #region 1.OverallStats
        public static int GetTotalAttakcs()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SharkAttacks");
                cmd.Connection = conn;
                int totalCount = (int)cmd.ExecuteScalar();
                return totalCount;
            }
        }

        public static double GetFatalityRate()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SharkAttacks WHERE Fatal = 'Y'", conn);
                int fatalCount = (int)cmd.ExecuteScalar();
                cmd.CommandText = "SELECT COUNT(*) FROM SharkAttacks";
                int totalCount = (int)cmd.ExecuteScalar();
                double fatalRate = (fatalCount / (double)totalCount) * 100;
                return fatalRate;
            }
        }
        #endregion

        #region 2.YearlySharkAttacks
        /// <summary>
        /// GetAttacksByYear geeft een Dictionary terug met key-value pair van jaar-hoevelheid
        /// </summary>
        /// <returns>Alle jaartallen met de hoeveelheid aanvallen per jaar</returns>
        public static Dictionary<int, int> GetAttacksByYear()
        {
            Dictionary<int, int> attacksByYear = new ();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Year, COUNT(*) FROM SharkAttacks GROUP BY Year ORDER BY Year", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int year = reader.GetInt32(0);
                    int count = reader.GetInt32(1);
                    attacksByYear.Add(year, count);
                }
                reader.Close();
            }
            return attacksByYear;
        }
        #endregion

        #region DangerousActivities
        /// <summary>
        /// GetAttacksByActivity geeft een Dictionary terug met key-value pair van activiteit-hoeveelheid
        /// </summary>
        /// <returns>Alle activiteiten met de hoeveelheid gerelateerde aanvallen</returns>
        public static Dictionary<string, int> GetAttacksByActivity()
        {
            Dictionary<string, int> attacksByActivities = new ();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Activity, COUNT(*) AS AttackCount FROM SharkAttacks GROUP BY Activity ORDER BY AttackCount DESC", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string activity = reader.GetString(0);
                    int count = reader.GetInt32(1);
                    attacksByActivities.Add(activity, count);
                }
                reader.Close();
            }
            return attacksByActivities;
        }
        #endregion

        #region AttacksByCountry
        public static List<string> GetAllCountries()
        {
            List<string> countries = new List<string>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT country FROM SharkAttacks ORDER BY COUNTRY");
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string country = reader.GetString(0);
                    countries.Add(country);
                }
                return countries;
            }
        }

        public static List<SharkAttack> GetAllAttacksByCountryName(string country)
        {
            List<SharkAttack> sharkAttacks = new ();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SharkAttacks WHERE COUNTRY = @Country ORDER BY DATE DESC");
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Connection = conn;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string ageFromDb = reader[10].ToString();
                    
                    int id = reader.GetInt32(0);
                    DateTime date = reader.GetDateTime(1);
                    string type = reader.GetString(3);
                    int age;
                    if (reader.IsDBNull(10) ||  reader.GetString(10).Equals("?") || String.IsNullOrWhiteSpace(reader.GetString(10)))
                    {
                        age = 0;
                    }
                    else
                    {
                        age = Convert.ToInt32(ageFromDb);
                    }
                    char sex;
                    char fatal;

                    sharkAttacks.Add(new SharkAttack(
                        id, date, type, reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        reader.GetString(9)[0],
                        age,
                        reader.GetString(11),
                        reader.GetString(12)[0],
                        reader.GetString(14)
                    ));
                }
            }
            return sharkAttacks;
        }
        #endregion
    }
}
