using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void Delete(string id)
        {
            if (id == null) throw new ArgumentNullException("id is null");
            var entity = context.Set<TEntity>().Find(id);
            if (entity == null)
                throw new ArgumentNullException("entity is null");
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null) return;
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();

        }

        public TEntity Get(string id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

    }
}