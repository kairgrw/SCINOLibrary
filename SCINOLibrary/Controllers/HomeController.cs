using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCINOLibrary.Models;
using SCINOLibrary.Helpers;
using System.Net;

namespace SCINOLibrary.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            if(User.Identity.IsAuthenticated)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                // создаем список книг, принадлежащих текущему пользователю
                ViewBag.UserBooks = db.Books.ToList().Find(x => x.Owner == user);
                // создаем список новых заявок, поступивших пользователю
                var suggestions = _bidHelper.CreateListOfNewBidsToUser(user);
                if (suggestions.Count > 0)
                {
                    Session["Suggestions"] = suggestions.Count;
                }
                // создаем список обработанных заявок для отображения в истории заявок
                var bids = _bidHelper.CreateBidList(user);
                int count = _bidHelper.GetNewBidsCount(bids, user);
                if (count > 0)
                    Session["Bids"] = count;
            }

            SearchBookModel model = new SearchBookModel();
            model.CreateLists();
            ViewBag.Genres = db.Genres.ToList();

            return View(model);
        }

        public ActionResult Search()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.UserBooks = db.Books.ToList().Find(x => x.Owner == user);
            }
            SearchBookModel model = new SearchBookModel();
            model.CreateLists();
            ViewBag.Genres = db.Genres.ToList();

            return View("Index", model);
        }

        /// <summary>
        /// Выполняет поиск книг
        /// </summary>
        /// <param name="model"></param>
        /// <param name="selectedGenres">список жанров, отмеченных пользователем</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(SearchBookModel model, int[] selectedGenres)
        {
            if(ModelState.IsValid)
            {
                double priceLow = 0;
                double priceHigh = 0;
                // проверяем диапазон цен
                if (model.PriceLow != null && model.PriceHigh != null)
                {
                    priceLow = Convert.ToDouble(model.PriceLow);
                    priceHigh = Convert.ToDouble(model.PriceHigh);
                }
                // если одно из полей диапазона заполнено, а другое нет
                if((model.PriceLow!=null && model.PriceHigh==null) ||
                        (model.PriceLow == null && model.PriceHigh != null))
                {
                    ModelState.AddModelError("", "Одно из полей не заполнено (цена)");
                }
                // проверяем диапазон годов издания
                else if (model.PublishYearFrom > model.PublishYearTo)
                {
                    ModelState.AddModelError("", "Левая граница интервала больше правой (год издания)");
                }
                else if (priceLow < 0 || priceHigh<0)
                {
                    ModelState.AddModelError("", "Некорректная цена");
                }
                else if(priceLow>priceHigh)
                {
                    ModelState.AddModelError("", "Левая граница интервала больше правой (цена)");
                }
                // если все данные корректны, выполняем поиск
                else
                {
                    List<Book> books = new List<Book>();

                    books = _searchHelper.Search(model, selectedGenres);
                    return View(books);
                }
            }

            ViewBag.Genres = db.Genres.ToList();
            model.CreateLists();

            return View("Index", model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        /// <summary>
        /// Получает сведения о пользователе (владелец книги)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="bookID"></param>
        /// <returns></returns>
        public ActionResult UserInfo(string userID, int bookID)
        {
            ApplicationUser user = db.Users.Find(userID);
            UserInfoModel model = new UserInfoModel()
            {
                User = user,
                BookID = bookID,
                userBirthDate = user.BirthDate != null ? user.BirthDate.Value.ToString("d") : ""
            };
            
            return View(model);            
        }

        protected ApplicationDbContext db = new ApplicationDbContext();

        private BidHelper _bidHelper = new BidHelper();
        private SearchHelper _searchHelper = new SearchHelper();
    }
}