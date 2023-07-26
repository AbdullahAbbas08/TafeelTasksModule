using CommiteeAndMeetings.DAL.Domains;
using CommiteeDatabase.Models.Domains;
using DocumentProcessor102;
using FileNet.Api.Authentication;
using FileNet.Api.Collection;
using FileNet.Api.Constants;
using FileNet.Api.Core;
using FileNet.Api.Util;
using IHelperServices;
using IHelperServices.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LFSO102Lib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Document = iTextSharp.text.Document;

namespace HelperServices
{
    public class ECMDocumentServices : _HelperService, IDocumentManagerServices
    {
        public AppSettings _AppSettings;
        private string _RootPath;
        private static object padlock = new object();
        private static object CleanDocumentsLocker = new object();

        IHostingEnvironment _hostingEnvironment;

        /*LF*/
        private LFDatabase _BaseDatabase;
        /**/

        /*FileNet*/
        private static IObjectStore os = null;
        /**/

        //private IGeneralUtilities _GeneralUtilities;
        //private readonly ISystemSettingsService _SystemSettingsService;
        //public string RelativeDirectory, YearIsHijri, LaserFicheServer, LaserFicheRepository, LaserFicheUserName, LaserFichePassword;
        //GeneralUtilities generalUtilities;

        private void ResetBaseDatabaseWithNewConnection()
        {
            LFApplication application = new LFApplication();
            LFServer server = application.GetServerByName(this._AppSettings.DocumentSettings.ECMServer);
            LFDatabase database = server.GetDatabaseByName(this._AppSettings.DocumentSettings.ECMRepository);
            LFConnection connection = new LFConnection();
            connection.UserName = this._AppSettings.DocumentSettings.ECMUserName;
            connection.Password = this._AppSettings.DocumentSettings.ECMPassword;
            connection.Create(database);
            _BaseDatabase = database;
        }

        public LFDatabase _Database
        {
            get
            {
                if (_BaseDatabase.CurrentConnection.IsTerminated)
                {
                    lock (padlock)
                    {
                        if (_BaseDatabase.CurrentConnection.IsTerminated) //check again to be thread-safe
                        {
                            ResetBaseDatabaseWithNewConnection();
                        }
                    }
                }
                return _BaseDatabase;
            }
        }

        public IObjectStore FNDatabaseInstance()
        {
            if (os == null)
            {
                lock (padlock)
                {
                    UsernameCredentials cred = new UsernameCredentials(this._AppSettings.DocumentSettings.ECMUserName, this._AppSettings.DocumentSettings.ECMPassword);
                    // now associate this Credentials with the whole process
                    ClientContext.SetProcessCredentials(cred);
                    IConnection connection = Factory.Connection.GetConnection(this._AppSettings.DocumentSettings.ECMServer);
                    //isCredentialsEstablished = true;
                    //IntializeVariables(connection);
                    IDomain domain = Factory.Domain.FetchInstance(connection, null, null);
                    os = Factory.ObjectStore.FetchInstance(domain, this._AppSettings.DocumentSettings.ECMRepository, null);
                }
            }
            return os;
        }

        //public string RelativeDirectory
        //{
        //    get
        //    {
        //        return _SystemSettingsService.GetSystemSettingByCode("RelativeDirectory").SystemSettingValue;
        //    }
        //}

        //public string LaserFicheServer
        //{
        //    get
        //    {
        //        return _SystemSettingsService.GetSystemSettingByCode("LaserFicheServer").SystemSettingValue;
        //    }
        //}

        //public string LaserFicheRepository
        //{
        //    get
        //    {
        //        return _SystemSettingsService.GetSystemSettingByCode("LaserFicheRepository").SystemSettingValue;
        //    }
        //}

        //public string LaserFicheUserName
        //{
        //    get
        //    {
        //        return _SystemSettingsService.GetSystemSettingByCode("LaserFicheUserName").SystemSettingValue;
        //    }
        //}

        //public string LaserFichePassword
        //{
        //    get
        //    {
        //        return _SystemSettingsService.GetSystemSettingByCode("LaserFichePassword").SystemSettingValue;
        //    }
        //}

        //public string YearIsHijri
        //{
        //    get
        //    {
        //        return _SystemSettingsService.GetSystemSettingByCode("YearIsHijri").SystemSettingValue;
        //    }
        //}

        //public ECMDocumentServices(IOptions<AppSettings> appSettings, ISystemSettingsService systemSettingsService)

        public ECMDocumentServices(IOptions<AppSettings> appSettings, IHostingEnvironment environment)
        {

            this._hostingEnvironment = environment;

            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            //RelativeDirectory = _SystemSettingsService.GetSystemSettingByCode("RelativeDirectory").SystemSettingValue;
            //YearIsHijri = _SystemSettingsService.GetSystemSettingByCode("YearIsHijri").SystemSettingValue;
            //LaserFicheServer = _SystemSettingsService.GetSystemSettingByCode("LaserFicheServer").SystemSettingValue;
            //LaserFicheRepository = _SystemSettingsService.GetSystemSettingByCode("LaserFicheRepository").SystemSettingValue;
            //LaserFicheUserName = _SystemSettingsService.GetSystemSettingByCode("LaserFicheUserName").SystemSettingValue;
            //LaserFichePassword = _SystemSettingsService.GetSystemSettingByCode("LaserFichePassword").SystemSettingValue;
            //this._SystemSettingsService = systemSettingsService;
            //_GeneralUtilities = new GeneralUtilities();

            _RootPath = _AppSettings.FileSettings.RelativeDirectory;
            string yearValue = DateTime.Now.ToString("yyyy");
            if (this._AppSettings.DocumentSettings.YearIsHijri.Equals("1"))
            {
                CultureInfo HijriCI = new CultureInfo("ar-SA");
                yearValue = DateTime.Now.ToString("yyyy", HijriCI);
            }

            if (this._AppSettings.DocumentSettings.ECMType == "1")
            {
                ResetBaseDatabaseWithNewConnection();
                if (this._AppSettings.DocumentSettings.EnableYearMonths.Equals("0"))
                {
                    CreateMASARRepositryFolderIfNotExist(GetSavedECMFolder(true, 0, null) + "\\" + "Incoming");
                    CreateMASARRepositryFolderIfNotExist(GetSavedECMFolder(true, 0, null) + "\\" + "Outgoing");
                }
                else if (this._AppSettings.DocumentSettings.EnableYearMonths.Equals("1"))
                {
                    CreateMASARRepositryFolderIfNotExist(GetSavedECMFolder(true, 0, null));
                    CreateMASARRepositryFolderIfNotExist(GetSavedECMFolder(true, 0, null));
                }
            }
            //else if (this._AppSettings.DocumentSettings.ECMType == "2")
            //{
            //    FNDatabaseInstance();
            //    CreateMASARRepositryFolderIfNotExist("MASAR\\" + yearValue + "\\" + "Incoming");
            //    CreateMASARRepositryFolderIfNotExist("MASAR\\" + yearValue + "\\" + "Outgoing");
            //}
        }

        public string CreateFolder(string name, int parentEntryId = 0, string parentPath = "")
        {
            string result = "0";
            var parentEntry = (LFFolder)_Database.GetEntryByID(parentEntryId);
            var NewFolder = new LFFolder();
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    NewFolder.Create(name, parentEntry, true);
                    result = Convert.ToString(NewFolder.ID);
                    NewFolder.Dispose();
                    parentEntry.Dispose();
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    //IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), parentPath/*"\\" + IncommingLfPath*/, null);
                    //IFolder nf = null;
                    //nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    //nf.FolderName = name;
                    //nf.Parent = f;
                    //nf.Save(RefreshMode.REFRESH);
                    //result = Convert.ToString(f.Id);

                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "CreateFolder" + " " + name + " " + parentPath;
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        result = compiler.StandardOutput.ReadToEnd();

                        compiler.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {
                NewFolder.Dispose();
                parentEntry.Dispose();
                throw ex;
            }
            return result;
        }

        private string GetSavedECMFolder(bool yearPath, int TransactionTypeId, DateTimeOffset? transactionDate)
        {
            string yearValue = DateTime.Now.ToString("yyyy");
            string monthValue = transactionDate.HasValue ? transactionDate.Value.ToString("MM") : "";

            if (this._AppSettings.DocumentSettings.YearIsHijri.Equals("1"))
            {
                CultureInfo HijriCI = new CultureInfo("ar-SA");
                yearValue = DateTime.Now.ToString("yyyy", HijriCI);
                monthValue = transactionDate.HasValue ? transactionDate.Value.ToString("MM", HijriCI) : "";
            }
            if (yearPath)
            {
                return this._AppSettings.DocumentSettings.ECMRepository + "\\" + this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + yearValue;
            }
            else if (this._AppSettings.DocumentSettings.EnableYearMonths.Equals("1"))
            {
                if (!CheckExistFolder(this._AppSettings.DocumentSettings.ECMRepository + "\\" + this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + yearValue + "\\" + monthValue))
                {
                    LFFolder parentEntry = new LFFolder();
                    LFFolder parentFol = (LFFolder)_Database.GetEntryByPath(this._AppSettings.DocumentSettings.ECMRepository + "\\" + this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + yearValue);
                    // Creates the document in the repository.
                    parentEntry.Create(Convert.ToString(monthValue), parentFol, true);
                    parentEntry.Dispose();
                }
                return this._AppSettings.DocumentSettings.ECMRepository + "\\" + this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + yearValue + "\\" + monthValue;
            }
            else
            {
                return this._AppSettings.DocumentSettings.ECMRepository + "\\" + this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming");
            }
        }

        public string CreateDocument(int parentEntryId, int TransactionId, int TransactionTypeId, string fileName, string contentType, byte[] binaryContent, out int pagesCount, Commitee _commitee)
        {
            //string yearValue = DateTime.Now.ToString("yyyy");
            //if (this._AppSettings.DocumentSettings.YearIsHijri.Equals("1"))
            //{
            //    CultureInfo HijriCI = new CultureInfo("ar-SA");
            //    yearValue = DateTime.Now.ToString("yyyy", HijriCI);
            //}
            //if (!string.IsNullOrEmpty(_commitee?.IncomingLetterNumber))
            //{
            //    TransactionTypeId = 2;
            //}
            //else
            //{
            TransactionTypeId = 1;
            //}


            string result = "0";
            pagesCount = 0;
            LFFolder parentEntry = null;
            LFDocument NewDocument = null;
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    parentEntry = new LFFolder();
                    parentEntry = (LFFolder)_Database.GetEntryByPath(GetSavedECMFolder(true, TransactionTypeId, _commitee?.CreatedOn));

                    if (TransactionId != 0 && TransactionTypeId != 0)
                    {
                        if (!CheckExistFolder(GetSavedECMFolder(false, TransactionTypeId, _commitee?.CreatedOn) + "\\" + TransactionId))
                        {
                            LFFolder parentFol = (LFFolder)_Database.GetEntryByPath(GetSavedECMFolder(false, TransactionTypeId, _commitee?.CreatedOn));
                            // Creates the document in the repository.
                            parentEntry.Create(Convert.ToString(TransactionId), parentFol, true);
                            parentEntry.Dispose();
                        }
                        else
                        {
                            parentEntry = (LFFolder)_Database.GetEntryByPath(GetSavedECMFolder(false, TransactionTypeId, _commitee?.CreatedOn) + "\\" + TransactionId);
                        }
                    }

                    NewDocument = new LFDocument();
                    NewDocument.Create(fileName, parentEntry, parentEntry.DefaultVolume, true);
                    var DocumentImporter = new DocumentImporter();
                    DocumentImporter.Document = NewDocument;
                    DocumentImporter.PageAction = Import_Page_Action.IMPORT_PAGE_ACTION_INSERT;
                    DocumentImporter.PageIndex = 1;
                    if (contentType.StartsWith("image"))
                    {
                        DocumentImporter.ImportImages(binaryContent);
                    }
                    else if (contentType.StartsWith("text"))
                    {
                        DocumentImporter.ImportText(System.Text.Encoding.UTF8.GetString(binaryContent));
                    }
                    else
                    {
                        DocumentImporter.ImportElectronicFileFromMemory(binaryContent, fileName.Split('.').LastOrDefault());
                    }
                    result = Convert.ToString(NewDocument.ID);
                    pagesCount = NewDocument.PageCount;

                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    //if (TransactionId != 0 && TransactionTypeId != 0)
                    //{
                    #region commented
                    //// Create Transaction Folder In case of not exist
                    //if (!CheckExistFolder("\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId))
                    //{
                    //    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming"), null);
                    //    IFolder nf = null;
                    //    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    //    nf.FolderName = Convert.ToString(TransactionId);
                    //    nf.Parent = f;
                    //    //return nf;
                    //    nf.Save(RefreshMode.REFRESH);
                    //    //FolderID = f.Id;
                    //}
                    ////RepostryFilePath = "\\" + IncommingLfPath + "\\" + FilePath;
                    //string mimeType = "";
                    //if (contentType.Contains(".tif"))
                    //{
                    //    mimeType = "image/tiff";
                    //}

                    //IDocument doc = null;
                    //doc = FNCreateDocument(true, _RootPath + doc.Id, mimeType, fileName, FNDatabaseInstance(), "Document");
                    //doc.Save(RefreshMode.REFRESH);
                    //String folder = "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId;
                    //if (folder.Length == 0)
                    //    folder = "/";
                    //IReferentialContainmentRelationship rcr = FileContainable(FNDatabaseInstance(), doc, folder);
                    //rcr.Save(RefreshMode.REFRESH);
                    ////var result = doc.Id;
                    ////EntryID = result;
                    ////return result;
                    //result = Convert.ToString(doc.Id);
                    ////TODO: get tif page count
                    //pagesCount = GetTifPagesCount(_RootPath + doc.Id);
                    #endregion

                    #region commented
                    //ImageFormat format = ImageFormat.Tiff;
                    //string importTempFilePath = string.Format(Path.GetFullPath(this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + fileName.Split('.').FirstOrDefault() + "_" + DateTime.Now.Ticks.ToString() + ".{0}"), format.ToString());
                    //byte[] byteArray = binaryContent; // Put the bytes of the image here....
                    //Image resultImage = null;
                    //resultImage = new Bitmap(new MemoryStream(byteArray));
                    //using (Image imageToExport = resultImage)
                    //{
                    //    imageToExport.Save(importTempFilePath, format);
                    //}

                    //foreach (var formFile in FileNetFiles)
                    //{
                    //    if (formFile.Length > 0)
                    //    {
                    //        using (var stream = new FileStream(importTempFilePath, FileMode.CreateNew))
                    //        {
                    //            formFile.CopyTo(stream);
                    //        }
                    //    }
                    //}
                    #endregion

                    fileName = new Random().Next(1, 999999) + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(TransactionId) + "_" + fileName;
                    string importTempFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", fileName);
                    File.WriteAllBytes(importTempFilePath, binaryContent);

                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"CreateDocument\"" + " \"" + TransactionId + "\" \"" + TransactionTypeId + "\" \"" + fileName + "\" \"" + contentType + "\" \"" + importTempFilePath /*+ "\""*/
                            + "\" \"" + Convert.ToString(_commitee?.CommiteeId) + "\" \"" + Convert.ToString(_commitee?.CreatedOn)
                            + "\" \"" + Convert.ToString(_commitee?.Title)?.Replace("\"", "\\\"") + "\" \"" + Convert.ToString(_commitee?.CommiteeId)?.Replace("\"", "\\\"")
                            + "\" \"" + Convert.ToString(_commitee?.CreatedOn) + "\" \"" + Convert.ToString(_commitee?.CommiteeId)?.Replace("\"", "\\\"") + "\"";

                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        result = compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                        pagesCount = GetTifPagesCount(importTempFilePath);
                    }
                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(importTempFilePath)); } } catch { }

                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (parentEntry != null)
                    parentEntry.Dispose();
                if (NewDocument != null)
                    NewDocument.Dispose();
            }
            return result.ToString();
        }

        public void CreateElecDoc(int entryId, byte[] officeBinaryContent, string extension)
        {
            LFDocument document = null;
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    document = (LFDocument)_Database.GetEntryByID(entryId);
                    var DocumentImporter = new DocumentImporter();
                    DocumentImporter.Document = document;
                    DocumentImporter.ImportElectronicFileFromMemory(officeBinaryContent, extension);
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    //if (TransactionId != 0 && TransactionTypeId != 0)
                    //{
                    #region commented
                    //// Create Transaction Folder In case of not exist
                    //if (!CheckExistFolder("\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId))
                    //{
                    //    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming"), null);
                    //    IFolder nf = null;
                    //    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    //    nf.FolderName = Convert.ToString(TransactionId);
                    //    nf.Parent = f;
                    //    //return nf;
                    //    nf.Save(RefreshMode.REFRESH);
                    //    //FolderID = f.Id;
                    //}
                    ////RepostryFilePath = "\\" + IncommingLfPath + "\\" + FilePath;
                    //string mimeType = "";
                    //if (contentType.Contains(".tif"))
                    //{
                    //    mimeType = "image/tiff";
                    //}

                    //IDocument doc = null;
                    //doc = FNCreateDocument(true, _RootPath + doc.Id, mimeType, fileName, FNDatabaseInstance(), "Document");
                    //doc.Save(RefreshMode.REFRESH);
                    //String folder = "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId;
                    //if (folder.Length == 0)
                    //    folder = "/";
                    //IReferentialContainmentRelationship rcr = FileContainable(FNDatabaseInstance(), doc, folder);
                    //rcr.Save(RefreshMode.REFRESH);
                    ////var result = doc.Id;
                    ////EntryID = result;
                    ////return result;
                    //result = Convert.ToString(doc.Id);
                    ////TODO: get tif page count
                    //pagesCount = GetTifPagesCount(_RootPath + doc.Id);
                    #endregion

                    #region commented
                    //ImageFormat format = ImageFormat.Tiff;
                    //string importTempFilePath = string.Format(Path.GetFullPath(this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + fileName.Split('.').FirstOrDefault() + "_" + DateTime.Now.Ticks.ToString() + ".{0}"), format.ToString());
                    //byte[] byteArray = binaryContent; // Put the bytes of the image here....
                    //Image resultImage = null;
                    //resultImage = new Bitmap(new MemoryStream(byteArray));
                    //using (Image imageToExport = resultImage)
                    //{
                    //    imageToExport.Save(importTempFilePath, format);
                    //}

                    //foreach (var formFile in FileNetFiles)
                    //{
                    //    if (formFile.Length > 0)
                    //    {
                    //        using (var stream = new FileStream(importTempFilePath, FileMode.CreateNew))
                    //        {
                    //            formFile.CopyTo(stream);
                    //        }
                    //    }
                    //}
                    #endregion


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (document != null)
                    document.Dispose();
            }
        }

        public int GetPagesCount(int entryId)
        {
            int result = 0;
            LFDocument document = null;
            IDocument doc = null;

            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    document = (LFDocument)_Database.GetEntryByID(entryId);
                    result = document.PageCount;
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    doc = FetchDocByID(FNDatabaseInstance(), Convert.ToString(entryId));
                    WriteContentToFile(doc, Path.Combine(_hostingEnvironment.ContentRootPath, _RootPath));
                    //TODO: get tif page count
                    result = GetTifPagesCount(Path.Combine(_hostingEnvironment.ContentRootPath, _RootPath) + doc.Id);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
                //if (doc != null)
                //{
                //    doc.Dispose();
                //}
            }
            return result;
        }

        public string GetDocMimeType(int entryId)
        {
            //document.ElectFile.Extension.Equals("msg")) && !document.ElectFile.IsEmpty)
            LFDocument document = null;
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    document = (LFDocument)_Database.GetEntryByID(entryId);
                    if (!document.ElectFile.IsEmpty)
                        return document.ElectFile.MimeType;
                    else
                        return "image/tiff";
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    return "image/tiff";
                }
                return "image/tiff";
            }
            catch (Exception)
            {
                return "image/tiff";
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
            }
        }

        public string GetDocName(int entryId)
        {
            LFDocument document = null;
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    document = (LFDocument)_Database.GetEntryByID(entryId);
                    return Convert.ToString(document.Name);
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    return "attachment name";
                }
                return "attachment name";
            }
            catch (Exception)
            {
                return "attachment name";
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
            }
        }

        public byte[] GetPageContent(string entryId, int pageNumber, bool thumb)
        {
            byte[] result = null;
            LFDocument document = null;
            IDocument doc = null;
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    document = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(entryId));
                    LFDocumentPages pages = document.Pages;
                    DocumentExporter exporter = new DocumentExporter() { Thumbnail = thumb };
                    exporter.Format = Document_Format.DOCUMENT_FORMAT_TIFFJPEG;
                    //DocumentInfo docInfo = Document.GetDocumentInfo(entryId, Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection));
                    //foreach (var item in docInfo.GetPageInfos())
                    //{
                    //    if(((PageInfo)item).PageNumber==pageNumber)
                    //    {
                    //        if(((PageInfo)item).ImageDataSize>5000)
                    //        {
                    //            exporter.Format = Document_Format.DOCUMENT_FORMAT_TIFFJPEG;
                    //        }                           
                    //    }
                    //    //Console.WriteLine(((PageInfo)item).PageNumber);
                    //    //Console.WriteLine(((PageInfo)item).ImageDataSize);
                    //    //Console.WriteLine(((PageInfo)item).ThumbnailDataSize);
                    //}
                    pages.MarkPageByIndex(pageNumber);
                    exporter.AddSourcePages(pages);
                    result = (byte[])exporter.Export();
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    ImageFormat format = ImageFormat.Tiff;
                    string tempFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + "." + format.ToString());

                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"DownloadDocument\"" + " \"" + entryId + "\" \"" + tempFileAbsolutePath + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    //TODO: get tif page content bytes
                    result = tifImageToTiffByteArray(tempFileAbsolutePath, pageNumber);
                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFileAbsolutePath)); } } catch { }
                }
            }
            catch (Exception ex)
            {
                result = new byte[] { };
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
                //if (doc != null)
                //{
                //    doc.Dispose();
                //}
            }
            return result;
        }

        //private static readonly Object obj = new Object();
        private ConcurrentDictionary<string, object> dictionary = new ConcurrentDictionary<string, object>();

        public byte[] GetPageContentOriginal(string entryId, int pageNumber, bool thumb)
        {
            byte[] result = null;
            LFDocument document = null;
            IDocument doc = null;

            try
            {

                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    var obj = dictionary.GetOrAdd(entryId, new object());
                    lock (obj)
                    {
                        document = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(entryId));
                        LFDocumentPages pages = document.Pages;
                        DocumentExporter exporter = new DocumentExporter() { Thumbnail = thumb };
                        exporter.Format = Document_Format.DOCUMENT_FORMAT_PNG;
                        //DocumentInfo docInfo = Document.GetDocumentInfo(entryId, Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection));
                        //foreach (var item in docInfo.GetPageInfos())
                        //{
                        //    if(((PageInfo)item).PageNumber==pageNumber)
                        //    {
                        //        if(((PageInfo)item).ImageDataSize>5000)
                        //        {
                        //            exporter.Format = Document_Format.DOCUMENT_FORMAT_TIFFJPEG;
                        //        }                           
                        //    }
                        //    //Console.WriteLine(((PageInfo)item).PageNumber);
                        //    //Console.WriteLine(((PageInfo)item).ImageDataSize);
                        //    //Console.WriteLine(((PageInfo)item).ThumbnailDataSize);
                        //}
                        pages.MarkPageByIndex(pageNumber);
                        exporter.AddSourcePages(pages);
                        result = (byte[])exporter.Export();
                    }
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    ImageFormat format = ImageFormat.Tiff;
                    string tempFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + "." + format.ToString());

                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"DownloadDocument\"" + " \"" + entryId + "\" \"" + tempFileAbsolutePath + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    //TODO: get tif page content bytes
                    result = tifImageToPngByteArray(tempFileAbsolutePath, pageNumber);
                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFileAbsolutePath)); } } catch { }
                }
            }
            catch (Exception ex)
            {
                result = new byte[] { };
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
                //if (doc != null)
                //{
                //    doc.Dispose();
                //}
            }
            return result;
        }

        public byte[] GetDocumentContent(string entryId, ref string fileName, ref string mimeType, bool getOriginal = false)
        {
            byte[] result = null;
            LFDocument document = null;
            string tempFilePath = "";
            try
            {
                #region commented
                //document = (LFDocument)_Database.GetEntryByID(entryId);
                //var documentName = document.Name;

                //if (document.ElectFile == null)
                //{
                //    fileName = fileName ?? (documentName.ToLower().EndsWith(".tiff") || documentName.ToLower().EndsWith(".tif") ? documentName : $"{documentName}.tiff");
                //    mimeType = mimeType ?? "image/tiff";
                //}
                //else
                //{
                //    fileName = fileName ?? (documentName.ToLower().EndsWith(document.ElectFile.Extension) ? documentName : $"{documentName}.{document.ElectFile.Extension}");
                //    mimeType = mimeType ?? (!string.IsNullOrEmpty(document.ElectFile.MimeType) ? document.ElectFile.MimeType : "image/tiff");
                //}

                //DocumentExporter exporter = new DocumentExporter() { SourceDocument = document };
                //result = (byte[])exporter.Export();
                //if ((result == null || result.Length == 0) && !document.ElectFile.IsEmpty)
                //{
                //    Directory.CreateDirectory(_RootPath);
                //    string tempFileAbsolutePath = Path.Combine(_RootPath, document.EntryGUID);
                //    fileName = $"{documentName}.{document.ElectFile.Extension}";
                //    document.ElectFile.ReadStream.Export(tempFileAbsolutePath);
                //    result = File.ReadAllBytes(tempFileAbsolutePath);
                //    Task.Run(() => File.Delete(tempFileAbsolutePath));
                //}
                #endregion

                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    document = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(entryId));
                    var documentName = document.Name;
                    fileName = (documentName.ToLower().EndsWith(".tiff") || documentName.ToLower().EndsWith(".tif")
                            || documentName.ToLower().EndsWith(".wav") || documentName.ToLower().EndsWith(".m4a")
                            || documentName.ToLower().EndsWith(".doc") || documentName.ToLower().EndsWith(".docx")
                            || documentName.ToLower().EndsWith(".xls") || documentName.ToLower().EndsWith(".xlsx")) ? documentName : $"{documentName}.tiff";
                    DocumentExporter exporter = new DocumentExporter() { Format = Document_Format.DOCUMENT_FORMAT_TIFFJPEG, SourceDocument = document };
                    /**/
                    if (!getOriginal)
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileName)}.tiff";
                        result = (byte[])exporter.Export();
                        mimeType = "image/tiff";
                    }
                    /**/
                    if ((result == null || result.Length == 0) && !document.ElectFile.IsEmpty && getOriginal)
                    {
                        if (!Directory.Exists(Path.Combine(_hostingEnvironment.ContentRootPath, _RootPath)))
                            Directory.CreateDirectory(Path.Combine(_hostingEnvironment.ContentRootPath, _RootPath));

                        tempFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, _RootPath, document.EntryGUID);
                        fileName = (documentName.ToLower().EndsWith(".tiff") || documentName.ToLower().EndsWith(".tif")
                            || documentName.ToLower().EndsWith(".wav") || documentName.ToLower().EndsWith(".m4a")
                            || documentName.ToLower().EndsWith(".doc") || documentName.ToLower().EndsWith(".docx")
                            || documentName.ToLower().EndsWith(".xls") || documentName.ToLower().EndsWith(".xlsx")) ? documentName : $"{documentName}.{document.ElectFile.Extension}";
                        mimeType = document.ElectFile.MimeType;

                        document.ElectFile.ReadStream.Export(tempFilePath);
                        result = File.ReadAllBytes(tempFilePath);
                        try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFilePath)); } } catch { }
                    }
                    else if ((result == null || result.Length == 0) && document.ElectFile.IsEmpty)
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileName)}.tiff";
                        mimeType = "image/tiff";
                        result = (byte[])exporter.Export();
                    }
                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    //doc = FetchDocByID(FNDatabaseInstance(), Convert.ToString(entryId));
                    //WriteContentToFile(doc, _RootPath);

                    mimeType = "image/tiff";
                    ImageFormat format = ImageFormat.Tiff;
                    string tempFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + "." + format.ToString());
                    fileName = "";
                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"DownloadDocument\"" + " \"" + entryId + "\" \"" + tempFileAbsolutePath + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    result = File.ReadAllBytes(tempFileAbsolutePath);
                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFileAbsolutePath)); } } catch { }
                }
            }
            catch (Exception ex)
            {
                result = new byte[] { };
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
            }
            return result;
        }

        public byte[] GetDocumentContentPdf(string entryId, ref string fileName, ref string mimeType)
        {
            byte[] result = null;
            LFDocument document = null;
            IDocument doc = null;
            try
            {
                #region commented
                //document = (LFDocument)_Database.GetEntryByID(entryId);
                //var documentName = document.Name;

                //if (document.ElectFile == null)
                //{
                //    fileName = fileName ?? (documentName.ToLower().EndsWith(".tiff") || documentName.ToLower().EndsWith(".tif") ? documentName : $"{documentName}.tiff");
                //    mimeType = mimeType ?? "image/tiff";
                //}
                //else
                //{
                //    fileName = fileName ?? (documentName.ToLower().EndsWith(document.ElectFile.Extension) ? documentName : $"{documentName}.{document.ElectFile.Extension}");
                //    mimeType = mimeType ?? (!string.IsNullOrEmpty(document.ElectFile.MimeType) ? document.ElectFile.MimeType : "image/tiff");
                //}

                //DocumentExporter exporter = new DocumentExporter() { SourceDocument = document };
                //result = (byte[])exporter.Export();
                //if ((result == null || result.Length == 0) && !document.ElectFile.IsEmpty)
                //{
                //    Directory.CreateDirectory(_RootPath);
                //    string tempFileAbsolutePath = Path.Combine(_RootPath, document.EntryGUID);
                //    fileName = $"{documentName}.{document.ElectFile.Extension}";
                //    document.ElectFile.ReadStream.Export(tempFileAbsolutePath);
                //    result = File.ReadAllBytes(tempFileAbsolutePath);
                //    Task.Run(() => File.Delete(tempFileAbsolutePath));
                //}
                #endregion

                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    string tempFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + ".pdf");
                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"LFDownloadPdf\"" + " \"" + entryId + "\" \"" + tempFileAbsolutePath + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    mimeType = "application/pdf";
                    result = File.ReadAllBytes(tempFileAbsolutePath);

                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFileAbsolutePath)); } } catch { }

                    //document = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(entryId));
                    //var documentName = document.Name;
                    //fileName = (documentName.ToLower().EndsWith(".pdf")) ? documentName : $"{documentName}.pdf";
                    //var pdfOptions = new ExportOptions();
                    //pdfOptions.SetLayers(DocumentLayers.All);
                    //pdfOptions.SetMetadataOptions(MetaDataOptions.All);
                    //PdfExporter.PdfExporter exporter = new PdfExporter.PdfExporter(pdfOptions);

                    //result = exporter.ExportPages(document);

                    /* */
                    //string tempFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + ".pdf");

                    //RepositoryRegistration myRepoReg = new RepositoryRegistration(this._AppSettings.DocumentSettings.ECMServer, this._AppSettings.DocumentSettings.ECMRepository);
                    //Session mySess = new Session();
                    //mySess.LogIn(this._AppSettings.DocumentSettings.ECMUserName, this._AppSettings.DocumentSettings.ECMPassword, myRepoReg);


                    //DocumentInfo DI = Document.GetDocumentInfo(Convert.ToInt32(entryId), mySess);
                    //Laserfiche.DocumentServices.DocumentExporter DE = new Laserfiche.DocumentServices.DocumentExporter();
                    ////DE.IncludeAnnotations = true; // convert Laserfiche annotations into Adobe Acrobat annotations.
                    //DE.ExportPdf(DI, DI.AllPages, PdfExportOptions.None, tempFileAbsolutePath);

                    //DI.Dispose();
                    //mySess.LogOut();

                    //result = File.ReadAllBytes(tempFileAbsolutePath);
                    //try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFileAbsolutePath)); } } catch { }                    

                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    mimeType = "application/pdf";
                    ImageFormat format = ImageFormat.Tiff;
                    string tempFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + "." + format.ToString());
                    string tempPdfFileAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", entryId + "_" + DateTime.Now.Ticks.ToString() + "." + "pdf");


                    fileName = "";
                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"DownloadDocument\"" + " \"" + entryId + "\" \"" + tempFileAbsolutePath + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    /* convert tif to pdf */
                    using (var stream = new FileStream(tempPdfFileAbsolutePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Document pdfdocument = new Document(PageSize.A4, 0, 0, 0, 0);
                        var writer = PdfWriter.GetInstance(pdfdocument, stream);
                        var bitmap = new System.Drawing.Bitmap(tempFileAbsolutePath);
                        var pages = bitmap.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                        pdfdocument.Open();
                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                        for (int i = 0; i < pages; ++i)
                        {
                            bitmap.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, i);
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Tiff);
                            // scale the image to fit in the page 
                            img.ScalePercent(72f / img.DpiX * 100);
                            //img.ScalePercent(72f / 200f * 100);
                            img.SetAbsolutePosition(0, 0);
                            /* remove below two lines to fit image sie on pdf page*/
                            img.ScaleAbsoluteHeight(pdfdocument.PageSize.Height);
                            img.ScaleAbsoluteWidth(pdfdocument.PageSize.Width);
                            /**/
                            cb.AddImage(img);
                            pdfdocument.NewPage();
                        }
                        pdfdocument.Close();
                        bitmap.Dispose();
                    }
                    /**/
                    result = File.ReadAllBytes(tempPdfFileAbsolutePath);

                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempFileAbsolutePath)); } } catch { }
                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(tempPdfFileAbsolutePath)); } } catch { }
                }
            }
            catch (Exception ex)
            {
                result = new byte[] { };
            }
            finally
            {
                if (document != null)
                {
                    document.Dispose();
                }
            }
            return result;
        }

        public IEnumerable<dynamic> Search(int? rootEntryId, string searchText)
        {
            LFSearch search = _Database.CreateSearch();
            try
            {
                //9530

                search.Command = !string.IsNullOrEmpty(searchText) && searchText.StartsWith("{") && searchText.EndsWith("}") ? searchText :
                //"{LF:Basic~=\"" + searchText + "\", option=\"LTN\"}";
                "({ LF: Basic~= \"" + searchText + "\", option = \"LTFN\"} | " + searchText + " | { LF: Name = \"" + searchText + "\", Type = \"DFS\"})";
                if (rootEntryId.HasValue)
                    search.Command = search.Command + " & {LF:Lookin=" + rootEntryId + ", Subfolders=Y}";
                search.BeginSearch(true);
                ILFCollection hits = search.GetSearchHits();
                var result = hits.Cast<dynamic>().Select(x => x.Entry).Select(x => new
                {
                    Id = x.ID,
                    Name = x.Name,
                    EntryType = Enum.GetName(typeof(Entry_Type), x.EntryType),
                    TemplateId = (int?)(x.Template == null ? null : x.Template.ID),
                    x.FullPath
                }).ToList();
                search.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                search.Dispose();
                throw ex;
                //return new List<dynamic>();
            }
        }

        public IEnumerable<dynamic> GetEntries(int? entryId)
        {
            LFFolder rootFolder;
            if (!entryId.HasValue)
            {
                rootFolder = _Database.RootFolder;
            }
            else
            {
                rootFolder = (LFFolder)_Database.GetEntryByID(entryId.Value);
            }
            ILFCollection allFolders = rootFolder.GetChildren();
            List<dynamic> result = new List<dynamic>();
            foreach (var item in allFolders)
            {
                if (((ILFEntry)item).EntryType == Entry_Type.ENTRY_TYPE_FOLDER)
                {
                    result.Add(new
                    {
                        Id = ((ILFEntry)item).ID,
                        Name = ((ILFEntry)item).Name,
                        EntryType = Enum.GetName(typeof(Entry_Type), ((ILFEntry)item).EntryType),
                        ((ILFEntry)item).FullPath
                    });
                }
            }
            return result;
        }

        public IEnumerable<dynamic> GetEntriesForTransAttachments(string folderEntryIds, string searchText)
        {
            if (string.IsNullOrEmpty(folderEntryIds))
            {
                return new List<dynamic>();
            }
            LFFolder rootFolder;
            List<dynamic> result = new List<dynamic>();
            if (folderEntryIds.Contains(','))
            {
                foreach (var rootEntryId in folderEntryIds.Split(','))
                {
                    if (string.IsNullOrEmpty(rootEntryId))
                        continue;
                    try
                    {
                        rootFolder = (LFFolder)_Database.GetEntryByID(Convert.ToInt32(rootEntryId));
                        if (string.IsNullOrEmpty(searchText) || ((ILFEntry)rootFolder).Name.Contains(searchText))
                        {
                            result.Add(new
                            {
                                Id = ((ILFEntry)rootFolder).ID,
                                Name = ((ILFEntry)rootFolder).Name,
                                EntryType = (bool)rootFolder.IsShortcut ? (((LFShortcut)rootFolder).EntryReferenced is LFFolder ? Enum.GetName(typeof(Entry_Type), Entry_Type.ENTRY_TYPE_FOLDER) : Enum.GetName(typeof(Entry_Type), rootFolder.EntryType)) : Enum.GetName(typeof(Entry_Type), rootFolder.EntryType),
                                ((ILFEntry)rootFolder).FullPath,
                                pagesCount = GetPagesCount(((ILFEntry)rootFolder).ID)
                            });
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                foreach (var rootEntryId in folderEntryIds.Split(','))
                {
                    if (string.IsNullOrEmpty(rootEntryId))
                        continue;
                    try
                    {
                        int entryID = 0;
                        string entryType = "";
                        ILFEntry lfEntry = (ILFEntry)_Database.GetEntryByID(Convert.ToInt32(rootEntryId));
                        if ((bool)((ILFEntry)lfEntry).IsShortcut)
                        {
                            rootFolder = (LFFolder)((LFShortcut)lfEntry).EntryReferenced;
                        }
                        else
                        {
                            rootFolder = (LFFolder)lfEntry;
                        }
                        ILFCollection allFolderEntry = rootFolder.GetChildren();
                        foreach (var item in allFolderEntry)
                        {
                            entryID = 0;
                            entryType = "";
                            if ((bool)((ILFEntry)item).IsShortcut)
                            {
                                if (((LFShortcut)item).EntryReferenced is LFFolder)
                                {
                                    entryID = ((LFFolder)((LFShortcut)item).EntryReferenced).ID;
                                    entryType = Enum.GetName(typeof(Entry_Type), Entry_Type.ENTRY_TYPE_FOLDER);
                                }
                                else if (((LFShortcut)item).EntryReferenced is LFDocument)
                                {
                                    entryID = ((LFDocument)((LFShortcut)item).EntryReferenced).ID;
                                    entryType = Enum.GetName(typeof(Entry_Type), Entry_Type.ENTRY_TYPE_DOCUMENT);
                                }
                            }
                            else
                            {
                                entryID = ((ILFEntry)item).ID;
                                entryType = Enum.GetName(typeof(Entry_Type), ((ILFEntry)item).EntryType);
                            }
                            if (string.IsNullOrEmpty(searchText) || ((ILFEntry)item).Name.Contains(searchText))
                            {
                                if (entryID != 0 && !string.IsNullOrEmpty(entryType))
                                {
                                    result.Add(new
                                    {
                                        Id = entryID,
                                        Name = ((ILFEntry)item).Name,
                                        //EntryType = Enum.GetName(typeof(Entry_Type), ((ILFEntry)item).EntryType),
                                        EntryType = entryType,
                                        ((ILFEntry)item).FullPath
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            //LFFolder folder = fEntry.IsShortcut ? ((LFShortcut)fEntry).EntryReferenced : (LFFolder)fEntry;
            return result;
        }

        public void Rename(int entryId, string name)
        {
            if (entryId != 0)
            {
                dynamic entry = _Database.GetEntryByID(entryId);
                try
                {
                    entry.Name = name;
                    entry.Dispose();
                }
                catch (Exception ex)
                {
                    entry.Dispose();
                    throw ex;
                }
            }
        }

        public bool RotatePage(int entryID, int pageNumber, int rotation)
        {
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"RotateImage\"" + " \"" + entryID + "\" \"" + pageNumber + "\" \"" + rotation + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    return true;
                }
                else
                {
                    //write here
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Delete(int entryId)
        {
            if (entryId != 0)
            {
                dynamic entry = _Database.GetEntryByID(entryId);
                try
                {
                    entry.Delete();
                    entry.Dispose();
                }
                catch (Exception ex)
                {
                    entry.Dispose();
                    throw ex;
                }
            }
        }


        public bool DeletePage(string entryId, int pageIndex)
        {
            if (!string.IsNullOrEmpty(entryId))
            {
                LFDocument document = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(entryId));
                try
                {
                    document.LockObject(Lock_Type.LOCK_TYPE_WRITE);
                    LFDocumentPages DocPages = document.Pages;
                    DocPages.MarkPageByIndex(pageIndex);
                    DocPages.DeleteMarkedPages();
                    document.UnlockObject();
                    document.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    document.Dispose();
                    return false;
                }
            }
            return false;
        }

        public bool MovePageTo(string entryId, int pageIndex, int moveLocation)
        {
            if (!string.IsNullOrEmpty(entryId))
            {
                //DocumentInfo docInfo = Laserfiche.RepositoryAccess.Document.GetDocumentInfo(Convert.ToInt32(entryId), Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection));
                //try
                //{
                //    docInfo.Lock(LockType.Exclusive);
                //    if (moveLocation > pageIndex)
                //        moveLocation++;
                //    docInfo.MovePagesTo(new PageRange(pageIndex, pageIndex), docInfo, moveLocation);
                //    docInfo.Save();
                //    docInfo.Unlock();
                //    docInfo.Dispose();
                //    return true;
                //}
                //catch (Exception ex)
                //{
                //    docInfo.Dispose();
                //    return false;
                //}
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"MovePageTo\"" + " \"" + entryId + "\" \"" + pageIndex + "\" \"" + moveLocation + "\"";
                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();
                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    return true;
                }
            }
            return false;
        }

        public string SplitDocument(string sourceID, int fromPageIndex, int toPageIndex)
        {
            if (!string.IsNullOrEmpty(sourceID))
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    string targetID = "";
                    using (Process compiler = new Process())
                    {
                        LFDocument sourceDoc = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(sourceID));
                        LFFolder parentEntry = sourceDoc.ParentFolder;

                        LFDocument targetDoc = new LFDocument();
                        targetDoc.Create(sourceDoc.Name, parentEntry, parentEntry.DefaultVolume, true);
                        targetID = Convert.ToString(targetDoc.ID);

                        targetDoc.Dispose();
                        parentEntry.Dispose();
                        sourceDoc.Dispose();

                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"SplitDocument\"" + " \"" + sourceID + "\" \"" + targetID + "\" \"" + fromPageIndex + "\" \"" + toPageIndex + "\"";

                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();
                        //compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                    }
                    return targetID;
                }
            }
            return null;
        }

        public dynamic Move(int entryId, int parentEntryId)
        {
            object result = null;
            if (entryId != 0)
            {
                dynamic entry = _Database.GetEntryByID(entryId);
                var parentEntry = (LFFolder)_Database.GetEntryByID(parentEntryId);
                try
                {
                    entry.Move(parentEntry, true);
                    result = new
                    {
                        Id = entry.ID,
                        Name = entry.Name,
                        EntryType = Enum.GetName(typeof(Entry_Type), entry.EntryType),
                        TemplateId = (entry.Template == null ? (int?)null : entry.Template.ID),
                        entry.FullPath
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    entry.Dispose();
                    parentEntry.Dispose();
                }
            }
            return result;
        }

        public dynamic Copy(int entryId, int parentEntryId)
        {
            object result = null;
            if (entryId != 0)
            {
                dynamic entry = _Database.GetEntryByID(entryId);
                var parentEntry = (LFFolder)_Database.GetEntryByID(parentEntryId);
                if (entry as LFFolder != null) // entry is folder
                {
                    var NewFolder = new LFFolder();
                    try
                    {
                        NewFolder.CreateCopyOf(entry, entry.Name, parentEntry, true);
                        result = new
                        {
                            Id = NewFolder.ID,
                            Name = NewFolder.Name,
                            EntryType = Enum.GetName(typeof(Entry_Type), NewFolder.EntryType),
                            TemplateId = (NewFolder.Template == null ? (int?)null : NewFolder.Template.ID),
                            NewFolder.FullPath
                        };
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        NewFolder.Dispose();
                    }
                }
                else if (entry as LFDocument != null) // entry is document
                {
                    var NewDocument = new LFDocument();
                    try
                    {
                        NewDocument.CreateCopyOf(entry, entry.Name, parentEntry, true);
                        result = new
                        {
                            Id = NewDocument.ID,
                            Name = NewDocument.Name,
                            EntryType = Enum.GetName(typeof(Entry_Type), NewDocument.EntryType),
                            TemplateId = (NewDocument.Template == null ? (int?)null : NewDocument.Template.ID),
                            NewDocument.FullPath
                        };
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        NewDocument.Dispose();
                    }
                }
                entry.Dispose();
                parentEntry.Dispose();
            }
            return result;
        }

        public IEnumerable<dynamic> GetTemplates()
        {
            return _Database.GetAllTemplates().Cast<dynamic>().Select(x => new { Id = x.ID, x.Name, x.Description });
        }

        public List<string> GetTemplatesNames()
        {
            List<LFTemplate> result = new List<LFTemplate>();
            var templates = _Database.GetAllTemplates();
            for (int i = 1; i <= templates.Count; i++)
            {
                result.Add((LFTemplate)templates[i]);
            }

            List<string> resultNames = new List<string>();

            resultNames = (from item in result select item.Name).ToList();
            return resultNames;
        }

        public int getTemplateIDByName(string temName)
        {
            try
            {
                LFTemplate myTemp = _Database.GetTemplateByName(temName);
                return myTemp.ID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public IEnumerable<dynamic> GetTemplateFields(int entryId, int templateId)
        {
            var result = new List<LFTemplateField>();
            dynamic entry = _Database.GetEntryByID(entryId);
            var template = _Database.GetTemplateByID(templateId);
            try
            {
                for (int i = 1; i <= template.Count; i++)
                {
                    result.Add((LFTemplateField)template.Item[i]);
                }

                return result
                    .Where(x => !x.IsDeleted)
                    .Select(x => new
                    {
                        x.Name,
                        Type = Enum.GetName(typeof(Field_Type), x.Type),
                        Format = Enum.GetName(typeof(Field_Format), x.Format),
                        x.FormatPattern,
                        x.MultiValued,
                        x.Regex,
                        x.RegexError,
                        x.Required,
                        x.Size,
                        Value = entry.FieldData.Field[x.Name]
                    });
            }
            catch { }
            finally
            {
                if (entry != null)
                {
                    entry.Dispose();
                }
            }
            return null;
        }

        public void SetTemplateFields(int entryId, int templateId, List<dynamic> templateFields)
        {
            dynamic entry = _Database.GetEntryByID(entryId);
            var fieldData = (LFFieldData)entry.FieldData;
            try
            {
                fieldData.LockObject(Lock_Type.LOCK_TYPE_WRITE);
                fieldData.Template = _Database.GetTemplateByID(templateId);
                foreach (var templateField in templateFields)
                {
                    fieldData.Field[templateField.Name] = templateField.Value;
                }
                fieldData.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fieldData.UnlockObject();
                entry.Dispose();
            }
        }

        public void CreateMASARRepositryFolderIfNotExist(string filePath)
        {
            string[] folderPath = filePath.Split(new string[] { "\\" }, StringSplitOptions.None);
            if (this._AppSettings.DocumentSettings.ECMType == "1")
            {
                if (!CheckExistFolder(this._AppSettings.DocumentSettings.ECMRepository + "\\" + folderPath[1]))
                {
                    // Instantiates new document object.
                    LFFolder fol = new LFFolder();
                    LFFolder parentFol = (LFFolder)_Database.GetEntryByPath(this._AppSettings.DocumentSettings.ECMRepository);
                    // Creates the document in the repository.
                    fol.Create(folderPath[1], parentFol, true);
                    fol.Dispose();
                }

                if (!CheckExistFolder(this._AppSettings.DocumentSettings.ECMRepository + "\\" + folderPath[1] + "\\" + folderPath[2]))
                {
                    // Instantiates new document object.
                    LFFolder fol = new LFFolder();
                    LFFolder parentFol = (LFFolder)_Database.GetEntryByPath(this._AppSettings.DocumentSettings.ECMRepository + "\\" + folderPath[1]);
                    // Creates the document in the repository.
                    fol.Create(folderPath[2], parentFol, true);
                    fol.Dispose();
                }

                if (folderPath.Length > 3)
                {
                    if (!CheckExistFolder(this._AppSettings.DocumentSettings.ECMRepository + "\\" + folderPath[1] + "\\" + folderPath[2] + "\\" + folderPath[3]))
                    {
                        // Instantiates new document object.
                        LFFolder fol = new LFFolder();
                        LFFolder parentFol = (LFFolder)_Database.GetEntryByPath(this._AppSettings.DocumentSettings.ECMRepository + "\\" + folderPath[1] + "\\" + folderPath[2]);
                        // Creates the document in the repository.
                        fol.Create(folderPath[3], parentFol, true);
                        fol.Dispose();
                    }
                }
            }
            else if (this._AppSettings.DocumentSettings.ECMType == "2")
            {
                //write Code For FileNet
                if (!CheckExistFolder("\\" + folderPath[0]))
                {

                    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\", null);
                    IFolder nf = null;
                    //if (className.Equals(""))
                    //    nf = Factory.Folder.CreateInstance(os, null);
                    //else
                    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    nf.FolderName = folderPath[0];
                    nf.Parent = f;
                    //return nf;
                    nf.Save(RefreshMode.REFRESH);
                }

                if (!CheckExistFolder("\\" + folderPath[0] + "\\" + folderPath[1]))
                {
                    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\" + folderPath[0], null);
                    IFolder nf = null;
                    //if (className.Equals(""))
                    //    nf = Factory.Folder.CreateInstance(os, null);
                    //else
                    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    nf.FolderName = folderPath[1];
                    nf.Parent = f;
                    //return nf;
                    nf.Save(RefreshMode.REFRESH);
                }

                if (!CheckExistFolder("\\" + folderPath[0] + "\\" + folderPath[1] + "\\" + folderPath[2]))
                {
                    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\" + folderPath[0] + "\\" + folderPath[1], null);
                    IFolder nf = null;
                    //if (className.Equals(""))
                    //    nf = Factory.Folder.CreateInstance(os, null);
                    //else
                    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    nf.FolderName = folderPath[2];
                    nf.Parent = f;
                    //return nf;
                    nf.Save(RefreshMode.REFRESH);
                }
            }
        }

        private bool CheckExistFolder(string passesPath)
        {
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType == "1")
                {
                    LFFolder parent = (LFFolder)_Database.GetEntryByPath(passesPath);
                    return true;
                }
                else if (this._AppSettings.DocumentSettings.ECMType == "2")
                {
                    //write Code For FileNet                    
                    //IDocument doc = Factory.Document.FetchInstance(FNObjectStoreSingleTon.FNDatabaseInstance(), passesPath, null);
                    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), passesPath, null);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetFolderName(int entryId = 0)
        {
            try
            {
                var folderEntry = (LFFolder)_Database.GetEntryByID(entryId);
                return folderEntry.FullPath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /*FN Helper Functions*/

        //
        // Creates the Document with or without content.
        //
        public static IDocument FNCreateDocument(bool withContent, String file, String mimeType, String docName, IObjectStore os, String className)
        {
            IDocument doc = null;
            if (className.Equals(""))
                doc = Factory.Document.CreateInstance(os, null);
            else
                doc = Factory.Document.CreateInstance(os, className);
            doc.Properties["DocumentTitle"] = docName;
            doc.MimeType = mimeType;
            if (withContent == true)
            {
                IContentElementList cel = CreateContentElementList(file);
                if (cel != null)
                    doc.ContentElements = cel;
            }
            return doc;
        }

        //
        // Creates the ContentElementList from ContentTransfer object.
        //
        public static IContentElementList CreateContentElementList(String fileName)
        {
            IContentElementList cel = null;
            if (CreateContentTransfer(fileName) != null)
            {
                cel = Factory.ContentElement.CreateList();
                IContentTransfer ct = CreateContentTransfer(fileName);
                cel.Add(ct);
            }
            return cel;
        }

        //
        // Creates the ContentTransfer object from supplied file's
        // content.
        //
        public static IContentTransfer CreateContentTransfer(String fileName)
        {
            IContentTransfer ct = null;
            FileInfo fi = new FileInfo(fileName);
            if (ReadContentFromFile(fileName) != null)
            {
                ct = Factory.ContentTransfer.CreateInstance();
                Stream s = new MemoryStream(ReadContentFromFile(fileName));
                ct.SetCaptureSource(s);
                ct.RetrievalName = fi.Name;
            }
            return ct;
        }

        //
        // Reads the content from a file and stores it
        // in a byte array. The byte array will later be
        // used to create ContentTransfer object.
        //
        public static byte[] ReadContentFromFile(String fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            long numBytes = fi.Length;
            byte[] buffer = null;
            if (numBytes > 0)
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    buffer = br.ReadBytes((int)numBytes);
                    br.Close();
                    fs.Close();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.StackTrace);
                }
            }
            return buffer;
        }

        public static IDocument FetchDocByID(IObjectStore os, String id)
        {
            Id ID = new Id(id);
            IDocument doc = Factory.Document.FetchInstance(os, ID, null);
            return doc;
        }

        public static void WriteContentToFile(IDocument doc, String path)
        {

            String fileName = Convert.ToString(doc.Id);
            String file = Path.Combine(path, fileName);
            try
            {
                FileStream fs = new FileStream(file, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);
                Stream s = doc.AccessContentStream(0);
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                s.Close();
                bw.Write(data);
                bw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
            }
        }

        public static IReferentialContainmentRelationship FileContainable(IObjectStore os, IContainable c, String folder)
        {
            IFolder f = Factory.Folder.FetchInstance(os, folder, null);
            IReferentialContainmentRelationship rcr = null;
            if (c is IDocument)
                rcr = f.File((IDocument)c, AutoUniqueName.AUTO_UNIQUE, ((IDocument)c).Name, DefineSecurityParentage.DO_NOT_DEFINE_SECURITY_PARENTAGE);
            else
                rcr = f.File((ICustomObject)c, AutoUniqueName.AUTO_UNIQUE, ((ICustomObject)c).Name, DefineSecurityParentage.DO_NOT_DEFINE_SECURITY_PARENTAGE);
            return rcr;
        }

        private int GetTifPagesCount(string filePath)
        {
            int pageCount = 0;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (System.Drawing.Image temp = System.Drawing.Image.FromStream(fs))
                    {
                        pageCount = temp.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                    }
                }
            }
            catch (Exception ex)
            {
                return pageCount;
            }
            return pageCount;
        }

        public byte[] tifImageToTiffByteArray(string filePath, int pageIndex)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (System.Drawing.Image temp = System.Drawing.Image.FromStream(fs))
                {
                    temp.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, pageIndex - 1); // going to the selected page
                    Bitmap myBmp = new Bitmap(temp, temp.Width, temp.Height); // setting the new page as an image 
                    MemoryStream ms = new MemoryStream();
                    myBmp.Save(ms, ImageFormat.Tiff);
                    return ms.ToArray();
                }
            }
        }

        public byte[] tifImageToPngByteArray(string filePath, int pageIndex)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (System.Drawing.Image temp = System.Drawing.Image.FromStream(fs))
                {
                    temp.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, pageIndex - 1); // going to the selected page
                    Bitmap myBmp = new Bitmap(temp, temp.Width, temp.Height); // setting the new page as an image 
                    MemoryStream ms = new MemoryStream();
                    myBmp.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public string CreateDocumentTransaction(int parentEntryId, int TransactionId, int TransactionTypeId, string fileName, string contentType, byte[] binaryContent, out int pagesCount, Transaction _Transaction)
        {
            //string yearValue = DateTime.Now.ToString("yyyy");
            //if (this._AppSettings.DocumentSettings.YearIsHijri.Equals("1"))
            //{
            //    CultureInfo HijriCI = new CultureInfo("ar-SA");
            //    yearValue = DateTime.Now.ToString("yyyy", HijriCI);
            //}
            if (!string.IsNullOrEmpty(_Transaction?.IncomingLetterNumber))
            {
                TransactionTypeId = 2;
            }
            else
            {
                TransactionTypeId = 1;
            }


            string result = "0";
            pagesCount = 0;
            LFFolder parentEntry = null;
            LFDocument NewDocument = null;
            try
            {
                if (this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    parentEntry = new LFFolder();
                    parentEntry = (LFFolder)_Database.GetEntryByPath(GetSavedECMFolder(true, TransactionTypeId, _Transaction?.TransactionDate));

                    if (TransactionId != 0 && TransactionTypeId != 0)
                    {
                        if (!CheckExistFolder(GetSavedECMFolder(false, TransactionTypeId, _Transaction?.TransactionDate) + "\\" + TransactionId))
                        {
                            LFFolder parentFol = (LFFolder)_Database.GetEntryByPath(GetSavedECMFolder(false, TransactionTypeId, _Transaction?.TransactionDate));
                            // Creates the document in the repository.
                            parentEntry.Create(Convert.ToString(TransactionId), parentFol, true);
                            parentEntry.Dispose();
                        }
                        else
                        {
                            parentEntry = (LFFolder)_Database.GetEntryByPath(GetSavedECMFolder(false, TransactionTypeId, _Transaction?.TransactionDate) + "\\" + TransactionId);
                        }
                    }

                    NewDocument = new LFDocument();
                    NewDocument.Create(fileName, parentEntry, parentEntry.DefaultVolume, true);
                    var DocumentImporter = new DocumentImporter();
                    DocumentImporter.Document = NewDocument;
                    DocumentImporter.PageAction = Import_Page_Action.IMPORT_PAGE_ACTION_INSERT;
                    DocumentImporter.PageIndex = 1;
                    if (contentType.StartsWith("image"))
                    {
                        DocumentImporter.ImportImages(binaryContent);
                    }
                    else if (contentType.StartsWith("text"))
                    {
                        DocumentImporter.ImportText(System.Text.Encoding.UTF8.GetString(binaryContent));
                    }
                    else
                    {
                        DocumentImporter.ImportElectronicFileFromMemory(binaryContent, fileName.Split('.').LastOrDefault());
                    }
                    result = Convert.ToString(NewDocument.ID);
                    pagesCount = NewDocument.PageCount;

                }
                else if (this._AppSettings.DocumentSettings.ECMType.Equals("2"))
                {
                    //if (TransactionId != 0 && TransactionTypeId != 0)
                    //{
                    #region commented
                    //// Create Transaction Folder In case of not exist
                    //if (!CheckExistFolder("\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId))
                    //{
                    //    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming"), null);
                    //    IFolder nf = null;
                    //    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    //    nf.FolderName = Convert.ToString(TransactionId);
                    //    nf.Parent = f;
                    //    //return nf;
                    //    nf.Save(RefreshMode.REFRESH);
                    //    //FolderID = f.Id;
                    //}
                    ////RepostryFilePath = "\\" + IncommingLfPath + "\\" + FilePath;
                    //string mimeType = "";
                    //if (contentType.Contains(".tif"))
                    //{
                    //    mimeType = "image/tiff";
                    //}

                    //IDocument doc = null;
                    //doc = FNCreateDocument(true, _RootPath + doc.Id, mimeType, fileName, FNDatabaseInstance(), "Document");
                    //doc.Save(RefreshMode.REFRESH);
                    //String folder = "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId;
                    //if (folder.Length == 0)
                    //    folder = "/";
                    //IReferentialContainmentRelationship rcr = FileContainable(FNDatabaseInstance(), doc, folder);
                    //rcr.Save(RefreshMode.REFRESH);
                    ////var result = doc.Id;
                    ////EntryID = result;
                    ////return result;
                    //result = Convert.ToString(doc.Id);
                    ////TODO: get tif page count
                    //pagesCount = GetTifPagesCount(_RootPath + doc.Id);
                    #endregion

                    #region commented
                    //ImageFormat format = ImageFormat.Tiff;
                    //string importTempFilePath = string.Format(Path.GetFullPath(this._AppSettings.DocumentSettings.RelativeDirectory + "\\" + fileName.Split('.').FirstOrDefault() + "_" + DateTime.Now.Ticks.ToString() + ".{0}"), format.ToString());
                    //byte[] byteArray = binaryContent; // Put the bytes of the image here....
                    //Image resultImage = null;
                    //resultImage = new Bitmap(new MemoryStream(byteArray));
                    //using (Image imageToExport = resultImage)
                    //{
                    //    imageToExport.Save(importTempFilePath, format);
                    //}

                    //foreach (var formFile in FileNetFiles)
                    //{
                    //    if (formFile.Length > 0)
                    //    {
                    //        using (var stream = new FileStream(importTempFilePath, FileMode.CreateNew))
                    //        {
                    //            formFile.CopyTo(stream);
                    //        }
                    //    }
                    //}
                    #endregion

                    fileName = new Random().Next(1, 999999) + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(TransactionId) + "_" + fileName;
                    string importTempFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Documents", fileName);
                    File.WriteAllBytes(importTempFilePath, binaryContent);

                    using (Process compiler = new Process())
                    {
                        compiler.StartInfo.FileName = Path.Combine(_hostingEnvironment.ContentRootPath, "exe", "FileNetIntegration.exe");
                        compiler.StartInfo.Arguments = "\"CreateDocument\"" + " \"" + TransactionId + "\" \"" + TransactionTypeId + "\" \"" + fileName + "\" \"" + contentType + "\" \"" + importTempFilePath /*+ "\""*/
                            + "\" \"" + Convert.ToString(_Transaction?.TransactionNumber) + "\" \"" + Convert.ToString(_Transaction?.TransactionDate)
                            + "\" \"" + Convert.ToString(_Transaction?.Subject)?.Replace("\"", "\\\"") + "\" \"" + Convert.ToString(_Transaction?.IncomingLetterNumber)?.Replace("\"", "\\\"")
                            + "\" \"" + Convert.ToString(_Transaction?.IncomingLetterDate) + "\" \"" + Convert.ToString(_Transaction?.IncomingOrganization?.OrganizationNameAr)?.Replace("\"", "\\\"") + "\"";

                        compiler.StartInfo.UseShellExecute = false;
                        compiler.StartInfo.RedirectStandardOutput = true;
                        compiler.Start();

                        result = compiler.StandardOutput.ReadLine();
                        compiler.WaitForExit();
                        pagesCount = GetTifPagesCount(importTempFilePath);
                    }
                    try { lock (CleanDocumentsLocker) { Task.Run(() => File.Delete(importTempFilePath)); } } catch { }

                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (parentEntry != null)
                    parentEntry.Dispose();
                if (NewDocument != null)
                    NewDocument.Dispose();
            }
            return result.ToString();
        }
    }
}