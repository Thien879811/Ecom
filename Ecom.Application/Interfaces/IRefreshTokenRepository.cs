using Ecom.Domain.Entities;

namespace Ecom.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash);

    Task<List<RefreshToken>> GetActiveByUserIdAsync(string userId);

    Task AddAsync(RefreshToken refreshToken);

    Task SaveChangesAsync();
}
