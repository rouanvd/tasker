using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SLib.DomainModel.Auditing
{
    public static class EntityAuditing
    {
        /* The OnSavingChanges() method is wired up to execute for the DbContext.SaveChanges() method
         * which is executed just before EF performs the actual save.  We can access the ChangeTracker and query it
         * for entries that implement the IAuditEntity interface, which will be added or updated.
         * 
         * For each IAuditEntity entity, we set the standard audit fields.
         * 
         * Note that the loggedInUser is also passed in here so we know who the currently logged in user is
         * so we can log that as part of the standard audit properties.
         */
        public static void OnSavingChanges(ChangeTracker changeTracker, IUserProfile loggedInUser)
        {
            // we might not always have a loggedInUser to perform standard entity audit logging;
            // the user might not be logged in so there is no loggedInUser yet.
            // So if loggedInUser is null, we do not try to perform standard entity audit logging.
            if (loggedInUser == null)
                return;

            AuditAddedAndUpdatedEntities( changeTracker, loggedInUser );
            AuditDeletedEntities( changeTracker, loggedInUser );
        }


        static void AuditAddedAndUpdatedEntities(ChangeTracker changeTracker, IUserProfile loggedInUser)
        {
            List<EntityEntry<IAddUpdateAuditable>> changes = 
                changeTracker.Entries<IAddUpdateAuditable>()
                             .Where( e => e.State == EntityState.Added | e.State == EntityState.Modified )
                             .ToList();

            foreach (EntityEntry<IAddUpdateAuditable> entry in changes)
                SetStandardAuditFieldValues( entry.Entity, loggedInUser, DateTime.Now );
        }


        static void AuditDeletedEntities(ChangeTracker changeTracker, IUserProfile loggedInUser)
        {
            List<EntityEntry<IDeleteAuditable>> changes = 
                changeTracker.Entries<IDeleteAuditable>()
                             .Where( e => e.State == EntityState.Deleted )
                             .ToList();

            DateTime now = DateTime.Now;

            foreach (EntityEntry<IDeleteAuditable> stateEntry in changes)
            {
                var entity = stateEntry.Entity;

                var logEntry = new Log_DeletedEntity();
                logEntry.EntityType        = entity.GetType().FullName;
                logEntry.EntityInfo        = entity.GetExtraInfoForDeleteAuditing();
                logEntry.DeletedBy         = loggedInUser.Username;
                logEntry.DeletedByFullName = loggedInUser.FullName;
                logEntry.DeletedOn         = now;

                changeTracker.Context.Add( logEntry );
            }
        }


        static void SetStandardAuditFieldValues(IAddUpdateAuditable entity, IUserProfile loggedInUser, DateTime now)
        {
            if (entity.CreatedBy == null)
            {
                entity.CreatedBy         = loggedInUser.Username;
                entity.CreatedByFullName = loggedInUser.FullName;
            }

            if (! entity.CreatedOn.HasValue)
                entity.CreatedOn = now;

            entity.UpdatedBy         = loggedInUser.Username;
            entity.UpdatedByFullName = loggedInUser.FullName;
            entity.UpdatedOn         = now;
        }
    }
}
