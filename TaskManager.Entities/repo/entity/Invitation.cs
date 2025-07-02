using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Helper;

namespace TaskManager.Entities
{
    public class Invitation
    {
        public long Id { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [ForeignKey("TeamsId")]
        public Teams Teams { get; set; }
        public InvitationStatus Status { get; set; }
    }
}
