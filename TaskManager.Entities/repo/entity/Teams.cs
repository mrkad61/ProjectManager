using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Helper;

namespace TaskManager.Entities
{
    public class Teams
    {
        public long id { get; set; }
        public string Name { get; set; }
        public int user_number { get; set; }
        public long User_id { get; set; }
        public UserType UserType { get; set; }
        
        public ICollection<TeamAssigment> Members { get; set; }
    }
}
