using Microsoft.AspNetCore.Http;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IFileService
    {
        Task<(string Url, string PublicId)> UploadAsync(IFormFile file, string? folderName = null);
        Task<bool> DeleteAsync(string publicId);
        Task<List<(string Url, string PublicId)>> UploadMultipleAsync(List<IFormFile> files, string? folderName = null);
    }
}
