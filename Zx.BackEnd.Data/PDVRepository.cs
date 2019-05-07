using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Zx.BackEnd.Data.Context;
using Zx.BackEnd.Data.Entity;
using Zx.BackEnd.Data.Interface;

namespace Zx.BackEnd.Data
{
    public class PDVRepository : IDisposable, IRepository<PontoDeVenda> 
    {
        /// <summary>
        /// DbContext
        /// </summary>
        public PontoDeVendaContext context { get; private set; }
        /// <summary>
        /// DbSet
        /// </summary>
        public DbSet<PontoDeVenda> dbSet { get; private set; }

        /// <summary>
        /// Primary Key Name
        /// </summary>
        public string PrimaryKeyName { get; private set; }

        internal IEntityType _entityType;
        internal IEnumerable<IProperty> _properties;
        internal IModel _model;

        public PDVRepository(PontoDeVendaContext dbContext)
        {
            context = dbContext;
            dbSet = context.Set<PontoDeVenda>();

            _model = context.Model;
            _entityType = _model.FindEntityType(typeof(PontoDeVenda));
            _properties = _entityType.GetProperties();
            PrimaryKeyName = _entityType.FindPrimaryKey().Properties.First().Name;
        }

      
        public async void Add(PontoDeVenda entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

     
        public async void AddRange(IEnumerable<PontoDeVenda> entity)
        {
            await dbSet.AddRangeAsync(entity);
            await context.SaveChangesAsync();
        }

 
        public IQueryable<PontoDeVenda> Get()
        {
            return dbSet.AsNoTracking();
        }

        public PontoDeVenda GetById(long id)
        {
            return dbSet.FirstOrDefault(e => e.Id == id);
        }

        public PontoDeVenda GetByCNPJ(string cnpj)
        {
            return dbSet.AsNoTracking().FirstOrDefault(e => e.Document == cnpj);
        }

        public async void Delete(PontoDeVenda entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

    
        public async void DeleteById(long id)
        {
            var entity = GetById(id);
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async void DeleteRange(Expression<Func<PontoDeVenda, bool>> filter)
        {
            IQueryable<PontoDeVenda> query = dbSet;
            IQueryable<PontoDeVenda> deleteItens = query.Where(filter);
            dbSet.RemoveRange(deleteItens);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete Elements
        /// </summary>
        /// <param name="entity"></param>
        public async void DeleteRange(IEnumerable<PontoDeVenda> entity)
        {
            dbSet.RemoveRange(entity);
            await context.SaveChangesAsync();
        }

        public async void Update(PontoDeVenda entity)
        {
            //pegar o valor da pk do objeto
            var entry = context.Entry(entity);
            var pkey = entity.GetType().GetProperty(PrimaryKeyName)?.GetValue(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = context.Set<PontoDeVenda>();
                PontoDeVenda attachedEntity = set.Find(pkey);  
                if (attachedEntity != null)
                {
                    var attachedEntry = context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; 
                }
            }

            await context.SaveChangesAsync();
        }

       
        public async void UpdateById(PontoDeVenda entity, long id)
        {
            PontoDeVenda attachedEntity = GetById(id);  
            if (attachedEntity != null)
            {
                var attachedEntry = context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
            }
        }

        public IQueryable<PontoDeVenda> Find(Expression<Func<PontoDeVenda, bool>> filter = null, Func<IQueryable<PontoDeVenda>, IOrderedQueryable<PontoDeVenda>> orderBy = null)
        {
            IQueryable<PontoDeVenda> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return orderBy != null ? orderBy(query) : query;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            dbSet = null;
            context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
