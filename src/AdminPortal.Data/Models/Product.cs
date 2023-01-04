using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(Product))]
    public class Product
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual string SKU { get; set; }

        [Required, StringLength(100)]
        public virtual string Name { get; set; }

        [Url]
        public virtual string Image { get; set; }

        [Required, ForeignKey(nameof(Merchant))]
        public virtual int MerchantId { get; set; }

        [Required, ForeignKey(nameof(ProductCategory))]
        public virtual int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual Status Status { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<ProductVariation> Variations { get; set; }
        public virtual ProductCategory Category { get; set; }
    }
}
