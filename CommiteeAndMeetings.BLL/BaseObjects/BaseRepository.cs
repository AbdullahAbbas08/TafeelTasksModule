using CommiteeAndMeetings.BLL.BaseObjects.PagedList;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using IHelperServices;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.BaseObjects
{
    /// <summary>
    /// Represents a default generic repository implements the <see cref="IRepository{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly MasarContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        private readonly ISessionServices _SessionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private ISession _session => _httpContextAccessor.HttpContext.Session;
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BaseRepository(MasarContext dbContext, ISessionServices sessionServices, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
            _SessionServices = sessionServices;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Changes the table name. This require the tables in the same database.
        /// </summary>
        /// <param name="table"></param>
        /// <remarks>
        /// This only been used for supporting multiple tables in the same model. This require the tables in the same database.
        /// </remarks>
        //public virtual void ChangeTable(string table)
        //{
        //    if (_dbContext.Model.FindEntityType(typeof(TEntity)).Relational() is RelationalEntityTypeAnnotations relational)
        //    {
        //        relational.TableName = table;
        //    }
        //}


        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        public virtual IQueryable<TEntity> GetAll(bool WithTracking = true)
        {
            if (WithTracking)
                return this._dbSet;
            else
                return this._dbSet.AsNoTracking();
        }
        public virtual TEntity GetById(object Id, bool WithTracking = true)
        {
            if (WithTracking)
                return this._dbSet.FindQuery(Id).FirstOrDefault();
            else
                return this._dbSet.FindQuery(Id).AsNoTracking().FirstOrDefault();
        }
        private void SetProperty(object obj, string property, object value)
        {
            try
            {
                var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                    prop.SetValue(obj, value, null);
            }
            catch { } //property not exist or inserted value doesn't match the property type!
        }
        public IQueryable<TEntity> GetAllWithoutInclude()
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            return query;
        }
        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TResult}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TResult}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                         Expression<Func<TEntity, bool>> predicate = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                         int pageIndex = 0,
                                                         int pageSize = 20,
                                                         bool disableTracking = true)
            where TResult : class
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.Select(selector).ToPagedList(pageIndex, pageSize);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                                    Expression<Func<TEntity, bool>> predicate = null,
                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                    int pageIndex = 0,
                                                                    int pageSize = 20,
                                                                    bool disableTracking = true,
                                                                    CancellationToken cancellationToken = default(CancellationToken))
            where TResult : class
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
            {
                return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
        }

        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                         bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }


        /// <inheritdoc />
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).FirstOrDefault();
            }
            else
            {
                return query.Select(selector).FirstOrDefault();
            }
        }

        /// <inheritdoc />
        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).Select(selector).FirstOrDefaultAsync();
            }
            else
            {
                return await query.Select(selector).FirstOrDefaultAsync();
            }
        }
        public object GetPropertyValue(object car, string propertyName)
        {
            var prop = car.GetType().GetProperties()
               .Single(pi => pi.Name.Contains(propertyName))
               .GetValue(car, null);
            return prop;
        }
        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The found entity or null.</returns>
        //public virtual TEntity Find(params object[] keyValues)
        //{
        //    var query = _dbContext.Set<TEntity>().AsQueryable();
        //    var navigations = _dbContext.Model.FindEntityType(typeof(TEntity))
        //        .GetDerivedTypesInclusive().SelectMany(type => type.GetNavigations()).Distinct();
        //    foreach (var property in navigations)
        //        query = query.Include(property.Name);
        //    var Id = typeof(TEntity).GetProperties().FirstOrDefault(prop => prop.Name == "Id");
        //    TEntity i = null;
        //    foreach (var item in query)
        //    {
        //        var x = (int)Id.GetValue(item);
        //        var y = (int)keyValues[0];
        //        if (x == y)
        //        {
        //            i = item;
        //            return i;
        //        }
        //    }
        //    return i;
        //}

        /// <summary>h
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A <see cref="Task{TEntity}" /> that represents the asynchronous insert operation.</returns>
       // public virtual async Task<TEntity> FindAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{TEntity}"/> that represents the asynchronous find operation. The task result contains the found entity or null.</returns>
        public virtual async Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken) => await _dbSet.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// Gets the count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        //public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
        //{
        //    if (predicate == null)
        //    {
        //        return _dbSet.Count();
        //    }
        //    else
        //    {
        //        return _dbSet.Count(predicate);
        //    }
        //}

        /// <summary>
        /// Inserts a new entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>

        /// <summary>
        /// Bulk Insert and Ordinary Multiple Insert
        /// </summary>
        /// <param name="Entities"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Insert(IEnumerable<TEntity> Entities)
        {
            #region BulkInsert
            //try
            //{
            //    List<TDbEntity> entityList = new List<TDbEntity>();
            //    foreach (var Entity in Entities)
            //    {
            //        if (typeof(IAuditableInsert).IsAssignableFrom(Entity.GetType()))
            //        {
            //            (Entity as IAuditableInsert).CreatedOn = DateTimeOffset.Now;
            //            (Entity as IAuditableInsert).CreatedBy = this._SessionServices.UserId;
            //        }
            //        entityList.Add(Entity);
            //    }
            //    _Context.BulkInsert(entityList, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = false });
            //    //_Context.BulkInsert(entityList, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true });
            //    return entityList;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return returnedData;
            #endregion
            try
            {
                //if (System.IO.File.Exists("e:\\logMasar4.txt"))
                //{
                //    using (StreamWriter sw = System.IO.File.AppendText("e:\\logMasar4.txt"))
                //    {
                //        sw.WriteLine(" start Log ");
                //    }
                //}
                int RecordsInserted;
                for (int i = 0; i < Entities.Count(); i++)
                {
                    //set CreatedOn, CreatedBy properties
                    SetProperty(Entities.ElementAt(i), "CreatedOn", DateTimeOffset.Now);
                    SetProperty(Entities.ElementAt(i), "CreatedBy", _SessionServices?.UserId);
                    var UserRoleId = Convert.ToString(_SessionServices.UserRoleId);
                    if (!string.IsNullOrEmpty(UserRoleId) && UserRoleId != "0")
                    {
                        SetProperty(Entities.ElementAt(i), "CreatedByRoleId", Convert.ToInt32(UserRoleId));
                    }

                    //add entity
                    var ent = this._dbSet.Add(Entities.ElementAt(i));
                    RecordsInserted = _dbContext.SaveChanges();
                    _dbContext.Entry(Entities.ElementAt(i)).Reload();
                }

                //if (System.IO.File.Exists("e:\\logMasar4.txt"))
                //{
                //    using (StreamWriter sw = System.IO.File.AppendText("e:\\logMasar4.txt"))
                //    {
                //        sw.WriteLine("End log");
                //    }
                //}
                //  _dbContext.Entry(Entities.First()).Reload();
                return Entities;

            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("e:\\logMasar4.txt"))
                {
                    using (StreamWriter sw = System.IO.File.AppendText("e:\\logMasar4.txt"))
                    {
                        sw.WriteLine("Exception=" + ex.StackTrace != null ? ex.StackTrace : ex.Message);
                    }
                }
                return Entities;
            }
        }

        /// <summary>
        /// Single Insert
        /// </summary>
        /// <param name="Entities"></param>
        /// <returns></returns>
        public virtual TEntity Insert(TEntity Entity)
        {
            try
            {
                //set CreatedOn, CreatedBy properties
                SetProperty(Entity, "CreatedOn", DateTimeOffset.Now);
                SetProperty(Entity, "CreatedBy", _SessionServices?.UserId);
                var UserRoleId = Convert.ToString(_SessionServices.UserRoleId);
                if (!string.IsNullOrEmpty(UserRoleId) && UserRoleId != "0")
                {
                    SetProperty(Entity, "CreatedByRoleId", Convert.ToInt32(UserRoleId));
                }


                //add entity
                int RecordsInserted;
                this._dbSet.Add(Entity);
                RecordsInserted = this._dbContext.SaveChanges();
                return Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            // Shadow properties?
            //var property = _dbContext.Entry(entity).Property("Created");
            //if (property != null) {
            //property.CurrentValue = DateTime.Now;
            //}
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous insert operation.</returns>
        public virtual Task InsertAsync(params TEntity[] entities) => _dbSet.AddRangeAsync(entities);

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken)) => _dbSet.AddRangeAsync(entities, cancellationToken);
        private object GetPrimaryKey(TEntity entry)
        {
            //    var key = _dbContext.Model.FindEntityType(typeof(TEntity).Name).FindPrimaryKey().Properties.FirstOrDefault();
            var myObject = entry;
            var property =
                myObject.GetType()
                    .GetProperties()
                    .FirstOrDefault(prop => prop.Name == typeof(TEntity).Name + "Id");
            return (int)property.GetValue(myObject, null);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>

        public virtual void Update(TEntity Entity)
        {
            //var DbEntry = this._Context.Attach(_DbSet.Find(this.GetKey(Entity)));
            if (typeof(IAuditableUpdate).IsAssignableFrom(Entity.GetType()))
            {
                (Entity as IAuditableUpdate).UpdatedOn = DateTimeOffset.Now;
                (Entity as IAuditableUpdate).UpdatedBy = this._SessionServices.UserId;
            }
            this._dbSet.Update(Entity);
            //_dbContext.Entry(Entity).State = EntityState.Modified;
            try
            {
                this._dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> Entites)
        {
            if (typeof(IAuditableUpdate).IsAssignableFrom(Entites.GetType()))
            {
                foreach (var Entity in Entites)
                {
                    (Entity as IAuditableUpdate).UpdatedOn = DateTimeOffset.Now;
                    (Entity as IAuditableUpdate).UpdatedBy = this._SessionServices?.UserId;
                }
            }
            this._dbSet.UpdateRange(Entites);
            this._dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void UpdateAsync(TEntity entity)
        {

            _dbSet.Update(entity);

        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(params TEntity[] entities) => _dbSet.UpdateRange(entities);

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            for (int i = 0; i < Ids.Count(); i++)
            {
                var ToBeRemoved = this.GetById(Ids.ElementAt(i));
                //if (typeof(IAuditableDelete).IsAssignableFrom(ToBeRemoved.GetType()))
                //{
                //    (ToBeRemoved as IAuditableDelete).DeletedOn = DateTime.Now;
                //    (ToBeRemoved as IAuditableDelete).DeletedBy = this._SessionServices.UserId;
                //}
                //else
                //{
                this._dbSet.Remove(ToBeRemoved);
                // }
            }
            this._dbContext.SaveChanges();
            return Ids;
        }
        public virtual void Delete(TEntity entity) => _dbSet.Remove(entity);

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public virtual void Delete(object id)
        {
            // using a stub entity to mark for deletion
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Delete(params TEntity[] entities) => _dbSet.RemoveRange(entities);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Delete(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

        public virtual object[] GetKey<T>(T entity)
        {
            var keyNames = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name);
            List<object> result = new List<object>();
            for (int i = 0; i < keyNames.Count(); i++)
            {
                result.Add(entity.GetType().GetProperty(keyNames.ElementAt(i)).GetValue(entity, null));
            }
            return result.ToArray<object>();
        }
        public virtual object[] GetKeyNames<T>(T entity)
        {
            return _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).ToArray();
        }

        public virtual TEntity Find(params object[] Ids)
        {
            return this._dbContext.Find<TEntity>(Ids);
        }

        public virtual async Task<TEntity> FindAsync(params object[] Ids)
        {
            return await this._dbContext.FindAsync<TEntity>(Ids);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this._dbSet.FirstOrDefaultAsync(Predicate, cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>> Predicate)
        {
            return this._dbSet.Count(Predicate);
        }

        public virtual IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> predicate, int size)
        {
            return _dbSet.Where(predicate).Take(size);
        }

    }
}
