namespace Application.Dal
{
    /// <summary>
    /// Collection of MimeType Constants for using to avoid Typos
    /// If needed MimeTypes missing feel free to add
    /// </summary>
    public static class MimeTypes
    {
        #region application/*

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationForceDownload = "application/force-download";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationJson = "application/json";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationManifestJson = "application/manifest+json";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationOctetStream = "application/octet-stream";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationPdf = "application/pdf";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationRssXml = "application/rss+xml";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationXml = "application/xml";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// Type
        /// </summary>
        public const string ApplicationXZipCo = "application/x-zip-co";

        #endregion

        #region image/*

        /// <summary>
        /// Type
        /// </summary>
        public const string ImageBmp = "image/bmp";

        /// <summary>
        /// Type
        /// </summary>
        public const string ImageGif = "image/gif";

        /// <summary>
        /// Type
        /// </summary>
        public const string ImageJpeg = "image/jpeg";

        /// <summary>
        /// Type
        /// </summary>
        public const string ImagePJpeg = "image/pjpeg";

        /// <summary>
        /// Type
        /// </summary>
        public const string ImagePng = "image/png";

        /// <summary>
        /// Type
        /// </summary>
        public const string ImageTiff = "image/tiff";

        #endregion

        #region text/*

        /// <summary>
        /// Type
        /// </summary>
        public const string TextCss = "text/css";

        /// <summary>
        /// Type
        /// </summary>
        public const string TextCsv = "text/csv";

        /// <summary>
        /// Type
        /// </summary>
        public const string TextJavascript = "text/javascript";

        /// <summary>
        /// Type
        /// </summary>
        public const string TextPlain = "text/plain";
        #endregion

        #region documents
        /// <summary>
        /// Document Microsoft Office Word
        /// </summary>
        public const string MsDoc = "application/msword";

        /// <summary>
        /// Document Microsoft Office Word Open XML
        /// </summary>
        public const string MsDocx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        /// <summary>
        /// Document Microsoft Office Excel
        /// </summary>
        public const string MsXls = "application/ms-excel";

        /// <summary>
        /// Document Microsoft Office Excel Open XML
        /// </summary>
        public const string MsXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// Document Microsoft Office Excel
        /// </summary>
        public const string MsPpt = "application/ms-powerpoint";

        /// <summary>
        /// Document Microsoft Office Excel Open XML
        /// </summary>
        public const string MsPptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        #endregion
    }
}