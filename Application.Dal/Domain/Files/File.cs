 
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
        public string OriginalName { get; set; }

        /// <summary>
        /// Mime-type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets file hash
        /// </summary>
        public string Md5Hash { get; set; }

        /// <summary>
        /// Gets or sets file Extension
        /// </summary>
        public string LastPart { get; set; }

        /// <summary>
        /// Gets or sets the download binary
        /// </summary>
        public byte[] FileBinary { get; set; }

        public string NewsItemId { get; set; }


    }
}