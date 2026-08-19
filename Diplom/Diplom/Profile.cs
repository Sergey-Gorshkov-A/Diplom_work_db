using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Profile
    {
        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? PersonalInfo { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
