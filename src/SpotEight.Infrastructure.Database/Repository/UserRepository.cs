using Microsoft.EntityFrameworkCore;
using SpotEight.Core.Domain.Entities;
using SpotEight.Core.Domain.Interfaces.Repository;
using SpotEight.Infrastructure.Database.Context;

namespace SpotEight.Infrastructure.Database.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserEntity>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }
}
