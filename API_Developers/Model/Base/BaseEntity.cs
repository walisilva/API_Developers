using System.ComponentModel.DataAnnotations.Schema;

namespace API_Developers.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
