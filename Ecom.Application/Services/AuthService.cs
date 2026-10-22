using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Constants;
using Ecom.Domain.Entities;

namespace Ecom.Application.Services;

public class AuthService
{
    private readonly IUserRepository _repository;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository repository,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _repository = repository;
        _refreshTokens = refreshTokens;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        Validate(dto.Username, dto.Password);

        var username = dto.Username.Trim();

        if (await _repository.GetByUsernameAsync(username) is not null)
            throw new ArgumentException("Tên đăng nhập đã tồn tại.");

        var user = new User
        {
            Username = username,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Role = Roles.Customer
        };

        await _repository.AddAsync(user);

        return await IssueTokensAsync(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _repository.GetByUsernameAsync(dto.Username.Trim());

        if (user is null || !_passwordHasher.Verify(user.PasswordHash, dto.Password))
            return null;

        return await IssueTokensAsync(user);
    }

    // Đổi refresh token cũ lấy cặp token mới (rotation)
    public async Task<AuthResponseDto?> RefreshAsync(string refreshToken)
    {
        var stored = await _refreshTokens.GetByHashAsync(
            _tokenService.HashToken(refreshToken));

        if (stored is null)
            return null;

        // Token đã thu hồi mà vẫn bị dùng lại => có thể đã bị lộ,
        // thu hồi toàn bộ phiên đăng nhập của user này
        if (stored.RevokedAt is not null)
        {
            await RevokeAllAsync(stored.UserId);
            return null;
        }

        if (!stored.IsActive)
            return null;

        stored.RevokedAt = DateTime.UtcNow;

        return await IssueTokensAsync(stored.User);
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var stored = await _refreshTokens.GetByHashAsync(
            _tokenService.HashToken(refreshToken));

        if (stored is null || !stored.IsActive)
            return false;

        stored.RevokedAt = DateTime.UtcNow;
        await _refreshTokens.SaveChangesAsync();

        return true;
    }

    // Tạo tài khoản Admin lúc khởi động nếu chưa có
    public async Task EnsureAdminAsync(string username, string password)
    {
        Validate(username, password);

        if (await _repository.GetByUsernameAsync(username.Trim()) is not null)
            return;

        await _repository.AddAsync(new User
        {
            Username = username.Trim(),
            PasswordHash = _passwordHasher.Hash(password),
            Role = Roles.Admin
        });
        await _repository.SaveChangesAsync();
    }

    private async Task<AuthResponseDto> IssueTokensAsync(User user)
    {
        var (accessToken, accessExpiresAt) =
            _tokenService.CreateAccessToken(user);
        var (refreshToken, refreshExpiresAt) =
            _tokenService.CreateRefreshToken();

        await _refreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = refreshExpiresAt
        });

        // Lưu luôn các thay đổi đang chờ (user mới, token cũ bị thu hồi)
        await _refreshTokens.SaveChangesAsync();

        return new AuthResponseDto(
            accessToken,
            accessExpiresAt,
            refreshToken,
            refreshExpiresAt,
            user.Username,
            user.Role);
    }

    private async Task RevokeAllAsync(string userId)
    {
        var tokens = await _refreshTokens.GetActiveByUserIdAsync(userId);

        foreach (var token in tokens)
            token.RevokedAt = DateTime.UtcNow;

        await _refreshTokens.SaveChangesAsync();
    }

    private static void Validate(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Tên đăng nhập không được để trống.");

        if (username.Trim().Length is < 3 or > 50)
            throw new ArgumentException("Tên đăng nhập phải từ 3 đến 50 ký tự.");

        if (string.IsNullOrEmpty(password) || password.Length < 6)
            throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");
    }
}
