﻿using System.ComponentModel.DataAnnotations;

namespace SpaceProjectBackend.DTOs
{
    public record BookUpdatePayload(string? Title, string? Description, bool? Real, string? Image, string? AuthorId, string? Usernotes);


    public record BookPostPayload
    {
        public string Title { get; init; }
        public string Description { get; init; }

        public bool Real { get; set; }

        public string Image { get; set; }
        public string AuthorId { get; init; }
        public string Usernotes { get; init; }
        public BookPostPayload(string title, string description, bool real, string image, string authorId, string usernotes)
        {
            Title = title;
            Description = description;
            Real = real;
            Image = image;
            AuthorId = authorId;
            Usernotes = usernotes;
        }

    }

    // People
    public record PersonUpdatePayload(string? Name, string? Image, bool? Real, string? Profile);

    public record PersonPostPayload
    {
        public string Name { get; init; }
        public string Image { get; init; }

        public bool Real { get; set; }
        public string Profile { get; init; }
        public PersonPostPayload(string name, string image, bool real, string profile)
        {
            Name = name;
            Image = image;
            Real = real;
            Profile = profile;
        }

    }

    // AIs
    public record AIUpdatePayload(string? Name, string? Description, bool? Real, string? CreatorId, string? Image, string? Usernotes);

    public record AIPostPayload
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; init; }

        [Required(ErrorMessage = "Real value is required")]
        public bool Real { get; init; }

        [Required(ErrorMessage = "CreatorId is required")]
        public string CreatorId { get; init; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; init; }

        [Required(ErrorMessage = "Usernotes are required")]
        public string Usernotes { get; init; }
        public AIPostPayload(string name, string description, bool real, string creatorId, string image, string usernotes)
        {
            Name = name;
            Description = description;
            Real = real;
            CreatorId = creatorId;
            Image = image;
            Usernotes = usernotes;
        }

    }


    // Spacecrafts
    public record SpacecraftUpdatePayload(string? Name, string? Description, bool? Real, string? CreatorId, string? Image, string? Usernotes);

    public record SpacecraftPostPayload
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool Real { get; init; }
        public string CreatorId { get; init; }
        public string Image { get; init; }
        public string Usernotes { get; init; }
        public SpacecraftPostPayload(string name, string description, bool real, string creatorId, string image, string usernotes)
        {
            Name = name;
            Description = description;
            Real = real;
            CreatorId = creatorId;
            Image = image;
            Usernotes = usernotes;
        }

    }

    // Theory
    public record TheoryUpdatePayload(string? Name, string? Description, bool? Real, string? Image, string? Usernotes);

    public record TheoryPostPayload
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool Real { get; set; }

        public string Image { get; init; }
        public string Usernotes { get; init; }
        public TheoryPostPayload(string name, string description, bool real, string image, string usernotes)
        {
            Name = name;
            Description = description;
            Real = real;
            Image = image;
            Usernotes = usernotes;
        }

    }
}
