using CTPS.API.DTOs.Pass;
using CTPS.API.Models;

namespace CTPS.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(int userId);
        Task<List<UserPass>> GetUserPasses(int userId);
    }
}
