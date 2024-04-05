using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;

namespace SpaceProjectBackend.Repository
{
    public interface ITheoryRepository
    {
        Task<IEnumerable<Theory>> GetTheories();

        Task<Theory?> GetTheory(string TheoryId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Theory?> CreateTheory(
                string name, 
                string description,    
                string usernotes);

        Task<Theory?> DeleteTheory(string TheoryId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Theory?> UpdateTheory(
                string TheoryId,  
                string name, 
                string description,    
                string usernotes);

        public void SaveChanges();
    }
}
