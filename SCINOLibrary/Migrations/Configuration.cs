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
        /// �������� ����� ������������ � ��������������
        /// �������� ������� ������ ��������������
        /// </summary>
        /// <param name="context"></param>
        private void InitUsers(ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(
                new IdentityRole { Name = "Admin" },
                new IdentityRole { Name = "User" }
                );

            // �������� ������ � �������� ��� � ����
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // ������� �������������
            var admin = new ApplicationUser
            {
                Email = "kairgrw@mail.ru",
                UserName = "kairgrw",
                Surname = "������",
                Name = "������"
            };
            string password = "123456";
            
            var result = userManager.Create(admin, password);
            var role = context.Roles.ToList().Find(x => x.Name == "Admin");
                                  
            // ���� �������� ������������ ������ �������
            if (result.Succeeded && role != null)
            {
                // ��������� ��� ������������ ����
                userManager.AddToRole(admin.Id, role.Name);
            }
        }
    }
}
