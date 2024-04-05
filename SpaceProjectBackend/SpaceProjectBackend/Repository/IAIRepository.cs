using SpaceProjectBackend.Enums;
using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.Repository
{
    public interface IAIRepository
    {
        Task<IEnumerable<AI>> GetAIs();
        Task<AI?> GetAI(string AiId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);
        Task<AI?> CreateAI(
                string name, 
                string description, 
                bool real, 
                string creatorId, 
                string image, 
                string usernotes);

        Task<AI?> DeleteAI(string AiId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);
        Task<AI?> UpdateAI(
                string AiId,  
                string name, 
                string description, 
                bool real, 
                string creatorId, 
                string image, 
                string usernotes, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        public void SaveChanges();
    }
}
