using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Lesson
    {
        public int Id { get; set; }
        public int? DayId { get; set; }
        public Day Day { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<Mark> Marks { get; set; }
    }
}
