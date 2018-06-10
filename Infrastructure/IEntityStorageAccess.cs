using System.Collections.Generic;

namespace Infrastructure
{
    public interface IEntityStorageAccess<TKey, TEntity> where TEntity : Entity<TKey>
    {
        IEnumerable<TEntity> LoadEntities();
        void SaveEntity(TEntity debt);
        bool DeleteEntity(TEntity debt);
        bool EntityWithIdExisis(TKey id);
    }
}