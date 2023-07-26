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
    public class ProjectService : BusinessService<Project, ProjectDTO>, IProjectService
    {
        private IUnitOfWork _unitOfWork;
        private IHelperServices.ISessionServices _sessionServices;
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }

        public List<LookUpDTO> GetListOfProjectsLookup(int take, int skip, string searchText)
        {
            if (searchText == "" || string.IsNullOrEmpty(searchText) || searchText == null)
            {
                return _unitOfWork.GetRepository<Project>().GetAll().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.ProjectNameAr : x.ProjectNameEn
                }).Take(take).Skip(skip).ToList();
            }
            else
            {
                return _unitOfWork.GetRepository<Project>().GetAll()
                    .Where(x => x.ProjectNameAr.Contains(searchText) || x.ProjectNameEn.Contains(searchText))
                    .Select(x => new LookUpDTO
                    {
                        Id = x.Id,
                        Name = _sessionServices.CultureIsArabic ? x.ProjectNameAr : x.ProjectNameEn
                    }).ToList();
            }

        }
    }
}
