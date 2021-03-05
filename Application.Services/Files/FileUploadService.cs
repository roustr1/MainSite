using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using Application.Dal;
using Application.Dal.Domain.Files;
using Application.Dal.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Application.Services.Files
{
    /// <summary>
    /// сервис выгрузки файлов (передачи клиенту)
    /// </summary>
    public class FileUploadService : IFileUploadService
    {
        #region Fields
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<FileBinary> _fileBinaryRepository;
        private readonly IAppFileProvider _fileProvider;
        private readonly IFileDownloadService _downloadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        #endregion

        public FileUploadService(IRepository<File> fileRepository, IRepository<FileBinary> fileBinaryRepository, IAppFileProvider fileProvider, IFileDownloadService downloadService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _fileBinaryRepository = fileBinaryRepository;
            _fileProvider = fileProvider;
            _downloadService = downloadService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        protected virtual byte[] LoadFileFromFileSystem(string fileId)
        {
            var file = _fileRepository.Get(fileId);

            return _fileProvider.ReadAllBytes(file.VirtualPath);
        }


        /// <summary>
        /// Get file path URL 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFilePathUrl()
        {
            var pathBase = _httpContextAccessor.HttpContext.Request.PathBase.Value ?? string.Empty;

            return @$"{pathBase}/files/";
        }
        /// <summary>
        /// Get file local path. Used when files stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local file path</returns>
        protected virtual string GetFileLocalPath(string fileName)
        {
            return _fileProvider.GetAbsolutePath("files", fileName);
        }

        public virtual FileBinary GetFileBinaryByFileId(string fileId)
        {
            return _fileBinaryRepository.Get(fileId);
        }
        /// <summary>
        /// Gets the loaded file binary depending on file storage settings
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="fromDb">Load from database; otherwise, from file system</param>
        /// <returns>file binary</returns>
        protected virtual byte[] LoadFileBinary(File file, bool fromDb)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var result = fromDb
                ? GetFileBinaryByFileId(file.Id)?.BinaryData ?? Array.Empty<byte>()
                : LoadFileFromFileSystem(file.Id);

            return result;
        }

        #region CRUD methods

        /// <summary>
        /// Gets a file
        /// </summary>
        /// <param name="fileId">file identifier</param>
        /// <returns>file</returns>
        public virtual File GetFileById(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
                return null;

            return _fileRepository.Get(fileId);
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="file">file</param>
        public virtual void DeleteFile(File file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            //delete from file system
            if (!StoreInDb)
                DeleteFileFromFileSystem(file);

            //delete from database
            _fileRepository.Delete(file);
            //event notification
            // _eventPublisher.EntityDeleted(file);
        }

        ///// <summary>
        ///// Gets a collection of files
        ///// </summary>
        ///// <param name="virtualPath">Virtual path</param>
        ///// <param name="pageIndex">Current page</param>
        ///// <param name="pageSize">Items on each page</param>
        ///// <returns>Paged list of files</returns>
        public virtual IPagedList<File> GetFiles(string virtualPath = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _fileRepository.GetAll();

            if (!string.IsNullOrEmpty(virtualPath))
                query = (ICollection<File>)(virtualPath.EndsWith('/') ? query.Where(p => p.VirtualPath.StartsWith(virtualPath) || p.VirtualPath == virtualPath.TrimEnd('/')) : query.Where(p => p.VirtualPath == virtualPath));

            var result = query.OrderByDescending(p => p.Id).AsQueryable();

            return new PagedList<File>(result, pageIndex, pageSize);
        }

        /// <summary>
        /// Save file on file system
        /// </summary>
        /// <param name="fileId">file identifier</param>
        /// <param name="fileBinary">file binary</param>
        /// <param name="mimeType">MIME type</param>
        protected virtual void SaveFileInFileSystem(string fileId, byte[] fileBinary, string mimeType)
        {
#warning Пересмотреть имя файла, сохранять в более понятном видел
            var fileName = $"{fileId}_0.{fileId}";
            _fileProvider.WriteAllBytes(GetFileLocalPath(fileName), fileBinary);
        }

        /// <summary>
        /// Delete a file on file system
        /// </summary>
        /// <param name="file">file</param>
        protected virtual void DeleteFileFromFileSystem(File file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileName = file.FileName;
            var filePath = GetFileLocalPath(fileName);
            _fileProvider.DeleteFile(filePath);
        }
        /// <summary>
        /// Inserts a file
        /// </summary>
        /// <param name="formFile">Form file</param>
        /// <param name="defaultFileName">File name which will be use if IFormFile.FileName not present</param>
        /// <param name="virtualPath">Virtual path</param>
        /// <returns>file</returns>
        public virtual File InsertFile(IFormFile formFile, string defaultFileName = "", string virtualPath = "")
        {
            var imgExt = new List<string>
            {
                ".bmp",
                ".gif",
                ".jpeg",
                ".jpg",
                ".jpe",
                ".jfif",
                ".pjpeg",
                ".pjp",
                ".png",
                ".tiff",
                ".tif"
            } as IReadOnlyCollection<string>;

            var fileName = formFile.FileName;
            if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(defaultFileName))
                fileName = defaultFileName;

            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = formFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            if (imgExt.All(ext => !ext.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase)))
                return null;

            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (string.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTypes.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTypes.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTypes.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTypes.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTypes.ImageTiff;
                        break;
                    default:
                        break;
                }
            }

            var file = InsertFile(_downloadService.GetDownloadBits(formFile), contentType, _fileProvider.GetFileNameWithoutExtension(fileName));

            if (string.IsNullOrEmpty(virtualPath))
                return file;

            file.VirtualPath = _fileProvider.GetVirtualPath(virtualPath);
            UpdateFile(file);

            return file;
        }
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
        public virtual File InsertFile(byte[] fileBinary, string mimeType,string fileName)
        {
            var file = new File()
            {
                MimeType = mimeType,
                Name =  fileName,
                
                
                

            };
            _fileRepository.Add(file);
            UpdateFileBinary(file, StoreInDb ? fileBinary : Array.Empty<byte>());

            if (!StoreInDb)
                SaveFileInFileSystem(file.Id, fileBinary, mimeType);

            //event notification
            // _eventPublisher.EntityInserted(file);

            return file;
        }


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
        public virtual File UpdateFile(string fileId, byte[] fileBinary, string mimeType,
               string titleAttribute = null)
        {

            var file = GetFileById(fileId);
            if (file == null)
                return null;
            file.MimeType = mimeType;
            //остальные обновляемые параметры
            _fileRepository.Update(file);
            UpdateFileBinary(file, StoreInDb ? fileBinary : Array.Empty<byte>());

            if (!StoreInDb)
                SaveFileInFileSystem(file.Id, fileBinary, mimeType);

            //event notification
            //_eventPublisher.EntityUpdated(file);

            return file;
        }
        /// <summary>
        /// Updates the file binary data
        /// </summary>
        /// <param name="file">The file object</param>
        /// <param name="binaryData">The file binary data</param>
        /// <returns>file binary</returns>
        protected virtual FileBinary UpdateFileBinary(File file, byte[] binaryData)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileBinary = GetFileBinaryByFileId(file.Id);

            var isNew = fileBinary == null;

            if (isNew)
                fileBinary = new FileBinary
                {
                    FileId = file.Id
                };

            fileBinary.BinaryData = binaryData;

            if (isNew)
            {
                _fileBinaryRepository.Add(fileBinary);
                //event notification
                //  _eventPublisher.EntityInserted(fileBinary);
            }
            else
            {
                _fileBinaryRepository.Update(fileBinary);

                //event notification
                //_eventPublisher.EntityUpdated(fileBinary);
            }

            return fileBinary;
        }

        /// <summary>
        /// Updates the file
        /// </summary>
        /// <param name="file">The file to update</param>
        /// <returns>file</returns>
        public virtual File UpdateFile(File file)
        {
            if (file == null)
                return null;

            _fileRepository.Update(file);

            UpdateFileBinary(file, StoreInDb ? GetFileBinaryByFileId(file.Id).BinaryData : Array.Empty<byte>());

            if (!StoreInDb)
                SaveFileInFileSystem(file.Id, GetFileBinaryByFileId(file.Id).BinaryData, file.MimeType);

            //event notification
            //_eventPublisher.EntityUpdated(file);

            return file;
        }

        /// <summary>
        /// Get product file binary by file identifier
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <returns>file binary</returns>
        public virtual FileBinary GetFileBinaryById(string fileId)
        {
            return _fileBinaryRepository.Get(fileId);
        }




        ///// <summary>
        ///// Get files hashes
        ///// </summary>
        ///// <param name="filesIds">files Ids</param>
        ///// <returns></returns>
        //public IDictionary<string, string> GetFilesHash(string[] filesIds)
        //{
        //    if (!filesIds.Any())
        //        return new Dictionary<string, string>();

        //    var hashes = _fileBinaryRepository.GetAll().Result.Where(p => filesIds.Contains(p.FileId))
        //            .Select(x => new
        //            {
        //                x.FileId,
        //                Hash = Hash(x.BinaryData)
        //            });

        //    return hashes.ToDictionary(p => p.FileId, p => p.Hash);
        //}
        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the images should be stored in data base.
        /// </summary>
        public virtual bool StoreInDb
        {
            get => bool.Parse(_configuration["StoreFilesIdDb"]);
            set
            {
                //check whether it's a new value
                if (StoreInDb == value)
                    return;

                //save the new setting value
                _configuration["StoreFilesIdDb"] = value.ToString();

                var pageIndex = 0;
                const int pageSize = 400;
                try
                {
                    while (true)
                    {
                        var files = GetFiles(pageIndex: pageIndex, pageSize: pageSize);
                        pageIndex++;

                        //all files converted?
                        if (!files.Any())
                            break;

                        foreach (var file in files)
                        {
                            if (!string.IsNullOrEmpty(file.VirtualPath))
                                continue;

                            var fileBinary = LoadFileBinary(file, !value);

                            //we used the code below before. but it's too slow
                            //let's do it manually (uncommented code) - copy some logic from "Updatefile" method
                            /*just update a file (all required logic is in "Updatefile" method)
                            we do not validate file binary here to ensure that no exception ("Parameter is not valid") will be thrown when "moving" files
                            Updatefile(file.Id,
                                          fileBinary,
                                          file.MimeType,
                                          file.SeoFilename,
                                          true,
                                          false);*/
                            if (value)
                                //delete from file system. now it's in the database
                                DeleteFileFromFileSystem(file);
                            else
                                //now on file system
                                SaveFileInFileSystem(file.Id, fileBinary, file.MimeType);
                            //update appropriate properties

                            UpdateFileBinary(file, value ? fileBinary : Array.Empty<byte>());

                            //raise event?
                            //_eventPublisher.EntityUpdated(file);
                        }

                        //save all at once
                        _fileRepository.Update(files);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        #endregion

    }
}