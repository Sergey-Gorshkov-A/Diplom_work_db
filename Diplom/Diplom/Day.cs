using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Day
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string DayNumber { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
