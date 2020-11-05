using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDueDateTracker.Models
{
    [Table("author")]
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(60)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeathDate { get; set; }

        [InverseProperty(nameof(Models.Book.Author))]
        public virtual ICollection<Book> Books { get; set; }
    }
}
