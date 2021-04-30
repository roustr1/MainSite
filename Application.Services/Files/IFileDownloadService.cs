using System.Collections.Generic;
using Application.Dal.Domain.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public interface IFileDownloadService
    {
        /// <summary>
        /// Gets a download
        /// </summary>
        /// <param name="downloadId">Download identifier</param>
        /// <returns>Download</returns>
        File GetDownloadById(string downloadId);

        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="download">Download</param>
        void DeleteDownload(File file);

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="download">Download</param>
        void InsertDownload(File download);

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="download">Download</param>
        void UpdateDownload(File download);

        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Download binary array</returns>
        byte[] GetDownloadBits(IFormFile file);

        IEnumerable<File> GetFilesByNewsId(string newsId);
    }
}