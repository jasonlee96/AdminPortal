using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPortal.Data.Models
{
    [Table(nameof(JwtToken))]
    public class JwtToken
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public virtual int UserId { get; set; }

        [Required]
        public virtual string Token { get; set; }
        public virtual bool IsValid { get; set; }

        public virtual DateTime LastAccessAt { get; set; }
    }
}
