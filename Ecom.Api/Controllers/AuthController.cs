using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecom.Application.DTOs;
using Ecom.Application.Services;

namespace Ecom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] RegisterDto dto)
    {
        try
        {
            return Ok(await _service.RegisterAsync(dto));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginDto dto)
    {
        var result = await _service.LoginAsync(dto);

        if (result is null)
            return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu." });

        return Ok(result);
    }

    // Dùng refresh token để lấy cặp token mới khi access token hết hạn
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
        [FromBody] RefreshTokenDto dto)
    {
        var result = await _service.RefreshAsync(dto.RefreshToken);

        if (result is null)
            return Unauthorized(new { message = "Refresh token không hợp lệ hoặc đã hết hạn." });

        return Ok(result);
    }

    // Thu hồi refresh token (đăng xuất)
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
    {
        await _service.LogoutAsync(dto.RefreshToken);

        return NoContent();
    }

    // Thông tin người dùng lấy từ token hiện tại
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            id = User.FindFirst("sub")?.Value,
            username = User.Identity?.Name,
            role = User.FindFirst("role")?.Value
        });
    }
}
