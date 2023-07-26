using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommiteeTypesController : _BaseController<CommiteeType, CommiteeTypeDTO>
    {
        private readonly ICommiteeTypeService _commiteeTypeService;

        public CommiteeTypesController(ICommiteeTypeService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteeTypeService = businessService;
        }
        [HttpPost("AddCommiteeTypeImage")]
        public object AddCommiteeTypeImage(int id)
        {
            if (Request.ContentType == null)
            {
                return null;
            }
            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            Microsoft.AspNetCore.Http.IFormFile file = Request.Form.Files[0];
            byte[] BinaryContent = null;
            using (System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
            {
                BinaryContent = binaryReader.ReadBytes((int)file.Length);
            }
            byte[] ProfileImage = BinaryContent;
            string ProfileImageMimeType = file.ContentType;
            CommiteeTypeDTO commiteeType = _commiteeTypeService.AddCommiteeTypeImage(id, ProfileImage, ProfileImageMimeType);
            return new { commiteeTypeId = commiteeType.CommiteeTypeId, ProfileImage = commiteeType.Image, ProfileImageMimeType = commiteeType.ImageMimeType };
        }
    }
}
