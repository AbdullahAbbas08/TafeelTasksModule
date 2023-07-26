using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using IHelperServices.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class UpdateTaskLogService : IUpdateTaskLogService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        IMapper _Mapper;
        public UpdateTaskLogService(IUnitOfWork unitOfWork, IMapper mapper, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)

        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _Mapper = mapper;
        }

        public List<UpdateTaskLogDTO> GetTaskUpdateslog(int CommiteeTaskId)
        {

            var logMain = _unitOfWork.GetRepository<UpdateTaskLogMainAssignedUser>().GetAll().Where(x => x.CommiteeTaskId == CommiteeTaskId).OrderByDescending(x => x.CreatedOn).ToList();
            var result = _Mapper.Map<List<UpdateTaskLogDTO>>(logMain);


            var logAssistant = _unitOfWork.GetRepository<UpdateTaskLogAssistantUser>().GetAll().Where(x => x.CommiteeTaskId == CommiteeTaskId).OrderByDescending(x => x.CreatedOn).ToList();
            result.AddRange(_Mapper.Map<List<UpdateTaskLogDTO>>(logAssistant));
            return result.OrderByDescending(x => x.CancelDate).ToList();


        }

    }
}
