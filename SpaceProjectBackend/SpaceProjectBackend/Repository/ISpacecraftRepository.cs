using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;

namespace SpaceProjectBackend.Repository
{
    public interface ISpacecraftRepository
    {
        
        Task<IEnumerable<Spacecraft>> GetSpacecrafts();

        Task<Spacecraft?> GetSpacecraft(string SpacecraftId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Spacecraft?> CreateSpacecraft(
                string name,
                bool real,
                string description,
                string creatorId, 
                string image,  
                string usernotes);

        Task<Spacecraft?> DeleteSpacecraft(string SpacecraftId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Spacecraft?> UpdateSpacecraft(
                string SpacecraftId, 
                string name,
                bool real,
                string description,
                string creatorId,
                string image,  
                string usernotes);

        public void SaveChanges();
    }
}
