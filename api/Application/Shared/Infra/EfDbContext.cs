using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SLib.DomainModel;
using SLib.DomainModel.Auditing;

namespace Application.Shared.Infra
{
    public sealed class EfDbContext : DbContext
    {
        readonly string _connectionStr = null;
        readonly IUserProfile _loggedInUser = null;

        /// <summary>
        ///   Create a new DbContext which does not automatically populate audit fields.
        /// </summary>
        public EfDbContext(string connectionStr)
        {
            _connectionStr = connectionStr;
            ChangeTracker.LazyLoadingEnabled = false;
        }


        /// <summary>
        ///   Create a new DbContext which automatically populates audit fields from the supplied loggedInUser.
        /// </summary>
        public EfDbContext(string connectionStr, IUserProfile loggedInUser)
        : this( connectionStr )
        {
            _loggedInUser = loggedInUser;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer( _connectionStr );
        }


        public override int SaveChanges()
        {
            /* Set up automatic setting of standard audit properties for entities that support standard auditing properties.
             * 
             * The loggedInUser is optional so it may be NULL.  If the loggedInUser is a null value, standard
             * entity audit logging will not be performed.
             */
            EntityAuditing.OnSavingChanges( ChangeTracker, _loggedInUser );

            return base.SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* add all the needed EF entity configuration classes that helps Entity Framework correctly
             * map the entities to the database schema.
             */
            modelBuilder.ApplyConfiguration( new Log_DeletedEntityEfConfig() );

            /* By default, EF  pluralizes the table name. In order to turn it off you have to remove
             * the Pluralizing convention from the convention collection of the modelBuilder object.
             */
            RemovePluralizingTableNameConvention( modelBuilder );

            base.OnModelCreating( modelBuilder );
        }


        static void RemovePluralizingTableNameConvention(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }
    }
}
