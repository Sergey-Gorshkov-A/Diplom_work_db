using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Parent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Fio { get; set; }
        public int? BalanceId { get; set; }
        public Balance? Balance { get; set; }
        public ICollection<Student> Children { get; set; }
    }
}
