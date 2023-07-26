using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeTaskEscalationService : BusinessService<CommiteeTaskEscalation, CommiteeTaskEscalationDTO>, ICommiteeTaskEscalationService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        MasarContext context;
        public CommiteeTaskEscalationService(IUnitOfWork unitOfWork, MasarContext _context, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
            : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            context = _context;
            _sessionServices = sessionServices;
        }

        //public override DataSourceResult<CommiteeTaskEscalationDTO> GetAll<CommiteeTaskEscalationDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        //{
            

            

        //}
        public DataSourceResult<CommiteeTaskEscalationDTO> GetAllWithCategory(DataSourceRequest dataSourceRequest)
        {
           
            var query = this._UnitOfWork.GetRepository<CommiteeTaskEscalation>().GetAll(false).Select(p => new CommiteeTaskEscalationDTO
            {
                NewMainAssinedUserId = p.NewMainAssinedUserId,
                ComiteeTaskCategoryId = p.ComiteeTaskCategoryId,
                CommiteeTaskEscalationIndex = p.CommiteeTaskEscalationIndex,
                DelayPeriod = p.DelayPeriod,
                MainAssinedUserId = p.MainAssinedUserId,
                ComiteeTaskCategorycategoryNameAr = p.ComiteeTaskCategory.categoryNameAr,
                ComiteeTaskCategorycategoryNameEn = p.ComiteeTaskCategory.categoryNameEn,
                MainAssinedUserFullNameAr=p.MainAssinedUser.FullNameAr,
                MainAssinedUserFullNameEn=p.MainAssinedUser.FullNameEn,
                NewMainAssinedUserFullNameAr=p.NewMainAssinedUser.FullNameAr,
                NewMainAssinedUserFullNameEn=p.NewMainAssinedUser.FullNameEn


            }).ToList();

            return query.Cast<CommiteeTaskEscalationDTO>().AsQueryable().ToDataSourceResult(dataSourceRequest);
        }
        public override IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            var DeletedRecords = this._UnitOfWork.GetRepository<CommiteeTaskEscalation>().GetAll().Where(x => Ids.Contains(x.CommiteeTaskEscalationIndex)).ToList();
            context.RemoveRange(DeletedRecords);
            context.SaveChanges();
            return DeletedRecords.Count() == Ids.Count() ? DeletedRecords : null;
        }

        public override CommiteeTaskEscalationDTO GetDetails(object Id, bool WithTracking = true)
        {
            if (Id == null) return null;

            //var Mapping = _Mapper.ConfigurationProvider.FindTypeMapFor(typeof(TDbEntity), typeof(TDetailsDTO));
            //if (Mapping == null)
            //{
            //    Mapping = _Mapper.ConfigurationProvider.ResolveTypeMap(typeof(TDbEntity), typeof(TDetailsDTO));
            //}

            //var EntityObject = this._UnitOfWork.GetRepository<TDbEntity>().GetById(Id, WithTracking);
            //if (typeof(TDbEntity) == typeof(TDetailsDTO))
            //    return EntityObject as TDetailsDTO;
            //else
            //    return _Mapper.Map(EntityObject, typeof(TDbEntity), typeof(TDetailsDTO)) as TDetailsDTO;

            return _Mapper.Map<CommiteeTaskEscalationDTO>(_UnitOfWork.GetRepository<CommiteeTaskEscalation>().GetAll().Where(x => x.CommiteeTaskEscalationIndex == Convert.ToInt32(Id)).FirstOrDefault());
        }

        public override IEnumerable<CommiteeTaskEscalationDTO> Update(IEnumerable<CommiteeTaskEscalationDTO> Entities)
        {
            int RecordsUpdated = 0;
            foreach (var Entity in Entities)
            {
                //To Copy Data not Sent From and To UI
                //var PrimaryKeysValues = this._UnitOfWork.GetRepository<TDbEntity>().GetKey<TDbEntity>(_Mapper.Map(Entity, typeof(TDetailsDTO), typeof(TDbEntity)) as TDbEntity);
                //var OldEntity = this._UnitOfWork.GetRepository<TDbEntity>().Find(PrimaryKeysValues);
                //object MappedEntity = _Mapper.Map(Entity, OldEntity, typeof(TDetailsDTO), typeof(TDbEntity));
                //this._UnitOfWork.GetRepository<TDbEntity>().Update(MappedEntity as TDbEntity);
                //RecordsUpdated++;
                var updated=context.CommiteeTaskEscalation.Where(x => x.CommiteeTaskEscalationIndex == Entity.CommiteeTaskEscalationIndex).FirstOrDefault();
                updated.NewMainAssinedUserId = Entity.NewMainAssinedUserId;
                updated.MainAssinedUserId=Entity.MainAssinedUserId;
                updated.DelayPeriod = Entity.DelayPeriod;
                updated.ComiteeTaskCategoryId= Entity.ComiteeTaskCategoryId;
                context.Update(updated);
                context.SaveChanges();

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

       
        


    }
}
