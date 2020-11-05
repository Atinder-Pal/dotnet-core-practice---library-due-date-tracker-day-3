using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDueDateTracker.Models
{
    [Table("borrow")]
    public class Borrow
    {

        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "int(10)")]
        public int BookID { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CheckedOutDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnedDate { get; set; }

        [Required]
        [Column(TypeName ="int(10)")]
        public int ExtensionCount { get; set; }

        [ForeignKey(nameof(BookID))]
        [InverseProperty(nameof(Models.Book.Borrows))]
        public virtual Book Book { get; set; }
    }
}
