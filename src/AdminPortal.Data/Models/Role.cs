﻿namespace AdminPortal.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Role))]
    public class Role
    {
        [Key, Required]
        public virtual int RoleId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
