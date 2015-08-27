using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SCINOLibrary.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"[A-ZА-ЯЁ][а-яa-zA-ZёА-ЯЁ]{3,30}((([\s|-][а-яёa-zA-ZА-ЯЁ]{0,20})?)*)",ErrorMessage="Некорректное название книги")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name="Автор")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Год издания")]
        public int PublishYear { get; set; }

        [Display(Name = "Обмен")]
        public bool OnExchange { get; set; }

        [Required]
        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Слишком короткий адрес", MinimumLength = 10)]
        [Display(Name = "Местоположение")]
        public string Placement { get; set; }

        [Display(Name = "Создан")]
        public DateTime Created { get; set; }

        [Display(Name = "Изменен")]
        public DateTime? Changed { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual Picture Image { get; set; }
        public Book()
        {
            Genres = new List<Genre>();
            Bids = new List<Bid>();
        }
    }
}