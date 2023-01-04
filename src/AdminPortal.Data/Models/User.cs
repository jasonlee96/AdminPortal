namespace AdminPortal.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(User))]
    public class User
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(128)]
        public virtual string UserName { get; set; }

        [StringLength(128)]
        public virtual string FirstName { get; set; }

        [StringLength(128)]
        public virtual string LastName { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 128)]
        public virtual string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public virtual string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual Status Status { get; set; }

        public virtual DateTime LastLoginDate { get; set; }
        public virtual string LastLoginIP { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual string NormalizedEmail { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual string NormalizedUserName { get; private set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
