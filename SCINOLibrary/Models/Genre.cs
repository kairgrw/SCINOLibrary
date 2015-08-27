using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SCINOLibrary.Models
{
    public class Genre
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^[а-яёА-ЯЁ][а-яёА-ЯЁ]{2,20}((\s[а-яёА-ЯЁ]{2,20})?)*$", ErrorMessage="Некорректное имя жанра")]
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public Genre()
        {
            Books = new List<Book>();
        }
    }
}