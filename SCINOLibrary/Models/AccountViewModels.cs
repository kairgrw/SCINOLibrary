using System;
using System.ComponentModel.DataAnnotations;

namespace SCINOLibrary.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class ManagePersonalDataViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        [RegularExpression("^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~]+@([-a-z0-9]+.)+[a-z]{2,5}$", ErrorMessage = "Некорректный e-mail!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите фамилию!")]
        [RegularExpression(@"^([a-zа-яё]|[A-ZА-ЯЁ])[a-zа-яё]{0,20}((-[a-zA-Zа-яёА-ЯЁ]{1,20})?|( [a-zA-Zа-яёА-ЯЁ]{2,20})?){0,2}$", ErrorMessage = "Некорректная фамилия!")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        [RegularExpression(@"^([a-zа-яё]|[A-ZА-ЯЁ])[a-zа-яё]{1,20}$", ErrorMessage = "Некорректное имя!")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[./ ](0[1-9]|1[012])[./ ](19|20|[12][0-9][0-9][0-9])$", ErrorMessage = "Некорректная дата!")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public string BirthDate { get; set; }

        [StringLength(100,ErrorMessage="Слишком короткий адрес",MinimumLength=10)]
        [Display(Name = "Место проживания")]
        public string Address { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~]+@([-a-z0-9]+.)+[a-z]{2,5}$", ErrorMessage = "Некорректный e-mail!")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage="Введите адрес электронной почты!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~]+@([-a-z0-9]+.)+[a-z]{2,5}$",ErrorMessage="Некорректный e-mail!")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите фамилию!")]
        [RegularExpression(@"^([a-zа-яё]|[A-ZА-ЯЁ])[a-zа-яё]{0,20}((-[a-zA-Zа-яёА-ЯЁ]{1,20})?|( [a-zA-Zа-яёА-ЯЁ]{2,20})?){0,2}$", ErrorMessage = "Некорректная фамилия!")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        [RegularExpression(@"^([a-zа-яё]|[A-ZА-ЯЁ])[a-zа-яё]{1,20}$", ErrorMessage = "Некорректное имя!")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
