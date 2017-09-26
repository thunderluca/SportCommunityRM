using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Services
{
    public interface IStorageService
    {
        Task<byte[]> GetFileBytesAsync(string fileId);

        Task<string> StoreFileAsync(string fileId, byte[] bytes);

        bool DeleteFile(string fileId);
    }
}
