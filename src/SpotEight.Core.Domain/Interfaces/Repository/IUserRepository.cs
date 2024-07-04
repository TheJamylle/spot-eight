using SpotEight.Core.Domain.Entities;

namespace SpotEight.Core.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAll();
}
