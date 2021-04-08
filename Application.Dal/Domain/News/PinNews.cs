using System.ComponentModel.DataAnnotations;

namespace Application.Dal.Domain.News
{
    public class PinNews : BaseEntity
    {
        public string CategoryId { get; set; }
        public string NewsItemId { get; set; }
        public int Index { get; set; }
    }
}