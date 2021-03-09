using System;

namespace Application.Dal.Domain
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {

        }

        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string LastChangeAuthor { get; set; }
    }
}