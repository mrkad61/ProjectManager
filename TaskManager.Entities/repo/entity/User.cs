using TaskManager.Entities.Helper;
using TaskManager.Entities.Project;

namespace TaskManager.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Register_date { get; set; } = DateTime.Now;
        public string PasswordHash { get; set; }
        public UserType Role { get; set; }
        public ICollection<TeamAssigment> Teams { get; set; } = new List<TeamAssignment>();
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    }
}