using System;

namespace Application.Dal.Domain
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }

        protected BaseEntity(string lastChangeAuthor) : this()
        {
            this.lastChangeAuthor = lastChangeAuthor;
            LastChangeDate = DateTime.Now;
        }
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string lastChangeAuthor { get; set; }
    }
}