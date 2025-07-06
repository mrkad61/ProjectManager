using System;
using System.Threading.Tasks;
using TaskManager.Business;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Project;
using AutoMapper;
using TaskManager.Business.DTOs;

namespace TaskManager.Business
{
    public class TaskService : ITaskService
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TaskService(
            ITaskAssignmentRepository taskAssignmentRepository,
            ITaskItemRepository taskItemRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
            _taskItemRepository = taskItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AssignTask(int taskId, int userId, int assignerId)
        {
            // Görev var mı kontrol et
            var task = await _taskItemRepository.GetAsync(taskId);
            if (task == null)
                throw new Exception("Görev bulunamadı.");

            // Kullanıcı var mı kontrol et
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            // Zaten atanmış mı kontrol et
            var existingAssignment = await _taskAssignmentRepository.GetAssignment(userId, taskId);
            if (existingAssignment != null)
                throw new Exception("Bu görev zaten kullanıcıya atanmış.");

            // Yeni atama oluştur
            var assignment = new TaskAssignment
            {
                TaskItemId = taskId,
                AssignedUserId = userId,
                CreatedDate = DateTime.Now,
                IsCompleted = false,
                IsApprovedByController = false
            };

            await _taskAssignmentRepository.AddAsync(assignment);
        }

        public async Task CompleteTask(int taskId, int userId)
        {
            var assignment = await _taskAssignmentRepository.GetAssignment(userId, taskId);
            if (assignment == null)
                throw new Exception("Görev ataması bulunamadı.");

            if (assignment.IsCompleted)
                throw new Exception("Görev zaten tamamlanmış.");

            assignment.IsCompleted = true;
            await _taskAssignmentRepository.UpdateAsync(assignment);
        }

        public async Task ApproveTaskCompletion(int taskAssignmentId, int controllerId)
        {
            var assignment = await _taskAssignmentRepository.GetAsync(taskAssignmentId);
            if (assignment == null)
                throw new Exception("Görev ataması bulunamadı.");

            if (!assignment.IsCompleted)
                throw new Exception("Görev henüz tamamlanmamış.");

            if (assignment.IsApprovedByController)
                throw new Exception("Görev zaten onaylanmış.");

            // Controller yetkisi kontrolü (opsiyonel)
            var controller = await _userRepository.GetUserById(controllerId);
            if (controller == null || controller.Role != UserType.Controller)
                throw new Exception("Onay yetkisi bulunmuyor.");

            assignment.IsApprovedByController = true;
            await _taskAssignmentRepository.UpdateAsync(assignment);
        }
    }
}
