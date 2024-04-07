using Microsoft.AspNetCore.Mvc;
using SpaceProjectBackend.Repository;
using SpaceProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using SpaceProjectBackend.DTOs;
using SpaceProjectBackend.Enums;
using System.Security.Claims;
using SpaceProjectBackend.Utils;

namespace SpaceProjectBackend.Endpoints
{
    public static class BookEndpoints
    {
        public static void ConfigureBookEndpoints(this WebApplication app)
        {
            var bookGroup = app.MapGroup("books");

            bookGroup.MapPost("/", CreateBook);
            bookGroup.MapGet("/", GetBooks);
            bookGroup.MapGet("/{Id}", GetBook);
            bookGroup.MapPut("/{Id}", UpdateBook);
            bookGroup.MapDelete("/{Id}", DeleteBook);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> CreateBook(
            BookPostPayload payload,
            IBookRepository bookRepository, 
            IAuthRepository authRepository,
            ClaimsPrincipal user)
        {

            var uid = user.UserId();
            // string role = user.UserRole().ToString();
            // var email = user.UserEmail();

            if (uid == null)
            {
                return Results.Unauthorized();
            }
            
            if (payload.Name == "" || payload.Description == "" || payload.Usernotes == "" || payload.Image == "")
            {
                return Results.BadRequest("Non-empty fields are required");
            }

            if (payload.Name == null || payload.Usernotes == null || payload.Description == null || payload.Image == null || payload.Real == null)
            {
                return Results.BadRequest("Non-null fields are required");
            }

            Book? Book = await bookRepository.CreateBook(payload.Name, payload.Description, payload.Real, payload.Image, payload.CreatorId, payload.Usernotes);

            if (Book == null)
            {
                return Results.BadRequest("Failed to create Book.");
            }

          
            bookRepository.SaveChanges();

            BookDTO mdto = new BookDTO(Book);

            return TypedResults.Created($"/books/{Book.Id}", mdto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetBooks(IBookRepository repository)
        {

            var res = await repository.GetBooks();

            List<BookDTO> reslist = new List<BookDTO>();

            foreach (Book bk in res)
            {
                BookDTO dt = new BookDTO(bk);

                reslist.Add(dt);
            }

            return TypedResults.Ok(reslist);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> UpdateBook(string bookId, BookUpdatePayload payload, IBookRepository repository, ClaimsPrincipal user)
        {

            if (payload.Name == null || payload.CreatorId == null || payload.Usernotes == null || payload.Description == null)
            {
                return Results.BadRequest("Non-null fields are required");
            }


            Book? ogBook = await repository.GetBook(bookId, PreloadPolicy.PreloadRelations);

            if (ogBook == null)
            {
                return Results.BadRequest("Book not found");
            }

            string newTitle = (payload.Name.Length > 0) ? payload.Name : ogBook.Name;

            string newDescription = (payload.Description.Length > 0) ? payload.Description : ogBook.Description;

            bool newReal = ogBook.Real;

            if (payload.Real != null && newReal != payload.Real)
            {
                newReal = (bool)payload.Real;
            }

            string newCreatorId = (payload.CreatorId.Length > 0) ? payload.CreatorId : ogBook.CreatorId;

            string newUsernotes = (payload.Usernotes.Length > 0) ? payload.Usernotes : ogBook.Usernotes;

            string newImage = (payload.Image.Length > 0) ? payload.Image : ogBook.Image;


            Book? Book = await repository.UpdateBook(ogBook.Id, newTitle, newDescription, newReal, newImage, newCreatorId, newUsernotes);

            if (Book == null)
            {
                return Results.BadRequest("Failed to create Message.");
            }


            BookDTO mv = new BookDTO(Book);


            repository.SaveChanges();

            return TypedResults.Created($"/books/{Book.Id}", mv);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> GetBook(string bookId, IBookRepository repository)
        {

            Book? book = await repository.GetBook(bookId, PreloadPolicy.PreloadRelations);

            if (book == null)
            {
                return Results.NotFound("Book not found");
            }

            var BookDTO = new BookDTO(book);

            repository.SaveChanges();

            return TypedResults.Ok(BookDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> DeleteBook(string bookId, IBookRepository repository, ClaimsPrincipal user)
        {

            Book? book = await repository.DeleteBook(bookId, PreloadPolicy.PreloadRelations);

            if (book == null)
            {
                return Results.NotFound("Book not found");
            }

            var PlayerDTO = new BookDTO(book);

            repository.SaveChanges();

            return TypedResults.Ok(PlayerDTO);
        }
    }
}
