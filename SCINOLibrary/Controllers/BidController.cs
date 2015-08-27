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
        public ActionResult Index()
        {
            ApplicationUser user = db.Users.Find(UserId);
            if (Session["UserName"] == null)
                Session["UserName"] = user.Surname + " " + user.Name;
            var bids = _bidHelper.CreateBidList(user);

            return View(bids);
        }

        // GET: /Bid/Details/5
        public ActionResult Details(int? id)
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
            return View(bid);
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
