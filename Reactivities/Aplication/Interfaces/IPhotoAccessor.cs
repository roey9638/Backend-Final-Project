using Microsoft.AspNetCore.Http;
using Reactivities.Photos;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Interfaces
{
    public interface IPhotoAccessor
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}
