using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class MeetingHeaderAndFooterService : BusinessService<MeetingHeaderAndFooter, MeetingHeaderAndFooterDTO>, IMeetingHeaderAndFooterService
    {
        private IUnitOfWork _unitOfWork;
        public MeetingHeaderAndFooterService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
        }
        public override IEnumerable<MeetingHeaderAndFooterDTO> Update(IEnumerable<MeetingHeaderAndFooterDTO> Entities)
        {
            foreach (var item in Entities)
            {
                var Meeting_HeaderAndFooter = _unitOfWork.GetRepository<Meeting_Meeting_HeaderAndFooter>().GetAll().Where(x => x.HeaderAndFooterId == item.Id).ToList();
                if (Meeting_HeaderAndFooter.Count() != 0)
                {
                    _unitOfWork.GetRepository<Meeting_Meeting_HeaderAndFooter>().Delete(Meeting_HeaderAndFooter);
                    _unitOfWork.SaveChanges();
                }
            }
            return base.Update(Entities);
        }
        public List<LookUpDTO> GetListOfMeetingLookup(int take, int skip, string searchText)
        {
            if (searchText == "" || string.IsNullOrEmpty(searchText) || searchText == null)
            {
                return _unitOfWork.GetRepository<Meeting>().GetAll()
                .Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = x.Title
                }).Take(take).Skip(skip).ToList();
            }
            else
            {
                return _unitOfWork.GetRepository<Meeting>().GetAll()
                   .Where(x => x.Title.Contains(searchText))
                   .Select(x => new LookUpDTO
                   {
                       Id = x.Id,
                       Name = x.Title
                   }).Take(take).Skip(skip).ToList();
            }

        }
    }
}