using Ecom.Domain.Entities;

namespace Ecom.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateAccessToken(User user);

    (string Token, DateTime ExpiresAt) CreateRefreshToken();

    string HashToken(string token);
}
