using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(AuditTrail))]
    public class AuditTrail
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual string TableName { get; set; }

        [Required]
        public virtual int Key { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual TrailAction Action { get; set; }

        [StringLength(3000)]
        public virtual string OldRecord { get; set; }
        [StringLength(3000)]
        public virtual string NewRecord { get; set; }
        public virtual string IPAddress { get; set; }
        public virtual int InitiatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime RecordedAt { get; set; }
    }
}
