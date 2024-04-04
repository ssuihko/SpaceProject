﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaceProjectBackend.Models;

namespace SpaceProjectBackend.Data
{
    public class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        public DbSet<Theory> Theories { get; set; }
        public DbSet<AI> AIs { get; set; }
        public DbSet<Spacecraft> Spacecrafts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string g1 = Guid.NewGuid().ToString();

            modelBuilder.Entity<Theory>().HasData(
                new Theory { 
                    Id = Guid.NewGuid().ToString(),
                    Name="N-body problem",
                    Description= "In physics, the n-body problem is the problem of predicting the individual motions of a group of celestial objects interacting with each other gravitationally. Solving this problem has been motivated by the desire to understand the motions of the Sun, Moon, planets, and visible stars. In the 20th century, understanding the dynamics of globular cluster star systems became an important n-body problem. The n-body problem in general relativity is considerably more difficult to solve due to additional factors like time and space distortions.",
                    Usernotes="often used to argue that plotting a path through space in inter-stellar travel would be impossible."
                });

            modelBuilder.Entity<AI>().HasData(
                new AI {  Id = Guid.NewGuid().ToString(),
                          Name="HAL 9000",
                          Description="The spaceship AI controlling the travel of Discovery One to Jupiter",
                          Real=false,
                          CreatorId=g1,
                          Image= "https://upload.wikimedia.org/wikipedia/commons/archive/2/2e/20210405121432%21Hal_9000_Panel.svg",
                          Usernotes="famous AI from the movie",
                });

            //modelBuilder.Entity<ApplicationUser>().HasMany(e => e.BlogPosts).WithOne(e => e.ApplicationUser).HasForeignKey(e => e.ApplicationUserId);

            modelBuilder.Entity<Book>().HasData(
                new Book  {
                    Id=Guid.NewGuid().ToString(),
                    Title="Childhood's End",
                    Description= "Childhood's End is a 1953 science fiction novel by the British author Arthur C. Clarke. The story follows the peaceful alien invasion[1] of Earth by the mysterious Overlords, whose arrival begins decades of apparent utopia under indirect alien rule, at the cost of human identity and culture.",
                    AuthorId=g1,
                    Usernotes="interesting concepts about aliens and human transcendence",
                });

            modelBuilder.Entity<Spacecraft>().HasData(
                new Spacecraft
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Discovery One",
                    Description= "The United States Spacecraft Discovery One is a fictional spaceship featured in the first two novels of the Space Odyssey series by Arthur C. Clarke and in the films 2001: A Space Odyssey (1968) directed by Stanley Kubrick and 2010: The Year We Make Contact (1984) directed by Peter Hyams. The ship is a nuclear-powered interplanetary spaceship, crewed by two men and controlled by the AI on-board computer HAL 9000. The ship is destroyed in the second novel and makes no further appearances.",
                    Real=false,
                    CreatorId=g1,
                    Image= "https://en.wikipedia.org/wiki/File:Discovery_One_from_trailer_of_2001_A_Space_Odyssey_(1968).png",
                    Usernotes="Interesting to me due to the creator and HAL9000"
                });

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = g1, 
                             Name = "Arthur C. Clarke", 
                             Image = "https://en.wikipedia.org/wiki/File:Arthur_C._Clarke_1965.jpg",
                             Profile="English science fiction writer, co-wrote the screenplay for 1968 film 2001: A Space Odyssey. Quote: ´Any advanced extraterrestrial intelligence will be indistinguishable from nature.´ ",
                }
            );
        }

    }
}