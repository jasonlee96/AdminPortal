namespace AdminPortal.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Role))]
    [Index(nameof(Code), IsUnique = true)]
    public class Role
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(15)]
        public virtual string Code { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual Status Status { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public virtual RoleGroup RoleGroup { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
