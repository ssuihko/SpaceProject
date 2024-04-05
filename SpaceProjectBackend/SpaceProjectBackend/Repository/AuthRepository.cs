using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private DatabaseContext _db;

        public AuthRepository(DatabaseContext db) 
        {
            _db = db;
        }

        public ApplicationUser? GetUser(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}
