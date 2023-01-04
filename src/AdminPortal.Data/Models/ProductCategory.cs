using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(ProductCategory))]
    public class ProductCategory
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual Status Status { get; set; }
    }
}
