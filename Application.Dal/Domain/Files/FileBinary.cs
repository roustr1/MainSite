namespace Application.Dal.Domain.Files
{
    public class FileBinary:BaseEntity
    {
        /// <summary>
        /// File in binary format
        /// </summary>
        public  byte[] BinaryData { get; set; }
        /// <summary>
        /// Reference to file info
        /// </summary>
        public string FileId { get; set; }
    }
}