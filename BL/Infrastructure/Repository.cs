﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BL.Infrastructure
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private AppDBContext _ctx;
        private DbSet<T> _set;

        public Repository(AppDBContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
          
        }

        public virtual void Add(T entity)
        {
            _set.Add(entity);
            
        }

        public virtual void Delete(params object[] id)
        {
            var entity = _set.Find(id);
            _set.Remove(entity);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _set.Where(where).AsNoTracking().FirstOrDefault();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _set.AsNoTracking();
        }

       

        public virtual T GetById(params object[] id)
        {
            var entity = _set.Find(id);
            if(entity!=null)
             _ctx.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            var result = _set.Where(where).AsNoTracking();
            return result;
        }


        public virtual void Update(T entity)
        {
            _set.Update(entity);
        }

        public virtual void NoTracking(T entity)
        {
            _ctx.Entry(entity).State = EntityState.Detached;
        
            
        }

       


    }
}
