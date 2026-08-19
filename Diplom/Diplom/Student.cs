using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Fio { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int ParentId { get; set; }
        public Parent Parent { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Mark> Marks { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PaidLesson> PaidLessons { get; set; }
    }
}
