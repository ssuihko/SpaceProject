using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;
using Microsoft.EntityFrameworkCore;

namespace SpaceProjectBackend.Repository
{
    public class SpacecraftRepository : ISpacecraftRepository
    {
        private DatabaseContext _db;

        public SpacecraftRepository(DatabaseContext db) {
            _db = db;
        }
    
        public async Task<IEnumerable<Spacecraft>> GetSpacecrafts()
        {
            return await _db.Spacecrafts.ToListAsync();
        }

        public async Task<Spacecraft?> GetSpacecraft(string SpacecraftId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _db.Spacecrafts.FirstOrDefaultAsync(s => s.Id == SpacecraftId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _db.Spacecrafts.FirstOrDefaultAsync(s => s.Id == SpacecraftId);
                default:
                    return null;
            }
        }

        public async Task<Spacecraft?> DeleteSpacecraft(string SpacecraftId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            var mv = await _db.Spacecrafts.FirstOrDefaultAsync(s => s.Id == SpacecraftId);

            if (mv == null)
            {
                return null;
            }

            _db.Spacecrafts.Remove(mv);

            return mv;
        }

        public async Task<Spacecraft?> CreateSpacecraft(
                string name,
                bool real,
                string description,
                string creatorId, 
                string image,  
                string usernotes)
        {
            var plr = _db.People.FirstOrDefault(x => x.Id == creatorId);
   
            if (name == "" || description == "" || usernotes == "" || image == "") return null;

            var Spacecraft = new Spacecraft { 
                Id = Guid.NewGuid().ToString(), 
                Name = name,
                Real = real, 
                Description = description, 
                CreatorId = creatorId, 
                Creator = plr,
                Image = image,  
                Usernotes = usernotes };

            await _db.Spacecrafts.AddAsync(Spacecraft);
            return Spacecraft;
        }

        public async Task<Spacecraft?> UpdateSpacecraft( 
                string SpacecraftId, 
                string name,
                bool real,
                string description,
                string creatorId,
                string image,  
                string usernotes)
        {
            var spc = await _db.Spacecrafts.FirstOrDefaultAsync(s => s.Id == SpacecraftId);

            if (spc == null)
            {
                return null;
            }

            if (name != null || name != "") 
            { 
                spc.Name = name; 
            }

            if (real != null) {
                spc.Real = real;
            }

            if (description != null || description != "") 
            { 
                spc.Description = description; 
            }

            if (creatorId != null || creatorId != "") 
            { 
                var auth = await _db.People.FirstOrDefaultAsync(s => s.Id == creatorId);
                spc.CreatorId = creatorId;
                spc.Creator = auth; 
            }

            if (image != null || image != "") 
            { 
                spc.Image = image; 
            }

            if (usernotes != null || usernotes != "") 
            { 
                spc.Usernotes = usernotes; 
            }

            return spc;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
