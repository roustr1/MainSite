namespace Application.Dal.Domain.Files
{
    public class File : BaseEntity
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public bool IsAvaliable { get; set; }
        /// <summary>
        /// Gets or sets the file virtual path
        /// </summary>
        public string VirtualPath { get; set; }

    }
}