// Alter with whatever parameters you like
public interface IAuditableEntity
{
    DateTime CreatedOn { get; set; }
}


// Could be altered with an "updatedAt"
public class UpdateAuditableEntititiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IAuditableEntity>> entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);

    }
}


// usage register this as SbSet in your EF core context.
   public class DataEntity : IAuditableEntity
    {
        public int Id { get; set; }
        // Other variable
        public DateTime CreatedOn { get; set; }
    }


//  public DbSet<DataEntity> Immos { get; set; }