namespace Application.Dal.Domain.Files
{
    /// <summary>
    /// 
    /// </summary>
    public class File : BaseEntity
    {
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Mime-type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAvaliable { get; set; }

        /// <summary>
        /// Gets or sets the file virtual path
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// Gets or sets file hash
        /// </summary>
        public string Md5Hash { get; set; }

    }
}