using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCINOLibrary.Models;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SCINOLibrary.Helpers
{
    /// <summary>
    /// Вспомогательный класс для аутентификации пользователя в системе
    /// </summary>
    public class AuthenticationHelper
    {
        public AuthenticationHelper()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// Создает и возвращает модель персональных данных пользователя
        /// Пароль пользователя не включен в модель
        /// </summary>
        /// <returns></returns>
        public ManagePersonalDataViewModel ConstructPersonalDataModel(string userID)
        {
            ManagePersonalDataViewModel model = new ManagePersonalDataViewModel();

            ApplicationUser user = db.Users.Find(userID);

            model.Surname = user.Surname;
            model.Name = user.Name;
            model.Email = user.Email;
            if (user.BirthDate != null)
                model.BirthDate = user.BirthDate.Value.ToString("d");
            else
                model.BirthDate = user.BirthDate.ToString();
            model.Address = user.Address;

            return model;
        }

        private ApplicationDbContext db;
    }
}