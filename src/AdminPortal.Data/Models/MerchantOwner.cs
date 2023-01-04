using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(MerchantOwner))]
    public class MerchantOwner
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public virtual int UserId { get; set; }

        [Required, ForeignKey(nameof(Merchant))]
        public virtual int MerchantId { get; set; }

        public virtual Merchant Merchant { get; set; }

        public virtual User User { get; set; }
    }
}
