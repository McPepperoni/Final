namespace WebApi.Helpers.JWT;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Persistence.Entities;
using Persistence.Managers;
using WebApi.Constants;
using WebApi.Settings;

public class JWTHelper
{
    private readonly AppSettings _settings;
    private readonly FinalUserManager _userManager;

    public JWTHelper(AppSettings settings, FinalUserManager userManager)
    {
        _settings = settings;
        _userManager = userManager;
    }

    public async Task<JWTTokenEntity> Create(UserEntity user, DateTime expiredAt, TokenName name = TokenName.AuthToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.JWT.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.FullName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            _settings.JWT.Issuer,
            _settings.JWT.Audience,
            claims,
            null,
            expiredAt,
            creds
        );

        return new JWTTokenEntity()
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

    public JwtSecurityToken ReadToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ReadJwtToken(token);
    }
}