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
        private readonly ApplicationContext _context;
        private readonly string _emptyGuid = Guid.Empty.ToString();

        public EfRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(CheckAndCreateGuid(entity));
            _context.SaveChanges();
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
            var entity = _context.Set<TEntity>().Find(id);
            if (entity == null)
                throw new ArgumentNullException("entity is null");
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null) return;
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
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
            return _context.Set<TEntity>().ToList();

        }

        public TEntity Get(string id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        private TEntity CheckAndCreateGuid(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id) || entity.Id == _emptyGuid)
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            return entity;
        }
    }
}