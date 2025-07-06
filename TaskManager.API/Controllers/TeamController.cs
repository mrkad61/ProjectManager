using Microsoft.AspNetCore.Mvc;
using TaskManager.Business;
using TaskManager.Business.DTOs;
using TaskManager.Business.DTOs.UserDTOs;
using TaskManager.Entities.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers;

/// <summary>
/// Takım yönetimi için API endpoint'leri
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    /// <summary>
    /// Tüm takımları getirir
    /// </summary>
    /// <returns>Takım listesi</returns>
    /// <response code="200">Takımlar başarıyla getirildi</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<TeamListDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllTeams()
    {
        try
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Takımlar getirilirken hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// ID'ye göre takım getirir
    /// </summary>
    /// <param name="id">Takım ID'si</param>
    /// <returns>Takım bilgileri</returns>
    /// <response code="200">Takım başarıyla getirildi</response>
    /// <response code="404">Takım bulunamadı</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeamDetailDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTeamById([Range(1, long.MaxValue, ErrorMessage = "ID pozitif bir sayı olmalıdır.")] long id)
    {
        try
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound(new { message = "Takım bulunamadı." });

            return Ok(team);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Takım getirilirken hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// Takım arama yapar
    /// </summary>
    /// <param name="keyword">Arama terimi</param>
    /// <returns>Arama sonuçları</returns>
    /// <response code="200">Arama başarıyla tamamlandı</response>
    /// <response code="400">Geçersiz arama terimi</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<TeamListDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> SearchTeams([Required][StringLength(50, MinimumLength = 2, ErrorMessage = "Arama terimi 2-50 karakter arasında olmalıdır.")][FromQuery] string keyword)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { message = "Arama terimi boş olamaz." });

            var teams = await _teamService.SearchTeamsByNameOrDescriptionAsync(keyword);
            return Ok(teams);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Takım arama sırasında hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// Takım üyelerini getirir
    /// </summary>
    /// <param name="teamId">Takım ID'si</param>
    /// <returns>Takım üyeleri</returns>
    /// <response code="200">Üyeler başarıyla getirildi</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpGet("{teamId}/members")]
    [ProducesResponseType(typeof(List<UserListDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTeamMembers([Range(1, long.MaxValue, ErrorMessage = "TeamId pozitif bir sayı olmalıdır.")] long teamId)
    {
        try
        {
            var members = await _teamService.GetTeamMembersAsync(teamId);
            return Ok(members);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Takım üyeleri getirilirken hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// Takım yöneticilerini getirir
    /// </summary>
    /// <param name="teamId">Takım ID'si</param>
    /// <returns>Takım yöneticileri</returns>
    /// <response code="200">Yöneticiler başarıyla getirildi</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpGet("{teamId}/admins")]
    [ProducesResponseType(typeof(List<UserListDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTeamAdmins([Range(1, long.MaxValue, ErrorMessage = "TeamId pozitif bir sayı olmalıdır.")] long teamId)
    {
        try
        {
            var admins = await _teamService.GetTeamAdminsAsync(teamId);
            return Ok(admins);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Takım yöneticileri getirilirken hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// Kullanıcının yönettiği takımları getirir
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Yönetilen takımlar</returns>
    /// <response code="200">Takımlar başarıyla getirildi</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpGet("user/{userId}/managed")]
    [ProducesResponseType(typeof(List<TeamListDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetTeamsManagedByUser([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
    {
        try
        {
            var teams = await _teamService.GetTeamsManagedByUserAsync(userId);
            return Ok(teams);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Kullanıcının yönettiği takımlar getirilirken hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// Yeni takım oluşturur
    /// </summary>
    /// <param name="dto">Takım oluşturma bilgileri</param>
    /// <returns>Oluşturma sonucu</returns>
    /// <response code="200">Takım başarıyla oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("create")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateTeam([Required][FromBody] CreateTeamDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teamService.CreateTeamAsync(dto);
            return Ok(new { message = "Takım başarıyla oluşturuldu." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Takıma üye davet eder
    /// </summary>
    /// <param name="dto">Davet bilgileri</param>
    /// <returns>Davet sonucu</returns>
    /// <response code="200">Davet başarıyla gönderildi</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("invite")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> InviteMember([Required][FromBody] InviteMemberDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teamService.InviteMemberAsync(dto);
            return Ok(new { message = "Davet başarıyla gönderildi." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Takımdan üye çıkarır
    /// </summary>
    /// <param name="dto">Üye çıkarma bilgileri</param>
    /// <returns>Çıkarma sonucu</returns>
    /// <response code="200">Üye başarıyla çıkarıldı</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="500">Sunucu hatası</response>
    [HttpPost("remove-member")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> RemoveMember([Required][FromBody] RemoveMemberDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teamService.RemoveMemberAsync(dto);
            return Ok(new { message = "Üye başarıyla çıkarıldı." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
