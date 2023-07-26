using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using Models;
using Models.ProjectionModels;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IDocumentService : IBusinessService<SavedAttachment, DocumentDTO>
    {
        byte[] Download(object id, ref string fileName, ref string mimeType, bool getOriginal = false);
        byte[] DownloadPdf(object id, ref string fileName, ref string mimeType);
        byte[] DownloadPage(object id, int pageNumber, bool thumb);
        bool RotatePage(int attachmentId, int pageNumber, int rotation);
        byte[] DownloadPageOriginal(object id, int pageNumber, bool thumb);
        byte[] DownloadPageOriginalForTransaction(object id, int pageNumber, bool thumb);
        AttachmentSummaryDTO InsertAttachment(DocumentDTO entity, params string[] include);
        SavedAttachmentDTO Insert(DocumentDTO entity, params string[] include);
        void InsertElecDoc(int entryId, byte[] officeBinaryContent, string extension);
        bool DeletePage(string id, int pageIndex);
        CommiteeAttachmentDTO MovePageTo(string CommiteeID, string sourceAttachID, int pageIndex, int moveLocation);
        IEnumerable<CommiteeAttachmentDTO> SplitDocument(string CommiteeID, string sourceAttachID, int fromPageIndex, int toPageIndex);
        List<string> GetTemplatesNames();
        int getTemplateIDByName(string temName);
        IEnumerable<dynamic> GetEntries(int? entryId);
        IEnumerable<dynamic> GetEntriesForTransAttachments(int userRoleId, string fentryid, string searchText, bool isEmployee);
        int GetCountOfAttachment(int CommiteeId);
       
        int CountOfAttachmentComment(int CommentId);
        byte[] GetDocumentContent(int? entryId, ref string fileName, ref string mimeType, bool getOriginal = false);
        Commitee GetCommiteeById(long transactionId);
        CommiteeAttachmentDTO insertCommitteeAttachment(CommiteeSavedAttachment commiteeAttachment);
       
        SavedAttachmentDTO InsertCommentAttachments(SavedAttachment CommentAttachment);
        CommitteeTaskAttachmentDTO InsertCommitteeTaskAttachment(CommitteeTaskAttachment committeeTaskAttachment);
        byte[] CreateMsgFile(string FromMail, string FromName, string ToMail, string Subject, string Body, string Attatchments, string fileName);
        SurveyAttachmentDTO InsertSurveyAttachment(SurveyAttachment surveyAttachmentDTO);
        Survey InsertSurvey(Survey survey);
        byte[] DownloadTransaction(object id, ref string fileName, ref string mimeType, bool getOriginal = false);
        UserDetailsDTO GetUserById(int? createdBy);
        void SurveySignalR(UserChatDTO user, SurveyDTO surveys);
        Meeting GetMeetingById(int? meetingId);
        Meeting GetMeetingByTopixId(int? meetingTopicId);
       // void AddNotificationForSurvey(int id, int surveyId, string subject);
        void AddNotificationForSurvey(int userId, int surveyId, string subject, int? MeetingId);
    }
}
