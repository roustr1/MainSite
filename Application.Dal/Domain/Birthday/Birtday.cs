using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dal.Domain.Birthday
{
    [Table("birthday_Table")]
    public class Birtday:BaseEntity
    {
        [Column("ShortDep")]
        public string DepartmentShortName { get; set; }
        [Column("Department")]
        public string DepartmentFullName { get; set; }
        public string FIO { get; set; }
        public DateTime Birth { get; set; }
    }
}