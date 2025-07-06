using Microsoft.AspNetCore.Mvc;
using TaskManager.Business;
using TaskManager.Business.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Controllers
{
    /// <summary>
    /// Kimlik doğrulama ve yetkilendirme için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        /// <param name="request">Giriş bilgileri</param>
        /// <returns>Giriş sonucu ve token</returns>
        /// <response code="200">Giriş başarılı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Kimlik doğrulama başarısız</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([Required][FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.LoginAsync(request.Email, request.Password);
                if (result == null)
                    return Unauthorized(new { message = "Geçersiz email veya şifre." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Giriş sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcı kaydı yapar
        /// </summary>
        /// <param name="request">Kayıt bilgileri</param>
        /// <returns>Kayıt sonucu</returns>
        /// <response code="200">Kayıt başarılı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="409">Kullanıcı zaten mevcut</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([Required][FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName);
                if (result == null)
                    return Conflict(new { message = "Bu email adresi zaten kullanılıyor." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kayıt sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Token yeniler
        /// </summary>
        /// <param name="request">Token yenileme bilgileri</param>
        /// <returns>Yeni token</returns>
        /// <response code="200">Token başarıyla yenilendi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Geçersiz token</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken([Required][FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RefreshTokenAsync(request.RefreshToken);
                if (result == null)
                    return Unauthorized(new { message = "Geçersiz refresh token." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Token yenileme sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcı çıkışı yapar
        /// </summary>
        /// <param name="request">Çıkış bilgileri</param>
        /// <returns>Çıkış sonucu</returns>
        /// <response code="200">Çıkış başarılı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Logout([Required][FromBody] LogoutRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _authService.LogoutAsync(request.RefreshToken);
                return Ok(new { message = "Başarıyla çıkış yapıldı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Çıkış sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Şifre sıfırlama isteği gönderir
        /// </summary>
        /// <param name="request">Şifre sıfırlama bilgileri</param>
        /// <returns>Sıfırlama sonucu</returns>
        /// <response code="200">Sıfırlama isteği gönderildi</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="404">Kullanıcı bulunamadı</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ForgotPassword([Required][FromBody] ForgotPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.ForgotPasswordAsync(request.Email);
                if (!result)
                    return NotFound(new { message = "Bu email adresi ile kayıtlı kullanıcı bulunamadı." });

                return Ok(new { message = "Şifre sıfırlama bağlantısı email adresinize gönderildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Şifre sıfırlama sırasında hata oluştu.", error = ex.Message });
            }
        }

        /// <summary>
        /// Şifre sıfırlar
        /// </summary>
        /// <param name="request">Şifre sıfırlama bilgileri</param>
        /// <returns>Sıfırlama sonucu</returns>
        /// <response code="200">Şifre başarıyla sıfırlandı</response>
        /// <response code="400">Geçersiz veri</response>
        /// <response code="401">Geçersiz token</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ResetPassword([Required][FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
                if (!result)
                    return Unauthorized(new { message = "Geçersiz veya süresi dolmuş token." });

                return Ok(new { message = "Şifreniz başarıyla sıfırlandı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Şifre sıfırlama sırasında hata oluştu.", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Giriş isteği modeli
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Email adresi
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Kayıt isteği modeli
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Email adresi
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Ad
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter arasında olmalıdır.")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Soyad
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 karakter arasında olmalıdır.")]
        public string LastName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Token yenileme isteği modeli
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Refresh token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Çıkış isteği modeli
    /// </summary>
    public class LogoutRequest
    {
        /// <summary>
        /// Refresh token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Şifre sıfırlama isteği modeli
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// Email adresi
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Şifre sıfırlama modeli
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Sıfırlama token'ı
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Yeni şifre
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}