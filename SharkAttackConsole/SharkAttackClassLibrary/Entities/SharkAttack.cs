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
        public int Year { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string Activity { get; set; }
        public string Name { get; set; }
        public char Sex { get; set; }
        public int Age { get; set; }
        public char Fatal { get; set; }
        public string Time { get; set; }
        public string Species { get; set; }
    }
}
