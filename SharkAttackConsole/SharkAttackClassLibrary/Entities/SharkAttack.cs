using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkAttackClassLibrary.Entities
{
    public class SharkAttack
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string Activity { get; set; }
        public string Name { get; set; }
        public char Sex { get; set; }
        public int Age { get; set; }
        public string Injury { get; set; }
        public char Fatal { get; set; }
        public string Species { get; set; }

        public SharkAttack(int id, DateTime date, string type, string country, string area, string location, string activity, string name, char sex, int age, string injury, char fatal, string species)
        {
            Id = id;
            Date = date;
            Type = type;
            Country = country;
            Area = area;
            Location = location;
            Activity = activity;
            Name = name;
            Sex = sex;
            Age = age;
            Injury = injury;
            Fatal = fatal;
            Species = species;
        }

        public override string ToString()
        {
            return $"{$"{Date.ToShortDateString()}".PadRight(10)} - {Country} {Area} {Location}\n" +
                $"\tVictim: {Name} ({Age}) - {Sex} - {Activity}\n" +
                $"\tInjury: {Injury}\n";
        }
    }
}
