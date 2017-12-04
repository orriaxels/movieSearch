﻿using System;
using System.Collections.Generic;
using System.Text;
using MovieSearch.Model;


namespace MovieDownload
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class ImageDownloader
    {
        private IImageStorage _imageStorage;

        public ImageDownloader(IImageStorage imageStorage)
        {
            this._imageStorage = imageStorage;
        }

        public string LocalPathForFilename(string remoteFilePath)
        {
            if (remoteFilePath == null)
            {
                return string.Empty;
            }

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string localPath = Path.Combine(documentsPath, remoteFilePath.TrimStart('/'));
            return localPath;
        }

        public async Task DownloadImage(string remoteFilePath, string localFilePath, CancellationToken token)
        {
            var fileStream = new FileStream(
                                 localFilePath,
                                 FileMode.Create,
                                 FileAccess.Write,
                                 FileShare.None,
                                 short.MaxValue,
                                 true);
            try
            {
                await this._imageStorage.DownloadAsync(remoteFilePath, fileStream, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task DownloadImagesInList(List<MovieDetails> movieDisplayInfoList)
        {
            foreach (var mInfo in movieDisplayInfoList)
            {
                var posterPath = mInfo.imageUrl;
                var localPosterPath = this.LocalPathForFilename(posterPath);
                mInfo.posterFilePath = localPosterPath;
                if (localPosterPath != string.Empty && !File.Exists(localPosterPath))
                {
                    await this.DownloadImage(posterPath, localPosterPath, new CancellationToken());
                }
            }
        }
    }
}
