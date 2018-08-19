using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SLib.DomainModel.Auditing
{
    public class Log_DeletedEntityEfConfig : IEntityTypeConfiguration<Log_DeletedEntity>
    {
        public void Configure(EntityTypeBuilder<Log_DeletedEntity> builder)
        {
            builder.HasKey(o => o.Id);

            // configure the ChangeTimestamp property for use in optimistic concurrency checks
            builder.Property(o => o.ChangeTimestamp).IsRowVersion();

            builder.Property(o => o.EntityType).HasMaxLength( 255 );
            builder.Property(o => o.DeletedBy).HasMaxLength( 100 );
            builder.Property(o => o.DeletedByFullName).HasMaxLength( 100 );
        }
    }
}
