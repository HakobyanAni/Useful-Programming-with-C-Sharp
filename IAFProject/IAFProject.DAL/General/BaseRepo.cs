using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IAFProject.DAL.General
{
    public class BaseRepo<TEntity> : IDisposable, IBaseRepo<TEntity> where TEntity : class, IBaseEntity
    {
        private DbSet<TEntity> _table;
        protected readonly DbContext _context;

        public BaseRepo(DbContext context = null)
        {
            _context = context ?? throw new NullReferenceException();
            _table = _context.Set<TEntity>();
        }

        public async Task<int> Add(TEntity entity)
        {
            await _table.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<int> AddRange(List<TEntity> entities)
        {
            await _table.AddRangeAsync(entities);
            return await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            int result;
            try
            {
                _context.ChangeTracker.DetectChanges();
                if (!_context.ChangeTracker.HasChanges())
                {
                    return 0;
                }
                result = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                string ex = "";
                foreach (var entry in exception.Entries)
                {
                    int id = (entry.Entity as IBaseEntity).Id;
                    string exEntity = entry.Entity.GetType().Name;
                    ex = "Concurrency exception \n Entity: " + exEntity + ", Id: " + id;
                }
                throw new Exception(ex);
            }
            catch (DbUpdateException exception)
            {
                string ex = "";
                foreach (var entry in exception.Entries)
                {
                    int id = (entry.Entity as IBaseEntity).Id;
                    string exEntity = entry.Entity.GetType().Name;
                    ex = "Update exception \n Entity: " + exEntity + ", Id: " + id;
                }
                throw new Exception(ex);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.StackTrace);
            }
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
