using System.ComponentModel.DataAnnotations.Schema;

namespace API_Developers.Model
{
    [Table("developers")]
    public class Developer : Person
    {
        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("squad")]
        public string? Squad { get; set; }

        [Column("login")]
        public string? Login { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("createdat")]
        public DateTime? createdAt { get; set; }
    }
}
