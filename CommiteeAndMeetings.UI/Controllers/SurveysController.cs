using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : _BaseController<Survey, SurveyDTO>
    {
        private readonly ISurveyService _commiteeAttachmentService;
        private readonly IDocumentService _IDocumentServices;
        private readonly ISystemSettingsService _systemSettingsService;
        public AppSettings _AppSettings;
        private readonly IHelperServices.ISessionServices _SessionServices;
        public SurveysController(ISurveyService businessService, IDocumentService IDocumentServices, IHelperServices.ISessionServices sessionSevices, IOptions<AppSettings> appSettings, ISystemSettingsService systemSettingsService) : base(businessService, sessionSevices)
        {
            this._commiteeAttachmentService = businessService;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            _SessionServices = sessionSevices;
            _systemSettingsService = systemSettingsService;
        }
        public override IEnumerable<SurveyDTO> Post([FromBody] IEnumerable<SurveyDTO> entities)
        {
            var survey = _commiteeAttachmentService.Insert(entities);
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
            var SurveyUser = Request.Form["surveyUsers"].ToString();
            var SurveyAnswer = Request.Form["SurveyAnswers"].ToString();
            var SurveyUsers = SurveyUser.Split(",");
            List<AttachmentUser> users = new List<AttachmentUser>();
            if (!allUser && SurveyUser != string.Empty && SurveyUser != "undefined")
            {
                foreach (var item in SurveyUsers)
                {
                    users.Add(new AttachmentUser { UserId = Convert.ToInt32(item) });
                }
            }

            var AdditionalParameters = Request.Query.Union(Request.Form);
            string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
            string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();

            int AttachmentTypeId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);

            int SurveyId;
            int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "SurveyId").Value, out SurveyId);

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
                    //  BinaryContent = BinaryContent
                };
                string[] param = new string[]
                {
                    SurveyId.ToString()
                };
                attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));
                CommitteAttachmentList.Add(_IDocumentServices.InsertSurveyAttachment(new SurveyAttachment
                {
                    Active = true,
                    AttachmentId = attachmentList[i].SavedAttachmentId,
                    SurveyId = SurveyId
                }));
                ContentType = null;
                if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
                {
                    _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
                }
                i++;
            }
            return survey;

        }

        [HttpGet("GetAllServey")]
        public AllSurveyDTO GetAllServey(int take, int skip, string CommitteId, DateTime? dateFrom, DateTime? dateTo, string SearchText)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId,true);

            return _commiteeAttachmentService.GetAllServey(take, skip, UserIdAndUserRoleId.Id, dateFrom, dateTo,SearchText);
        }
    }
}
