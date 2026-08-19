using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
