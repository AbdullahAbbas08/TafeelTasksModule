using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class TaskCommentService : BusinessService<TaskComment, TaskCommentDTO>, ITaskCommentService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;
        public TaskCommentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
        }
        public override IEnumerable<TaskCommentDTO> Insert(IEnumerable<TaskCommentDTO> entities)
        {
            var loc = _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewCommentNotificationText");
            var result = base.Insert(entities);
            foreach (var item in result)
            {
                var Role = _unitOfWork.GetRepository<CommiteeUsersRole>().Find(item.CreatedByRoleId);
                if (Role != null)
                {
                    item.CreatedByRole = new CommiteeDetailsUsersRoleDTO
                    {
                        CommiteeUsersRoleId = (int)item.CreatedByRoleId,
                        Role = new CommiteeDetailsRoleDTO
                        {
                            CommiteeRoleId = (int)item.CreatedByRoleId,
                            CommiteeRolesNameAr = Role?.Role.CommiteeRolesNameAr,
                            CommiteeRolesNameEn = Role?.Role.CommiteeRolesNameEn,

                        }
                    };
                }
                //item.CreatedByRole = new CommiteeDetailsUsersRoleDTO
                //{
                //    CommiteeUsersRoleId = (int)item.CreatedByRoleId,
                //    Role = new CommiteeDetailsRoleDTO
                //    {
                //        CommiteeRoleId = (int)item.CreatedByRoleId,
                //        CommiteeRolesNameAr = Role?.Role.CommiteeRolesNameAr,
                //        CommiteeRolesNameEn = Role?.Role.CommiteeRolesNameEn,

                //    }
                //};
                var Task = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().FirstOrDefault(x => x.CommiteeTaskId == item.TaskId);
                List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO>();

                if (Task.IsShared)
                {
                    var committemebers = _UnitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == Task.CommiteeId).ToList();
                    if (Task.MainAssinedUserId != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification2 = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)Task.MainAssinedUserId,
                            TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                            TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                            CommentId = result.FirstOrDefault(x => x.TaskId == item.TaskId).CommentId,
                            CommiteeId = Task.CommiteeId
                        };
                        committeeNotifications.Add(committeeNotification2);
                    }
                    foreach (var item2 in committemebers)
                    {
                        if (item2.UserId != _sessionServices.UserId)
                        {
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = (int)item2.UserId,
                                TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                                TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                                CommentId = result.FirstOrDefault(x => x.TaskId == item.TaskId).CommentId,
                                CommiteeId = item2.CommiteeId
                            };
                            committeeNotifications.Add(committeeNotification);
                        }
                    }
                    if (Task.CreatedBy != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification3 = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)Task.CreatedBy,
                            TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                            TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                            CommentId = result.FirstOrDefault(x => x.TaskId == item.TaskId).CommentId,
                            CommiteeId = Task.CommiteeId
                        };
                        committeeNotifications.Add(committeeNotification3);
                    }
                    _committeeNotificationService.Insert(committeeNotifications);

                }
                else
                {
                    if (Task.MainAssinedUserId != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification2 = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)Task.MainAssinedUserId,
                            TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                            TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                            CommentId = result.FirstOrDefault(x => x.TaskId == item.TaskId).CommentId,
                            CommiteeId = Task.CommiteeId,
                            CommiteeTaskId = Task.CommiteeTaskId
                        };
                        committeeNotifications.Add(committeeNotification2);
                    }
                    foreach (var item2 in Task.AssistantUsers)
                    {
                        if (item2.UserId != _sessionServices.UserId)
                        {
                            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                            {
                                IsRead = false,
                                UserId = (int)item2.UserId,
                                TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                                TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                                CommentId = item.CommentId,
                                CommiteeId = Task.CommiteeId,
                                CommiteeTaskId= Task.CommiteeTaskId
                            };
                            committeeNotifications.Add(committeeNotification);
                        }
                    }
                    if (Task.CreatedBy != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification3 = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)Task.CreatedBy,
                            TextAR = loc.CommiteeLocalizationAr + " " + item.Comment.Text,
                            TextEn = loc.CommiteeLocalizationEn + " " + item.Comment.Text,
                            CommentId = result.FirstOrDefault(x => x.TaskId == item.TaskId).CommentId,
                            CommiteeId = Task.CommiteeId,
                            CommiteeTaskId = Task.CommiteeTaskId
                        };
                        committeeNotifications.Add(committeeNotification3);
                    }
                    _committeeNotificationService.Insert(committeeNotifications);
                }
            }
            return result;
        }
    }
}