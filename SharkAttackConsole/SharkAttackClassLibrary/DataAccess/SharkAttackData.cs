// import Entities.SharkData in 4. AttacksByCountry
// using SharkAttackClassLibrary.Entities;
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
        private static string _connectionString = "jouw sql connectiestring";

        #region 1.OverallStats
        public static int GetTotalAttakcs()
        {
            throw new NotImplementedException();
        }

        public static double GetFatalityRate()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 2.YearlySharkAttacks
        /// <summary>
        /// GetAttacksByYear geeft een Dictionary terug met key-value pair van jaar-hoevelheid
        /// </summary>
        /// <returns>Alle jaartallen met de hoeveelheid aanvallen per jaar</returns>
        public static Dictionary<int, int> GetAttacksByYear()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 3.DangerousActivities
        /// <summary>
        /// GetAttacksByActivity geeft een Dictionary terug met key-value pair van activiteit-hoeveelheid
        /// </summary>
        /// <returns>Alle activiteiten met de hoeveelheid gerelateerde aanvallen</returns>
        public static Dictionary<string, int> GetAttacksByActivity()
        {
            throw new NotImplementedException();

        }
        #endregion

        #region 4.AttacksByCountry
        public static List<string> GetAllCountries()
        {
            throw new NotImplementedException();
        }

        public static List<SharkAttack> GetAllAttacksByCountryName(string country)
        {
            throw new NotImplementedException();

        }
        #endregion
    }
}
