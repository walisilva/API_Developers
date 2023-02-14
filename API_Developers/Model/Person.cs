using API_Developers.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Developers.Model
{
    //[Table("person")]
    public class Person : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; }
    }
}
