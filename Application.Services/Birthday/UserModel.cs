using System;

namespace Application.Services.Birthday
{
    public class UserModel {
        public string Id { get; set; }
        public string MrangName { get; set; }
        public string FullFio { get; set; }
        public string PhotoPath { get; set; }
        public string FullPhotoPath { get; set; }
        public string ShortFio { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SubDivision { get; set; }
        public string SubDivisionId { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}