using System;

namespace Application.Dal.Domain
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.Empty.ToString();
        }

        public string Id { get; set; }
    }
}