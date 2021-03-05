using Application.Dal;
using Application.Dal.Domain.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public interface IFileUploadService
    {
        FileBinary GetFileBinaryByFileId(string fileId);

        /// <summary>
        /// Gets a file
        /// </summary>
        /// <param name="fileId">file identifier</param>
        /// <returns>file</returns>
        File GetFileById(string fileId);

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
        /// <param name="defaultFileName">File name which will be use if IFormFile.FileName not present</param>
        /// <param name="virtualPath">Virtual path</param>
        /// <returns>File</returns>
        File InsertFile(IFormFile formFile, string defaultFileName = "", string virtualPath = "");

        /// <summary>
        /// Inserts a file
        /// </summary>
        /// <param name="fileBinary">The file binary</param>
        /// <param name="mimeType">The file MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the file is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided file binary</param>
        /// <returns>file</returns>
        File InsertFile(byte[] fileBinary, string mimeType,string fileName);

        /// <summary>
        /// Updates the file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="fileBinary">The file binary</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the file is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided file binary</param>
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
        /// Gets or sets a value indicating whether the images should be stored in data base.
        /// </summary>
        bool StoreInDb { get; set; }
    }
}