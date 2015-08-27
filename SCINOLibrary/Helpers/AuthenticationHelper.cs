using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCINOLibrary.Models;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SCINOLibrary.Helpers
{
    public class AuthenticationHelper
    {
        public AuthenticationHelper()
        {
            db = new ApplicationDbContext();
        }

        /*public void SendMail(string userID)
        {
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.Find(userID);
                // наш email с заголовком письма
                MailAddress from = new MailAddress("SCINOLibrary@inostudio.com", "Web Registration");
                // кому отправляем
                MailAddress to = new MailAddress(user.Email);
                // создаем объект сообщения
                MailMessage message = new MailMessage(from, to);
                // тема письма
                message.Subject = "Email confirmation";
                // текст письма - включаем в него ссылку
                message.Body = string.Format("Thanks for registration!");
                message.IsBodyHtml = true;
                // адрес smtp-сервера, с которого мы и будем отправлять письмо
                SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.yandex.ru", 25);
                // логин и пароль
                smtp.Credentials = new System.Net.NetworkCredential("SCINOLibrary@inostudio.com", "password");
                smtp.Send(message);
            }
        }*/

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