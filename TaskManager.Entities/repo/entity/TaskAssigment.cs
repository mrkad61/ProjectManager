using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities;

namespace TaskManager.Entities.Project
{
    public class TaskAssignment
    {
        public long Id { get; set; }

        public long TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        public long AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsApprovedByController { get; set; }
    }

}