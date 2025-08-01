using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskManager.Business;
using TaskManager.Business.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers
{
    /// <summary>
    /// Davet yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        /// <summary>
        /// Tüm davetleri getirir (Sadece Admin erişebilir)
        /// </summary>
        /// <returns>Davet listesi</returns>
        /// <response code="200">Davetler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(List<InvitationListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInvitations()
        {
            try
            {
                var invitations = await _invitationService.GetAllInvitationsAsync();
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Davetler getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// ID'ye göre davet getirir (Davet sahibi veya Admin erişebilir)
        /// </summary>
        /// <param name="id">Davet ID'si</param>
        /// <returns>Davet bilgileri</returns>
        /// <response code="200">Davet başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="404">Davet bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvitationDetailDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvitationById([Range(1, long.MaxValue, ErrorMessage = "ID pozitif bir sayı olmalıdır.")] long id)
        {
            try
            {
                // Bu endpoint için davet sahibi kontrolü yapılmalı
                // Şimdilik sadece Admin erişebilir
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin")
                {
                    return Forbid();
                }

                var invitation = await _invitationService.GetInvitationByIdAsync(id);
                if (invitation == null)
                    return NotFound(new { message = "Davet bulunamadı." });

                return Ok(invitation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Davet getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının davetlerini getirir (Kendi davetleri veya Admin erişebilir)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Kullanıcının davetleri</returns>
        /// <response code="200">Davetler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<InvitationListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvitationsByUserId([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
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

                var invitations = await _invitationService.GetInvitationsByUserIdAsync(userId);
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcının davetleri getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Bekleyen davetleri getirir (Kendi davetleri veya Admin erişebilir)
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si</param>
        /// <returns>Bekleyen davetler</returns>
        /// <response code="200">Davetler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("user/{userId}/pending")]
        [ProducesResponseType(typeof(List<InvitationListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPendingInvitations([Range(1, long.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")] long userId)
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

                var invitations = await _invitationService.GetPendingInvitationsAsync(userId);
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bekleyen davetler getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Takım davetlerini getirir (Takım yöneticisi veya Admin erişebilir)
        /// </summary>
        /// <param name="teamId">Takım ID'si</param>
        /// <returns>Takım davetleri</returns>
        /// <response code="200">Davetler başarıyla getirildi</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpGet("team/{teamId}")]
        [ProducesResponseType(typeof(List<InvitationListDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvitationsByTeamId([Range(1, long.MaxValue, ErrorMessage = "TeamId pozitif bir sayı olmalıdır.")] long teamId)
        {
            try
            {
                // Bu endpoint için takım yöneticisi kontrolü yapılmalı
                // Şimdilik sadece Admin ve Manager erişebilir
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserRole != "Manager")
                {
                    return Forbid();
                }

                var invitations = await _invitationService.GetInvitationsByTeamIdAsync(teamId);
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Takım davetleri getirilirken hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Davet oluşturur (Takım yöneticisi veya Admin oluşturabilir)
        /// </summary>
        /// <param name="request">Davet oluşturma bilgileri</param>
        /// <returns>Oluşturma sonucu</returns>
        /// <response code="200">Davet başarıyla oluşturuldu</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("create")]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInvitation([Required][FromBody] CreateInvitationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Davet eden kişinin yetkisini kontrol et
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.InviterUserId)
                {
                    return Forbid();
                }

                await _invitationService.CreateInvitationAsync(request.TeamId, request.InvitedUserId, request.InviterUserId);
                return Ok(new { message = "Davet başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Daveti kabul eder (Davet edilen kullanıcı kabul edebilir)
        /// </summary>
        /// <param name="request">Kabul bilgileri</param>
        /// <returns>Kabul sonucu</returns>
        /// <response code="200">Davet başarıyla kabul edildi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("accept")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AcceptInvitation([Required][FromBody] InvitationActionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Sadece davet edilen kullanıcı veya Admin kabul edebilir
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.UserId)
                {
                    return Forbid();
                }

                await _invitationService.AcceptInvitationAsync(request.InvitationId, request.UserId);
                return Ok(new { message = "Davet başarıyla kabul edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Daveti reddeder (Davet edilen kullanıcı reddedebilir)
        /// </summary>
        /// <param name="request">Red bilgileri</param>
        /// <returns>Red sonucu</returns>
        /// <response code="200">Davet başarıyla reddedildi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("reject")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RejectInvitation([Required][FromBody] InvitationActionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Sadece davet edilen kullanıcı veya Admin reddedebilir
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.UserId)
                {
                    return Forbid();
                }

                await _invitationService.RejectInvitationAsync(request.InvitationId, request.UserId);
                return Ok(new { message = "Davet başarıyla reddedildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Daveti iptal eder (Davet eden kullanıcı veya Admin iptal edebilir)
        /// </summary>
        /// <param name="request">İptal bilgileri</param>
        /// <returns>İptal sonucu</returns>
        /// <response code="200">Davet başarıyla iptal edildi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Yetkisiz erişim</response>
        /// <response code="403">Yetersiz yetki</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("cancel")]
        [Authorize(Policy = "RequireManagerRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CancelInvitation([Required][FromBody] InvitationActionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Sadece davet eden kullanıcı veya Admin iptal edebilir
                var currentUserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole != "Admin" && currentUserId != request.UserId)
                {
                    return Forbid();
                }

                await _invitationService.CancelInvitationAsync(request.InvitationId, request.UserId);
                return Ok(new { message = "Davet başarıyla iptal edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Davet oluşturma isteği modeli
    /// </summary>
    public class CreateInvitationRequest
    {
        /// <summary>
        /// Takım ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId pozitif bir sayı olmalıdır.")]
        public int TeamId { get; set; }

        /// <summary>
        /// Davet edilen kullanıcı ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "InvitedUserId pozitif bir sayı olmalıdır.")]
        public int InvitedUserId { get; set; }

        /// <summary>
        /// Davet eden kullanıcı ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "InviterUserId pozitif bir sayı olmalıdır.")]
        public int InviterUserId { get; set; }
    }

    /// <summary>
    /// Davet aksiyonu isteği modeli
    /// </summary>
    public class InvitationActionRequest
    {
        /// <summary>
        /// Davet ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "InvitationId pozitif bir sayı olmalıdır.")]
        public int InvitationId { get; set; }

        /// <summary>
        /// Kullanıcı ID'si
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId pozitif bir sayı olmalıdır.")]
        public int UserId { get; set; }
    }
} 