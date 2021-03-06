﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly ILogger Logger;

        public LocalStorageService(ILogger<LocalStorageService> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static string RootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "users");

        private void EnsureRootPathExists()
        {
            if (!Directory.Exists(RootPath))
                Directory.CreateDirectory(RootPath);
        }

        private static string CreateFilePath(string fileId) => Path.Combine(RootPath, $"{fileId}.jpg");

        public async Task<byte[]> GetFileBytesAsync(string fileId)
        {
            this.EnsureRootPathExists();

            var filePath = CreateFilePath(fileId);

            if (!File.Exists(filePath)) return null;

            var bytes = await File.ReadAllBytesAsync(filePath);

            return bytes;
        }

        public async Task<string> StoreFileAsync(string fileId, byte[] bytes)
        {
            this.EnsureRootPathExists();

            var filePath = CreateFilePath(fileId);
            
            try
            {
                await File.WriteAllBytesAsync(filePath, bytes);
                return filePath;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                return null;
            }
        }

        public bool DeleteFile(string fileId)
        {
            this.EnsureRootPathExists();

            var filePath = CreateFilePath(fileId);

            if (!File.Exists(filePath)) return true;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                return false;
            }
        }
    }
}
