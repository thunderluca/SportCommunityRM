using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Services
{
    public interface IStorageService
    {
        Task<byte[]> GetFileBytesAsync(string fileId);

        Task<bool> StoreDataAsync(string fileId, byte[] bytes);
    }
}
