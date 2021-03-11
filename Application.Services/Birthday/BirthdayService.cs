﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Birthday
{
    public class BirthdayService : IBirthdayService
    {
        public IEnumerable<UserTest> GetUsers()
        {
            var listTest = new List<UserTest> {
                new UserTest(){ Fio="Долгополов А.Р.", Photo ="/content/layout_icons/user.png"},
                new UserTest(){ Fio="Заостровский П.Ф.", Photo ="/content/layout_icons/user.png"},
                new UserTest(){ Fio="Куркунов Ф.К.", Photo ="/content/layout_icons/user.png"},
                new UserTest(){ Fio="Иванов И.И.", Photo ="/content/layout_icons/user.png"}
            };

            return listTest;
        }
    }
}