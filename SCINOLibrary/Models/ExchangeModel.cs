using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SCINOLibrary.Models
{
    public class ExchangeModel
    {
        public int WantedBookID { get; set; }
        public string Title { get; set; }
        public List<Book> Books { get; set; }
        public SelectList BooksToSuggest { get; set; }

        public ExchangeModel()
        {
            Books = new List<Book>();
        }
        public void CreateBooksList(ApplicationDbContext db, string userID)
        {
            Books = db.Books.Where(x => (x.Owner.Id == userID && x.OnExchange)).ToList();
            // добавить фильтрацию по предложенным на обмен книгам в заявках
            List<string> titles = new List<string>();
            foreach(var book in Books)
            {
                titles.Add(book.Title);
            }
            BooksToSuggest = new SelectList(titles);
        }
    }
}