using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Dal;
using Application.Dal.Infrastructure;
using Microsoft.AspNetCore.Http;
using File = Application.Dal.Domain.Files.File;

namespace Application.Services.Files
{    /// <summary>
     /// Сервис обработки файлов
     /// </summary>
    public partial class FileDownloadService : IFileDownloadService
    {
        #region Fields

        //   private readonly IEventPublisher _eventPubisher;
        private readonly IRepository<File> _downloadRepository;
        private readonly IAppFileProvider _fileProvider;
        #endregion

        #region Ctor

        public FileDownloadService(IRepository<File> downloadRepository, IAppFileProvider fileProvider)
        {
            _downloadRepository = downloadRepository;
            _fileProvider = fileProvider;
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
            if (downloadId == null)
                return null;

            return _downloadRepository.Get(downloadId);
        }



        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void DeleteDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));

            _downloadRepository.Delete(download);

            //event notification
            //   _eventPubisher.EntityDeleted(download);
        }

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void InsertDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));

            _downloadRepository.Add(download);

            //event notification
            //   _eventPubisher.EntityInserted(download);
        }

        /// <summary>
        /// Save file on file system
        /// </summary>
        /// <param name="fileName">file name  </param>
        /// <param name="fileBinary">file binary</param>
        protected virtual void SaveFileInFileSystem(string fileName, byte[] fileBinary)
        {
            _fileProvider.WriteAllBytes(GetLocalPath(fileName), fileBinary);
        }
        protected virtual string GetLocalPath(string fileName)
        {
            return _fileProvider.GetAbsolutePath("files", fileName);
        }

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void UpdateDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));

            _downloadRepository.Update(download);

            //event notification
            //_eventPubisher.EntityUpdated(download);
        }

        public virtual IEnumerable<File> GetFilesByNewsId(string newsId)
        {
            return _downloadRepository.GetAll.Where(c => c.NewsItemId == newsId);
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