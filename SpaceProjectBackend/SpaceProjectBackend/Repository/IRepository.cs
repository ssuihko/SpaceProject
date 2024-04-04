using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.Repository
{
    public interface IRepository
    {
        public ApplicationUser? GetUser(string email);
    }
}
