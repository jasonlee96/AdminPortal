using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(ProductVariation))]
    public class ProductVariation
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required, StringLength(100)]
        public virtual string Name { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public virtual decimal Price { get; set; }

        [Required]
        public virtual string SKU { get; set; }
        public virtual string Description { get; set; }
        [Required]
        public virtual int AvailableQuantity { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual Status Status { get; set; }

        [Required,ForeignKey(nameof(Product))]
        public virtual int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
