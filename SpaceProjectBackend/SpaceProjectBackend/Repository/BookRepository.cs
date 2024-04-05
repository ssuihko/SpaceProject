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
                string title, 
                string description,  
                string authorId,  
                string usernotes)
        {
            var plr = _db.People.FirstOrDefault(x => x.Id == authorId);
   
            if (title == "" || description == "" || usernotes == "" ) return null;

            var Book = new Book { 
                Id = Guid.NewGuid().ToString(), 
                Title = title, 
                Description = description, 
                AuthorId = authorId, 
                Author = plr,  
                Usernotes = usernotes };

            await _db.Books.AddAsync(Book);
            return Book;
        }

        public async Task<Book?> UpdateBook( 
                string bookId,  
                string title, 
                string description,  
                string authorId,  
                string usernotes)
        {
            var bk = await _db.Books.FirstOrDefaultAsync(s => s.Id ==bookId);

            if (bk == null)
            {
                return null;
            }

            if (title != null || title != "") 
            { 
                bk.Title = title; 
            }

            if (description != null || description != "") 
            { 
                bk.Description = description; 
            }

            if (authorId != null || authorId != "") 
            { 
                var auth = await _db.People.FirstOrDefaultAsync(s => s.Id == authorId);
                bk.AuthorId = authorId;
                bk.Author = auth; 
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
