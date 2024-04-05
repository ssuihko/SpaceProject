using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.Repository
{
    public interface IAuthRepository
    {
        public ApplicationUser? GetUser(string email);
    }
}
