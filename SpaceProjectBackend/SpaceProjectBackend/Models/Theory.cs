using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceProjectBackend.Models
{
    [Table("theory")]
    public class Theory
    {
        [Column("id")]
        public string Id { get; set; }


        [Column("name")]
        public string Name { get; set; }


        [Column("description")]
        public string Description { get; set; }


        [Column("usernotes")]
        public string Usernotes { get; set; }
    }
}
