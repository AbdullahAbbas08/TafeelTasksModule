using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.IO;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeTypeService : BusinessService<CommiteeType, CommiteeTypeDTO>, ICommiteeTypeService
    {
        IUnitOfWork _uow;
        public CommiteeTypeService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _uow = unitOfWork;
        }
        public Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            if (imageIn != null)
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            return ms.ToArray();
        }
        public CommiteeTypeDTO AddCommiteeTypeImage(int id, byte[] profileImage, string profileImageMimeType)
        {
            MemoryStream ms = new MemoryStream(profileImage);
            Image newImage = GetReducedImage(100, 100, ms);
            byte[] ProfileImageThumbnail = ImageToByteArray(newImage);
            CommiteeTypeDTO commiteeTypeDTO = new CommiteeTypeDTO();
            CommiteeType commiteeType = _uow.GetRepository<CommiteeType>().GetById(id);

            commiteeType.Image = ProfileImageThumbnail;
            commiteeType.ImageMimeType = profileImageMimeType;
            _uow.GetRepository<CommiteeType>().Update(commiteeType);
            return _Mapper.Map(commiteeType, commiteeTypeDTO);
        }
    }
}
