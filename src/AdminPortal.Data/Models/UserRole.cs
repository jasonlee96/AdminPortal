namespace AdminPortal.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;

    [Table(nameof(UserRole))]
    public class UserRole
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required, ForeignKey(nameof(User))]
        public virtual int UserId { get; set; }

        [Required, ForeignKey(nameof(Role))]
        public virtual int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual User User { get; set; }
    }
}
