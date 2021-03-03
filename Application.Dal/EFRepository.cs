using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dal.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly ApplicationContext context;

        public EfRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await Add(entity);
            }
        }

        public async Task Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await Update(entity);
            }
        }

        public async Task Delete(string id)
        {
            if (id == null) throw new ArgumentNullException("id is null");
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                throw new ArgumentNullException("entity is null");
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();


        }

        public async Task Delete(TEntity entity)
        {
            if (entity == null) return;
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await Delete(entity);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();

        }

        public async Task<TEntity> Get(string id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }



        public async Task Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

    }
}