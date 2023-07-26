using Laserfiche.RepositoryAccess;
using LFSO102Lib;
using PdfExporter;
using System;
using System.IO;

namespace FileNetIntegration
{
    class LFFunctions
    {
        private LFDatabase _BaseDatabase;
        private static object padlock = new object();

        public LFFunctions()
        {
            ResetBaseDatabaseWithNewConnection();
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

        private void ResetBaseDatabaseWithNewConnection()
        {
            LFApplication application = new LFApplication();
            LFServer server = application.GetServerByName(Properties.Settings.Default.LFServer);
            LFDatabase database = server.GetDatabaseByName(Properties.Settings.Default.LFRepository);
            LFConnection connection = new LFConnection();
            connection.UserName = Properties.Settings.Default.LFUserName;
            connection.Password = Properties.Settings.Default.LFPassword;
            connection.Create(database);
            _BaseDatabase = database;
        }

        public void DisposeConnection()
        {
            _BaseDatabase.CurrentConnection.Terminate();
            _BaseDatabase = null;
        }

        public void LFDownloadPdf(string Id, string filePath)
        {
            byte[] binaryContent = null;

            LFDocument document = (LFDocument)_Database.GetEntryByID(Convert.ToInt32(Id));
            var documentName = document.Name;
            //fileName = (documentName.ToLower().EndsWith(".pdf")) ? documentName : $"{documentName}.pdf";
            var pdfOptions = new ExportOptions();
            pdfOptions.SetLayers(DocumentLayers.All);
            pdfOptions.SetMetadataOptions(MetaDataOptions.All);
            PdfExporter.PdfExporter exporter = new PdfExporter.PdfExporter(pdfOptions);

            binaryContent = exporter.ExportPages(document);

            File.WriteAllBytes(filePath, binaryContent);

            document.Dispose();
            DisposeConnection();
        }

        public bool MovePageTo(string entryId, int pageIndex, int moveLocation)
        {
            if (!string.IsNullOrEmpty(entryId))
            {
                DocumentInfo docInfo = Laserfiche.RepositoryAccess.Document.GetDocumentInfo(Convert.ToInt32(entryId), Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection));
                try
                {
                    docInfo.Lock(LockType.Exclusive);
                    if (moveLocation > pageIndex)
                        moveLocation++;
                    docInfo.MovePagesTo(new PageRange(pageIndex, pageIndex), docInfo, moveLocation);
                    docInfo.Save();
                    docInfo.Unlock();
                    docInfo.Dispose();
                    DisposeConnection();
                    return true;
                }
                catch (Exception ex)
                {
                    docInfo.Dispose();
                    return false;
                }
            }
            return false;
        }

        public bool SplitDocument(string sourceID, string targetID, int fromPageIndex, int toPageIndex)
        {
            try
            {

                // Get references to both the source and target documents...
                DocumentInfo docSource = Laserfiche.RepositoryAccess.Document.GetDocumentInfo(Convert.ToInt32(sourceID), Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection));
                DocumentInfo docTarget = Laserfiche.RepositoryAccess.Document.GetDocumentInfo(Convert.ToInt32(targetID), Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection));

                // Lock them both...
                docSource.Lock(LockType.Exclusive);
                docTarget.Lock(LockType.Exclusive);

                // Move all of the pages from the source document to the target document...
                // NOTE: MovePages method parameters;
                // Parameter1 = page range to copy.  In this example we will copy all of the pages...
                // Parameter2 = the reference to the target document
                // Parameter3 = the destination page number.  In this example we want to move the copied pages to the end of the target document...
                docSource.MovePagesTo(new PageRange(fromPageIndex, toPageIndex), docTarget, docTarget.PageCount + 1);

                // Delete the source document...
                //docSource.Delete();
                docSource.Save();
                docTarget.Save();

                // Unlock the target document...
                docSource.Unlock();
                docTarget.Unlock();

                // Cleanup...
                docTarget.Dispose();
                docSource.Dispose();
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }


        public bool RotatePage(int entryID, int pageNumber, int rotation)
        {
            try
            {
                var LFSession = Session.CreateFromSerializedLFConnection(_Database.CurrentConnection.SerializedConnection);
                DocumentInfo di = Document.GetDocumentInfo(entryID, LFSession);
                //PageRotation rotationAmount = PageRotation.None;
                PageInfo pi = di.GetPageInfo(pageNumber);
                //switch (rotation)
                //{
                //    case 0:
                //        rotationAmount = PageRotation.None;
                //        break;
                //    case 90:
                //        rotationAmount = PageRotation.Clockwise;
                //        break;
                //    case 180:
                //        rotationAmount = PageRotation.UpsideDown;
                //        break;
                //    case 270:
                //        rotationAmount = PageRotation.Counterclockwise;
                //        break;
                //}
                pi.ImageRotationAngle = pi.ImageRotationAngle + rotation;
                pi.Save();
                di.Save();
                DisposeConnection();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /**/
    }
}
