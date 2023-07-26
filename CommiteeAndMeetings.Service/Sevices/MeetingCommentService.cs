using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Laserfiche.RepositoryAccess;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class MeetingCommentService : BusinessService<MeetingComment, MeetingCommentDTO>, IMeetingCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;

        public MeetingCommentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }

        public List<MeetingCommentDTO> GetMeetingCommentsByMeetingId(int meetingId)
        {
            var lsMeetingComment = _unitOfWork.GetRepository<MeetingComment>().GetAll().Where(c => c.MeetingId == meetingId).ToList();
            if (lsMeetingComment.Count()==0)
            {
                return _unitOfWork.GetRepository<MeetingComment>().GetAll().Where(c => c.MeetingId == meetingId)
               .Select(x => new MeetingCommentDTO
               {
                   Comment = new CommentDTO
                   {
                       CommentId = x.CommentId,
                       Text = x.Comment.Text,
                       CreatedBy = x.Comment.CreatedBy,
                       CreatedOn = x.Comment.CreatedOn
                   },
                   CommentId = x.CommentId,
                   CommentType = x.CommentType,
                   CreatedBy = x.CreatedBy,
                   CreatedOn = x.CreatedOn,
                   Id = x.Id,
                   MeetingId = x.MeetingId

               }).ToList();
            }
            // get SurveyAnswers and then put it in list of MeetingCommentDTO
            //var lsMeetingComment = _unitOfWork.GetRepository<MeetingComment>().GetAll().Where(c => c.MeetingId == meetingId).ToList();
            List<MeetingCommentDTO> meetingCommentDTOs = new List<MeetingCommentDTO>();

            foreach (var item in lsMeetingComment)
            {
                List<MeetingCommentDTO> itemSurveyAnswer = new List<MeetingCommentDTO>();
                 itemSurveyAnswer = _unitOfWork.GetRepository<MeetingComment>().GetAll().Where(c => c.Id == item.Id)
                 .Select(x => new MeetingCommentDTO
                 {
                     Comment = new CommentDTO
                     {
                         CommentId = x.CommentId,
                         Text = x.Comment.Text,
                         CreatedBy = x.Comment.CreatedBy,
                         CreatedOn = x.Comment.CreatedOn
                     },
                     CommentId = x.CommentId,
                     CommentType = x.CommentType,
                     CreatedBy = x.CreatedBy,
                     CreatedOn = x.CreatedOn,
                     Id = x.Id,
                     MeetingId = x.MeetingId,

                     SurveyAnswers = _unitOfWork.GetRepository<SurveyAnswer>(false).GetAll(false).Where(x => x.MeetingComment.Id == item.Id).Select(x => new SurveyAnswerDTO
                     {

                         SurveyAnswerId = x.SurveyAnswerId,
                         SurveyId = x.SurveyId,
                         Answer = x.Answer,
                         SurveyAnswerUsers = x.SurveyAnswerUsers.Select(y => new SurveyAnswerUserDTO
                         {
                             SurveyAnswerId = y.SurveyAnswerId,
                             SurveyAnswerUserId = y.SurveyAnswerUserId,
                             User = new Models.UserDetailsDTO

                             {
                                 UserId = y.User.UserId,
                                 UserName = y.User.Username,
                                 FullNameAr = y.User.FullNameAr,
                                 FullNameEn = y.User.FullNameEn,
                                 FullNameFn = y.User.FullNameFn,
                                 ProfileImage = y.User.ProfileImage
                             },


                         }).ToList(),
                     }).ToList()
                 }).ToList();
                meetingCommentDTOs.AddRange(itemSurveyAnswer);
            }
            return meetingCommentDTOs;

            
        }

        
        public MeetingCommentDTO InsertMeetingComment(MeetingCommentDTO entity)
        {
            //SurveyAnswer
            List<SurveyAnswer> Answers = new List<SurveyAnswer>();

            Answers.Add(new SurveyAnswer { Answer = "موافق" });
            Answers.Add(new SurveyAnswer { Answer = "غير موافق" });
            Answers.Add(new SurveyAnswer { Answer = "متحفظ" });
            _unitOfWork.GetRepository<SurveyAnswer>().Insert(Answers);
            //survey
            var survey = new Survey
            {
                CommiteeId = null,
                IsShared = true,
                Multi = false,
                Subject = "إضافه تصويت على توصيه",
                SurveyAnswers = Answers,
               // MeetingId = entity.MeetingId,
                
                
            };

            _unitOfWork.GetRepository<Survey>().Insert(survey);
            var lastInsertedSurvey =  _unitOfWork.GetRepository<Survey>().GetAll().OrderByDescending(x => x.SurveyId).FirstOrDefault();
           var AnswersSurvey = _unitOfWork.GetRepository<SurveyAnswer>().GetAll().Where(x=>x.SurveyId == lastInsertedSurvey.SurveyId).ToList();

            MeetingComment meetingComment = new MeetingComment()
            {
                Comment = new Comment { Text = entity.Comment.Text },
                CommentType = CommentType.Recommendation,
                MeetingId = entity.MeetingId,
                SurveyAnswers = AnswersSurvey
            };
            _unitOfWork.GetRepository<MeetingComment>().Insert(meetingComment);
            var useritem = _unitOfWork.GetRepository<User>().GetAll().Where(x=>x.UserId == _sessionServices.UserId).FirstOrDefault();


            MeetingCommentDTO meetingCommentDto = new MeetingCommentDTO()
            {
                Comment = entity.Comment,
                Id = _unitOfWork.GetRepository<MeetingComment>().GetAll().OrderByDescending(x=>x.Id).FirstOrDefault().Id,
                CommentType = entity.CommentType,
                MeetingId = entity.MeetingId,
                CreatedByUser = new Models.UserDetailsDTO 
                { UserId = useritem.UserId,
                    FullNameAr = useritem.FullNameAr ,
                    FullNameEn = useritem.FullNameEn,
                    ProfileImage= useritem.ProfileImage,
                    UserName = useritem.Username
                },
                SurveyAnswers = AnswersSurvey.Select(x => new SurveyAnswerDTO 
                { Answer = x.Answer,
                    SurveyAnswerId = x.SurveyAnswerId ,
                    SurveyId = x.SurveyId,
                   // SurveyAnswerUsers = x.SurveyAnswerUsers.Select(y=> new SurveyAnswerUserDTO
                    //{
                   //     SurveyAnswerId=y.SurveyAnswerId,
                   //     SurveyAnswerUserId=y.SurveyAnswerUserId,
                    //    User = new Models.UserDetailsDTO
                        
                    //        {
                     //           UserId = y.User.UserId,
                    //            UserName = y.User.Username,
                     //           FullNameAr = y.User.FullNameAr,
                     //           FullNameEn = y.User.FullNameEn,
                      //          FullNameFn = y.User.FullNameFn,
                      //          ProfileImage = y.User.ProfileImage
                      //      },
                        
                    //   UserId = y.UserId
                    //}).ToList(),
                    }).ToList(),
            };

            return meetingCommentDto;
        }

        public override IEnumerable<MeetingCommentDTO> Update(IEnumerable<MeetingCommentDTO> Entities)
        {
            List<SurveyAnswer> surveyAnswers = new List<SurveyAnswer>();
            MeetingComment itemOldComment = new MeetingComment();


            foreach (var item in Entities)
            {
                surveyAnswers = _unitOfWork.GetRepository<SurveyAnswer>().GetAll().Where(x => x.MeetingComment.Id == item.Id).ToList();
                foreach (var itemSurveyAnswerUpdate in item.SurveyAnswers)
                {

                
                    //get SurveyAnswer
                    foreach (var itemSurvey in surveyAnswers)
                    {


                        itemSurvey.Answer = itemSurveyAnswerUpdate.Answer;
                        itemSurvey.SurveyId = itemSurveyAnswerUpdate.SurveyId;
                        //itemSurvey.SurveyAnswerUsers = _Mapper.Map<List<SurveyAnswerUser>>(itemSurveyAnswerUpdate.SurveyAnswerUsers);
                    }
                }
            }
            foreach (var item in Entities)
            {
                
                
                 itemOldComment = _unitOfWork.GetRepository<MeetingComment>().GetAll().Where(x=>x.Id == item.Id).FirstOrDefault();

                itemOldComment.Comment = new Comment { Text = item.Comment.Text };
                itemOldComment.CommentType = CommentType.Recommendation;
                itemOldComment.MeetingId = item.MeetingId;
                itemOldComment.CommentType = item.CommentType;
                itemOldComment.CreatedBy = item.CreatedBy;
                itemOldComment.CommentId = item.CommentId;
                itemOldComment.SurveyAnswers = surveyAnswers;


                _unitOfWork.GetRepository<MeetingComment>().Update(itemOldComment);
            }
           var ReturnMeetingCommentDTO = _Mapper.Map<MeetingCommentDTO>(itemOldComment);
            List<MeetingCommentDTO> ReturnMeetingCommentDTOList =new List<MeetingCommentDTO>();
            ReturnMeetingCommentDTOList.Add(ReturnMeetingCommentDTO);
            return ReturnMeetingCommentDTOList;

        }

        public bool deleteMeetingComment (int id)
        {
            try
            {

                var AnswersSurvey = _unitOfWork.GetRepository<SurveyAnswer>().GetAll().Where(x => x.MeetingComment.Id == id).ToList();
                foreach (var surveyAnsweruser in AnswersSurvey)
                {

                        var userInSurveyAnswer = _unitOfWork.GetRepository<SurveyAnswerUser>().GetAll().Where(x => x.SurveyAnswerId == surveyAnsweruser.SurveyAnswerId).ToList();
                       _unitOfWork.GetRepository<SurveyAnswerUser>().Delete(userInSurveyAnswer);
                }
                
                _unitOfWork.GetRepository<SurveyAnswer>().Delete(AnswersSurvey);
                var itemComment = _unitOfWork.GetRepository<MeetingComment>().GetAll().Where(x => x.Id == id).FirstOrDefault();
                _unitOfWork.GetRepository<MeetingComment>().Delete(itemComment);
                _unitOfWork.SaveChanges();
                return true;
                
            }
            catch
            {
                return false;
            }

        }
    }
}
