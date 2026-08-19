using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Admin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Fio { get; set; }
    }
}
