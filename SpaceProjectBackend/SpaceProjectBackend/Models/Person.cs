using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceProjectBackend.Models
{
    [Table("person")]
    public class Person
    {
        [Column("id")]
        public string Id { get; set; }


        [Column("name")]
        public string Name { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("profile")]
        public string Profile { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<AI> AIs { get; set; } = new List<AI>();

        public ICollection<Spacecraft> Spacecrafts { get; set; } = new List<Spacecraft>();


    }
}
