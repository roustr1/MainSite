using System;
using System.IO;
using Application.Dal;
using Microsoft.AspNetCore.Http;
using File = Application.Dal.Domain.Files.File;

namespace Application.Services.Files
{    /// <summary>
     /// Download service
     /// </summary>
    public partial class FileDownloadService : IFileDownloadService
     {
        #region Fields

        private readonly IRepository<File> _repository;
        #endregion

        #region Ctor

        public FileDownloadService(IRepository<File> repository)
        {
            _repository = repository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a download
        /// </summary>
        /// <param name="downloadId">Download identifier</param>
        /// <returns>Download</returns>
        public virtual File GetDownloadById(string downloadId)
        {
            if (string.IsNullOrEmpty(downloadId))
                return null;

            return _repository.Get(downloadId);
        }



        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void DeleteDownload(File file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            _repository.Delete(file);
        }

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void InsertDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));
            _repository.Add(download);
        }

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void UpdateDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));
            _repository.Update(download);
        }

        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Download binary array</returns>
        public virtual byte[] GetDownloadBits(IFormFile file)
        {
            using var fileStream = file.OpenReadStream();
            using var ms = new MemoryStream();
            fileStream.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return fileBytes;
        }

        #endregion
    }
}