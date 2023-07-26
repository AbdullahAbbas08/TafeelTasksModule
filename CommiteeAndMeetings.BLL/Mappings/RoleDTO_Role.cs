using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeDatabase.Models.Domains;
using DbContexts.MasarContext.ProjectionModels;
using Models;
using Models.ProjectionModels;
using System;
using System.Linq;

namespace CommiteeAndMeetings.BLL.Mappings
{
    public class RoleDTO_Role : Profile
    {

        public RoleDTO_Role()
        {
            // _unitOfWork = unitOfWork;
            #region Commitee
            CreateMap<Commitee, CommiteeDTO>().ForMember(x => x.CommiteeIdEncrpt, opt => opt.Ignore());
            CreateMap<CommitteeTaskAttachment, CommitteeTaskAttachmentDTO>();
            CreateMap<CommitteeTaskAttachmentDTO, CommitteeTaskAttachment>();
            CreateMap<CommiteetaskMultiMissionDTO, CommiteeTaskMultiMission>();
            CreateMap<CommiteeTaskMultiMission, CommiteetaskMultiMissionDTO>();
            CreateMap<ComiteeTaskCategoryDTO, ComiteeTaskCategory>();
            CreateMap<ComiteeTaskCategory, ComiteeTaskCategoryDTO>();
            CreateMap<CommiteeTaskMultiMissionUser, UpdateTaskLogAssistantUser>()
                .ForMember(x => x.AssistantUserId, options => options.MapFrom(x => x.UserId))
                .AfterMap((src, dest) => dest.CancelDate = DateTimeOffset.Now);
            CreateMap<CommiteeTaskMultiMissionUserDTO, UpdateTaskLogAssistantUser>()
                .ForMember(x => x.AssistantUserId, options => options.MapFrom(x => x.UserId))
                .AfterMap((src, dest) => dest.CancelDate = DateTimeOffset.Now);
            CreateMap<UpdateTaskLogDTO, UpdateTaskLogAssistantUser>();
            CreateMap<UpdateTaskLogAssistantUser, UpdateTaskLogDTO>();
            CreateMap<UpdateTaskLogMainAssignedUser, UpdateTaskLogDTO>();
            CreateMap<UpdateTaskLogDTO, UpdateTaskLogMainAssignedUser>();
            CreateMap<CommitteeMeetingSystemSetting, CommitteeMeetingSystemSettingDTO>();
            CreateMap<CommitteeMeetingSystemSettingDTO, CommitteeMeetingSystemSetting>();
            CreateMap<ThemeDTO, CommitteeTheme>();
            CreateMap<CommitteeTheme, ThemeDTO>();
            CreateMap<CommiteeTaskEscalation, CommiteeTaskEscalationDTO>();
            CreateMap<CommiteeTaskEscalationDTO, CommiteeTaskEscalation>();
            CreateMap<ComiteeTaskCategory, ComiteeTaskCategoryDTO>();
            CreateMap<ComiteeTaskCategoryDTO, ComiteeTaskCategory>();
            
            





            CreateMap<CommiteeDTO, Commitee>().ForMember(x => x.Members, opt => opt.Ignore()).ForMember(x => x.ValidityPeriod, opt => opt.Ignore());
            CreateMap<Commitee, CommiteeDetailsDTO>();
            CreateMap<CommiteeDetailsDTO, Commitee>();
            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();
            CreateMap<RolePermission, RolePermissionDTO>()
                 .ForMember(d => d.PermissionNameAr, options => options.MapFrom(s => s.Permission.PermissionNameAr))
                 .ForMember(d => d.PermissionNameEn, options => options.MapFrom(s => s.Permission.PermissionNameEn))
                 .ForMember(d => d.PermissionStatus, options => options.MapFrom(s => s.Permission.Enabled));
            CreateMap<RolePermissionDTO, RolePermission>();
            CreateMap<CommiteeUsersRole, CommiteeDetailsUsersRoleDTO>();
            CreateMap<CommiteeDetailsUsersRoleDTO, CommiteeUsersRole>();
            CreateMap<CommiteeDetailsRoleDTO, CommiteeRole>();
            CreateMap<CommiteeRole, CommiteeDetailsRoleDTO>();
            CreateMap<CommiteeAttachmentDTO, CommiteeSavedAttachment>();
            CreateMap<CommiteeSavedAttachment, CommiteeAttachmentDTO>();
            CreateMap<SavedAttachment, SavedAttachmentDTO>();
            CreateMap<SavedAttachmentDTO, SavedAttachment>();
            //AttachmentComment
            CreateMap<AttachmentComment, AttachmentCommentDTO>();
            CreateMap<AttachmentCommentDTO, AttachmentComment>();
            CreateMap<User, UserDetailsDTO>().ReverseMap();
            CreateMap<CommiteeMember, CommiteeMemberDTO>().ForMember(x => x.CommiteeIdEncrypt, opt => opt.Ignore());
            //.ForMember(dest => dest.User, opt => opt.MapFrom<UserCommitteeValueResolver>())
            //.ForMember(d => d.CountOfTasks, opt => opt.MapFrom<UserTaskCountCommitteeValueResolver>())
            //.ForMember(d => d.CountOfAttachments, opt => opt.MapFrom<UserAttachmentCountCommitteeValueResolver>())
            ;
            CreateMap<CommiteeMemberDTO, CommiteeMember>().ForMember(d => d.MemberState, options => options.MapFrom(s => s.MemberState));

            CreateMap<Group, GroupDto>();
            CreateMap<GroupDto, Group>();
            CreateMap<GroupUsers, GroupUsersDto>();
            CreateMap<GroupUsersDto, GroupUsers>();
            CreateMap<TaskGroups, TaskGroupDto>();
            CreateMap<TaskGroupDto, TaskGroups>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            //Commitee_CommiteePermissionDTO. 
            CreateMap<CommiteeUsersPermission, Commitee_CommiteePermissionDTO>();
            CreateMap<Commitee_CommiteePermissionDTO, CommiteeUsersPermission>();
            CreateMap<CurrentStatusReason, CurrentStatusReasonDTO>();
            CreateMap<CurrentStatusReasonDTO, CurrentStatusReason>();
            CreateMap<Organization, OrganizationSummaryDTO>();
            CreateMap<OrganizationSummaryDTO, Organization>();
            CreateMap<Survey, SurveyDTO>()
                 .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom<UserValueResolver>());

            //  .ForMember(d => d.CreatedByUser, options => options.MapFrom(s => _unitOfWork.GetRepository<User>(false).GetAll(false).Select(x => new UserDetailsDTO
            //{
            //    UserName = x.Username
            //}).Where(c => c.UserId == s.CreatedBy).FirstOrDefault()));
            CreateMap<SurveyDTO, Survey>();

            CreateMap<SurveyAttachment, SurveyAttachmentDTO>();
            CreateMap<SurveyAttachmentDTO, SurveyAttachment>();
            CreateMap<SurveyComment, SurveyCommentDTO>();
            CreateMap<SurveyCommentDTO, SurveyComment>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<SurveyAnswer, SurveyAnswerDTO>();
            CreateMap<SurveyAnswerDTO, SurveyAnswer>();
            CreateMap<SurveyAnswerUser, SurveyAnswerUserDTO>();
            CreateMap<SurveyAnswerUserDTO, SurveyAnswerUser>();
            CreateMap<CommiteeTask, CommiteeTaskDTO>().ForMember(x => x.CommiteeIdEncrypted, opt => opt.Ignore());

            CreateMap<CommiteeTaskDTO, CommiteeTask>();
            CreateMap<TaskComment, TaskCommentDTO>();
            CreateMap<TaskCommentDTO, TaskComment>();
                
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();
            CreateMap<ValidityPeriod, ValidityPeriodDTO>();
            CreateMap<ValidityPeriodDTO, ValidityPeriod>();
            CreateMap<UserTaskDTO, UserTask>();
            CreateMap<UserTask, UserTaskDTO>();
            CreateMap<CommiteeTaskMultiMissionUserDTO, CommiteeTaskMultiMissionUser>();
            CreateMap<CommiteeTaskMultiMissionUser, CommiteeTaskMultiMissionUserDTO>();
            CreateMap<AttachmentUser, AttachmentUserDTO>();
            CreateMap<AttachmentUserDTO, AttachmentUser>();
            CreateMap<SurveyUser, SurveyUserDTO>();
            CreateMap<SurveyUserDTO, SurveyUser>();
            CreateMap<CommiteeUsersRole, CommiteeUsersRoleDTO>().ForMember(x => x.CommiteeIdEncrypt, opt => opt.Ignore());
            CreateMap<CommiteeUsersRoleDTO, CommiteeUsersRole>();
            CreateMap<Transaction, TransactionDetailsDTO>();
            CreateMap<TransactionDetailsDTO, Transaction>();
            CreateMap<DocumentDTO, AttachmentSummaryDTO>().ForMember(d => d.AttachmentId, options => options.MapFrom(s => s.SavedAttachmentId));
            CreateMap<AttachmentSummaryDTO, DocumentDTO>();
            CreateMap<TransactionAttachmentDTO, TransactionAttachment>();
            CreateMap<TransactionAttachment, TransactionAttachmentDTO>().ForMember(x => x.attachmentIdEncypted, opt => opt.Ignore()).ForMember(x => x.transactionIdEncypted, opt => opt.Ignore()); 

            CreateMap<CommitteeNotification, CommitteeNotificationDTO>();
            CreateMap<CommitteeNotificationDTO, CommitteeNotification>();

            CreateMap<CommiteeTaskEscalation, CommiteeTaskEscalationDTO>()
                .ForMember(d => d.ComiteeTaskCategorycategoryNameAr, options => options.MapFrom(s => s.ComiteeTaskCategory.categoryNameAr))
                .ForMember(d => d.ComiteeTaskCategorycategoryNameEn, options => options.MapFrom(s => s.ComiteeTaskCategory.categoryNameEn));
            #endregion
            #region Meetings
            CreateMap<Meeting, MeetingDTO>();
            CreateMap<MeetingDTO, Meeting>();
            CreateMap<MeetingAttendee, MeetingAttendeeDTO>().ForMember(x => x.AlternativeAttendee, o => o.MapFrom(s => s.AlternativeAttendee));
            CreateMap<MeetingAttendeeDTO, MeetingAttendee>();
            CreateMap<MeetingCoordinator, MeetingCoordinatorDTO>();
            CreateMap<MeetingCoordinatorDTO, MeetingCoordinator>();
            CreateMap<MeetingHeaderAndFooter, MeetingHeaderAndFooterDTO>();
            CreateMap<MeetingHeaderAndFooterDTO, MeetingHeaderAndFooter>();
            CreateMap<MeetingProject, MeetingProjectDTO>();
            CreateMap<MeetingProjectDTO, MeetingProject>();
            CreateMap<MeetingTopic, MeetingTopicDTO>();
            CreateMap<MeetingTopicDTO, MeetingTopic>();
            CreateMap<MeetingURl, MeetingURlDTO>();
            CreateMap<MeetingURlDTO, MeetingURl>();
            CreateMap<MinuteOfMeeting, MinuteOfMeetingDTO>();
            CreateMap<MinuteOfMeetingDTO, MinuteOfMeeting>();
            CreateMap<MeetingComment, MeetingCommentDTO>();
            CreateMap<MeetingCommentDTO, MeetingComment>();
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();
            CreateMap<TopicComment, TopicCommentDTO>();
            CreateMap<TopicCommentDTO, TopicComment>();
            CreateMap<TopicPauseDate, TopicPauseDateDTO>();
            CreateMap<TopicPauseDateDTO, TopicPauseDate>();
            CreateMap<Meeting_Meeting_HeaderAndFooter, Meeting_Meeting_HeaderAndFooterDTO>().ForMember(x => x.Title, o => o.MapFrom(s => s.Meeting.Title));
            CreateMap<Meeting_Meeting_HeaderAndFooterDTO, Meeting_Meeting_HeaderAndFooter>();
            CreateMap<MeetingTopicLookupDTO, MinuteOfMeetingTopic>();
            CreateMap<MinuteOfMeetingTopic, MeetingTopicLookupDTO>().ForMember(x => x.Points, o => o.MapFrom(s => s.MeetingTopic.TopicPoints)).ForMember(x => x.Title, o => o.MapFrom(s => s.MeetingTopic.TopicTitle));
            CreateMap<MOMComment, MOMCommentDTO>().ReverseMap();
            CreateMap<MinuteOfMeetingTopic, MinuteOfMeetingTopicDTO>().ReverseMap();
            CreateMap<MeetingTopic, MeetingTopicLookupDTO>().ForMember(x => x.Points, o => o.MapFrom(s => s.TopicPoints)).ForMember(x => x.Title, o => o.MapFrom(s => s.TopicTitle));
            CreateMap<MeetingTopicLookupDTO, MeetingTopic>().ForMember(x => x.TopicPoints, o => o.MapFrom(s => s.Points))
                .ForMember(x => x.TopicTitle, o => o.MapFrom(s => s.Title));
            CreateMap<Meeting, MeetingLookUpDTO>().ReverseMap();
            CreateMap<CommiteeTask, CommiteeTaskDTO>()
                .ForMember(dest => dest.MeetingTitle, opt => opt.MapFrom(src => src.Title)).ReverseMap();
            
            #endregion
        }
    }

    public class UserCommitteeValueResolver : IValueResolver<CommiteeMember, CommiteeMemberDTO, UserDetailsDTO>
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserCommitteeValueResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDetailsDTO Resolve(CommiteeMember source, CommiteeMemberDTO destination, UserDetailsDTO destMember, ResolutionContext context)
        {
            return _unitOfWork.GetRepository<User>(false).GetAll(false).Select(x => new UserDetailsDTO
            {
                UserName = x.Username
            }).Where(c => c.UserId == source.CreatedBy).FirstOrDefault();
        }
    }
    public class UserValueResolver : IValueResolver<Survey, SurveyDTO, UserDetailsDTO>
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserValueResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDetailsDTO Resolve(Survey source, SurveyDTO destination, UserDetailsDTO destMember, ResolutionContext context)
        {
            return _unitOfWork.GetRepository<User>(false).GetAll(false).Select(x => new UserDetailsDTO
            {
                UserName = x.Username,
                RoleNameAr = this._unitOfWork.GetRepository<CommiteeUsersRole>(false).GetAll(false).FirstOrDefault(x => x.UserId == source.CreatedBy).Role.CommiteeRolesNameAr,
                RoleNameEn = this._unitOfWork.GetRepository<CommiteeUsersRole>(false).GetAll(false).FirstOrDefault(x => x.UserId == source.CreatedBy).Role.CommiteeRolesNameEn,
            }).Where(c => c.UserId == source.CreatedBy).FirstOrDefault();
        }
    }

    public class UserTaskCountCommitteeValueResolver : IValueResolver<CommiteeMember, CommiteeMemberDTO, int>
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserTaskCountCommitteeValueResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Resolve(CommiteeMember source, CommiteeMemberDTO destination, int destMember, ResolutionContext context)
        {
            return _unitOfWork.GetRepository<CommiteeTask>(false).GetAll(false).Where(x => x.MainAssinedUserId == source.UserId).Count();
        }
    }
    public class UserAttachmentCountCommitteeValueResolver : IValueResolver<CommiteeMember, CommiteeMemberDTO, int>
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserAttachmentCountCommitteeValueResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Resolve(CommiteeMember source, CommiteeMemberDTO destination, int destMember, ResolutionContext context)
        {
            return _unitOfWork.GetRepository<CommiteeSavedAttachment>(false).GetAll(false).Where(x => x.CreatedBy == source.UserId).Count();
        }
    }
}
