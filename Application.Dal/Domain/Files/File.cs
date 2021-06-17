
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dal.Domain.Files
{
    /// <summary>
    /// Represents a download
    /// </summary>
    public class File : BaseEntity
    {
 
        
        /// <summary>
        /// Gets or sets a download URL
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Gets or sets the download binary
        /// </summary>
        public byte[] DownloadBinary { get; set; }

        /// <summary>
        /// The mime-type of the download
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The filename of the download
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Refferer to news item 
        /// </summary>
        public string NewsItemId { get; set; }
        /// <summary>
        /// Gets or set storing name for file
        /// </summary>
        public string Name { get; set; }

    }

}
