using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Logbook
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<Mark> Marks { get; set; }
    }
}
