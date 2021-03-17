using System.Collections.Generic;
using Application.Dal;
using Application.Dal.Domain.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public interface IFileUploadService
    {
        FileBinary GetFileBinaryByFileId(string fileId);

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="file">file</param>
        /// <returns>Picture binary</returns>
        byte[] LoadFileBinary(File file);

        /// <summary>
        /// Gets a file
        /// </summary>
        /// <param name="fileId">file identifier</param>
        /// <returns>file</returns>
        File GetFileById(string fileId);

        IEnumerable<File> GetFilesByNewsId(string id);

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="file">file</param>
        void DeleteFile(File file);

        IPagedList<File> GetFiles(string virtualPath = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Inserts a file
        /// </summary>
        /// <param name="formFile">Form file</param>
        /// <param name="defaultFileName">File name which will be use if IFormFile.OriginalName not present</param>
        /// <param name="virtualPath">Virtual path</param>
        /// <returns>file</returns>
        File InsertFile(IFormFile formFile, string defaultFileName = "");

        /// <summary>
        /// Inserts a file
        /// </summary>
        /// <param name="fileBinary">The file binary</param>
        /// <param name="mimeType">The file MIME type</param>
        /// <param name="fileName"></param>
        /// <returns>file</returns>
        File InsertFile(byte[] fileBinary, string mimeType, string fileName,string lastPart );

        /// <summary>
        /// Updates the file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="fileBinary">The file binary</param>
        /// <param name="mimeType">file mime type</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <returns>file</returns>
        File UpdateFile(string fileId, byte[] fileBinary, string mimeType,
            string titleAttribute = null);

        /// <summary>
        /// Updates the file
        /// </summary>
        /// <param name="file">The file to update</param>
        /// <returns>file</returns>
        File UpdateFile(File file);

        /// <summary>
        /// Get product file binary by file identifier
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <returns>file binary</returns>
        FileBinary GetFileBinaryById(string fileId);

        /// <summary>
        /// Gets or sets a value indicating whether the files should be stored in data base.
        /// </summary>
        bool StoreInDb { get; set; }
    }
}