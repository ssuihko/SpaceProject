using SpaceProjectBackend.Models;
using SpaceProjectBackend.Enums;

namespace SpaceProjectBackend.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooks();

        Task<Book?> GetBook(string BookId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Book?> CreateBook(
                string title, 
                string description,
                bool real,
                string image,
                string authorId,  
                string usernotes);

        Task<Book?> DeleteBook(string BookId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);

        Task<Book?> UpdateBook(
                string bookId,  
                string title, 
                string description,
                bool real,
                string image,
                string authorId,  
                string usernotes);

        public void SaveChanges();
    }
}
