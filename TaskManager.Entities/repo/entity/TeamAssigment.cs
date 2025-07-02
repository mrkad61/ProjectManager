using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Helper;
namespace TaskManager.Entities
{
    public class TeamAssigment
    {
        public long Id { get; set; }
        public long Team_id { get; set; }
        public long Member_id { get; set; }
        
        public User Member { get; set; }
        public UserType UserType { get; set; }
        public Teams Team { get; set; }
    }
}