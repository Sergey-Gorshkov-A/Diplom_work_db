using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Logbook> Logbooks { get; set; }
    }
}
