using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;
using Microsoft.EntityFrameworkCore;

namespace SpaceProjectBackend.Repository
{
    public class TheoryRepository : ITheoryRepository
    {
        private DatabaseContext _db;

        public TheoryRepository(DatabaseContext db) {
            _db = db;
        }
    
        public async Task<IEnumerable<Theory>> GetTheories()
        {
            return await _db.Theories.ToListAsync();
        }

        public async Task<Theory?> GetTheory(string TheoryId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _db.Theories.FirstOrDefaultAsync(s => s.Id == TheoryId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _db.Theories.FirstOrDefaultAsync(s => s.Id == TheoryId);
                default:
                    return null;
            }
        }

        public async Task<Theory?> DeleteTheory(string TheoryId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            var mv = await _db.Theories.FirstOrDefaultAsync(s => s.Id == TheoryId);

            if (mv == null)
            {
                return null;
            }

            _db.Theories.Remove(mv);

            return mv;
        }

        public async Task<Theory?> CreateTheory(
                string name, 
                string description,    
                string usernotes)
        {

            if (name == "" || description == "" || usernotes == "" ) return null;

            var Theory = new Theory { 
                Id = Guid.NewGuid().ToString(), 
                Name = name, 
                Description = description, 
                Usernotes = usernotes };

            await _db.Theories.AddAsync(Theory);
            return Theory;
        }

        public async Task<Theory?> UpdateTheory( 
                string TheoryId,  
                string name, 
                string description,    
                string usernotes)
        {
            var thr = await _db.Theories.FirstOrDefaultAsync(s => s.Id ==TheoryId);

            if (thr == null)
            {
                return null;
            }

            if (name != null || name != "") 
            { 
                thr.Name = name; 
            }

            if (description != null || description != "") 
            { 
                thr.Description = description; 
            }

            if (usernotes != null || usernotes != "") 
            { 
               thr.Usernotes = usernotes; 
            }

            return thr;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
