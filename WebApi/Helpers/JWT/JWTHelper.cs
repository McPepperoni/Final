namespace WebApi.Helpers.JWT;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Persistence.Entities;
using WebApi.Constants;
using WebApi.DTOs;
using WebApi.Settings;

public class JWTHelper
{
    private readonly AppSettings _settings;

    public JWTHelper(AppSettings settings)
    {
        _settings = settings;
    }

    public JWTTokenEntity Create(UserDTO user, DateTime expiredAt, TokenName name = TokenName.AuthToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.JWT.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, name.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        };

        foreach (var item in user.Roles)
        {
            claims.Add(new Claim("Role", item));
        }

        var token = new JwtSecurityToken(
            _settings.JWT.Issuer,
            _settings.JWT.Audience,
            claims,
            null,
            expiredAt,
            creds
        );

        return new()
        {
            Token = tokenHandler.WriteToken(token),
            Expires = expiredAt,
        };
    }

    public async Task<JwtSecurityToken> AnalyzeToken(string token, TokenName name = TokenName.AuthToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.JWT.Key);
        var validationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        });

        return (JwtSecurityToken)validationResult.SecurityToken;
    }
}