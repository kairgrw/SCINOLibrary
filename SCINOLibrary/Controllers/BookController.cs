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
using System.IO;

namespace SCINOLibrary.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        [AllowAnonymous]
        // GET: /Book/
        public ActionResult Index()
        {
            ApplicationUser user = db.Users.Find(UserId);
            MyBooksModel model = new MyBooksModel()
            {
                Books = db.Books.ToList().Where(x => x.Owner == user).ToList(),
                Bids = _bidHelper.CreateListOfNewBidsToUser(user)
            };
            return View(model);
        }

        [AllowAnonymous]
        // GET: /Book/Details/5
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

        // GET: /Book/Create
        public ActionResult Create()
        {
            ApplicationUser user = db.Users.Find(UserId);
            Book model = new Book();
            model.Placement = user.Address;
            ViewBag.Genres = db.Genres.ToList();
            return View(model);
        }

        // POST: /Book/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Author,OnExchange,Price,Placement,PublishYear,Created,Changed")] Book book, int[] selectedGenres)
        {
            if (ModelState.IsValid)
            {
                if (book.PublishYear <= 0 || book.PublishYear > DateTime.Now.Year)
                {
                    ModelState.AddModelError("", "Некорректный год издания");
                }
                else
                {
                    if (selectedGenres != null)
                        foreach (var genre in db.Genres.Where(x => selectedGenres.Contains(x.ID)))
                            book.Genres.Add(genre);
                    ApplicationUser user = db.Users.Find(UserId);
                    book.Owner = user;
                    book.Created = DateTime.Now;

                    db.Books.Add(book);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            ViewBag.Genres = db.Genres.ToList();
            return View(book);
        }

        // GET: /Book/Edit/5
        public ActionResult Edit(int? id)
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

            ViewBag.Genres = db.Genres.ToList();

            return View(book);
        }

        // POST: /Book/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Author,OnExchange,Price,Placement,PublishYear,Created,Changed")] Book book, int[] selectedGenres)
        {
            if (ModelState.IsValid)
            {
                if (book.PublishYear <= 0 || book.PublishYear > DateTime.Now.Year)
                {
                    ModelState.AddModelError("", "Некорректный год издания");
                }
                else
                {
                    Book newBook = db.Books.Find(book.ID);
                    newBook.Title = book.Title;
                    newBook.Author = book.Author;
                    newBook.PublishYear = book.PublishYear;
                    newBook.Price = book.Price;
                    newBook.Placement = book.Placement;
                    newBook.OnExchange = book.OnExchange;
                    newBook.Created = book.Created;
                    newBook.Image = book.Image;
                    newBook.Owner = book.Owner;
                    newBook.Changed = DateTime.Now;

                    newBook.Genres.Clear();
                    if (selectedGenres != null)
                        foreach (var genre in db.Genres.Where(x => selectedGenres.Contains(x.ID)))
                        {
                            newBook.Genres.Add(genre);
                        }

                    db.Entry(newBook).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(book);
        }

        // GET: /Book/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: /Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddImage(int? id, HttpPostedFileBase uploadImage)
        {
            Book book = db.Books.Find(id);
            
            if (uploadImage != null && book != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                Picture pic = new Picture()
                {
                    Image = imageData
                };

                book.Image = pic;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.Genres = db.Genres.ToList();
            return View("Edit", book);   
        }

        [HttpPost]
        public ActionResult DeleteImage(int? id)
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

            Picture image = db.Images.Find(book.Image.Id);
            db.Images.Remove(image);
            db.SaveChanges();

            ViewBag.Genres = db.Genres.ToList();
            return View("Edit", book);   
        }

        /// <summary>
        /// Оформление заявки на покупку книги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Buy(int? id)
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

            // оформление заявки на покупку книги

            ApplicationUser user = db.Users.Find(UserId);

            var bid = db.Bids.ToList().Find(x => (x.UserCreate == user && x.BookToBuy == book));
            if (bid!=null)
            {
                return View("NewBid", bid);
            }

            bid = new Bid
            {
                Status = EStatus.Created,
                IsChecked = false,
                CreateAt = DateTime.Now,
                UserCreate = user,
                BookToBuy = book
            };

            db.Bids.Add(bid);
            book.Bids.Add(bid);
            db.Entry(book).State = EntityState.Modified;

            db.SaveChanges();

            bid = db.Bids.ToList().Find(x => x.BookToBuy == book);

            return View("NewBid", bid);
        }

        /// <summary>
        /// Отмена заявки на покупку книги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UndoBuy(int? id)
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

            Bid bid = db.Bids.ToList().Find(x => x.BookToBuy == book);
            db.Bids.Remove(bid);
            db.SaveChanges();

            return RedirectToAction("Search", "Home");
        }

        [HttpGet]
        public ActionResult SuggestExchange(int? id)
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
            var model = new ExchangeModel()
            {
                WantedBookID = book.ID
            };
            model.CreateBooksList(db, UserId);

            return View(model);
        }

        /// <summary>
        /// Оформление заявки на обмен книгами
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SuggestExchange(ExchangeModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(UserId);
                model.CreateBooksList(db, UserId);
                var bid = db.Bids.ToList().Find(x => (x.UserCreate == user &&
                    (x.WantedBook != null && x.WantedBook.ID == model.WantedBookID) &&
                    x.SuggestedBook == model.Books.Find(y => y.Title == model.Title)));
                if (bid != null)
                {
                    return View("NewBid", bid);
                }

                bid = new Bid
                {
                    Status = EStatus.Created,
                    CreateAt = DateTime.Now,
                    IsChecked = false,
                    UserCreate = user,
                    WantedBook = db.Books.Find(model.WantedBookID),
                    SuggestedBook = model.Books.Find(y => y.Title == model.Title)
                };

                Book wantedBook = db.Books.Find(bid.WantedBook.ID);
                Book suggestedBook = db.Books.Find(bid.SuggestedBook.ID);

                db.Bids.Add(bid);
                wantedBook.Bids.Add(bid);
                db.Entry(wantedBook).State = EntityState.Modified;

                db.SaveChanges();

                bid = db.Bids.ToList().Find(x => (x.WantedBook == wantedBook && x.SuggestedBook == suggestedBook));

                return View("NewBid", bid);
            }

            model.CreateBooksList(db, UserId);
            return View(model);
        }

        /// <summary>
        /// Отмена заявки на обмен книгами
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UndoExchange(int? id)
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

            Bid bid = db.Bids.ToList().Find(x => x.WantedBook == book);
            db.Bids.Remove(bid);
            db.SaveChanges();

            return RedirectToAction("Search", "Home");
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
