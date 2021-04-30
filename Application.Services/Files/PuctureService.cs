using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Application.Dal;
using Application.Dal.Domain.Files;
using Application.Dal.Infrastructure;
using Application.Services.Settings;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static SixLabors.ImageSharp.Configuration;
using Image = SixLabors.ImageSharp.Image;
using Size = SixLabors.ImageSharp.Size;


namespace Application.Services.Files
{
    /// <summary>
    /// Picture service
    /// </summary>
    public partial class PictureService : IPictureService
    {
        #region Fields

        private readonly IFileDownloadService _downloadService;
        //  private readonly IEventPublisher _eventPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppFileProvider _fileProvider;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<FileBinary> _pictureBinaryRepository;
        private readonly ISettingsService _settingService;
        private static bool StoreInDb = false;

        #endregion

        #region Ctor

        public PictureService(IFileDownloadService downloadService, IHttpContextAccessor httpContextAccessor, IAppFileProvider fileProvider, IRepository<Picture> pictureRepository, IRepository<FileBinary> pictureBinaryRepository, ISettingsService settingService)
        {
            _downloadService = downloadService;
            _httpContextAccessor = httpContextAccessor;
            _fileProvider = fileProvider;
            _pictureRepository = pictureRepository;
            _pictureBinaryRepository = pictureBinaryRepository;
            _settingService = settingService;
        }
        #endregion

        #region Utilities



        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="resizeType">Resize type</param>
        /// <param name="ensureSizePositive">A value indicating whether we should ensure that size values are positive</param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }

                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (!ensureSizePositive)
                return new Size((int)Math.Round(width), (int)Math.Round(height));

            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            //we invoke Math.Round to ensure that no white background is rendered - https://www.nopcommerce.com/boards/topic/40616/image-resizing-bug
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        /// <summary>
        /// Loads a picture from file
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureFromFile(string pictureId, string mimeType)
        {
            var lastPart = GetFileExtensionFromMimeType(mimeType);
            var fileName = $"{pictureId}.{lastPart}";
            var filePath = GetPictureLocalPath(fileName);

            return _fileProvider.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Save picture on file system
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        protected virtual void SavePictureInFile(string pictureId, byte[] pictureBinary, string mimeType)
        {
            var lastPart = GetFileExtensionFromMimeType(mimeType);
            var fileName = $"{pictureId}.{lastPart}";
            _fileProvider.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
        }

        /// <summary>
        /// Delete a picture on file system
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            var fileName = $"{picture.Id}.{lastPart}";
            var filePath = GetPictureLocalPath(fileName);
            _fileProvider.DeleteFile(filePath);
        }

        /// <summary>
        /// Delete picture thumbs
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            var filter = $"{picture.Id}*.*";
            var currentFiles = _fileProvider.GetFiles(_fileProvider.GetAbsolutePath(AppMediaDefaults.ImageThumbsPath), filter, false);
            foreach (var currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                _fileProvider.DeleteFile(thumbFilePath);
            }
        }

        /// <summary>
        /// Get picture (thumb) local path
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(AppMediaDefaults.ImageThumbsPath);



            var thumbFilePath = _fileProvider.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        /// <summary>
        /// Get images path URL 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetImagesPathUrl()
        {
            var pathBase = _httpContextAccessor.HttpContext.Request.PathBase.Value ?? string.Empty;

            var imagesPathUrl = $"{pathBase}/"; // _mediaSettings.UseAbsoluteImagePath ? storeLocation : $"{pathBase}/";

            imagesPathUrl = string.IsNullOrEmpty(imagesPathUrl)
                ? _httpContextAccessor.HttpContext.Request.PathBase.Value
                : imagesPathUrl;

            imagesPathUrl += "images/";

            return imagesPathUrl;
        }

        /// <summary>
        /// Get picture (thumb) URL 
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbUrl(string thumbFileName)
        {
            var url = GetImagesPathUrl() + "thumbs/";

            url = url + thumbFileName;
            return url;
        }

        /// <summary>
        /// Get picture local path. Used when images stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local picture path</returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return _fileProvider.GetAbsolutePath("images", fileName);
        }

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <param name="fromDb">Load from database; otherwise, from file system</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureBinary(Picture picture, bool fromDb)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var result = fromDb
                ? GetPictureBinaryByPictureId(picture.Id)?.BinaryData ?? Array.Empty<byte>()
                : LoadPictureFromFile(picture.Id, picture.MimeType);

            return result;
        }

        /// <summary>
        /// Get a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <returns>Result</returns>
        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {
            return _fileProvider.FileExists(thumbFilePath);
        }

        /// <summary>
        /// Save a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <param name="mimeType">MIME type</param>
        /// <param name="binary">Picture binary</param>
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            //ensure \thumb directory exists
            var thumbsDirectoryPath = _fileProvider.GetAbsolutePath(AppMediaDefaults.ImageThumbsPath);
            _fileProvider.CreateDirectory(thumbsDirectoryPath);

            //save
            _fileProvider.WriteAllBytes(thumbFilePath, binary);
        }

        /// <summary>
        /// Updates the picture binary data
        /// </summary>
        /// <param name="picture">The picture object</param>
        /// <param name="binaryData">The picture binary data</param>
        /// <returns>Picture binary</returns>
        protected virtual FileBinary UpdatePictureBinary(Picture picture, byte[] binaryData)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var pictureBinary = GetPictureBinaryByPictureId(picture.Id);

            var isNew = pictureBinary == null;

            if (isNew)
                pictureBinary = new FileBinary
                {
                    FileId = picture.Id
                };

            pictureBinary.BinaryData = binaryData;

            if (isNew)
            {
                _pictureBinaryRepository.Add(pictureBinary);

                //event notification
                //   _eventPublisher.EntityInserted(pictureBinary);
            }
            else
            {
                _pictureBinaryRepository.Update(pictureBinary);

                //event notification
                //    _eventPublisher.EntityUpdated(pictureBinary);
            }

            return pictureBinary;
        }

        /// <summary>
        /// Encode the image into a byte array in accordance with the specified image format
        /// </summary>
        /// <typeparam name="TPixel">Pixel data type</typeparam>
        /// <param name="image">Image data</param>
        /// <param name="imageFormat">Image format</param>
        /// <param name="quality">Quality index that will be used to encode the image</param>
        /// <returns>Image binary data</returns>
        protected virtual byte[] EncodeImage<TPixel>(Image<TPixel> image, IImageFormat imageFormat, int? quality = null)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using var stream = new MemoryStream();
            var imageEncoder = Default.ImageFormatsManager.FindEncoder(imageFormat);
            switch (imageEncoder)
            {
                case JpegEncoder jpegEncoder:
                    jpegEncoder.Subsample = JpegSubsample.Ratio444;
                    jpegEncoder.Quality = quality ?? 80;
                    jpegEncoder.Encode(image, stream);
                    break;

                case PngEncoder pngEncoder:
                    pngEncoder.ColorType = PngColorType.RgbWithAlpha;
                    pngEncoder.Encode(image, stream);
                    break;

                case BmpEncoder bmpEncoder:
                    bmpEncoder.BitsPerPixel = BmpBitsPerPixel.Pixel32;
                    bmpEncoder.Encode(image, stream);
                    break;

                case GifEncoder gifEncoder:
                    gifEncoder.Encode(image, stream);
                    break;

                default:
                    imageEncoder.Encode(image, stream);
                    break;
            }

            return stream.ToArray();
        }

        #endregion

        #region Getting picture local path/URL methods

        /// <summary>
        /// Returns the file extension from mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>File extension</returns>
        public virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            var parts = mimeType.Split('/');
            var lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }

            return lastPart;
        }

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            return LoadPictureBinary(picture, StoreInDb);
        }



        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetDefaultPictureUrl(int targetSize = 0,
            PictureType defaultPictureType = PictureType.Entity)
        {
            var defaultImageFileName = defaultPictureType switch
            {
                PictureType.Avatar => AppMediaDefaults.DefaultAvatarFileName,
                _ => AppMediaDefaults.DefaultImageFileName
            };
            var filePath = GetPictureLocalPath(defaultImageFileName);
            if (!_fileProvider.FileExists(filePath))
            {
                return string.Empty;
            }

            if (targetSize == 0)
            {
                var url = GetImagesPathUrl( ) + defaultImageFileName;

                return url;
            }
            else
            {
                var fileExtension = _fileProvider.GetFileExtension(filePath);
                var thumbFileName = $"{_fileProvider.GetFileNameWithoutExtension(filePath)}_{targetSize}{fileExtension}";
                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    using var image = Image.Load<Rgba32>(filePath, out var imageFormat);
                    image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = CalculateDimensions(image.Size(), targetSize)
                    }));
                    var pictureBinary = EncodeImage(image, imageFormat);
                    SaveThumb(thumbFilePath, thumbFileName, imageFormat.DefaultMimeType, pictureBinary);
                }

                var url = GetThumbUrl(thumbFileName);
                return url;
            }
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(string pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true,

            PictureType defaultPictureType = PictureType.Entity)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(ref picture, targetSize, showDefaultPicture, defaultPictureType);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Reference instance of Picture</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(ref Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true,
            PictureType defaultPictureType = PictureType.Entity)
        {
            if (picture == null)
                return showDefaultPicture ? GetDefaultPictureUrl(targetSize, defaultPictureType) : string.Empty;

            byte[] pictureBinary = null;
            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);
                pictureBinary = LoadPictureBinary(picture);

                if ((pictureBinary?.Length ?? 0) == 0)
                    return showDefaultPicture ? GetDefaultPictureUrl(targetSize, defaultPictureType) : string.Empty;

                //we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown
                picture = UpdatePicture(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false
                     );
            }


            var lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName = $"{picture.Id}.{lastPart}";

            var thumbFilePath = GetThumbLocalPath(thumbFileName);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (GeneratedThumbExists(thumbFilePath, thumbFileName))
                    return GetThumbUrl(thumbFileName);

                mutex.WaitOne();

                //check, if the file was created, while we were waiting for the release of the mutex.
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    pictureBinary ??= LoadPictureBinary(picture);

                    if ((pictureBinary?.Length ?? 0) == 0)
                        return showDefaultPicture ? GetDefaultPictureUrl(targetSize, defaultPictureType) : string.Empty;

                    byte[] pictureBinaryResized;
                    if (targetSize != 0)
                    {
                        //resizing required
                        using var image = Image.Load<Rgba32>(pictureBinary, out var imageFormat);
                        image.Mutate(imageProcess => imageProcess.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = CalculateDimensions(image.Size(), targetSize)
                        }));

                        pictureBinaryResized = EncodeImage(image, imageFormat);
                    }
                    else
                    {
                        //create a copy of pictureBinary
                        pictureBinaryResized = pictureBinary.ToArray();
                    }

                    SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                }

                mutex.ReleaseMutex();
            }

            return GetThumbUrl(thumbFileName);
        }

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            var url = GetPictureUrl(ref picture, targetSize, showDefaultPicture);
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            return GetThumbLocalPath(_fileProvider.GetFileName(url));
        }

        #endregion

        #region CRUD methods

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public virtual Picture GetPictureById(string pictureId)
        {
            if (pictureId == null)
                return null;

            return _pictureRepository.Get(pictureId);
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture">Picture</param>
        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            //delete thumbs
            DeletePictureThumbs(picture);

            //delete from file system
            if (!StoreInDb)
                DeletePictureOnFileSystem(picture);

            //delete from database
            _pictureRepository.Delete(picture);

            //event notification
            //     _eventPublisher.EntityDeleted(picture);
        }

        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="virtualPath">Virtual path</param>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        public virtual IPagedList<Picture> GetPictures(string virtualPath = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pictureRepository.GetAll.AsQueryable();

            if (!string.IsNullOrEmpty(virtualPath))
                query = virtualPath.EndsWith('/') ? query.Where(p => p.VirtualPath.StartsWith(virtualPath) || p.VirtualPath == virtualPath.TrimEnd('/')) : query.Where(p => p.VirtualPath == virtualPath);

            query = query.OrderByDescending(p => p.Id);

            return new PagedList<Picture>(query, pageIndex, pageSize);
        }



        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public virtual Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true)
        {


            var picture = new Picture
            {
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttribute,
                TitleAttribute = titleAttribute,
                IsNew = isNew
            };
            _pictureRepository.Add(picture);
            UpdatePictureBinary(picture, StoreInDb ? pictureBinary : Array.Empty<byte>());

            if (!StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            //   _eventPublisher.EntityInserted(picture);

            return picture;
        }

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="formFile">Form file</param>
        /// <param name="defaultFileName">File name which will be use if IFormFile.FileName not present</param>
        /// <param name="virtualPath">Virtual path</param>
        /// <returns>Picture</returns>
        public virtual Picture InsertPicture(IFormFile formFile,  string virtualPath = "")
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

            var picture = InsertPicture(_downloadService.GetDownloadBits(formFile), contentType, _fileProvider.GetFileNameWithoutExtension(fileName));

            if (string.IsNullOrEmpty(virtualPath))
                return picture;
            picture.VirtualPath = _fileProvider.GetVirtualPath(virtualPath);
            UpdatePicture(picture);

            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture UpdatePicture(string pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool isNew = true)
        {

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);
            UpdatePictureBinary(picture, StoreInDb ? pictureBinary : Array.Empty<byte>());

            if (!StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            //    _eventPublisher.EntityUpdated(picture);

            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="picture">The picture to update</param>
        /// <returns>Picture</returns>
        public virtual Picture UpdatePicture(Picture picture)
        {
            if (picture == null)
                return null;

            var seoFilename = picture.SeoFilename;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.SeoFilename = seoFilename;

            _pictureRepository.Update(picture);
            UpdatePictureBinary(picture, StoreInDb ? GetPictureBinaryByPictureId(picture.Id).BinaryData : Array.Empty<byte>());

          //  if (!StoreInDb)
              //  SavePictureInFile(picture.Id, GetPictureBinaryByPictureId(picture.Id).BinaryData, picture.MimeType);

            //event notification
            //  _eventPublisher.EntityUpdated(picture);

            return picture;
        }

        /// <summary>
        /// Get product picture binary by picture identifier
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <returns>Picture binary</returns>
        public virtual FileBinary GetPictureBinaryByPictureId(string pictureId)
        {
            return _pictureBinaryRepository.GetAll.FirstOrDefault(pb => pb.FileId == pictureId);
        }


        #endregion
    }
}