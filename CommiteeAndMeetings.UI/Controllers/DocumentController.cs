using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using HelperServices;
using IHelperServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Models;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : _BaseController<SavedAttachment, DocumentDTO>
    {
        private readonly IDocumentService _IDocumentServices;
        public AppSettings _AppSettings;
        private readonly IHelperServices.ISessionServices _SessionServices;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IMeetingService meetingService;
        IUnitOfWork _unitOfWork;
        public DocumentController(IUnitOfWork unitOfWork,IDocumentService businessService, 
                                    IHelperServices.ISessionServices sessionSevices,
                                    IOptions<AppSettings> appSettings,
                                    ISystemSettingsService systemSettingsService,
                                    IMeetingService meetingService,
                                    IHostingEnvironment environment) : base(businessService, sessionSevices)
        {
            _unitOfWork = unitOfWork;
            _IDocumentServices = businessService;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            _SessionServices = sessionSevices;
            _systemSettingsService = systemSettingsService;
            this.meetingService = meetingService;
            Hosting.HostingEnvironment = environment;
        }
        // [Authorize]
        [RequestSizeLimit(6000000000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]
        [HttpPost("upload")]
        public IEnumerable<AttachmentSummaryDTO> Upload()
        {
            string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
            string AddIndexToAttachment = _systemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;
            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            // Get Doc Setting For Office 
            var OfficeSetting = this._AppSettings.DocumentSettings.OfficeSetting;
            bool ActiveOfficeConvertor = false;
            if (OfficeSetting != null)
            {
                if (OfficeSetting == "1") ActiveOfficeConvertor = true; else ActiveOfficeConvertor = false;
            }
            #region Get Additional Parameters
            var xx = Request.Form;
            var AdditionalParameters = Request.Query.Union(Request.Form);
            string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
            string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();

            int AttachmentTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);

            int TransactionTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "TransactionTypeId").Value, out TransactionTypeId);

            int TransactionId;
            //int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "TransactionId").Value, out TransactionId);
            string transactionIdValue = AdditionalParameters.FirstOrDefault(x => x.Key == "TransactionId").Value;
            if (transactionIdValue.Contains(' '))
            {
                transactionIdValue.Replace(" ", "+");
                UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRole =
                _SessionServices.UserIdAndRoleIdAfterDecrypt(transactionIdValue, true);
                TransactionId = UserIdAndUserRole.Id;
            }
            else
            {


            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = 
                _SessionServices.UserIdAndRoleIdAfterDecrypt(AdditionalParameters.FirstOrDefault(x => x.Key == "TransactionId").Value, true);

            TransactionId = UserIdAndUserRoleId.Id;
            }
            #endregion Get Additional Parameters
            var files = Request.Form.Files;
            var attachmentList = new List<AttachmentSummaryDTO>();
            int i = 1;
            foreach (var file in files)
            {
                byte[] BinaryContent = null;
                byte[] OfficeBinaryContent = null;

                using (var binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
                {
                    BinaryContent = binaryReader.ReadBytes((int)file.Length);
                }
                if ((Path.GetExtension(file.FileName).ToLower() == ".docx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, Path.GetExtension(file.FileName));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if ((Path.GetExtension(file.FileName).ToLower() == ".xlsx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (Path.GetExtension(file.FileName)));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if (Path.GetExtension(file.FileName).ToLower() == ".pdf".ToLower())
                {
                    //convert html file to tiff
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                string AttachmentIndex = "";
                if (AddIndexToAttachment == "1")
                {
                    int index = _IDocumentServices.GetCountOfAttachment(TransactionId) + i;
                    AttachmentIndex = index.ToString() + " - ";
                }
                var attachmentEntity = new DocumentDTO()
                {
                    AttachmentTypeId = AttachmentTypeId == 0 ? 1 : AttachmentTypeId,
                    AttachmentName = FileName ?? AttachmentIndex + file.FileName.Split('\\').Last(),
                    OriginalName = FileName ?? AttachmentIndex + file.FileName.Split('\\').Last(),
                    MimeType = ContentType ?? file.ContentType,
                    Size = BinaryContent.Length,
                    BinaryContent = BinaryContent,
                };
                string[] param = new string[]
                {
                    TransactionTypeId.ToString(),
                    TransactionId.ToString()
                };
                attachmentList.Add(_IDocumentServices.InsertAttachment(attachmentEntity, param));
                ContentType = null;
                if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
                }
                i++;
            }

            return attachmentList.OrderByDescending(c => c.AttachmentName);
        }
        [RequestSizeLimit(6000000000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]

        [HttpPost("UploadAttachmentToCommitte")]
        public IEnumerable<CommiteeAttachmentDTO> UploadAttachmentToCommitte()
        {
            string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
            string AddIndexToAttachment = _systemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;

            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            // Get Doc Setting For Office 
            var OfficeSetting = this._AppSettings.DocumentSettings.OfficeSetting;
            bool ActiveOfficeConvertor = false;
            if (OfficeSetting != null)
            {
                if (OfficeSetting == "1") ActiveOfficeConvertor = true; else ActiveOfficeConvertor = false;
            }
            #region Get Additional Parameters
            var xx = Request.Form;
            var Description = Request.Form["description"].ToString();
            bool allUser = Convert.ToBoolean(Request.Form["allUsers"].ToString());
            var selectedUsers = Request.Form["selectedUsers"].ToString();
            var selcteduser = selectedUsers.Split(",");
            List<AttachmentUser> users = new List<AttachmentUser>();
            if (!allUser && selectedUsers != string.Empty && selectedUsers != "undefined")
            {
                foreach (var item in selcteduser)
                {
                    users.Add(new AttachmentUser { UserId = Convert.ToInt32(item) });
                }
            }

            var AdditionalParameters = Request.Query.Union(Request.Form);
            string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
            string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();

            int AttachmentTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);

            // int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "CommiteeId").Value, out CommiteeId);
            string commiteeValue = AdditionalParameters.FirstOrDefault(x => x.Key == "CommiteeId").Value;
            string CommiteeAfterCheck = commiteeValue.Contains(' ') ? commiteeValue.Replace(' ', '+') : commiteeValue;
            //if (commiteeValue.Contains(' '))
            //{
            //    string x = commiteeValue.Replace(' ', '+');
            //}
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
                _SessionServices.UserIdAndRoleIdAfterDecrypt(CommiteeAfterCheck, false);

            int CommiteeId = UserIdAndUserRoleId.Id;

            #endregion Get Additional Parameters
            var files = Request.Form.Files;
            var attachmentList = new List<SavedAttachmentDTO>();
            var CommitteAttachmentList = new List<CommiteeAttachmentDTO>();
            int i = 1;
            foreach (var file in files)
            {
                byte[] BinaryContent = null;
                byte[] OfficeBinaryContent = null;

                using (var binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
                {
                    BinaryContent = binaryReader.ReadBytes((int)file.Length);
                }


                if ((Path.GetExtension(file.FileName).ToLower() == ".docx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, Path.GetExtension(file.FileName));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if ((Path.GetExtension(file.FileName).ToLower() == ".xlsx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (Path.GetExtension(file.FileName)));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if (Path.GetExtension(file.FileName).ToLower() == ".pdf".ToLower())
                {
                    //convert html file to tiff
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }

                string AttachmentIndex = "";
                if (AddIndexToAttachment == "1")
                {
                    int index = _IDocumentServices.GetCountOfAttachment(CommiteeId) + i;
                    AttachmentIndex = index.ToString() + " - ";
                }
                var attachmentEntity = new DocumentDTO()
                {
                    AttachmentTypeId = AttachmentTypeId == 0 ? 1 : AttachmentTypeId,
                    AttachmentName = FileName ?? AttachmentIndex + file.FileName.Split('\\').Last(),
                    OriginalName = FileName ?? AttachmentIndex + file.FileName.Split('\\').Last(),
                    MimeType = ContentType ?? file.ContentType,
                    Size = BinaryContent.Length,
                    BinaryContent = BinaryContent
                };
                string[] param = new string[]
                {
                    CommiteeId.ToString()
                };
                attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));


                ContentType = null;
                if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
                }
                i++;
            }
            //foreach (var item in attachmentList)
            //{
            CommitteAttachmentList.Add(_IDocumentServices.insertCommitteeAttachment(new CommiteeSavedAttachment
            {
                AllUsers = allUser,
                //AttachmentId = item.SavedAttachmentId,
                Attachments = attachmentList.Select(x => new SavedAttachment
                {
                    AttachmentName = x.AttachmentName,
                    AttachmentTypeId = x.AttachmentTypeId,
                    BinaryContent = x.BinaryContent,
                    Height = x.Height,
                    IsDisabled = x.IsDisabled,
                    LFEntryId = x.LFEntryId,
                    MimeType = x.MimeType,
                    Notes = x.Notes,
                    OriginalName = x.OriginalName,
                    PagesCount = x.PagesCount,
                    PhysicalAttachmentTypeId = x.PhysicalAttachmentTypeId,
                    // SavedAttachmentId = x.SavedAttachmentId,
                    Size = x.Size,
                    Width = x.Width
                }).ToList(),
                AttachmentUsers = users,
                CommiteeId = CommiteeId,
                Description = Description,
                
            }));
            //}

            return CommitteAttachmentList;
        }

        //upload AttachmentToComment
        [RequestSizeLimit(6000000000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]

        [HttpPost("UploadAttachmentToComment")]
        public IEnumerable<SavedAttachmentDTO> UploadAttachmentToComment()
        {
            string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
            string AddIndexToAttachment = _systemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;

            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            // Get Doc Setting For Office 
            var OfficeSetting = this._AppSettings.DocumentSettings.OfficeSetting;
            bool ActiveOfficeConvertor = false;
            if (OfficeSetting != null)
            {
                if (OfficeSetting == "1") ActiveOfficeConvertor = true; else ActiveOfficeConvertor = false;
            }
            #region Get Additional Parameters
           
            var AdditionalParameters = Request.Query.Union(Request.Form);
            string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
            string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();

            int AttachmentTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);

            //int CommentId;
            //int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "CommentId").Value, out CommentId);

           // int CommentId = Convert.ToInt32(Request.Form["CommentId"].ToString());
            //int CommiteeTaskId = Convert.ToInt32(Request.Form["CommiteeTaskId"].ToString());

            UserIdAndRoleIdAfterDecryptDTO IdIdAndUserRoleId = 
                _SessionServices.UserIdAndRoleIdAfterDecrypt(Request.Form["CommentId"], false);
            int CommentId = IdIdAndUserRoleId.Id;

            #endregion Get Additional Parameters
            var files = Request.Form.Files;
            var attachmentList = new List<SavedAttachmentDTO>();
           // var CommitteAttachmentList = new List<CommentAttachmentInTaskDTO>();
            //var CommentAttachmentList = new List<SavedAttachmentDTO>();
            int i = 1;
            foreach (var file in files)
            {
                byte[] BinaryContent = null;
                byte[] OfficeBinaryContent = null;

                using (var binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
                {
                    BinaryContent = binaryReader.ReadBytes((int)file.Length);
                }


                if ((Path.GetExtension(file.FileName).ToLower() == ".docx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, Path.GetExtension(file.FileName));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if ((Path.GetExtension(file.FileName).ToLower() == ".xlsx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (Path.GetExtension(file.FileName)));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if (Path.GetExtension(file.FileName).ToLower() == ".pdf".ToLower())
                {
                    //convert html file to tiff
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }

                string AttachmentIndex = "";
                if (AddIndexToAttachment == "1")
                {
                    int index = _IDocumentServices.CountOfAttachmentComment(CommentId) + i;
                    AttachmentIndex = index.ToString() + " - ";
                }
                var attachmentEntity = new DocumentDTO()
                {
                    AttachmentTypeId = AttachmentTypeId == 0 ? 1 : AttachmentTypeId,
                    AttachmentName = FileName ?? AttachmentIndex + file.FileName.Split('\\').Last(),
                    OriginalName = FileName ?? AttachmentIndex + file.FileName.Split('\\').Last(),
                    MimeType = ContentType ?? file.ContentType,
                    Size = BinaryContent.Length,
                    BinaryContent = BinaryContent,
                    CommentId = CommentId
                };

                
                string[] param = new string[]
                {
                    CommentId.ToString()
                };
                attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));
                

                ContentType = null;
                if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
                }
                i++;
            }

            //attachmentList.Add(_IDocumentServices.InsertCommentAttachments( new SavedAttachment
            //        {
            //            AttachmentName = item.AttachmentName,
            //            AttachmentTypeId = item.AttachmentTypeId,
            //            BinaryContent = item.BinaryContent,
            //            Height = item.Height,
            //            IsDisabled = item.IsDisabled,
            //            LFEntryId = item.LFEntryId,
            //            MimeType = item.MimeType,
            //            Notes = item.Notes,
            //            OriginalName = item.OriginalName,
            //            PagesCount = item.PagesCount,
            //            PhysicalAttachmentTypeId = item.PhysicalAttachmentTypeId,
            //            // SavedAttachmentId = x.SavedAttachmentId,
            //            Size = item.Size,
            //            Width = item.Width,
            //            CommentId=item.CommentId
            //        }
            //        //AttachmentId = item.SavedAttachmentId,
            //        //CommentId = CommentId,
            //        //commiteeTaskId= CommiteeTaskId
            //    ));
            

            return attachmentList;
        }

        [RequestSizeLimit(6000000000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]
        [HttpPost("UploadAttachmentToSurvey")]
        public SurveyDTO UploadAttachmentToSurvey()
        {
            #region UploadAttachmentToSurvey => Original Code

            string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
            string AddIndexToAttachment = _systemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;

            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            // Get Doc Setting For Office 
            var OfficeSetting = this._AppSettings.DocumentSettings.OfficeSetting;
            bool ActiveOfficeConvertor = false;
            if (OfficeSetting != null)
            {
                if (OfficeSetting == "1") ActiveOfficeConvertor = true; else ActiveOfficeConvertor = false;
            }
            #region Get Additional Parameters
            var xx = Request.Form;
            var CommiteeId = 0;
            if (Request.Form["commiteeId"].Count != 0)
            {
                UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(Request.Form["commiteeId"],true);

                // CommiteeId = Convert.ToInt32(Request.Form["commiteeId"].ToString());
                CommiteeId = UserIdAndUserRoleId.Id;
            }
            var TopicId = 0;
            if (Request.Form["meetingTopicId"].Count != 0)
            {
                TopicId = Convert.ToInt32(Request.Form["meetingTopicId"].ToString());
            }
            var MeetingId = 0;
            if (Request.Form["MeetingId"].Count != 0)
            {
                MeetingId = Convert.ToInt32(Request.Form["MeetingId"].ToString());
            }
            var Subject = Request.Form["subject"].ToString();
            //DateTime dt = new DateTime();
            //DateTime.TryParse(Convert.ToString(Request.Form["selectedDate"]), out dt);
            //DateTimeOffset SurveyEndDate = new DateTimeOffset(dt, TimeSpan.Zero);
            var SurveyEndDate = Request.Form["selectedDate"].ToString();
            bool isShared = Convert.ToBoolean(Request.Form["isShared"].ToString());
            bool Multi = Convert.ToBoolean(Request.Form["multi"].ToString());
            var surveyUser = Request.Form["surveyUsers"].ToString();
            var surveyUsers = surveyUser.Split(",");
            var SurveyAnswer = Request.Form["surveyAnswers"].ToString();
            var SurveyAnswers = SurveyAnswer.Split(",");
            List<SurveyUser> users = new List<SurveyUser>();
            if (!isShared && surveyUser != string.Empty && surveyUser != "undefined")
            {
                foreach (var item in surveyUsers)
                {
                    users.Add(new SurveyUser { UserId = Convert.ToInt32(item) });
                }
            }
            List<SurveyAnswer> Answers = new List<SurveyAnswer>();
            if (SurveyAnswer != string.Empty && SurveyAnswer != "undefined")
            {
                foreach (var item in SurveyAnswers)
                {
                    Answers.Add(new SurveyAnswer { Answer = item });
                }
            }
            var Survey = new Survey
            {
                CommiteeId = (CommiteeId == 0) ? null : CommiteeId,
                IsShared = isShared,
                Multi = Multi,
                Subject = Subject,
                SurveyAnswers = Answers,
                SurveyUsers = users,
                MeetingTopicId = (TopicId == 0) ? null : TopicId,
                MeetingId = (MeetingId == 0) ? null : MeetingId,
                SurveyEndDate = SurveyEndDate,
                CreatedBy= _SessionServices.UserId
            };
            var insertedSurvey = _IDocumentServices.InsertSurvey(Survey);
            var AdditionalParameters = Request.Query.Union(Request.Form);
            string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
            string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();

            int AttachmentTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);

            int SurveyId = insertedSurvey.SurveyId;
            // int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "SurveyId").Value, out SurveyId);

            #endregion Get Additional Parameters
            var files = Request.Form.Files;
            var attachmentList = new List<SavedAttachmentDTO>();
            var CommitteAttachmentList = new List<SurveyAttachmentDTO>();
            int i = 1;
            foreach (var file in files)
            {
                byte[] BinaryContent = null;
                byte[] OfficeBinaryContent = null;

                using (var binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
                {
                    BinaryContent = binaryReader.ReadBytes((int)file.Length);
                }


                if ((Path.GetExtension(file.FileName).ToLower() == ".docx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, Path.GetExtension(file.FileName));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if ((Path.GetExtension(file.FileName).ToLower() == ".xlsx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (Path.GetExtension(file.FileName)));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if (Path.GetExtension(file.FileName).ToLower() == ".pdf".ToLower())
                {
                    //convert html file to tiff
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }


                var attachmentEntity = new DocumentDTO()
                {
                    AttachmentTypeId = AttachmentTypeId == 0 ? 1 : AttachmentTypeId,
                    AttachmentName = FileName ?? file.FileName.Split('\\').Last(),
                    OriginalName = FileName ?? file.FileName.Split('\\').Last(),
                    MimeType = ContentType ?? file.ContentType,
                    Size = BinaryContent.Length,
                    BinaryContent = BinaryContent
                };
                string[] param = new string[]
                {
                    SurveyId.ToString()
                };
                attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));

                ContentType = null;
                if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
                }
                i++;

            }
            foreach (var item in attachmentList)
            {
                CommitteAttachmentList.Add(_IDocumentServices.InsertSurveyAttachment(new SurveyAttachment
                {
                    Active = true,
                    Attachment = new SavedAttachment
                    {
                        AttachmentName = item.AttachmentName,
                        AttachmentTypeId = item.AttachmentTypeId,
                        BinaryContent = item.BinaryContent,
                        Height = item.Height,
                        IsDisabled = item.IsDisabled,
                        LFEntryId = item.LFEntryId,
                        MimeType = item.MimeType,
                        Notes = item.Notes,
                        OriginalName = item.OriginalName,
                        PagesCount = item.PagesCount,
                        PhysicalAttachmentTypeId = item.PhysicalAttachmentTypeId,
                        // SavedAttachmentId = x.SavedAttachmentId,
                        Size = item.Size,
                        Width = item.Width
                    },
                    AttachmentId = item.SavedAttachmentId,
                    SurveyId = SurveyId
                }));
            }
            UserDetailsDTO creator = _IDocumentServices.GetUserById(Survey.CreatedBy);
            var newSurvey = new SurveyDTO
            {
                Attachments = CommitteAttachmentList,
                IsShared = Survey.IsShared,
                CreatedOn = Survey.CreatedOn,
                CreatedBy = Survey.CreatedBy,
                SurveyEndDate = Survey.SurveyEndDate,
                CreatedByUser = creator,
                CommiteeId = (Survey.CommiteeId == null) ? null : (int)Survey.CommiteeId,
                MeetingId = (Survey.MeetingId == null) ? null : (int)Survey.MeetingId,
                MeetingTopicId = (Survey.MeetingTopicId == null) ? null : (int)Survey.MeetingTopicId,
                Multi = Survey.Multi,
                Subject = Survey.Subject,
                SurveyAnswers = Survey.SurveyAnswers.Select(x => new SurveyAnswerDTO { Answer = x.Answer, SurveyAnswerId = x.SurveyAnswerId }).ToList(),
                SurveyId = Survey.SurveyId,
                SurveyUsers = Survey.SurveyUsers.Select(x => new SurveyUserDTO { UserId = x.UserId }).ToList(),
                CreatedByRoleId = Survey.CreatedByRoleId,
                CreatedByRole = new CommiteeDetailsUsersRoleDTO
                {
                    CommiteeUsersRoleId = insertedSurvey.CreatedByRoleId == null ? 0 : (int)insertedSurvey.CreatedByRoleId,
                    Role = new CommiteeDetailsRoleDTO
                    {
                        CommiteeRoleId = insertedSurvey.CreatedByRoleId == null ? 0 : (int)insertedSurvey.CreatedByRoleId,
                        CommiteeRolesNameAr = insertedSurvey.CreatedByRole?.Role?.CommiteeRolesNameAr,
                        CommiteeRolesNameEn = insertedSurvey.CreatedByRole?.Role?.CommiteeRolesNameEn,
                    }
                }
            };
            if (newSurvey.MeetingId != null || newSurvey.MeetingTopicId != null)
            {
                Meeting meeting = new Meeting();
                if (newSurvey.MeetingId == null && newSurvey.MeetingTopicId != null)
                {
                    meeting = _IDocumentServices.GetMeetingByTopixId(newSurvey.MeetingTopicId);
                }
                else
                    meeting = _IDocumentServices.GetMeetingById(newSurvey.MeetingId);
                var Attendees = meeting.MeetingAttendees.Select(c => new UserChatDTO
                {
                    Id = c.AttendeeId,
                    UserId = c.AttendeeId,
                    UserName = c.Attendee.Username

                }).ToList();
                var Coordinators = meeting.MeetingCoordinators.Select(c => new UserChatDTO
                {
                    Id = c.CoordinatorId,
                    UserId = c.CoordinatorId,
                    UserName = c.Coordinator.Username

                }).ToList();
                foreach (var user in Attendees)
                {
                    _IDocumentServices.AddNotificationForSurvey(user.Id, newSurvey.SurveyId, newSurvey.Subject , meeting.Id);
                    _IDocumentServices.SurveySignalR(user, newSurvey);
                }
                foreach (var user in Coordinators)
                {
                    _IDocumentServices.AddNotificationForSurvey(user.Id, newSurvey.SurveyId, newSurvey.Subject , meeting.Id);
                    _IDocumentServices.SurveySignalR(user, newSurvey);
                }
            }
            #endregion

         
            return newSurvey;
        }

        [RequestSizeLimit(6000000000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6000000000)]
        [HttpPost("UploadAttachmentToCommitteeTask")]
        public List<CommitteeTaskAttachmentDTO> UploadAttachmentToCommitteeTask()
        {
            string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
            string AddIndexToAttachment = _systemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;

            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            // Get Doc Setting For Office 
            var OfficeSetting = this._AppSettings.DocumentSettings.OfficeSetting;
            bool ActiveOfficeConvertor = false;
            if (OfficeSetting != null)
            {
                if (OfficeSetting == "1") ActiveOfficeConvertor = true; else ActiveOfficeConvertor = false;
            }
            var AdditionalParameters = Request.Query.Union(Request.Form);
            int CommiteeTaskId = Convert.ToInt32(Request.Form["CommiteeTaskId"].ToString());
            if (CommiteeTaskId == 0)
            {
                CommiteeTaskId = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x=>x.CommiteeTaskId).Select(r=>r.CommiteeTaskId).FirstOrDefault();
            }
            string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
            string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();
            int AttachmentTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);
            var files = Request.Form.Files;
            var attachmentList = new List<SavedAttachmentDTO>();
            var CommitteAttachmentList = new List<CommitteeTaskAttachmentDTO>();
            int i = 1;
            foreach (var file in files)
            {
                byte[] BinaryContent = null;
                byte[] OfficeBinaryContent = null;

                using (var binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
                {
                    BinaryContent = binaryReader.ReadBytes((int)file.Length);
                }


                if ((Path.GetExtension(file.FileName).ToLower() == ".docx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, Path.GetExtension(file.FileName));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if ((Path.GetExtension(file.FileName).ToLower() == ".xlsx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
                {
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (Path.GetExtension(file.FileName)));
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }
                else if (Path.GetExtension(file.FileName).ToLower() == ".pdf".ToLower())
                {
                    //convert html file to tiff
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
                    BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                }


                var attachmentEntity = new DocumentDTO()
                {
                    AttachmentTypeId = AttachmentTypeId == 0 ? 1 : AttachmentTypeId,
                    AttachmentName = FileName ?? file.FileName.Split('\\').Last(),
                    OriginalName = FileName ?? file.FileName.Split('\\').Last(),
                    MimeType = ContentType ?? file.ContentType,
                    Size = BinaryContent.Length,
                    BinaryContent = BinaryContent
                };
                string[] param = new string[]
                {
                    CommiteeTaskId.ToString()
                };
                attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));

                ContentType = null;
                if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
                }
                i++;

            }
            foreach (var item in attachmentList)
            {
                CommitteAttachmentList.Add(_IDocumentServices.InsertCommitteeTaskAttachment(new CommitteeTaskAttachment
                {
                    Attachment = new SavedAttachment
                    {
                        AttachmentName = item.AttachmentName,
                        AttachmentTypeId = item.AttachmentTypeId,
                        BinaryContent = item.BinaryContent,
                        Height = item.Height,
                        IsDisabled = item.IsDisabled,
                        LFEntryId = item.LFEntryId,
                        MimeType = item.MimeType,
                        Notes = item.Notes,
                        OriginalName = item.OriginalName,
                        PagesCount = item.PagesCount,
                        PhysicalAttachmentTypeId = item.PhysicalAttachmentTypeId,
                        // SavedAttachmentId = x.SavedAttachmentId,
                        Size = item.Size,
                        Width = item.Width
                    },
                    AttachmentId = item.SavedAttachmentId,
                    CommiteeTaskId = CommiteeTaskId
                }));
            }
            return CommitteAttachmentList;
        }

        [HttpPost("UploadByPath")]
        public IEnumerable<SavedAttachmentDTO> UploadByPath(string tiff_path, long CommiteeId, int CommiteeTypeId, int? ReferenceAttachmentId = null)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(tiff_path);
            byte[] BinaryContent = System.IO.File.ReadAllBytes(tiff_path);

            var attachmentList = new List<SavedAttachmentDTO>();
            var attachmentEntity = new DocumentDTO()
            {
                AttachmentTypeId = (int)AttachmentTypeEnum.Document,
                AttachmentName = fileInfo.Name,
                OriginalName = fileInfo.Name,
                MimeType = "image/tiff",
                Size = BinaryContent.Length,
                BinaryContent = BinaryContent,
                ReferenceAttachmentId = ReferenceAttachmentId
            };
            string[] param = new string[]
            {
                CommiteeTypeId.ToString(),
                CommiteeId.ToString()
            };
            attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));
            try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { } //Clean
            return attachmentList;
        }
        [HttpPost("UploadByContents")]
        public IEnumerable<SavedAttachmentDTO> UploadByContents([FromBody] dynamic attachment)//byte[] fileContent,string fileName,string mimeType, long CommiteeId, int CommiteeTypeId)
        {
            var attachmentList = new List<SavedAttachmentDTO>();
            var attachmentEntity = new DocumentDTO()
            {
                AttachmentTypeId = (int)AttachmentTypeEnum.Document,
                AttachmentName = attachment.fileName,
                OriginalName = attachment.fileName,
                MimeType = attachment.mimeType,
                Size = ((byte[])attachment.fileContent).Length,
                BinaryContent = attachment.fileContent
            };
            string[] param = new string[]
            {
                attachment.CommiteeTypeId.ToString(),
                attachment.CommiteeId.ToString()
            };
            attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));
            return attachmentList;
        }

        [HttpGet("DownloadVoiceNote")]
        public IActionResult DownloadFile(string id)
        {
            string fileName = "", mimeType = "";
            var binaryContent = _IDocumentServices.Download(id, ref fileName, ref mimeType, true);

            //System.Net.Http.HttpContent content = binaryContent;
            try
            {
                return Ok(new { response = binaryContent, contentType = mimeType, fileName });
            }
            catch (Exception)
            {

                throw new FileNotFoundException();
            }

        }
        [HttpGet("download")]
        public FileResult Download(string id, bool getOriginal = false, bool IsTransaction = false)
        {
            FileContentResult result = null;
            string fileName = "", mimeType = "";

            if (!string.IsNullOrEmpty(id))
            {
                byte[] binaryContent = new byte[] { };
                if (IsTransaction)
                {
                    binaryContent = _IDocumentServices.DownloadTransaction(id, ref fileName, ref mimeType, getOriginal) ?? new byte[] { };
                }
                else
                {
                    binaryContent = _IDocumentServices.Download(id, ref fileName, ref mimeType, getOriginal);
                }
                if (binaryContent != null)
                {
                    result = new FileContentResult(binaryContent, mimeType)
                    {
                        FileDownloadName = fileName
                    };
                }
            }
            if (result == null && (mimeType == null || mimeType.StartsWith("image")))
            {
                result = new FileContentResult(new byte[] { }, "image/png");
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet("GetAttachmentUrl")]
        public string GetAttachmentUrl(int id)
        {
            if (TempData["UserId"] != null)
            {
                string fileName = "", mimeType = "";
                var binaryContent = _IDocumentServices.Download(id, ref fileName, ref mimeType, true);
                if (binaryContent != null)
                {
                    var exe = fileName.Split(".")[1];
                    var name = Guid.NewGuid() + fileName;
                    var file_path = Path.Combine(Hosting.RootPath, "wwwroot\\Doc\\" + name);
                    System.IO.File.WriteAllBytes(file_path, binaryContent);
                    var fileStream = new FileStream(file_path, FileMode.Create, FileAccess.Write);
                    fileStream.Write(binaryContent, 0, binaryContent.Length);
                    fileStream.Close();
                    string host = Request.Scheme + "://" + Request.Host.Value + "/Doc";
                    TempData["UserId"] = _SessionServices.UserId;
                    return host + "/" + name;
                }
            }
            return "";
        }
        [AllowAnonymous]
        [HttpGet("DeleteAttachmentFile")]
        public bool DeleteAttachmentFile(string url)
        {
            try
            {
                url = url.Replace(Request.Scheme + "://" + Request.Host.Value, "");
                string host = Request.Scheme + "://" + Request.Host.Value + "/Doc";
                var file_path = Hosting.RootPath + "\\wwwroot" + url.Replace("/", "\\");
                FileInfo fileDel = new FileInfo(file_path);
                fileDel.Delete();
                TempData["UserId"] = _SessionServices.UserId;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet("downloadPdf")]
        public FileResult DownloadPdf(string id)
        {
            FileContentResult result = null;
            string fileName = "", mimeType = "";
            if (!string.IsNullOrEmpty(id))
            {
                var binaryContent = _IDocumentServices.DownloadPdf(id, ref fileName, ref mimeType);
                if (binaryContent != null)
                {
                    result = new FileContentResult(binaryContent, mimeType)
                    {
                        FileDownloadName = fileName
                    };
                }
            }
            if (result == null && (mimeType == null || mimeType.StartsWith("image")))
            {
                result = new FileContentResult(new byte[] { }, "image/png");
            }
            return result;
        }

        [HttpGet("GetFileInBase64Format")]
        public string GetFileInBase64Format(string id)
        {
            string result = string.Empty;
            string fileName = "", mimeType = "";
            if (!string.IsNullOrEmpty(id))
            {
                var binaryContent = _IDocumentServices.Download(id, ref fileName, ref mimeType);
                if (binaryContent != null)
                {
                    result = Convert.ToBase64String(binaryContent);
                }
            }
            if (result == null && (mimeType == null || mimeType.StartsWith("image")))
            {
                result = Convert.ToBase64String(new FileContentResult(new byte[] { }, "image/png").FileContents);
            }
            return result;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id">The AttachmentId</param>
        /// <param name="pageNumber">The PageNumber needed to be shown, It should greater than or equals 1 and less than or equals the Attachment's PagesCount</param>
        /// <param name="thumb">True if the thumb is the only need (smaller size)</param>
        /// <param name="fileName">The file name that the user can SAVE the image with</param>
        /// <param name="extension">Only needed for some UI controls like LightBox, and should be the last parameter passed in the given URL, and with the preceeding dot (e.g. ".png")</param>
        /// <returns></returns>
        // [Authorize]
        [HttpGet("downloadpage")]
        public FileResult DownloadPage(string id, int pageNumber, bool thumb, string fileName, string extension)
        {
            FileContentResult result = null;
            if (!string.IsNullOrEmpty(id))
            {
                var binaryContent = _IDocumentServices.DownloadPage(id, pageNumber, thumb) ?? new byte[] { };
                result = new FileContentResult(binaryContent, "image/tiff")
                {
                    FileDownloadName = fileName
                };
            }
            return result;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id">The AttachmentId</param>
        /// <param name="pageNumber">The PageNumber needed to be shown, It should greater than or equals 1 and less than or equals the Attachment's PagesCount</param>
        /// <param name="thumb">True if the thumb is the only need (smaller size)</param>
        /// <param name="fileName">The file name that the user can SAVE the image with</param>
        /// <param name="extension">Only needed for some UI controls like LightBox, and should be the last parameter passed in the given URL, and with the preceeding dot (e.g. ".png")</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("downloadpageoriginal")]
        public FileResult DownloadPageOriginal(string Id, int pageNumber, bool thumb, string fileName, string extension, bool IsTransaction = false)
        {
            //bool isAuthenticated = User.Identity.IsAuthenticated;
            //if (isAuthenticated)
            //{
            string IdAfterReplace = Id.Replace(" ", "+");
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(IdAfterReplace,false);
            int id = UserIdAndUserRoleId.Id;

            FileContentResult result = null;
            if (!string.IsNullOrEmpty(Id))
            {
                byte[] binaryContent = new byte[] { };


                if (IsTransaction)
                {
                    binaryContent = _IDocumentServices.DownloadPageOriginalForTransaction(id, pageNumber, thumb) ?? new byte[] { };
                }
                else
                {
                    binaryContent = _IDocumentServices.DownloadPageOriginal(id, pageNumber, thumb) ?? new byte[] { };
                }

                result = new FileContentResult(binaryContent, "image/png")
                {
                    FileDownloadName = fileName
                };
            }
            return result;
            //}
            //else
            //{
            //    throw new Exception("401, Unauthorized access");
            //    //throw new HttpResponseException(HttpStatusCode.Unauthorized);
            //}
        }
        [HttpPost("DeletePage")]
        public bool DeletePage(string id, int pageIndex)
        {
            try
            {
                return _IDocumentServices.DeletePage(id, pageIndex);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //[AllowAnonymous]
        [HttpPost("MovePageTo")]
        public CommiteeAttachmentDTO MovePageTo(string CommiteeID, string sourceAttachID, int pageIndex, int moveLocation)
        {
            try
            {
                return _IDocumentServices.MovePageTo(CommiteeID, sourceAttachID, pageIndex, moveLocation);
            }
            catch (Exception ex)
            {
                return new CommiteeAttachmentDTO();
            }
        }
        //[AllowAnonymous]
        [HttpPost("SplitDocument")]
        public IEnumerable<CommiteeAttachmentDTO> SplitDocument(string CommiteeID, string sourceAttachID, int fromPageIndex, int toPageIndex)
        {
            try
            {
                //var attachmentList = new List<AttachmentSummaryDTO>();
                //attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));
                return _IDocumentServices.SplitDocument(CommiteeID, sourceAttachID, fromPageIndex, toPageIndex);
            }
            catch (Exception ex)
            {
                return new List<CommiteeAttachmentDTO>();
            }
        }
        // [Authorize]
        [HttpGet("RotatePage")]
        public bool RotatePage(int attachmentId, int pageNumber, int rotation)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(Convert.ToString(attachmentId)))
            {
                result = _IDocumentServices.RotatePage(attachmentId, pageNumber, rotation);
                return result;
            }
            return result;
        }
        [HttpPost("UploadHtmlBinary")]
        public SavedAttachmentDTO UploadHtmlBinary([FromBody] byte[] htmlContent, string CommiteeId)
        {
            string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);


            GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
            string tempPath = Hosting.RootPath + "\\Temp\\" + (new Random()).Next();
            bool folderExists = Directory.Exists(tempPath);
            if (!folderExists)
                Directory.CreateDirectory(tempPath);
            System.IO.File.WriteAllBytes(tempPath + "\\ZipFile.zip", htmlContent);
            ZipFile.ExtractToDirectory((tempPath + "\\ZipFile.zip"), tempPath, true);
            string text = System.IO.File.ReadAllText(tempPath + "\\EmailBody.HTML");
            int startIndex = text.IndexOf("charset=") + 8;
            int endIndex = text.IndexOf('>', startIndex) - 1;
            text = text.Substring(startIndex, (endIndex - startIndex));
            // System.IO.File.WriteAllText(tempPath + "\\EmailBody1.HTML",text);
            var tiff_path = ghostScriptFactory.ConvertHTMLtoTIFFOutlook(tempPath + "\\EmailBody.HTML", tempPath + "\\EmailBody.pdf", true, text);
            string ContentType = "image/tiff";
            byte[] BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
            try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
            var attachmentEntity = new DocumentDTO()
            {
                AttachmentTypeId = 1,
                AttachmentName = "محنوي البريد الالكتروني",
                OriginalName = "محنوي البريد الالكتروني",
                MimeType = ContentType,
                Size = BinaryContent.Length,
                BinaryContent = BinaryContent
            };
            string[] param = new string[]
            {
                   "1",
                   CommiteeId
            };
            return _IDocumentServices.Insert(attachmentEntity, param);
            //return new AttachmentSummaryDTO();
        }
        //[HttpPost("UploadHtmlBinary")]
        //public IEnumerable<AttachmentSummaryDTO> UploadHtmlBinary(string holder)
        //{
        //    JavaScriptSerializer
        //    var attachmentList = new List<AttachmentSummaryDTO>();
        //            //convert html file to tiff
        //            GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath);
        //            var tiff_path = ghostScriptFactory.ConvertHTMLtoTIFF(html_bytes);
        //            html_bytes = System.IO.File.ReadAllBytes(tiff_path);

        //            try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
        //        var attachmentEntity = new DocumentDTO()
        //        {
        //            AttachmentTypeId = 1,
        //            AttachmentName = "Body",
        //            OriginalName = "Body",
        //            MimeType = "image/tiff",
        //            Size = html_bytes.Length,
        //            BinaryContent = html_bytes
        //        };
        //        string[] param = new string[]
        //        {
        //            "1",
        //            ""
        //        };

        //        attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));

        //    return attachmentList;
        //}
        public class BytesHolder
        {
            public byte[] html_bytes { get; set; }
        }
        [HttpGet, Route("GetTemplatesNames")]
        public List<string> GetTemplatesNames()
        {
            return _IDocumentServices.GetTemplatesNames();
        }

        [HttpGet, Route("getTemplateIDByName")]
        public int getTemplateIDByName(string temName)
        {
            return _IDocumentServices.getTemplateIDByName(temName);
        }

        [HttpGet, Route("GetEntries")]
        public IEnumerable<dynamic> GetEntries(int? entryId)
        {
            return _IDocumentServices.GetEntries(entryId);
        }
        [HttpGet, Route("GetEntriesForTransAttachments")]
        public IEnumerable<dynamic> GetEntriesForTransAttachments(int userRoleId, string fentryid, string searchText, bool isEmployee)
        {
            return _IDocumentServices.GetEntriesForTransAttachments(userRoleId, fentryid, searchText, isEmployee);
        }
        private string SaveFileDirectToLF(string fileName, string FolderName, string AttachmentNameAr, string PathOfLF, string existingPhysicalFilePath
                                       , string LFServerName, string LFRepostry, string LFUserName, string LFUserPassword)
        {
            string SavedEntryID = "";
            try
            {
                string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
                Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
                string documents_root = Path.Combine(Hosting.RootPath, "Documents\\");
                if (!Directory.Exists(documents_root))
                {
                    Directory.CreateDirectory(documents_root);
                }

                string convertedFileName = "";
                int ParsedValue;
                //int dbi = String.IsNullOrEmpty(_systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue) ? 200 : (int.TryParse(_systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue, out ParsedValue) == false ? 200 : ParsedValue);

                string _extension = fileName.Substring(fileName.LastIndexOf("."));
                fileName = fileName.Replace(_extension, "");

                fileName = "Temp_" + DateTime.Now.Ticks.ToString();
                fileName = fileName + _extension;
                //to create Folder for Save
                string FolderToSave = Guid.NewGuid().ToString();
                string subPath = documents_root + FolderToSave;
                if (!Directory.Exists(subPath))
                {
                    Directory.CreateDirectory(subPath);
                }
                ////////////////////

                if (existingPhysicalFilePath == null)
                    System.IO.File.WriteAllBytes(documents_root + FolderToSave + "\\" + fileName, null);
                else
                {
                    if (System.IO.File.Exists(existingPhysicalFilePath))
                    {
                        System.IO.File.Copy(existingPhysicalFilePath, documents_root + FolderToSave + "\\" + fileName);
                    }
                }

                bool convertOfficeToTiff = true; //MASAR2.BLL.IntegrationSetting.getIntegrationSettingValueByKeyStored("ConvertOfficeToTiff") == "1" ? true : false;

                #region Convert Images to Tiff
                string ConvertToPdfOutPut;
                string ConvertToTiffOutPut;
                bool uploadElectronicDoc = false;
                string ContentType = "";
                //mkh Check If the file is PDF or image Then Convert It  
                //to Tiff and save IT with the same Name in the same Path
                byte[] BinaryContent = null;
                byte[] OfficeBinaryContent = null;

                BinaryContent = System.IO.File.ReadAllBytes(documents_root + FolderToSave + "\\" + fileName);
                bool ActiveOfficeConvertor = true;
                if ((_extension.ToLower() == ".docx".ToLower() || _extension.ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
                {
                    uploadElectronicDoc = true;
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(documents_root + FolderToSave + "\\", Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, _extension);
                    //BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                    string fileNameConverted = Path.GetFileName(tiff_path);
                    _extension = fileNameConverted.Substring(fileNameConverted.LastIndexOf("."));
                    convertedFileName = fileNameConverted;
                    LFDirectConnector.DocumentPath = documents_root + FolderToSave + "\\" + "Documents\\";
                }
                else if ((_extension.ToLower() == ".xlsx".ToLower() || _extension.ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
                {
                    uploadElectronicDoc = true;
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(documents_root + FolderToSave + "\\", Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    OfficeBinaryContent = BinaryContent;
                    var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (_extension));
                    //BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                    string fileNameConverted = Path.GetFileName(tiff_path);
                    _extension = fileNameConverted.Substring(fileNameConverted.LastIndexOf("."));
                    convertedFileName = fileNameConverted;
                    LFDirectConnector.DocumentPath = documents_root + FolderToSave + "\\" + "Documents\\";
                }
                else if (_extension.ToLower() == ".pdf".ToLower())
                {
                    //convert html file to tiff
                    GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(documents_root + FolderToSave + "\\", Hosting.AngularRootPath, pdfResolution, pdfToColor);
                    var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
                    //BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
                    ContentType = "image/tiff";
                    //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
                    string fileNameConverted = Path.GetFileName(tiff_path);
                    _extension = fileNameConverted.Substring(fileNameConverted.LastIndexOf("."));
                    convertedFileName = fileNameConverted;
                    LFDirectConnector.DocumentPath = documents_root + FolderToSave + "\\" + "Documents\\";
                }
                else
                {
                    _extension = fileName.Substring(fileName.LastIndexOf("."));
                    convertedFileName = fileName;
                    LFDirectConnector.DocumentPath = documents_root + FolderToSave;
                }


                #endregion

                //add to DB

                #region Add To DB
                //-------------Set Credetials------------------------------------//
                LFDirectConnector.setLFConnectionCredentials(LFServerName, LFRepostry, LFUserName, LFUserPassword);

                //Saving Commitee Attachment
                LFDirectConnector.DocumentName = convertedFileName.ToLower();
                LFDirectConnector.VolumeName = "DEFAULT";



                SavedEntryID = Convert.ToString(LFDirectConnector.UploadDocumentOnCertainPath(PathOfLF, FolderName));

                //**//

                if (uploadElectronicDoc)
                    LFDirectConnector.UploadWordToDocument(Convert.ToInt32(SavedEntryID), documents_root + FolderToSave + "\\" + fileName);


                return SavedEntryID;
                #endregion
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private void DownloadFileDirectFromLF(int EntryID, string DownloadedFilePath, string LFServerName, string LFRepostry, string LFUserName, string LFUserPassword)
        {
            try
            {
                int PageCount = 0;

                //LFConnector.DownloadDocumentByEntryID(DownloadedFilePath, EntryID, Thumb, PageNumber);

                //-------------Set Credetials------------------------------------//
                LFDirectConnector.setLFConnectionCredentials(LFServerName, LFRepostry, LFUserName, LFUserPassword);

                PageCount = LFDirectConnector.GetDocumentPageCountByEntryID(EntryID);
                if (PageCount > 0)
                {
                    for (int i = 1; i <= PageCount; i++)
                    {
                        LFDirectConnector.DownloadDocumentByEntryID(DownloadedFilePath, EntryID, false, i);
                        LFDirectConnector.DownloadDocumentByEntryID(DownloadedFilePath, EntryID, true, i);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [AllowAnonymous]
        [HttpPost, Route("UploadFiles")]
        public StatusResult UploadFiles([FromBody] UploadFilesParams ObjectToSearch_Param)
        {

            StatusResult _resultEmpty = null;
            string result = "0";
            string EntryIDs = "";
            try
            {
                if (!string.IsNullOrEmpty(ObjectToSearch_Param.FolderPath) && Directory.Exists(ObjectToSearch_Param.FolderPath))
                {
                    String[] filePathsFromDirectory = Directory.GetFiles(ObjectToSearch_Param.FolderPath).Where(name => !name.EndsWith(".db")).ToArray();
                    string dirName = new DirectoryInfo(ObjectToSearch_Param.FolderPath).Name;

                    if (filePathsFromDirectory.Count() > 0)
                    {

                        foreach (string filePath in filePathsFromDirectory)
                        {
                            string fileName = Path.GetFileName(filePath);
                            if (fileName.Split('.').Count() == 2 && System.IO.File.Exists(filePath))
                                EntryIDs += SaveFileDirectToLF(filePath, dirName, fileName.Split('.')[0], ObjectToSearch_Param.PathOfLF, filePath,
                                    ObjectToSearch_Param.LFServerName, ObjectToSearch_Param.LFRepostry, ObjectToSearch_Param.LFUserName, ObjectToSearch_Param.LFUserPassword);
                            EntryIDs += ",";
                        }
                        EntryIDs = EntryIDs.TrimEnd(',');
                    }
                }
                if (!string.IsNullOrEmpty(ObjectToSearch_Param.FilePath) && System.IO.File.Exists(ObjectToSearch_Param.FilePath))
                {
                    string fileName = Path.GetFileName(ObjectToSearch_Param.FilePath);
                    string nametoSave = fileName.Split('.')[0];
                    if (fileName.Split('.').Count() == 2 && string.IsNullOrEmpty(EntryIDs))
                    {

                        EntryIDs += SaveFileDirectToLF(fileName, nametoSave, nametoSave, ObjectToSearch_Param.PathOfLF, ObjectToSearch_Param.FilePath, ObjectToSearch_Param.LFServerName, ObjectToSearch_Param.LFRepostry, ObjectToSearch_Param.LFUserName, ObjectToSearch_Param.LFUserPassword);
                    }
                    else if (fileName.Split('.').Count() == 2 && !string.IsNullOrEmpty(EntryIDs))
                    {
                        EntryIDs += ",";
                        EntryIDs += SaveFileDirectToLF(fileName, nametoSave, nametoSave, ObjectToSearch_Param.PathOfLF, ObjectToSearch_Param.FilePath, ObjectToSearch_Param.LFServerName, ObjectToSearch_Param.LFRepostry, ObjectToSearch_Param.LFUserName, ObjectToSearch_Param.LFUserPassword);
                    }

                }

                if (!string.IsNullOrEmpty(EntryIDs))
                {
                    _resultEmpty = new StatusResult()
                    {
                        EntryIDs = EntryIDs,
                        Status = "تم الحفظ "
                    };
                    return _resultEmpty;
                }
                else
                {
                    _resultEmpty = new StatusResult()
                    {
                        EntryIDs = "-1",
                        Status = "لا يوجد مرفقات "
                    };
                    return _resultEmpty;
                }

                //*****************************************//
            }
            catch (Exception ex)
            {
                _resultEmpty = new StatusResult()
                {
                    EntryIDs = "-1",
                    Status = "حدث خطأ . برجاء مراجعة مسئول النظام"
                };
                return _resultEmpty;
            }


        }
        [AllowAnonymous]
        [HttpPost, Route("DownloadFiles")]
        //  [AduitFilter("Document", "DownloadFiles")]
        public StatusResult DownloadFiles([FromBody] DownloadFilesParams ObjectToSearch_Param)
        {
            StatusResult _resultEmpty = null;
            string result = "0";
            string EntryIDs = "";
            try
            {

                if (string.IsNullOrEmpty(ObjectToSearch_Param.EntryID))
                {
                    _resultEmpty = new StatusResult()
                    {
                        Status = "EntryID Not Valid"
                    };
                    return _resultEmpty;
                }

                string[] EntryIdnames = ObjectToSearch_Param.EntryID.Split(',');

                if (EntryIdnames.Length == 0)
                {
                    _resultEmpty = new StatusResult()
                    {
                        Status = "EntryID Not Valid"
                    };
                    return _resultEmpty;
                }

                if (string.IsNullOrEmpty(ObjectToSearch_Param.FolderPathToSave))
                {
                    _resultEmpty = new StatusResult()
                    {
                        Status = "FolderPath Not Valid"
                    };
                    return _resultEmpty;
                }



                foreach (string str in EntryIdnames)
                {
                    int entryIDval = -1;
                    if (int.TryParse(str, out entryIDval))
                    {
                        if (!Directory.Exists(ObjectToSearch_Param.FolderPathToSave))
                        {
                            Directory.CreateDirectory(ObjectToSearch_Param.FolderPathToSave);
                        }

                        string DocumentViewerTempFolder = ObjectToSearch_Param.FolderPathToSave + "\\" + entryIDval.ToString();

                        LFDirectConnector.DownloadedFilePath = DocumentViewerTempFolder;

                        DownloadFileDirectFromLF(entryIDval, DocumentViewerTempFolder, ObjectToSearch_Param.LFServerName, ObjectToSearch_Param.LFRepostry, ObjectToSearch_Param.LFUserName, ObjectToSearch_Param.LFUserPassword);
                    }
                }


                _resultEmpty = new StatusResult()
                {
                    Status = "تم الحفظ "
                };
                return _resultEmpty;

            }
            catch (Exception ex)
            {
                _resultEmpty = new StatusResult()
                {
                    Status = "حدث خطأ . برجاء مراجعة مسئول النظام"
                };
                return _resultEmpty;
            }
        }
    }
}
