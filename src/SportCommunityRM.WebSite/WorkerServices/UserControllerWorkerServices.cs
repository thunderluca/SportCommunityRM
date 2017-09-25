using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.ViewModels.User;
using SportCommunityRM.WebSite.Services;
using System.Threading.Tasks;
using SportCommunityRM.Data.Models;
using SkiaSharp;
using System.IO;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class UserControllerWorkerServices : BaseControllerWorkerServices
    {
        private readonly IStorageService StorageService;

        public UserControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService,
            IStorageService storageService,
            ILogger<BaseControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, logger)
        {
            this.StorageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public DetailViewModel GetDetailViewModel(Guid registeredUserId)
        {
            var model = (from registeredUser in this.Database.RegisteredUsers
                         where registeredUser.Id == registeredUserId
                         let teams = registeredUser.Teams
                            .Select(rut => new DetailViewModel.Team
                            {
                                Id = rut.TeamId,
                                Name = rut.Team.Name
                            })
                         select new DetailViewModel
                         {
                             Id = registeredUserId,
                             FirstName = registeredUser.FirstName,
                             LastName = registeredUser.LastName,
                             BirthDate = registeredUser.BirthDate,
                             Teams = teams
                         }).SingleOrDefault();

            return model;
        }

        public async Task<byte[]> GetUserPictureAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var user = await this.GetApplicationUserAsync(username);

            var registeredUser = this.Database.RegisteredUsers.WithUserId(user.Id);
            if (registeredUser == null || string.IsNullOrWhiteSpace(registeredUser.PictureId))
                return null;

            var bytes = await this.StorageService.GetFileBytesAsync(registeredUser.PictureId);
            return bytes;
        }

        private static async Task<byte[]> ResizeImageAsync(byte[] originalBuffer, int size, int quality)
        {
            using (var inputMemoryStream = new MemoryStream())
            {
                await inputMemoryStream.WriteAsync(originalBuffer, 0, originalBuffer.Length);

                using (var inputStream = new SKManagedStream(inputMemoryStream))
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        var scaled = ScaledSize(original.Width, original.Height, size);

                        using (var resized = original.Resize(new SKImageInfo(scaled.width, scaled.height), SKBitmapResizeMethod.Lanczos3))
                        {
                            if (resized == null) return null;

                            using (var image = SKImage.FromBitmap(resized))
                            {
                                using (var outputMemoryStream = new MemoryStream())
                                {
                                    await Task.Run(() => image.Encode(SKEncodedImageFormat.Jpeg, quality).SaveTo(outputMemoryStream));

                                    return outputMemoryStream.ToArray();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static (int width, int height) ScaledSize(int inWidth, int inHeight, int outSize)
        {
            var width = inWidth > inHeight ? outSize : (int)Math.Round(inWidth * outSize / (double)inHeight);
            var height = inWidth > inHeight ? (int)Math.Round(inHeight * outSize / (double)inWidth) : outSize;

            return (width, height);

        }
    }
}
