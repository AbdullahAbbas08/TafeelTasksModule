using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class Committe_AttachmentService : BusinessService<SavedAttachment, SavedAttachmentDTO>, ICommitte_AttachmentService
    {
        //readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public Committe_AttachmentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
        }

        public int GetCommiteeAttachmentsCount(int CommiteeId)
        {
            return base._UnitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll(false).Where(x => x.CommiteeId == CommiteeId).Count();
        }

        public IEnumerable<PhysicalAttachmentDTO> InsertPhysicalAttachments(IEnumerable<PhysicalAttachmentDTO> physicalAttachments)
        {
            List<SavedAttachmentDTO> ToBeInserted = new List<SavedAttachmentDTO>();
            foreach (var physicalAttachment in physicalAttachments)
            {
                ToBeInserted.Add(new SavedAttachmentDTO()
                {
                    PhysicalAttachmentTypeId = physicalAttachment.PhysicalAttachmentTypeId,
                    //PhysicalAttachmentTypeName = physicalAttachment.PhysicalAttachmentTypeName,
                    PagesCount = physicalAttachment.PagesCount,
                    Notes = physicalAttachment.Notes
                });
            }
            var ToBeReturned = base.Insert(ToBeInserted);
            return ToBeReturned.AsQueryable().ProjectTo<PhysicalAttachmentDTO>(_Mapper.ConfigurationProvider);
        }

        public override IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            foreach (var Id in Ids)
            {
                var ToBeRemoved = base._UnitOfWork.GetRepository<SavedAttachment>().GetAll()
                    .FirstOrDefault(x => x.SavedAttachmentId == (int)Id);

            }
            base._UnitOfWork.SaveChanges(true);
            return base.Delete(Ids);
        }

        public LettersDTO AddLetterAttachment(LettersDTO lettersDTO)
        {
            AttachmentVersion attachmentVersion = new AttachmentVersion();
            CommiteeSavedAttachment CommiteeAttachment = new CommiteeSavedAttachment();
            var attachment = new SavedAttachment
            {
                SavedAttachmentId = lettersDTO.AttachmentId,
                AttachmentName = lettersDTO.AttachmentName,
                AttachmentTypeId = 2,
            };
            _UnitOfWork.GetRepository<SavedAttachment>().Insert(attachment);
            attachmentVersion = new AttachmentVersion()
            {
                AttachmentId = attachment.SavedAttachmentId,
                Text = lettersDTO.Text
            };
            _UnitOfWork.GetRepository<AttachmentVersion>().Insert(attachmentVersion);
            CommiteeAttachment.CommiteeId = (int)lettersDTO.CommiteeId;
            //  CommiteeAttachment.AttachmentId = attachmentVersion.AttachmentId;
            _UnitOfWork.GetRepository<CommiteeSavedAttachment>().Insert(CommiteeAttachment);

            return new LettersDTO
            {
                AttachmentId = attachment.SavedAttachmentId,
                CommiteeId = CommiteeAttachment.CommiteeId,
                AttachmentName = attachment.AttachmentName,
                AttachmentTypeId = attachment.AttachmentTypeId,
                Text = attachmentVersion.Text,
                CommiteeAttachmentId = CommiteeAttachment.CommiteeAttachmentId

            };
        }

        public bool UpdateLetter(IEnumerable<AttachmentDetailsDTO> letters)
        {
            var letter = letters.FirstOrDefault();
            var current = (_UnitOfWork.GetRepository<Attachment>()).GetAll().FirstOrDefault(a => a.AttachmentId == letter.AttachmentId);
            if (current != null)
            {
                current.IsDisabled = letter.IsDisabled;
                (_UnitOfWork.GetRepository<Attachment>()).Update(current);
                return true;
            }
            return false;
        }

        public bool DeleteIndividualUserAttachment(int UserId, int attachementId)
        {
            User user = this._UnitOfWork.GetRepository<User>().GetAll().FirstOrDefault(w => w.UserId == UserId && w.IndividualAttachmentId == attachementId);
            if (user == null)
                return false;
            try
            {
                user.IndividualAttachmentId = null;
                Attachment attachment = this._UnitOfWork.GetRepository<Attachment>().GetById(attachementId);
                attachment.IsDisabled = true;

                this._UnitOfWork.GetRepository<User>().Update(user);
                this._UnitOfWork.GetRepository<Attachment>().Update(attachment);

                _UnitOfWork.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }



        }
    }
}
