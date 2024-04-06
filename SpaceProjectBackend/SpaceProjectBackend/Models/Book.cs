using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceProjectBackend.Models
{

    [Table("book")]
    public class Book
    {
        [Column("id")]
        public string Id { get; set; }


        [Column("title")]
        public string Title { get; set; }

        [Column("real")]
        public bool Real { get; set; }


        [Column("description")]
        public string Description { get; set; }

        [Column("image")]
        public string Image { get; set; }


        [Column("author_id")]
        public string? AuthorId { get; set; }
        public Person? Author { get; set; }


        [Column("usernotes")]
        public string Usernotes { get; set; }
    }
}
