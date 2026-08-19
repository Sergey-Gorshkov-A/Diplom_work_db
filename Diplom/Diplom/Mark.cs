using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Mark
    {
        public int Id { get; set; }
        public int LogbookId { get; set; }
        public Logbook Logbook { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public int? MarkValue { get; set; }
    }
}
