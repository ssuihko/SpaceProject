using Microsoft.AspNetCore.Identity;
using SpaceProjectBackend.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceProjectBackend.Models
{
    [Table("applicationusers")]
    public class ApplicationUser : IdentityUser
    {

        [Column("role")]
        public UserRole Role { get; set; }


    }
}
