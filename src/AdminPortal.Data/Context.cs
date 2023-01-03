using AdminPortal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Data
{

    /// <summary>
    /// Data access context for accessing data entities.
    /// </summary>
    public class Context : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions{Context}"/> containing connection information.</param>
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the collection that contains <see cref="Role"/> entities.
        /// </summary>
        public virtual DbSet<Role> Roles
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection that contains <see cref="User"/> entities.
        /// </summary>
        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection that contains <see cref="UserRole"/> entities.
        /// </summary>
        public virtual DbSet<UserRole> UserRoles
        {
            get;
            set;
        }

        public virtual DbSet<Merchant> Merchants
        {
            get;
            set;
        }
        public virtual DbSet<MerchantOwner> MerchantOwners
        {
            get;
            set;
        }
        public virtual DbSet<Product> Products
        {
            get;
            set;
        }
        public virtual DbSet<ProductCategory> ProductCategories
        {
            get;
            set;
        }
        public virtual DbSet<ProductVariation> ProductVariations
        {
            get;
            set;
        }
    }
}