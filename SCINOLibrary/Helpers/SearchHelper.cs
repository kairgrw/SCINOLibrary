using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCINOLibrary.Models;


namespace SCINOLibrary.Helpers
{
    /// <summary>
    /// Вспомогательный класс для поиска книг
    /// </summary>
    public class SearchHelper
    {
        public SearchHelper()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// Возвращает список найденных книг по запросу пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="selectedGenres"></param>
        /// <returns></returns>
        public List<Book> Search(SearchBookModel model, int[] selectedGenres)
        {
            List<Book> books = new List<Book>();
            // формируем строковый запрос к БД
            string query = "SELECT * FROM dbo.Books WHERE ";

            // поиск по автору
            query += model.Author != null ? "Author LIKE N'%" + model.Author + "%' and " : "";
            // поиск по названию
            query += model.Title != null ? "Title LIKE N'%" + model.Title + "%' and " : "";
            // поиск по цене
            query += (model.PriceLow != null && model.PriceHigh != null) ? "Price between " + model.PriceLow.Replace(',', '.') + " and " + model.PriceHigh.Replace(',', '.') + " and " : "";
            // поиск в интервале по году издания
            query += model.PublishYearFrom != DateTime.Now.Year ? "PublishYear between " + model.PublishYearFrom + " and " + model.PublishYearTo + " and " : "";
            // поиск по нас. пункту владельца
            query += model.Placement != null ? "Placement LIKE N'%" + model.Placement + "%' and " : "";
            int i = model.OnExchange ? 1 : 0;
            // поиск по обмену
            query += "OnExchange = " + i + " and ";
            // поиск по жанрам
            if (selectedGenres != null)
            {
                query+="Books.ID in ( SELECT Book_ID FROM dbo.GenreBooks WHERE Genre_ID IN (SELECT Genres.ID FROM dbo.Genres WHERE Genres.ID in (";
                foreach (var genre in db.Genres.Where(x => selectedGenres.Contains(x.ID)))
                {
                    query += genre.ID + ",";
                }
                query = query.Remove(query.LastIndexOf(','));
                query += "))) and ";
            }
            if (query.LastIndexOf(" and ") != -1)
            {
                query = query.Remove(query.LastIndexOf(" and "));
                books = db.Books.SqlQuery(query).ToList();
            }
            // сортируем список книг вначале по дате создания профиля по убыванию, затем - по имени владельца
            var sortedBooks = books.OrderByDescending(x => x.Created).ThenBy(x => x.Owner.Name);
            return sortedBooks.ToList();
        }

        private ApplicationDbContext db;
    }
}