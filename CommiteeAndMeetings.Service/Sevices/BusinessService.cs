using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.Service.ISevices;
//using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using HelperServices;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommiteeAndMeetings.Services.Sevices
{
    public abstract class BusinessService : IBusinessService
    {

        protected readonly IUnitOfWork _UnitOfWork;
        protected readonly IMapper _Mapper;
        protected readonly bool IsAuditEnabled;
        protected readonly IOptions<AppSettings> _appSettingsOption;

        public ISessionServices SessionServices
        {
            get { return SessionServices; }
        }
        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettingsOption)
        {
            _UnitOfWork = unitOfWork;
            _Mapper = mapper;
            _appSettingsOption = appSettingsOption;
            IsAuditEnabled = _appSettingsOption.Value.AuditSettings.IsEnabled == 1 ? true : false;
        }

    }

    public abstract class BusinessService<TDbEntity, TDetailsDTO> : BusinessService, IBusinessService<TDbEntity, TDetailsDTO>
             where TDbEntity : class
             where TDetailsDTO : class

    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        protected IStringLocalizer _stringLocalizer;
        private IOptions<AppSettings> appSettings;

        private readonly IHelperServices.ISessionServices _SessionServices;

        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper, Microsoft.Extensions.Localization.IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _SessionServices = sessionServices;
        }







        //protected BusinessService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISessionServices sessionServices, IOptions<AppSettings> appSettings)
        //{
        //    this.unitOfWork = unitOfWork;
        //    this.mapper = mapper;
        //    this.stringLocalizer = stringLocalizer;
        //    this.sessionServices = sessionServices;
        //    this.appSettings = appSettings;
        //}

        public virtual DataSourceResult<T> GetAll<T>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = this._UnitOfWork.GetRepository<TDbEntity>().GetAll(WithTracking);
            // Global Query Filter Added
            //if (typeof(IAuditableDelete).IsAssignableFrom(typeof(TDbEntity)))
            //{
            //    query = query.Where(x => !((IAuditableDelete)x).DeletedOn.HasValue);
            //}
            if (typeof(TDbEntity) == typeof(T))
                return query.Cast<T>().ToDataSourceResult(dataSourceRequest);
            else
                return query.ProjectTo<T>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
        }

        public virtual TDetailsDTO GetDetails(object Id, bool WithTracking = true)
        {
            if (Id == null) return null;

            var Mapping = _Mapper.ConfigurationProvider.FindTypeMapFor(typeof(TDbEntity), typeof(TDetailsDTO));
            if (Mapping == null)
            {
                Mapping = _Mapper.ConfigurationProvider.ResolveTypeMap(typeof(TDbEntity), typeof(TDetailsDTO));
            }

            var EntityObject = this._UnitOfWork.GetRepository<TDbEntity>().GetById(Id, WithTracking);
            if (typeof(TDbEntity) == typeof(TDetailsDTO))
                return EntityObject as TDetailsDTO;
            else
                return _Mapper.Map(EntityObject, typeof(TDbEntity), typeof(TDetailsDTO)) as TDetailsDTO;
        }

        public virtual IEnumerable<TDetailsDTO> Insert(IEnumerable<TDetailsDTO> entities)
        {
            var Mapping = _Mapper.ConfigurationProvider.FindTypeMapFor(typeof(TDbEntity), typeof(TDetailsDTO));
            if (Mapping == null)
            {
                Mapping = _Mapper.ConfigurationProvider.ResolveTypeMap(typeof(TDbEntity), typeof(TDetailsDTO));
            }
            var TDbEntities = entities.AsQueryable().ProjectTo<TDbEntity>(_Mapper.ConfigurationProvider, _SessionServices).ToList();
            var ToBereturned = this._UnitOfWork.GetRepository<TDbEntity>().Insert(TDbEntities);

            return _Mapper.Map(ToBereturned, typeof(IEnumerable<TDbEntity>), typeof(IEnumerable<TDetailsDTO>)) as IEnumerable<TDetailsDTO>;
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


        //public virtual TDetailsDTO InsertSingleRecord(TDetailsDTO entity)
        //{
        //    var TDbEntity = entity;
        //    var ToBereturned = this._UnitOfWork.Repository<TDbEntity>().Insert(TDbEntity as TDbEntity);
        //    return _Mapper.Map(ToBereturned, typeof(TDetailsDTO), typeof(TDetailsDTO)) as TDetailsDTO;
        //}

        public virtual IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            var DeletedRecords = this._UnitOfWork.GetRepository<TDbEntity>().Delete(Ids);
            #region commented
            //int RecordDeleted;
            //try
            //{
            //    RecordDeleted = this._UnitOfWork.Save(true);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
            return DeletedRecords.Count() == Ids.Count() ? DeletedRecords : null;
        }

        public virtual IEnumerable<TDetailsDTO> Update(IEnumerable<TDetailsDTO> Entities)
        {
            int RecordsUpdated = 0;
            foreach (var Entity in Entities)
            {
                //To Copy Data not Sent From and To UI
                var PrimaryKeysValues = this._UnitOfWork.GetRepository<TDbEntity>().GetKey<TDbEntity>(_Mapper.Map(Entity, typeof(TDetailsDTO), typeof(TDbEntity)) as TDbEntity);
                var OldEntity = this._UnitOfWork.GetRepository<TDbEntity>().Find(PrimaryKeysValues);
                object MappedEntity = _Mapper.Map(Entity, OldEntity, typeof(TDetailsDTO), typeof(TDbEntity));
                this._UnitOfWork.GetRepository<TDbEntity>().Update(MappedEntity as TDbEntity);
                RecordsUpdated++;
            }
            #region commented
            //try
            //{
            //    RecordsUpdated = this._UnitOfWork.Save(true);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
            return RecordsUpdated == Entities.Count() ? Entities : null;
        }

        public bool CheckIfExist(CheckUniqueDTO checkUniqueDTO)
        {
            return this._UnitOfWork.IsExisted(checkUniqueDTO);
        }


    }
}
