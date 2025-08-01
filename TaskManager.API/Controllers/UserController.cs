using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskManager.Business;
using TaskManager.Business.DTOs;
using TaskManager.Business.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers
{
    /// <summary>
    /// Kullanıcı yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Tüm kullanıcıları getirir (Sadece Admin erişebilir)
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        /// <response code="200">Kullanıcılar başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(List<UserListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcılar getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// ID'ye göre kullanıcı getirir (Kendi bilgileri veya Admin erişebilir)
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı bilgileri</returns>
        /// <response code="200">Kullanıcı başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="404">Kullanıcı bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById([Range(1, long.MaxValue, ErrorMessage = "ID pozitif bir sayı olmalıdır.")] long id)
        {
            try
            {
                // Kullanıcının kendi verilerini veya Admin'in herkesin verilerini görebilmesini sağla
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != id)
                {
                    return Forbid();
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "Kullanıcı bulunamadı." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcı getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının takımlarını getirir (Kendi takımları veya Admin erişebilir)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcının takımları</returns>
        /// <response code="200">Takımlar başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{userId}/teams")]
        [ProducesResponseType(typeof(List<TeamListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserTeams([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
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

                var teams = await _userService.GetTeamsByUserIdAsync(userId);
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının takımları getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının takımlarını detaylarıyla getirir (Kendi takımları veya Admin erişebilir)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcının takımları detayları</returns>
        /// <response code="200">Takım detayları başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{userId}/teams-with-details")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserTeamsWithDetails([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
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

                var userWithTeams = await _userService.GetUserWithTeamsAsync(userId);
                if (userWithTeams == null)
                    return NotFound(new { message = "Kullanıcı bulunamadı." });

                return Ok(userWithTeams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının takım detayları getirilirken hata oluştu.", error = ex.Message });
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
        [HttpGet("{userId}/assignments")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserAssignments([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
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

                var userWithAssignments = await _userService.GetUserWithAssignmentsAsync(userId);
                if (userWithAssignments == null)
                    return NotFound(new { message = "Kullanıcı bulunamadı." });

                return Ok(userWithAssignments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının görevleri getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Rol bazında kullanıcıları getirir (Sadece Admin erişebilir)
        /// </summary>
        /// <param name="role">Kullanıcı rolü</param>
        /// <returns>Rol bazında kullanıcılar</returns>
        /// <response code="200">Kullanıcılar başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("by-role/{role}")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(List<UserListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUsersByRole([Required] UserType role)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Rol bazında kullanıcılar getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcı arama yapar (Sadece Admin ve Manager erişebilir)
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
        [ProducesResponseType(typeof(List<UserListDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SearchUsers([Required][StringLength(50, MinimumLength = 2, ErrorMessage = "Arama terimi 2-50 karakter arasında olmalıdır.")][FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Arama terimi boş olamaz." });

                var users = await _userService.SearchUsersAsync(keyword);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcı arama sırasında hata oluştu.", error = ex.Message });
            }
        }
    }
} 