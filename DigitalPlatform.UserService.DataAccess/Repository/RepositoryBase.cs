﻿using System.Linq.Expressions;
using DigitalPlatform.UserService.Database;
using DigitalPlatform.UserService.Entity._base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace DigitalPlatform.UserService.DataAccess.Repository
{
    public abstract class RepositoryBase<T, TC> : IRepository<T>
        where T : class, IEntityBase
        where TC : DatabaseContext
    {
        protected readonly TC DataContext;
        protected readonly DbSet<T> Dbset;

        protected RepositoryBase(TC dataContext)
        {
            DataContext = dataContext;
            Dbset = dataContext.Set<T>();
        }

        #region Virtual Method

        public virtual void Add(T entity)
        {
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Dbset.Add(entity);
        }

        public virtual void Add(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            UpdateEntityObject(entity);
        }

        public virtual void Delete(T entity)
        {
            entity.IsDeleted = true;
            UpdateEntityObject(entity);
        }

        public virtual void Delete(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var entities = GetQuery(where).AsEnumerable();
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void HardDelete(T entity)
        {
            Dbset.Remove(entity);
        }

        public virtual void HardDelete(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                HardDelete(entity);
            }
        }

        public virtual void HardDelete(Expression<Func<T, bool>> where)
        {
            var entities = GetQuery(where).AsEnumerable();
            foreach (var entity in entities)
            {
                HardDelete(entity);
            }
        }

        public virtual T GetById(Guid id)
        {
            return Dbset.Find(id);
        }

        public virtual ValueTask<T> GetByIdAsync(Guid id)
        {
            return Dbset.FindAsync(id);
        }

        public IQueryable<T> GetQueryById(Guid id)
        {
            return GetQuery(m => m.Id == id);
        }

        // Example: unitOfWork.ExampleRepository.GetPropertyByRecordId(id, m => m.Id)
        public Task<TResult> GetPropertyByRecordId<TResult>(Guid id, Expression<Func<T, TResult>> selector)
        {
            return GetQueryById(id).Select(selector).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetQuery(bool withDeleted = false)
        {
            var q = Dbset.AsQueryable();
            return withDeleted ? q : q.Where(c => !c.IsDeleted).AsQueryable();
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> where)
        {
            return GetQuery().Where(where);
        }

        //public void BulkAdd(IList<T> entities)
        //{
        //    DataContext.BulkInsert(entities);
        //}

        //public async void BulkAddAsync(IList<T> entity)
        //{
        //    await DataContext.BulkInsertAsync(entity);
        //}

        public T Refresh(T entity)
        {
            DataContext.Entry(entity).State = EntityState.Detached;
            return GetById(entity.Id);
        }

        public int SaveChanges()
        {
            try
            {
                return DataContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message, new List<IUpdateEntry>());
            }
        }

        #endregion

        #region Private Methods

        private void UpdateEntityObject(T entity)
        {
            //Dbset.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
            DataContext.Entry(entity).GetDatabaseValues().ToObject();
        }
        #endregion
    }
}
