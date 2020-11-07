using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDueDateTracker.Models
{
    [Table("book")]
    public class Book
    {
        public Book()
        {
            Borrows = new HashSet<Borrow>();
        }

        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "int(10)")]
        public int AuthorID { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime PublicationDate { get; set; }

        //Citation
        //https://stackoverflow.com/questions/40730/what-is-the-best-way-to-give-a-c-sharp-auto-property-an-initial-value
        //Add initialvalue to C# auto implemented property
        [Required]
        [Column(TypeName = "boolean")]
        public bool Archived { get; set; } = false;
        //End Citation

        [ForeignKey(nameof(AuthorID))]
        [InverseProperty(nameof(Models.Author.Books))]
        public virtual Author Author { get; set; }

        [InverseProperty(nameof(Models.Borrow.Book))]
        public virtual ICollection<Borrow> Borrows { get; set; }
    }
}
