using System.ComponentModel.DataAnnotations.Schema;
using Application.Dal.Domain.News;

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
        /// Naming for storing in file system
        /// </summary>
        public string StoredName { get; set; }

        /// <summary>
        /// Gets or sets the file virtual path
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// Gets or sets file hash
        /// </summary>
        public string Md5Hash { get; set; }

        /// <summary>
        /// Gets or sets file Extension
        /// </summary>
        public string LastPart { get; set; }

        public string NewsItemId { get; set; }


    }
}