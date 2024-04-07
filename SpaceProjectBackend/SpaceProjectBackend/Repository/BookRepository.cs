using SpaceProjectBackend.Data;
using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;
using Microsoft.EntityFrameworkCore;

namespace SpaceProjectBackend.Repository
{
    public class BookRepository : IBookRepository
    {
        private DatabaseContext _db;

        public BookRepository(DatabaseContext db) {
            _db = db;
        }
    
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<Book?> GetBook(string BookId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _db.Books.FirstOrDefaultAsync(s => s.Id == BookId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _db.Books.FirstOrDefaultAsync(s => s.Id == BookId);
                default:
                    return null;
            }
        }

        public async Task<Book?> DeleteBook(string BookId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {
            var mv = await _db.Books.FirstOrDefaultAsync(s => s.Id == BookId);

            if (mv == null)
            {
                return null;
            }

            _db.Books.Remove(mv);

            return mv;
        }

        public async Task<Book?> CreateBook(
                string name, 
                string description,
                bool real,
                string image,
                string creatorId,  
                string usernotes)
        {
            var plr = _db.People.FirstOrDefault(x => x.Id == creatorId);
   
            if (name == "" || description == "" || usernotes == "" || image == "") return null;

            var Book = new Book { 
                Id = Guid.NewGuid().ToString(), 
                Name = name, 
                Description = description, 
                Real = real,
                Image = image,
                CreatorId = creatorId, 
                Creator = plr,  
                Usernotes = usernotes };

            await _db.Books.AddAsync(Book);
            return Book;
        }

        public async Task<Book?> UpdateBook( 
                string bookId,  
                string name, 
                string description,
                bool real,
                string image,
                string creatorId,  
                string usernotes)
        {
            var bk = await _db.Books.FirstOrDefaultAsync(s => s.Id ==bookId);

            if (bk == null)
            {
                return null;
            }

            if (bk.Real != real) // booleans don't match
            {
                bk.Real = real;
            }

            if (name != null || name != "") 
            { 
                bk.Name = name; 
            }

            if (image != null || image != "")
            {
                bk.Image = image;
            }

            if (description != null || description != "") 
            { 
                bk.Description = description; 
            }

            if (creatorId != null || creatorId != "") 
            { 
                var auth = await _db.People.FirstOrDefaultAsync(s => s.Id == creatorId);
                bk.CreatorId = creatorId;
                bk.Creator = auth; 
            }

            if (usernotes != null || usernotes != "") 
            { 
                bk.Usernotes = usernotes; 
            }

            return bk;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
