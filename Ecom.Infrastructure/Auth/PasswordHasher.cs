using Microsoft.AspNetCore.Identity;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;

namespace Ecom.Infrastructure.Auth;

// Bọc PasswordHasher của ASP.NET Identity (PBKDF2 + salt)
public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string passwordHash, string password)
    {
        return _hasher.VerifyHashedPassword(null!, passwordHash, password)
            != PasswordVerificationResult.Failed;
    }
}
