using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Birthday
{
    public interface IBirthdayService
    {
        IEnumerable<UserTest> GetUsers();
    }

    public class UserTest {
        public string Fio { get; set; }
        public string Photo { get; set; }
    }
}
