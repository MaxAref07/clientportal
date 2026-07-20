using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClientPortal.Application.DTOs;
using ClientPortal.Application.Interfaces;
using ClientPortal.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClientPortal.Infrastructure.Auth;

public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public AuthTokenDto GenerateToken(Guid userId, string email, UserRole role)
    {
        var secretKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Secret Key is not configured.");
        
        var issuer = configuration["Jwt:Issuer"] ?? "ClientPortal";
        var audience = configuration["Jwt:Audience"] ?? "ClientPortal.Frontend";
        
        if (!int.TryParse(configuration["Jwt:ExpiryInHours"], out var expiryHours))
        {
            expiryHours = 24; 
        }
        
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var symmetricKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        var expiresAt = DateTime.UtcNow.AddHours(expiryHours);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role.ToString()),
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthTokenDto(tokenHandler.WriteToken(token), expiresAt);
    }
}