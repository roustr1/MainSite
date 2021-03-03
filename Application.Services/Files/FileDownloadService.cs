using System;
using System.IO;
using Application.Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using File = Application.Dal.Domain.Files.File;

namespace Application.Services.Files
{    /// <summary>
     /// Download service
     /// </summary>
    public partial class FileDownloadService : IFileDownloadService
     {
        #region Fields

        private readonly ApplicationContext _context;
        #endregion

        #region Ctor

        public FileDownloadService(ApplicationContext context)
        {
            _context = context;
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

            return _context.Files.Find(downloadId);
        }



        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void DeleteDownload(File file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            _context.Files.Remove(file);
            _context.SaveChanges();
            //event notification
            //_eventPubisher.EntityDeleted(file);
        }

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void InsertDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));
            _context.Files.Add(download);
            _context.SaveChanges();
            //event notification
            //  _eventPubisher.EntityInserted(download);
        }

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="download">Download</param>
        public virtual void UpdateDownload(File download)
        {
            if (download == null)
                throw new ArgumentNullException(nameof(download));
            _context.Entry(download).State = EntityState.Modified;
            _context.SaveChanges();
            //event notification
          //  _eventPubisher.EntityUpdated(download);
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