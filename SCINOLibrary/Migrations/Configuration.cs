namespace SCINOLibrary.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SCINOLibrary.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SCINOLibrary.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SCINOLibrary.Models.ApplicationDbContext";
        }

        protected override void Seed(SCINOLibrary.Models.ApplicationDbContext context)
        {
        }

        /// <summary>
        /// Создание ролей пользователя и администратора
        /// Создание учетной записи администратора
        /// </summary>
        /// <param name="context"></param>
        private void InitUsers(ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(
                new IdentityRole { Name = "Admin" },
                new IdentityRole { Name = "User" }
                );

            // Создание админа и привязка его к роли
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // создаем пользователей
            var admin = new ApplicationUser
            {
                Email = "kairgrw@mail.ru",
                UserName = "kairgrw",
                Surname = "Клюйко",
                Name = "Андрей"
            };
            string password = "123456";
            
            var result = userManager.Create(admin, password);
            var role = context.Roles.ToList().Find(x => x.Name == "Admin");
                                  
            // если создание пользователя прошло успешно
            if (result.Succeeded && role != null)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role.Name);
            }
        }
    }
}
