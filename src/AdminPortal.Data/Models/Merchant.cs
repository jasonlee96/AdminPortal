using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(Merchant))]
    [Index(nameof(Slug), IsUnique = true)]
    public class Merchant
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(150)]
        public virtual string Name { get; set; }

        [Url]
        public virtual string Logo { get; set; }

        [Required]

        public virtual string Slug { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual Status Status { get; set; }

        [StringLength(150)]
        public virtual string Address { get; set; }
        [StringLength(45)]
        public virtual string State { get; set; }
        [StringLength(10)]
        public virtual string PostalCode { get; set; }
        [StringLength(45)]
        public virtual string Country { get; set; }

        public virtual string Lat { get; set; }
        public virtual string Lng { get; set; }


        public virtual ICollection<User> Owners { get; set; }


    }
}
