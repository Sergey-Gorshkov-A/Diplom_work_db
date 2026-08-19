using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Homework
    {
        public int Id { get; set; }
        public int LogbookId { get; set; }
        public Logbook Logbook { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public string Content { get; set; }
    }
}
