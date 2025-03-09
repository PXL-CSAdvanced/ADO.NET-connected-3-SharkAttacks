using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkAttackClassLibrary.Entities
{
    public class AttacksByYear
    {
        public int Year { get; set; }
        public int NumberOfAttacks { get; set; }
        public AttacksByYear(int year, int numberOfAttacks)
        {
            Year = year;
            NumberOfAttacks = numberOfAttacks;
        }
    }
}
