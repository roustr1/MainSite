using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Dal.Domain.Files;
using Microsoft.AspNetCore.Http;

namespace MainSite.Models
{
    public class NewsItemModel
    {
        [Display(Name = "Название"), Required]
        public string Header { get; set; }

        [Display(Name = "Описание"), Required]
        public string Description { get; set; }

        [Display(Name = "Текущая категория")]
        public string Category { get; set; }

        public IFormFile[] UploadedFiles { get; set; }


    }
}
