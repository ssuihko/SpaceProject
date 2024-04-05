using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace SpaceProjectBackend.Models
{
    [Table("ai")]
    public class AI
    {
        [Column("id")]
        public string Id { get; set; }


        [Column("name")]
        public string Name { get; set; }


        [Column("description")]
        public string Description { get; set; }

        [Column("real")]
        public bool Real { get; set; }


        [Column("creator_id")]
        public string? CreatorId { get; set; }
        public Person? Creator { get; set; }


        [Column("image")]
        public string Image { get; set; }


        [Column("usernotes")]
        public string Usernotes { get; set; }


    }
}
