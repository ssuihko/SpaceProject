using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;
using Microsoft.EntityFrameworkCore;

namespace SpaceProjectBackend.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private DatabaseContext _db;

        public PersonRepository(DatabaseContext db) {
            _db = db;
        }
    
        public async Task<IEnumerable<Person>> GetPeople()
        {
          
            return await _db.People.Include(x => x.Books).Include(x => x.AIs).Include(x => x.Spacecrafts).ToListAsync();
        }

        public async Task<Person?> GetPerson(string PersonId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _db.People.Include(x => x.Books).Include(x => x.AIs).Include(x => x.Spacecrafts).FirstOrDefaultAsync(s => s.Id == PersonId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _db.People.FirstOrDefaultAsync(s => s.Id == PersonId);
                default:
                    return null;
            }
        }

        public async Task<Person?> DeletePerson(string PersonId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            var mv = await _db.People.FirstOrDefaultAsync(s => s.Id == PersonId);

            if (mv == null)
            {
                return null;
            }

            _db.People.Remove(mv);

            return mv;
        }

        public async Task<Person?> CreatePerson(
                string name,
                string image,
                bool real,
                string description)
        {

            if (name == "" || image == "" || description == "") return null;

            var Person = new Person { 
                Id = Guid.NewGuid().ToString(), 
                Name = name, 
                Image = image, 
                Real = real,
                Description = description };

            await _db.People.AddAsync(Person);
            return Person;
        }

        public async Task<Person?> UpdatePerson( 
                string PersonId,  
                string name,
                string image,
                bool real,
                string description)
        {
            var prs = await _db.People.FirstOrDefaultAsync(s => s.Id ==PersonId);

            if (prs == null)
            {
                return null;
            }

            if (prs.Real != real)
            {
                prs.Real = real;
            }

            if (name != null || name != "") 
            { 
                prs.Name = name; 
            }

            if (image != null || image != "") 
            { 
                prs.Image = image; 
            }

            if (description != null || description != "") 
            { 
                prs.Description = description; 
            }

            return prs;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
        
    }
}
