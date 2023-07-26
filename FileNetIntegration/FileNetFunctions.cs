using FileNet.Api.Authentication;
using FileNet.Api.Collection;
using FileNet.Api.Constants;
using FileNet.Api.Core;
using FileNet.Api.Property;
using FileNet.Api.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace FileNetIntegration
{
    class FileNetFunctions
    {
        private static IObjectStore os = null;
        private static object padlock = new object();

        public FileNetFunctions()
        {
            string yearValue = DateTime.Now.ToString("yyyy");
            if (Properties.Settings.Default.YearIsHijri.Equals("1"))
            {
                CultureInfo HijriCI = new CultureInfo("ar-SA");
                yearValue = DateTime.Now.ToString("yyyy", HijriCI);
            }
            FNDatabaseInstance();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.RelativeDirectory))
            {
                CreateMASARRepositryFolderIfNotExist(Properties.Settings.Default.RelativeDirectory);
            }
            CreateMASARRepositryFolderIfNotExist(GetRelativeDirectoryByDelim() + "MASAR\\" + yearValue + "\\" + "Incoming");
            CreateMASARRepositryFolderIfNotExist(GetRelativeDirectoryByDelim() + "MASAR\\" + yearValue + "\\" + "Outgoing");
        }



        public IObjectStore FNDatabaseInstance()
        {
            if (os == null)
            {
                lock (padlock)
                {
                    UsernameCredentials cred = new UsernameCredentials(Properties.Settings.Default.ECMUserName, Properties.Settings.Default.ECMPassword);
                    // now associate this Credentials with the whole process
                    ClientContext.SetProcessCredentials(cred);
                    IConnection connection = Factory.Connection.GetConnection(Properties.Settings.Default.ECMServer);
                    //isCredentialsEstablished = true;
                    //IntializeVariables(connection);
                    IDomain domain = Factory.Domain.FetchInstance(connection, null, null);
                    os = Factory.ObjectStore.FetchInstance(domain, Properties.Settings.Default.ECMRepository, null);
                    //Console.WriteLine("Connected");
                }
            }
            return os;
        }

        public void CreateMASARRepositryFolderIfNotExist(string filePath)
        {
            string[] folderPath = filePath.Split(new string[] { "\\" }, StringSplitOptions.None);

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
            if (folderPath.Length < 2)
                return;

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

            if (folderPath.Length > 3)
            {
                if (!CheckExistFolder("\\" + folderPath[0] + "\\" + folderPath[1] + "\\" + folderPath[2] + "\\" + folderPath[3]))
                {
                    IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), "\\" + folderPath[0] + "\\" + folderPath[1] + "\\" + folderPath[2], null);
                    IFolder nf = null;
                    //if (className.Equals(""))
                    //    nf = Factory.Folder.CreateInstance(os, null);
                    //else
                    nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                    nf.FolderName = folderPath[3];
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
                //write Code For FileNet                    
                //IDocument doc = Factory.Document.FetchInstance(FNObjectStoreSingleTon.FNDatabaseInstance(), passesPath, null);
                IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), passesPath, null);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public string CreateFolder(string name, string parentPath = "")
        {
            string result = "0";
            try
            {
                IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), parentPath/*"\\" + IncommingLfPath*/, null);
                IFolder nf = null;
                nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                nf.FolderName = name;
                nf.Parent = f;
                nf.Save(RefreshMode.REFRESH);
                result = Convert.ToString(f.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private string GetRelativeDirectory()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.RelativeDirectory))
                return "\\" + Properties.Settings.Default.RelativeDirectory;
            else
                return "";
        }

        private string GetRelativeDirectoryByDelim()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.RelativeDirectory))
                return Properties.Settings.Default.RelativeDirectory + "\\";
            else
                return "";
        }

        public string CreateDocument(int TransactionId, int TransactionTypeId, string fileName, string contentType, string filePath
            , string TransactionNumber, string TransactionDate, string TransactionSubject, string IncommingLetterNumber, string IncommingLetterDate, string IncommingLetterOrganization)
        {
            string yearValue = DateTime.Now.ToString("yyyy");
            if (Properties.Settings.Default.YearIsHijri.Equals("1"))
            {
                CultureInfo HijriCI = new CultureInfo("ar-SA");
                yearValue = DateTime.Now.ToString("yyyy", HijriCI);
            }

            string result = "0";
            //pagesCount = 0;
            try
            {

                //if (TransactionId != 0 && TransactionTypeId != 0)
                //{
                // Create Transaction Folder In case of not exist
                if (TransactionId != 0)
                {
                    if (!CheckExistFolder(GetRelativeDirectory() + "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId))
                    {
                        IFolder f = Factory.Folder.FetchInstance(FNDatabaseInstance(), GetRelativeDirectory() + "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming"), null);
                        IFolder nf = null;
                        nf = Factory.Folder.CreateInstance(FNDatabaseInstance(), "Folder");
                        nf.FolderName = Convert.ToString(TransactionId);
                        nf.Parent = f;
                        //return nf;
                        nf.Save(RefreshMode.REFRESH);
                        //FolderID = f.Id;
                    }
                }
                //RepostryFilePath = "\\" + IncommingLfPath + "\\" + FilePath;
                string mimeType = "";
                if (contentType.Contains("tif"))
                {
                    mimeType = "image/tiff";
                }

                IDocument doc = null;
                doc = FNCreateDocument(true, filePath, mimeType, fileName, FNDatabaseInstance(), "Document");
                doc.Save(RefreshMode.REFRESH);

                String folder = "";
                if (TransactionId != 0)
                {
                    folder = GetRelativeDirectory() + "\\MASAR\\" + yearValue + "\\" + (TransactionTypeId == 1 ? "Outgoing" : "Incoming") + "\\" + TransactionId;
                }
                else
                {
                    folder = GetRelativeDirectory() + "\\MASAR\\" + yearValue;
                }

                if (folder.Length == 0)
                    folder = "/";
                IReferentialContainmentRelationship rcr = FileContainable(FNDatabaseInstance(), doc, folder);
                rcr.Save(RefreshMode.REFRESH);
                //var result = doc.Id;
                //EntryID = result;
                //return result;
                result = Convert.ToString(doc.Id).Replace("{", "").Replace("}", "");
                //TODO: get tif page count
                //pagesCount = GetTifPagesCount(filePath + doc.Id);


                doc.ChangeClass(Properties.Settings.Default.DocumentClass);
                // Return document properties.
                IProperties props = doc.Properties;
                // Change property value.
                doc.Properties["TransactionNumber"] = TransactionNumber;
                doc.Properties["TransactionDate"] = TransactionDate;
                doc.Properties["TransactionSubject"] = TransactionSubject.Length > 60 ? TransactionSubject.Substring(0, 60) : TransactionSubject;
                doc.Properties["IncommingLetterNumber"] = IncommingLetterNumber;
                doc.Properties["IncommingLetterDate"] = IncommingLetterDate;
                doc.Properties["IncommingLetterOrganization"] = IncommingLetterOrganization;
                //
                doc.Checkin(AutoClassify.DO_NOT_AUTO_CLASSIFY, CheckinType.MAJOR_VERSION);
                doc.Save(RefreshMode.REFRESH);

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            //Console.Clear();
            return result.ToString();
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
            //doc.ChangeClass(className);
            //doc.Save(RefreshMode.REFRESH);
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


        public void DocToWriteContentToFile(string Id, string filePath)
        {
            IDocument doc = null;
            doc = FetchDocByID(FNDatabaseInstance(), Id);
            WriteContentToFile(doc, filePath);
        }

        public static void WriteContentToFile(IDocument doc, string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.CreateNew);
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
                    using (Image temp = Image.FromStream(fs))
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

        public byte[] tifImageToByteArray(string filePath, int pageIndex)
        {
            using (Image imageFile = Image.FromFile(filePath))
            {
                FrameDimension frameDimensions = new FrameDimension(imageFile.FrameDimensionsList[0]);
                // Gets the number of pages from the tiff image (if multipage) 
                //int frameNum = imageFile.GetFrameCount(frameDimensions);
                // Selects one frame at a time and save as jpeg. 
                imageFile.SelectActiveFrame(frameDimensions, pageIndex);
                using (Bitmap bmp = new Bitmap(imageFile))
                {
                    MemoryStream stream = new MemoryStream();
                    bmp.Save(stream, ImageFormat.Tiff);
                    return stream.ToArray();
                }
            }

            //MemoryStream ms = new MemoryStream();
            //System.Drawing.Image imageIn = new System.Drawing.Image();
            //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
            //return ms.ToArray();
        }

        /**/



    }
}
