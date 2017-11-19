using System.Threading.Tasks;

namespace PrivateTutorOnline.Models
{
    public interface ICustomUserStore<TUser, TKey>
    {
        Task CreateAsync(ApplicationUser user);
        Task DeleteAsync(ApplicationUser user);
        void Dispose();
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<string> GetPasswordHashAsync(ApplicationUser user);
        Task<bool> HasPasswordAsync(ApplicationUser user);
        Task SetPasswordHashAsync(ApplicationUser user, string passwordHash);
        Task UpdateAsync(ApplicationUser user);
    }
}