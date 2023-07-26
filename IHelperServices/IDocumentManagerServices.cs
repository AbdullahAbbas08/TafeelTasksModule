using CommiteeAndMeetings.DAL.Domains;
using CommiteeDatabase.Models.Domains;
using System.Collections.Generic;

namespace IHelperServices
{
    public interface IDocumentManagerServices : _IHelperService
    {
        string CreateFolder(string name, int parentEntryId, string parentPath);
        string CreateDocumentTransaction(int parentEntryId, int CommiteeId, int CommiteeTypeId, string fileName, string contentType, byte[] binaryContent, out int pagesCount, Transaction _Transaction);
        string CreateDocument(int parentEntryId, int CommiteeId, int CommiteeTypeId, string fileName, string contentType, byte[] binaryContent, out int pagesCount, Commitee _Commitee);

        void CreateElecDoc(int entryId, byte[] officeBinaryContent, string extension);

        int GetPagesCount(int entryId);

        string GetDocMimeType(int entryId);

        string GetDocName(int entryId);

        byte[] GetPageContent(string entryId, int pageNumber, bool thumb);

        byte[] GetPageContentOriginal(string entryId, int pageNumber, bool thumb);

        byte[] GetDocumentContent(string entryId, ref string fileName, ref string mimeType, bool getOriginal = false);

        byte[] GetDocumentContentPdf(string entryId, ref string fileName, ref string mimeType);

        IEnumerable<dynamic> Search(int? rootEntryId, string searchText);

        IEnumerable<dynamic> GetEntries(int? entryId);

        IEnumerable<dynamic> GetEntriesForTransAttachments(string folderEntryIds, string searchText);

        void Rename(int entryId, string name);

        bool RotatePage(int entryID, int pageNumber, int rotation);

        void Delete(int entryId);

        bool DeletePage(string entryId, int pageIndex);

        bool MovePageTo(string entryId, int pageIndex, int moveLocation);
        string SplitDocument(string sourceID, int fromPageIndex, int toPageIndex);

        dynamic Move(int entryId, int parentEntryId);

        dynamic Copy(int entryId, int parentEntryId);

        IEnumerable<dynamic> GetTemplates();
        List<string> GetTemplatesNames();

        int getTemplateIDByName(string temName);

        string GetFolderName(int entryId = 0);

        IEnumerable<dynamic> GetTemplateFields(int entryId, int templateId);

        void SetTemplateFields(int entryId, int templateId, List<dynamic> templateFields);

    }
}