using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MainSite.Models
{
    public class NewsItemModel
    {
        public NewsItemModel()
        {
            UploadedFiles = new List<IFormFile>();
        }
        [Display(Name = "Название"), Required]
        public string Header { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Текущая категория")]
        public string Category { get; set; }

        public ICollection<IFormFile> UploadedFiles { get; set; }

    }
}
