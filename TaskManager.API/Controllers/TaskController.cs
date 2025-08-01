using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskManager.Business;
using TaskManager.Business.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers
{
    /// <summary>
    /// Görev yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskItemService _taskItemService;

        public TaskController(ITaskService taskService, ITaskItemService taskItemService)
        {
            _taskService = taskService;
            _taskItemService = taskItemService;
        }

        /// <summary>
        /// Tüm görevleri getirir (Sadece Admin ve Manager erişebilir)
        /// </summary>
        /// <returns>Görev listesi</returns>
        /// <response code="200">Görevler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(typeof(List<TaskItemListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskItemService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Görevler getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// ID'ye göre görev getirir (Görev atanmış kullanıcı veya Admin erişebilir)
        /// </summary>
        /// <param name="id">Görev ID'si</param>
        /// <returns>Görev bilgileri</returns>
        /// <response code="200">Görev başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="404">Görev bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskItemDetailDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTaskById([Range(1, long.MaxValue, ErrorMessage = "ID pozitif bir sayı olmalıdır.")] long id)
        {
            try
            {
                // Kullanıcının bu görevin atanmış kişisi olup olmadığını kontrol et
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Admin ise tüm görevleri görebilir
                if (currentUserRole != "Admin")
                {
                    var userTasks = await _taskItemService.GetTasksByUserIdAsync(currentUserId);
                    if (!userTasks.Any(t => t.Id == id))
                    {
                        return Forbid();
                    }
                }

                var task = await _taskItemService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound(new { message = "Görev bulunamadı." });

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Görev getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Görev  arama yapar (Sadece Admin ve Manager erişebilir)
        /// </summary>
        /// <param name="keyword">Arama terimi</param>
        /// <returns>Arama sonuçları</returns>
        /// <response code="200">Arama başarıyla tamamlandı</response>
        /// <response code="400">Geçersiz arama terimi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("search")]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(typeof(List<TaskItemListDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SearchTasks([Required][StringLength(50, MinimumLength = 2, ErrorMessage = "Arama terimi 2-50 karakter arasında olmalıdır.")][FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Arama terimi boş olamaz." });

                var tasks = await _taskItemService.SearchTasksAsync(keyword);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Görev arama sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının görevlerini getirir (Kendi görevleri veya Admin erişebilir)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcının görevleri</returns>
        /// <response code="200">Görevler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<TaskItemListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTasksByUserId([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
        {
            try
            {
                // Kullanıcının kendi verilerini veya Admin'in herkesin verilerini görebilmesini sağla
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != userId)
                {
                    return Forbid();
                }

                var tasks = await _taskItemService.GetTasksByUserIdAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının görevleri getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Proje görevlerini getirir (Proje üyesi veya Admin erişebilir)
        /// </summary>
        /// <param name="projectId">Proje ID'si</param>
        /// <returns>Proje görevleri</returns>
        /// <response code="200">Görevler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(List<TaskItemListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTasksByProjectId([Range(1, long.MaxValue, ErrorMessage = "ProjectId pozitif bir sayı olmalıdır.")] long projectId)
        {
            try
            {
                // Bu endpoint için proje üyeliği kontrolü yapılmalı
                // Şimdilik sadece Admin ve Manager erişebilir
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserRole != "Manager")
                {
                    return Forbid();
                }

                var tasks = await _taskItemService.GetTasksByProjectIdAsync(projectId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Proje görevleri getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Görev atar (Sadece Admin ve Manager atayabilir)
        /// </summary>
        /// <param name="request">Görev atama bilgileri</param>
        /// <returns>Atama sonucu</returns>
        /// <response code="200">Görev başarıyla atandı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("assign")]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AssignTask([Required][FromBody] TaskAssignmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Atayan kişinin yetkisini kontrol et
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.AssignerId)
                {
                    return Forbid();
                }

                await _taskService.AssignTask(request.TaskId, request.UserId, request.AssignerId);
                return Ok(new { message = "Görev başarıyla atandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Görevi tamamlar (Atanmış kullanıcı tamamlayabilir)
        /// </summary>
        /// <param name="request">Görev tamamlama bilgileri</param>
        /// <returns>Tamamlama sonucu</returns>
        /// <response code="200">Görev başarıyla tamamlandı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("complete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CompleteTask([Required][FromBody] TaskCompletionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Sadece atanmış kullanıcı veya Admin tamamlayabilir
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.UserId)
                {
                    return Forbid();
                }

                await _taskService.CompleteTask(request.TaskId, request.UserId);
                return Ok(new { message = "Görev başarıyla tamamlandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Görev tamamlanmasını onaylar (Sadece Controller ve Admin onaylayabilir)
        /// </summary>
        /// <param name="request">Onay bilgileri</param>
        /// <returns>Onay sonucu</returns>
        /// <response code="200">Görev tamamlanması onaylandı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("approve")]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ApproveTaskCompletion([Required][FromBody] TaskApprovalRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Onaylayan kişinin yetkisini kontrol et
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.ControllerId)
                {
                    return Forbid();
                }

                await _taskService.ApproveTaskCompletion(request.TaskAssignmentId, request.ControllerId);
                return Ok(new { message = "Görev tamamlanması onaylandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Görev atama isteği modeli
    /// </summary>
    public class TaskAssignmentRequest
    {
        /// <summary>
        /// Görev ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TaskId pozitif bir sayı olmalıdır.")]
        public int TaskId { get; set; }

        /// <summary>
        /// Atanacak kullanıcı ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")]
        public int UserId { get; set; }

        /// <summary>
        /// Atayan kullanıcı ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "AssignerId pozitif bir sayı olmalıdır.")]
        public int AssignerId { get; set; }
    }

    /// <summary>
    /// Görev tamamlama isteği modeli
    /// </summary>
    public class TaskCompletionRequest
    {
        /// <summary>
        /// Görev ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TaskId pozitif bir sayı olmalıdır.")]
        public int TaskId { get; set; }

        /// <summary>
        /// Tamamlayan kullanıcı ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// Görev onaylama isteği modeli
    /// </summary>
    public class TaskApprovalRequest
    {
        /// <summary>
        /// Görev atama ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TaskAssignmentId pozitif bir sayı olmalıdır.")]
        public int TaskAssignmentId { get; set; }

        /// <summary>
        /// Onaylayan controller ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ControllerId pozitif bir sayı olmalıdır.")]
        public int ControllerId { get; set; }
    }
} 