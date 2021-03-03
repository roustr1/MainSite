namespace Application.Dal.Domain.Files
{
    public class FileBinary:BaseEntity
    {
        public  byte[] BinaryData { get; set; }
        public string FileId { get; set; }
    }
}