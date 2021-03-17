using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Birthday
{
    public class BirthdayService : IBirthdayService
    {
        public IEnumerable<UserTest> GetUsers()
        {
            var listTest = new List<UserTest> {
                new UserTest(){ Fio="Пирогов А.Р.", Photo ="/images/layout_icons/user.png"},
                new UserTest(){ Fio="Пирогов А.Р.", Photo ="/images/layout_icons/user.png"},
                new UserTest(){ Fio="Пирогов А.Р.", Photo ="/images/layout_icons/user.png"},
                new UserTest(){ Fio="Пирогов А.Р.", Photo ="/images/layout_icons/user.png"},
                new UserTest(){ Fio="Пирогов А.Р.", Photo ="/images/layout_icons/user.png"}
            };

            return listTest;
        }
    }
}
