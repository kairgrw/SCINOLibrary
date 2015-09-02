using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCINOLibrary.Models;
using SCINOLibrary.Helpers;

namespace SCINOLibrary.Controllers
{
    [Authorize(Roles="User")]
    public class BidController : Controller
    {
        // GET: /Bid/
        public ActionResult Index(int page = 1)
        {
            ApplicationUser user = db.Users.Find(UserId);
            // создаем список новых заявок, поступивших пользователю
            var suggestions = _bidHelper.CreateListOfNewBidsToUser(user);
            if (suggestions.Count > 0)
            {
                Session["Suggestions"] = suggestions.Count;
            }
            // создаем список обработанных заявок для отображения в истории заявок
            var newBids = _bidHelper.CreateBidList(user);
            int count = _bidHelper.GetNewBidsCount(newBids, user);
            if (count > 0)
                Session["Bids"] = count;

            var bids = _bidHelper.CreateBidList(user);

            // формируем модель, реализующую отображение списка по принципу пагинации
            int pageSize = 10; // количество объектов на страницу
            IEnumerable<Bid> bidsPerPages = bids.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = bids.Count };
            IndexViewModel model = new IndexViewModel { PageInfo = pageInfo, Bids = bidsPerPages };

            return View(model);
        }

        // GET: /Bid/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bid bid = db.Bids.Find(id);
            
            if(bid.Status > EStatus.Created && bid.UserCreate.Id == UserId && (!bid.IsChecked))
            {
                bid.IsChecked = true;
                db.Entry(bid).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (bid == null)
            {
                return HttpNotFound();
            }
            return View(bid);
        }

        /// <summary>
        /// Отмечает заявку как просмотренную
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Check(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bid bid = db.Bids.Find(id);
            if (bid == null)
            {
                return HttpNotFound();
            }

            bid.IsChecked = true;
            bid.Status = EStatus.Processing;
            bid.CheckedAt = DateTime.Now;
            db.Entry(bid).State = EntityState.Modified;
            db.SaveChanges();

            ApplicationUser user = db.Users.Find(UserId);

            if (Session["Bids"] != null)
            {
                var bids = _bidHelper.CreateBidList(user);
                int count = _bidHelper.GetNewBidsCount(bids, user);
                if (count > 0)
                    Session["Bids"] = count;
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Подтверждает заявку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bid bid = db.Bids.Find(id);
            if (bid == null)
            {
                return HttpNotFound();
            }

            // если в заявке покупка книги
            if(bid.BookToBuy!=null)
            {
                Book book = db.Books.Find(bid.BookToBuy.ID);
                if (book == null)
                {
                    return HttpNotFound();
                }
                ApplicationUser user = db.Users.Find(bid.UserCreate.Id);

                book.Owner = user;
                book.Bids.Remove(bid);
                db.Entry(book).State = EntityState.Modified;
            }
            // если в заявке обмен книгами
            else
            {
                Book suggestedBook = db.Books.Find(bid.SuggestedBook.ID);
                Book wantedBook = db.Books.Find(bid.WantedBook.ID);
                if (suggestedBook == null || wantedBook == null)
                {
                    return HttpNotFound();
                }
                ApplicationUser sender = db.Users.Find(suggestedBook.Owner.Id);
                ApplicationUser receiver = db.Users.Find(wantedBook.Owner.Id);
                suggestedBook.Owner = receiver;
                wantedBook.Owner = sender;
                wantedBook.Bids.Remove(bid);
                db.Entry(suggestedBook).State = EntityState.Modified;
                db.Entry(wantedBook).State = EntityState.Modified;
            }

            bid.Status = EStatus.Approved;
            bid.IsChecked = false;
            bid.CheckedAt = DateTime.Now;

            db.Entry(bid).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Отклоняет заявку
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bid bid = db.Bids.Find(id);
            if (bid == null)
            {
                return HttpNotFound();
            }

            // если в заявке покупка книги
            if (bid.BookToBuy != null)
            {
                Book book = db.Books.Find(bid.BookToBuy.ID);
                if (book == null)
                {
                    return HttpNotFound();
                }
                book.Bids.Remove(bid);
                db.Entry(book).State = EntityState.Modified;
            }
            // если в заявке обмен книгами
            else
            {
                Book wantedBook = db.Books.Find(bid.WantedBook.ID);
                if (wantedBook == null)
                {
                    return HttpNotFound();
                }
                wantedBook.Bids.Remove(bid);
                db.Entry(wantedBook).State = EntityState.Modified;
            }

            bid.Status = EStatus.Rejected;
            bid.IsChecked = false;
            bid.CheckedAt = DateTime.Now;

            db.Entry(bid).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected ApplicationDbContext db = new ApplicationDbContext();
        private BidHelper _bidHelper = new BidHelper();
        public string UserId { get { return User.Identity.GetUserId(); } }
    }
}
