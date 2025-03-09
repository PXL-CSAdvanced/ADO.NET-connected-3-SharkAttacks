using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkAttackClassLibrary.Entities
{
    public class AttacksByActivity
    {
        public string Activity { get; set; }
        public int NumberOfAttacks { get; set; }

        public AttacksByActivity(string activity, int numberOfAttacks)
        {
            Activity = activity;
            NumberOfAttacks = numberOfAttacks;
        }
    }
}
