using DocumentProcessor102;
using LFSO102Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HelperServices
{
    public static class LFDirectConnector
    {

        private static string _selectedNodePath = string.Empty;
        private static string _parentsPath = string.Empty;


        // Creates a new application object.
        public static LFApplication App { get; set; }

        // Finds the appropriate server.
        public static LFServer Serv { get; set; }

        // LFConnection
        public static LFConnection Conn { get; set; }

        // LFDatabase db
        public static LFDatabase Db { get; set; }


        // Created File Name
        public static string DocumentName { get; set; }

        // Created File Path
        public static string DocumentPath { get; set; }

        // LF Repostry File Path
        public static string RepostryFilePath { get; set; }

        //Volume Name
        public static string VolumeName { get; set; }
        // Document Folder ID
        public static int FolderID { get; set; }
        // Downloaded File Path
        public static string DownloadedFilePath { get; set; }

        // Downloaded File Name
        public static string DownloadedFileName { get; set; }

        // Downloaded File Name
        public static string DownloadedFileNameWithPath { get; set; }

        //Document Entry ID
        public static int EntryID { get; set; }


        static LFDirectConnector()
        {
            //CreateRepostryYearIncomingOutcomingFolderIfNotExist(IncommingLfPath);
            //CreateRepostryYearIncomingOutcomingFolderIfNotExist(OutgoingLfPath);
        }

        public static void setLFConnectionCredentials(string _LFServerStr,
                                                      string _LFRepostryStr,
                                                      string _LFUserName,
                                                      string _LFUserPassword)
        {
            LFDirectConnectionSingleTon.setLFConnectionCredentials(_LFServerStr, _LFRepostryStr, _LFUserName, _LFUserPassword);

        }

        private static bool CheckExistFolder(string passesPath)
        {
            try
            {
                LFFolder parent = (LFFolder)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByPath(passesPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int UploadDocumentOnCertainPath(string lfFilePath, string FilePath)
        {

            string randomName = DateTime.Now.Ticks.ToString();

            try
            {

                string[] folderPath = lfFilePath.Split(new string[] { "\\" }, StringSplitOptions.None);

                int arrLength = folderPath.Length;
                string LFRepositoryName = LFDirectConnectionSingleTon.LFRepostryStr;

                for (int i = 1; i < arrLength; i++)
                {
                    LFRepositoryName += "\\";
                    LFRepositoryName += folderPath[i];
                    if (!CheckExistFolder(LFRepositoryName))
                    {
                        // Instantiates new document object.
                        LFDirectConnectionSingleTon.ConnectionTerminate();
                        int idx = LFRepositoryName.LastIndexOf("\\");
                        if (idx != -1)
                        {
                            string MyParent = LFRepositoryName.Substring(0, idx);
                            string FolderNeedToCreate = LFRepositoryName.Substring(idx + 1);

                            LFFolder fol = new LFFolder();
                            LFFolder parentFol = (LFFolder)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByPath(MyParent);
                            // Creates the document in the repository.
                            fol.Create(FolderNeedToCreate, parentFol, true);
                            fol.Dispose();
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }

            if (!string.IsNullOrEmpty(lfFilePath))
            {

                // Create Transaction Folder In case of not exist
                if (!CheckExistFolder(lfFilePath + "\\" + FilePath))
                {
                    // Instantiates new document object.
                    LFFolder fol = new LFFolder();

                    LFFolder parentFol = (LFFolder)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByPath(lfFilePath);
                    // Creates the document in the repository.
                    fol.Create(FilePath, parentFol, true);
                    fol.Dispose();
                    FolderID = parentFol.ID;

                }
                RepostryFilePath = lfFilePath + "\\" + FilePath;
            }

            //Create Document On LF
            var doc1 = new LFDocument();

            //=================================================================================================
            LFVolume vol = LFDirectConnectionSingleTon.LFDatabaseInstance().GetVolumeByName(VolumeName);
            LFFolder parent = (LFFolder)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByPath(RepostryFilePath);
            doc1.Create(randomName + "_" + DocumentName, parent, vol, true);
            doc1.Dispose();

            // Instantiates a new document importer.
            DocumentImporter DocImporter = new DocumentImporter();
            // Retrieves a document from the repository and assigns
            // it to the document importer.
            LFDocument doc = (LFDocument)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByPath(RepostryFilePath + "\\" + randomName + "_" + DocumentName);
            DocImporter.Document = doc;

            if (DocumentName.Contains(".tif")
                 || DocumentName.Contains(".bmp")
                    || DocumentName.Contains(".gif")
                    || DocumentName.Contains(".tiff")
                    || DocumentName.Contains(".jpg")

                    || DocumentName.Contains(".png")

                    || DocumentName.Contains(".jpeg")
                    )
            {

                // Updated By Mohamed Zein 19/11/2015 for fixing Color Scanning Issue. 
                // Imports an electronic file tiff.
                DocImporter.Document = doc;
                DocImporter.ImportImagesFromFile(DocumentPath + "\\" + DocumentName);
                doc.Dispose();
                //DocImporter.ImportImages(DocumentPath + "\\" + DocumentName);
            }
            else
            {

                string extension;
                var Arr = new byte[100];
                using (FileStream eFile = new FileStream(DocumentPath + "\\" + DocumentName, FileMode.Open, FileAccess.Read))
                {
                    Arr = new byte[eFile.Length];
                    eFile.Read(Arr, 0, Arr.Length);
                    string[] nameSegments = eFile.Name.Split('.');
                    extension = nameSegments[nameSegments.Length - 1];

                }
                //***************************************##############################################
                DocImporter.ImportElectronicFileFromMemory(Arr, extension);
                //DocImporter.ImportImagesFromFile(
            }
            var result = doc.ID;
            EntryID = result;

            return result;

        }

        public static int GetDocumentPageCountByEntryID(int entryID)
        {
            int PageCount = 0;
            try
            {

                var lfDocument = (LFDocument)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByID(entryID);
                PageCount = lfDocument.PageCount;
                return PageCount;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        public static string[] DownloadDocumentByEntryID(string DownloadedFilePath, int entryID, bool thumb, out int pageCount, int? pageNumber = null)
        {
            List<string> result = new List<string>();

            var lfDocument = (LFDocument)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByID(entryID);
            var lfPages = lfDocument.Pages;
            var de = new DocumentExporter() { Format = Document_Format.DOCUMENT_FORMAT_PNG, Thumbnail = thumb };
            Directory.CreateDirectory(DownloadedFilePath);
            pageCount = lfDocument.PageCount;
            if (pageNumber.HasValue)
            {
                lfPages.MarkPageByIndex(pageNumber.Value);
                var DownloadedFileNameWithPath = Path.Combine(DownloadedFilePath, pageNumber.Value.ToString() + (thumb ? "_small" : "") + ".png");
                if (!File.Exists(DownloadedFileNameWithPath))
                {
                    de.AddSourcePages(lfPages);
                    de.ExportToFile(DownloadedFileNameWithPath);
                }
                result.Add(DownloadedFileNameWithPath);
            }
            else
            {
                for (pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                {
                    lfPages.MarkPageByIndex(pageNumber.Value);
                    var DownloadedFileNameWithPath = Path.Combine(DownloadedFilePath, pageNumber.Value.ToString() + (thumb ? "_small" : "") + ".png");
                    if (!File.Exists(DownloadedFileNameWithPath))
                    {
                        de.AddSourcePages(lfPages);
                        de.ExportToFile(DownloadedFileNameWithPath);
                    }
                    result.Add(DownloadedFileNameWithPath);
                    lfPages.UnmarkAllPages();
                }
            }
            return result.ToArray();
        }
        public static string DownloadDocumentByEntryID(string DownloadedFilePath, int entryID, bool thumb, int pageNumber)
        {
            int pageCount;
            var filePaths = DownloadDocumentByEntryID(DownloadedFilePath, entryID, thumb, out pageCount, pageNumber);
            if (filePaths.Any())
                return filePaths[0];
            return string.Empty;
        }

        public static void UploadWordToDocument(int entryId, string FilePath)
        {
            // Instantiates a new document importer.
            DocumentImporter DocImporter = new DocumentImporter();
            // Assigns the document to the document importer object.
            //DocImporter.Document = (LFDocument)Document.GetDocumentInfo(entryId, LFSession);
            DocImporter.Document = (LFDocument)LFDirectConnectionSingleTon.LFDatabaseInstance().GetEntryByID(entryId);
            // Imports an image file.
            //DocImporter.ImportEdoc("pdf", "D:\\Laserfiche\\Work Projects\\JODC\\WindowsFormTools\\InsertionBackupData\\tempimportfiles\\JODC_Installation sign-off.pdf");
            DocImporter.ImportElectronicFile(FilePath);

        }
    }
}
