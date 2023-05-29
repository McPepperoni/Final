namespace WebApi.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

using WebApi.Helpers.JWT;



public interface IJWTService
{
    Task<List<JWTTokenEntity>> GetExpired();
    Task Add(string refreshToken);
    Task<string> Delete(JWTTokenEntity token);
}

public class JWTService : BaseService<JWTTokenEntity>, IJWTService
{
    private readonly JWTHelper _jwtHelper;
    public JWTService(ApplicationDbContext dbContext, JWTHelper jwtHelper, IMapper mapper) : base(dbContext, mapper)
    {
        _jwtHelper = jwtHelper;
    }

    public async Task<List<JWTTokenEntity>> GetExpired()
    {
        return await _dbSet.Where(x => x.Expires < DateTime.UtcNow).ToListAsync();
    }

    public async Task Add(string refreshToken)
    {
        var analyzedToken = await _jwtHelper.AnalyzeToken(refreshToken);
        var token = await _dbSet.AddAsync(new()
        {
            Token = refreshToken,
            Expires = analyzedToken.ValidTo,
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> Delete(JWTTokenEntity token)
    {
        _dbContext.Remove(token);
        await _dbContext.SaveChangesAsync();
        return token.Id;
    }

}