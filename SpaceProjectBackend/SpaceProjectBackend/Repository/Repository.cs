using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext _db;

        public Repository(DatabaseContext db) 
        {
            _db = db;
        }

        public ApplicationUser? GetUser(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}
