using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingHallManagement.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime MemberSince { get; set; }

        public Member(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
            MemberSince = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Email: {Email}, MemberSince: {MemberSince.ToShortDateString()}";
        }

    }
}
