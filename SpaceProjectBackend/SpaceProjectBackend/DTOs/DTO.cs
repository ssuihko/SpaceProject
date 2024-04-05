using SpaceProjectBackend.Enums;
using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.DTOs
{
    public record RegisterDto(string Email, string Password);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, UserRole Role);

    class BookDTO
    {

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string? AuthorId { get; set; }

        public string Usernotes { get; set; }

        public BookDTO(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Description = book.Description;
            AuthorId = book.AuthorId;
            Usernotes = book.Usernotes;
        }

    }

    class AIDTO
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Real { get; set; }

        public string Description { get; set; }

        public string? CreatorId { get; set; }

        public string Image { get; set; }

        public string Usernotes { get; set; }

        public AIDTO(AI ai)
        {
            Id = ai.Id;
            Name = ai.Name;
            Real = ai.Real;
            Description = ai.Description;
            CreatorId = ai.CreatorId;
            Image = ai.Image;
            Usernotes = ai.Usernotes;
        }

    }

    class SpacecraftDTO
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Real { get; set; }

        public string Description { get; set; }

        public string? CreatorId { get; set; }

        public string Image { get; set; }

        public string Usernotes { get; set; }

        public SpacecraftDTO(Spacecraft spc)
        {
            Id = spc.Id;
            Name = spc.Name;
            Real = spc.Real;
            Description = spc.Description;
            CreatorId = spc.CreatorId;
            Image = spc.Image;
            Usernotes = spc.Usernotes;
        }

    }

    class TheoryDTO
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Usernotes { get; set; }

        public TheoryDTO(Theory theory)
        {
            Id = theory.Id;
            Name = theory.Name;
            Description = theory.Description;
            Usernotes = theory.Usernotes;
        }

    }

    class PersonFullDTO
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Profile { get; set; }

        public List<BookDTO> Books { get; set; } = new List<BookDTO>();

        public List<AIDTO> AIs { get; set; } = new List<AIDTO>();

        public List<SpacecraftDTO> Spacecrafts { get; set; } = new List<SpacecraftDTO>();

        public PersonFullDTO(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Image = person.Image;
            Profile = person.Profile;


            foreach(Book bk in person.Books)
            {
                Books.Add( new BookDTO(bk) );
            }

            foreach(AI ai in person.AIs)
            {
                AIs.Add( new AIDTO(ai) );
            }

            foreach(Spacecraft spacecraft in person.Spacecrafts)
            {
                Spacecrafts.Add( new SpacecraftDTO(spacecraft) );
            }
        }

    }

    class PersonSingleDTO
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Profile { get; set; }

        public string Usernotes { get; set; }

        public PersonSingleDTO(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Image = person.Image;
            Profile = person.Profile;
        }

    }
}
