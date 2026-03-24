using CTPS.API.DTOs.Pass;
using CTPS.API.Models;

namespace CTPS.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByMobile(string mobile);
        Task<User> CreateUser(User user);
        Task<List<UserPass>> GetUserPasses(int userId);
    }
}
