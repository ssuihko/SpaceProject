using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;
using Microsoft.EntityFrameworkCore;


namespace SpaceProjectBackend.Repository
{
    public class AIRepository : IAIRepository
    {
        private DatabaseContext _db;

        public AIRepository(DatabaseContext db) {
            _db = db;
        }
    
        public async Task<IEnumerable<AI>> GetAIs()
        {
            return await _db.AIs.ToListAsync();
        }

        public async Task<AI?> GetAI(string AiId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _db.AIs.FirstOrDefaultAsync(s => s.Id == AiId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _db.AIs.FirstOrDefaultAsync(s => s.Id == AiId);
                default:
                    return null;
            }
        }

        public async Task<AI?> DeleteAI(string AiId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            var mv = await _db.AIs.FirstOrDefaultAsync(s => s.Id == AiId);

            if (mv == null)
            {
                return null;
            }

            _db.AIs.Remove(mv);

            return mv;
        }

        public async Task<AI?> CreateAI(
                string name, 
                string description, 
                bool real, 
                string creatorId, 
                string image, 
                string usernotest)
        {
            var plr = _db.People.FirstOrDefault(x => x.Id == creatorId);

            Console.WriteLine("in the createAI repository");
            Console.WriteLine(plr);

            if (name == "" || description == "" || image == "" || usernotest == "" ) return null;

            var AI = new AI { 
                Id = Guid.NewGuid().ToString(), 
                Name = name, 
                Description = description, 
                Real = real, 
                CreatorId = creatorId, 
                Creator = plr, 
                Image = image, 
                Usernotes = usernotest };

            await _db.AIs.AddAsync(AI);
            return AI;
        }

        public async Task<AI?> UpdateAI( 
                string AiId,  
                string name, 
                string description, 
                bool real, 
                string creatorId, 
                string image, 
                string usernotes, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            var ai = await _db.AIs.FirstOrDefaultAsync(s => s.Id == AiId);

            if (ai == null)
            {
                return null;
            }

            if (name != null || name != "") 
            { 
                ai.Name = name; 
            }

            if (description != null || description != "") 
            { 
                ai.Description = description; 
            }

            if (real != null) 
            { 
                ai.Real = real; 
            }

            if (creatorId != null || creatorId != "") 
            { 
                var person = await _db.People.FirstOrDefaultAsync(s => s.Id == creatorId);
                ai.CreatorId = creatorId;
                ai.Creator = person; 
            }

            if (image != null || image != "") 
            { 
                ai.Image = image; 
            }

            if (usernotes != null || usernotes != "") 
            { 
                ai.Usernotes = usernotes; 
            }

            return ai;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
