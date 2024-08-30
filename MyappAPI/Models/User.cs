using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyappAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        [Column("name", TypeName = "nvarchar(50)")]
        public string name { set; get; } = "";

        [Column("age" ,TypeName = "int")]
        public int age { set; get; }

        [Column("birth" ,TypeName = "datetime")]
        public DateTime birth { set; get; }
    }

}
