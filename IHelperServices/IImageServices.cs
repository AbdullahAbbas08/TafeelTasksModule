using IHelperServices.Models;

namespace IHelperServices
{
    public interface IImageServices : _IHelperService
    {
        ImageSize GetImageSize(byte[] content);
    }
}