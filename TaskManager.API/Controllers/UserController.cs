using Microsoft.AspNetCore.Mvc;
using TaskManager.Business;
using TaskManager.Business.DTOs.UserDTOs;
using TaskManager.Entities.Helper;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers
{
    /// <summary>
    /// Kullanıcı yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Tüm kullanıcıları getirir
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        /// <response code="200">Kullanıcılar başarıyla getirildi</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserListDto>), 200)]
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
        /// ID'ye göre kullanıcı getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı bilgileri</returns>
        /// <response code="200">Kullanıcı başarıyla getirildi</response>
        /// <response code="404">Kullanıcı bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById([Range(1, long.MaxValue, ErrorMessage = "ID pozitif bir sayı olmalıdır.")] long id)
        {
            try
            {
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
        /// Kullanıcı arama yapar
        /// </summary>
        /// <param name="keyword">Arama terimi</param>
        /// <returns>Arama sonuçları</returns>
        /// <response code="200">Arama başarıyla tamamlandı</response>
        /// <response code="400">Geçersiz arama terimi</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<UserListDto>), 200)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Belirli bir role sahip kullanıcıları getirir
        /// </summary>
        /// <param name="role">Kullanıcı rolü</param>
        /// <returns>Rol bazlı kullanıcı listesi</returns>
        /// <response code="200">Kullanıcılar başarıyla getirildi</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("role/{role}")]
        [ProducesResponseType(typeof(List<UserListDto>), 200)]
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
                return StatusCode(500, new { message = "Rol bazlı kullanıcılar getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının üye olduğu takımları getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Takım listesi</returns>
        /// <response code="200">Takımlar başarıyla getirildi</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{userId}/teams")]
        [ProducesResponseType(typeof(List<Teams>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserTeams([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
        {
            try
            {
                var teams = await _userService.GetTeamsByUserIdAsync(userId);
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının takımları getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının takım detaylarını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı ve takım detayları</returns>
        /// <response code="200">Detaylar başarıyla getirildi</response>
        /// <response code="404">Kullanıcı bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{userId}/teams-with-details")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserTeamsWithDetails([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
        {
            try
            {
                var user = await _userService.GetUserWithTeamsAsync(userId);
                if (user == null)
                    return NotFound(new { message = "Kullanıcı bulunamadı." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının takım detayları getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının görev atamalarını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı ve görev atamaları</returns>
        /// <response code="200">Atamalar başarıyla getirildi</response>
        /// <response code="404">Kullanıcı bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{userId}/assignments")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserAssignments([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
        {
            try
            {
                var user = await _userService.GetUserWithAssignmentsAsync(userId);
                if (user == null)
                    return NotFound(new { message = "Kullanıcı bulunamadı." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının görev atamaları getirilirken hata oluştu.", error = ex.Message });
            }
        }
    }
} 