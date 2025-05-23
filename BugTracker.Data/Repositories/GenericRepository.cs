﻿using BugTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Data.Repositories
{
    public abstract class GenericRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context) => Context = context;

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> Delete(int id)
        {
            var entity = await Context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> GetObjectById(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }
        
        public virtual async Task<TEntity> GetObjectById(string id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllObjects()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> Update(TEntity item)
        {
            Context.Entry(item).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return item;
        }
    }
}