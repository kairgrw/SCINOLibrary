using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SCINOLibrary.Models
{
    public class SearchBookModel
    {
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [RegularExpression(@"[а-яa-zёA-ZА-ЯЁ][а-яa-zA-ZёА-ЯЁ]{3,30}(((\s[а-яёa-zA-ZА-ЯЁ]{0,20})?)*)", ErrorMessage = "Некорректное название книги")]
        [Display(Name = "Название")]
        public string Title { get; set; }

        public List<Genre> Genres;

        public int PublishYearFrom { get; set; }
        public int PublishYearTo { get; set; }
        public SelectList ListPublishYearFrom { get; set; }

        public SelectList ListPublishYearTo { get; set; }

        [Display(Name = "Обмен")]
        public bool OnExchange { get; set; }

        [RegularExpression(@"^[0-9]{0,10}((,[0-9]{0,5})?)$", ErrorMessage = "Некорректная цена")]
        public string PriceLow { get; set; }
        [RegularExpression(@"^[0-9]{0,10}((,[0-9]{0,5})?)$", ErrorMessage = "Некорректная цена")]
        public string PriceHigh { get; set; }

        [StringLength(100, ErrorMessage = "Слишком короткий адрес", MinimumLength = 10)]
        [Display(Name = "Населенный пункт владельца")]
        public string Placement { get; set; }

        public void CreateLists()
        {
            var years = new List<int>();
            for (int i = DateTime.Now.Year; i >= 1600; i--)
                years.Add(i);
            ListPublishYearFrom = new SelectList(years);
            ListPublishYearTo = new SelectList(years);
        }
    }
}