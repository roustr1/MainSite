using System;

namespace Application.Dal.Domain
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Today;
        }

        protected BaseEntity(string lastChangeAuthor)
        {
            this.lastChangeAuthor = lastChangeAuthor;
            LastChangeDate = DateTime.Now;
        }
        public string Id { get; set; }
        public DateTime CreatedDate { get; }
        public DateTime LastChangeDate { get; }
        public string lastChangeAuthor { get; }
    }
}