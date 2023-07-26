using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class GroupService : BusinessService<Group, GroupDto>, IGroupService
    {
        private readonly IHelperServices.ISessionServices _sessionServices;
        private readonly IUnitOfWork _uow;
        public GroupService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _sessionServices = sessionServices;
            _uow = unitOfWork;
        }

        // override insert

        public Group InsertGroup(GroupDto entity)
        {

            List<GroupUsers> groupUsers = new List<GroupUsers>();

            foreach (var item in entity.GroupUsers)
            {
                groupUsers.Add(new GroupUsers() { /*GroupId = entity.GroupId,*/ UserId = item.UserId });
            }

            var group = new Group();

            //group.GroupId = entity.GroupId;
            group.GroupNameEn = entity.GroupNameEn;
            group.GroupNameAr = entity.GroupNameAr;
            group.CreatedBy = _sessionServices.UserId;
            group.GroupUsers = groupUsers;

            _uow.GetRepository<Group>().Insert(group);



            return group;
        }

        //override update
        public override IEnumerable<GroupDto> Update(IEnumerable<GroupDto> Entities)
        {
            foreach (var item in Entities)
            {
                var group = _UnitOfWork.GetRepository<Group>().GetAll(false).FirstOrDefault(x => x.GroupId == item.GroupId);

                group.GroupNameAr = item.GroupNameAr;
                group.GroupNameEn = item.GroupNameEn;
                group.UpdatedBy = _sessionServices.UserId;

                var groupUsers = _UnitOfWork.GetRepository<GroupUsers>().GetAll().Where(x => x.GroupId == item.GroupId).ToList();

                if (groupUsers.Count() != 0)
                {
                    _UnitOfWork.GetRepository<GroupUsers>().Delete(groupUsers);
                    _UnitOfWork.SaveChanges();
                }
                group.GroupUsers = item.GroupUsers.Select(x => new GroupUsers
                {
                    GroupId = item.GroupId,
                    UserId = x.UserId
                }).ToList();

                _UnitOfWork.GetRepository<Group>().Update(group);
            }
            return Entities;
        }

        //override delete
        public bool DeleteGroupFromCreatedUser(int groupId/*,int userId*/)
        {
            try
            {
                var group = _UnitOfWork.GetRepository<Group>().GetAll()
                        .Where(c => c.GroupId == groupId && (c.CreatedBy == _sessionServices.UserId /*|| c.CreatedBy == userId*/)).FirstOrDefault();

                if (group != null)
                {


                    var groupUsers = _UnitOfWork.GetRepository<GroupUsers>().GetAll().Where(x => x.GroupId == groupId);

                    if (groupUsers.Count() != 0)
                    {
                        _UnitOfWork.GetRepository<GroupUsers>().Delete(groupUsers);
                        _UnitOfWork.SaveChanges();
                    }

                    _UnitOfWork.GetRepository<Group>().Delete(group);
                    _UnitOfWork.SaveChanges();



                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        //Get All Group For Espical User
        public override DataSourceResult<GroupDto> GetAll<GroupDto>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = _UnitOfWork.GetRepository<Group>(false).GetAll(WithTracking).OrderByDescending(x => x.CreatedOn).Where(x =>x.CreatedBy == _sessionServices.UserId);

            return query.ProjectTo<GroupDto>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
           
            //return base.GetAll<T>(dataSourceRequest, WithTracking);
        }


        public DataSourceResult<GroupDto> GetAllForUser(DataSourceRequest dataSourceRequest,int createdUserId , bool WithTracking = true)
        {
            IQueryable<Group> query = this._UnitOfWork.GetRepository<Group>().GetAll(WithTracking)
                                                      .OrderByDescending(x => x.CreatedBy).Where(x =>x.CreatedBy == createdUserId);

            var dta = _Mapper.Map<List<GroupDto>>(query.Skip(dataSourceRequest.Skip).Take(dataSourceRequest.Take).ToList());
            
            DataSourceResult<GroupDto> returned = new DataSourceResult<GroupDto>
            {
                Count = query.Count(),
                Data = dta

            };
            return returned;
            //var group = _UnitOfWork.GetRepository<Group>().GetAll().Where(x => x.CreatedBy == createdUserId).Select(x => new GroupDto
            //{
            //    GroupId=x.GroupId,
            //    CreatedBy = x.CreatedBy,
            //    GroupNameAr = x.GroupNameAr,
            //    GroupNameEn = x.GroupNameEn,

            //    GroupUsers = (ICollection<GroupUsersDto>)x.GroupUsers.Select(y => new GroupUsersDto
            //    {
            //        GroupUsersId=y.GroupUsersId,
            //        GroupId=y.GroupId,
            //        UserId = y.UserId,

            //        userDetailsDTO = new UserDetailsDTO()
            //        {
            //            UserName = y.User.Username,
            //            FullNameAr = y.User.FullNameAr,
            //            FullNameEn= y.User.FullNameEn

            //        }

            //    }),
            //});
          //  return group;
            
            
        }

        public GroupDto GetByGroupId(int groupId)
        {
            var group = _UnitOfWork.GetRepository<Group>().GetAll().Where(x => x.GroupId == groupId).Select(x => new GroupDto
            {
                GroupId = x.GroupId,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                GroupNameAr = x.GroupNameAr,
                GroupNameEn = x.GroupNameEn,
                
                GroupUsers = (ICollection<GroupUsersDto>)x.GroupUsers.Select(y => new GroupUsersDto
                {
                    GroupId=y.GroupId,
                    GroupUsersId = y.GroupUsersId,
                    UserId = y.UserId,
                    userDetailsDTO=new UserDetailsDTO()
                    {
                       UserName=y.User.Username,
                       FullNameAr=y.User.FullNameAr,
                       FullNameEn = y.User.FullNameEn,
                    }

                })
            }).FirstOrDefault();

            return group;
        }

    }
}
    

