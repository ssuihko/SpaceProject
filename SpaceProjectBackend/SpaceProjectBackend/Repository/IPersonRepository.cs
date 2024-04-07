using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;

namespace SpaceProjectBackend.Repository
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPeople();

        Task<Person?> GetPerson(string PersonId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Person?> CreatePerson(
                string name, 
                string image,
                bool real,
                string description);

        Task<Person?> DeletePerson(string PersonId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Person?> UpdatePerson(
                string PersonId,  
                string name, 
                string image,
                bool real,
                string description);

        public void SaveChanges();
    }
}
