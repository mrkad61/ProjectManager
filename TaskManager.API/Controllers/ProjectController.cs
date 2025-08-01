using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskManager.Business;
using TaskManager.Business.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers
{
    /// <summary>
    /// Proje yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Tüm projeleri getirir (Sadece Admin ve Manager erişebilir)
        /// </summary>
        /// <returns>Proje listesi</returns>
        /// <response code="200">Projeler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(typeof(List<ProjectListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Projeler getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// ID'ye göre proje getirir (Proje üyesi veya Admin erişebilir)
        /// </summary>
        /// <param name="id">Proje ID'si</param>
        /// <returns>Proje bilgileri</returns>
        /// <response code="200">Proje başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="404">Proje bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDetailDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProjectById([Range(1, long.MaxValue, ErrorMessage = "ID pozitif bir sayı olmalıdır.")] long id)
        {
            try
            {
                // Kullanıcının bu projenin üyesi olup olmadığını kontrol et
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Admin ise tüm projeleri görebilir
                if (currentUserRole != "Admin")
                {
                    var userProjects = await _projectService.GetProjectsByUserIdAsync(currentUserId);
                    if (!userProjects.Any(p => p.Id == id))
                    {
                        return Forbid();
                    }
                }

                var project = await _projectService.GetProjectByIdAsync(id);
                if (project == null)
                    return NotFound(new { message = "Proje bulunamadı." });

                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Proje getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Proje arama yapar (Sadece Admin ve Manager erişebilir)
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
        [ProducesResponseType(typeof(List<ProjectListDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SearchProjects([Required][StringLength(50, MinimumLength = 2, ErrorMessage = "Arama terimi 2-50 karakter arasında olmalıdır.")][FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Arama terimi boş olamaz." });

                var projects = await _projectService.SearchProjectsAsync(keyword);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Proje arama sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının projelerini getirir (Kendi projeleri veya Admin erişebilir)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcının projeleri</returns>
        /// <response code="200">Projeler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<ProjectListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProjectsByUserId([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
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

                var projects = await _projectService.GetProjectsByUserIdAsync(userId);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının projeleri getirilirken hata oluştu.", error = ex.Message });
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
        [HttpGet("{projectId}/tasks")]
        [ProducesResponseType(typeof(List<TaskItemListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProjectTasks([Range(1, long.MaxValue, ErrorMessage = "ProjectId pozitif bir sayı olmalıdır.")] long projectId)
        {
            try
            {
                // Kullanıcının bu projenin üyesi olup olmadığını kontrol et
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin")
                {
                    var userProjects = await _projectService.GetProjectsByUserIdAsync(currentUserId);
                    if (!userProjects.Any(p => p.Id == projectId))
                    {
                        return Forbid();
                    }
                }

                var tasks = await _projectService.GetProjectTasksAsync(projectId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Proje görevleri getirilirken hata oluştu.", error = ex.Message });
            }
        }
    }
}