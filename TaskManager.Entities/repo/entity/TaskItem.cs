using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Entities.Project
{
    public class TaskItem
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<TaskAssignment> Assignments { get; set; }
    }
}

