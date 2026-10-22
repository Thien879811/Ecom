namespace Ecom.Application.DTOs;

public record RegisterDto(
    string Username,
    string Password);

public record LoginDto(
    string Username,
    string Password);

public record RefreshTokenDto(
    string RefreshToken);

public record AuthResponseDto(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    string Username,
    string Role);
