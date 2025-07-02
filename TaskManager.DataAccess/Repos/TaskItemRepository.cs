using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Entities;
using TaskManager.DataAccess.Repos;

namespace TaskManager.DataAccess.Repos
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskManagerDbContext _context;

        public TaskItemRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetTasksByUserId(long userId)
        {
            return await _context.TaskAssignments
                .Where(ta => ta.AssignedUserId == userId)
                .Include(ta => ta.TaskItem)
                .Select(ta => ta.TaskItem)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetTasksByProjectId(long projectId)
        {
            return await _context.TaskItems
                .Where(ti => ti.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> SearchTasks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _context.TaskItems.ToListAsync();
            return await _context.TaskItems
                .Where(ti => ti.Title.ToLower().Contains(keyword.ToLower()) ||
                             (ti.Description != null && ti.Description.ToLower().Contains(keyword.ToLower())))
                .ToListAsync();
        }
    }
} 