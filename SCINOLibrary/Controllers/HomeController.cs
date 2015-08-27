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
                if (Session["UserName"] == null)
                    Session["UserName"] = user.Surname + " " + user.Name;
                ViewBag.UserBooks = db.Books.ToList().Find(x => x.Owner == user);

                if (Session["Suggestions"] == null)
                {
                    var suggestions = _bidHelper.CreateListOfNewBidsToUser(user);
                    if (suggestions.Count > 0)
                    {
                        Session["Suggestions"] = suggestions.Count;
                    }  
                }
                if (Session["Bids"] == null)
                {
                    var bids = _bidHelper.CreateBidList(user);
                    int count = _bidHelper.GetNewBidsCount(bids, user);
                    if (count > 0)
                        Session["Bids"] = count;
                }
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
                if (Session["UserName"] == null)
                    Session["UserName"] = user.Surname + " " + user.Name;
                ViewBag.UserBooks = db.Books.ToList().Find(x => x.Owner == user);
            }
            SearchBookModel model = new SearchBookModel();
            model.CreateLists();
            ViewBag.Genres = db.Genres.ToList();

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Search(SearchBookModel model, int[] selectedGenres)
        {
            if(ModelState.IsValid)
            {
                double priceLow = 0;
                double priceHigh = 0;

                if (model.PriceLow != null && model.PriceHigh != null)
                {
                    priceLow = Convert.ToDouble(model.PriceLow);
                    priceHigh = Convert.ToDouble(model.PriceHigh);
                }

                if((model.PriceLow!=null && model.PriceHigh==null) ||
                        (model.PriceLow == null && model.PriceHigh != null))
                {
                    ModelState.AddModelError("", "Одно из полей не заполнено (цена)");
                }
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