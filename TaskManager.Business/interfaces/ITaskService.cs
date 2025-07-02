using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Business
{
    public interface ITaskService
    {
        Task AssignTask(int taskId, int userId, int assignerId);
        Task CompleteTask(int taskId, int userId);
        Task ApproveTaskCompletion(int taskAssignmentId, int controllerId);
    }

}
