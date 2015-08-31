using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCINOLibrary.Models;

namespace SCINOLibrary.Helpers
{
    public class BidHelper
    {
        public BidHelper()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// Возвращает список всех заявок, которые пользователь отправил или получил
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Bid> CreateBidList(ApplicationUser user)
        {
            var bids = db.Bids.Where(x => x.BookToBuy.Owner.Id == user.Id || x.WantedBook.Owner.Id == user.Id || x.SuggestedBook.Owner.Id == user.Id || x.UserCreate.Id == user.Id).ToList();
            
            if (bids == null)
                bids = new List<Bid>();

            return bids.OrderByDescending(x => x.CheckedAt).ThenByDescending(x => x.CreateAt).ToList();
        }

        /// <summary>
        /// Возвращает список заявок, пришедших пользователю
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Bid> CreateListOfNewBidsToUser(ApplicationUser user)
        {
            var bids = db.Bids.Where(x => (x.BookToBuy.Owner.Id == user.Id || x.WantedBook.Owner.Id == user.Id) && (x.Status == EStatus.Created) && (!x.IsChecked)).ToList();
            
            if (bids == null)
                bids = new List<Bid>();

            return bids.OrderByDescending(x => x.CreateAt).ToList();
        }

        /// <summary>
        /// Возвращает число заявок, поступивших пользователю или изменивших статус
        /// </summary>
        /// <param name="bids"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetNewBidsCount(List<Bid> bids, ApplicationUser user)
        {
            int count = 0;
            if (bids.Count > 0)
            {
                foreach (var bid in bids)
                {
                    if ((bid.UserCreate.Id != user.Id || bid.Status >= EStatus.Approved) && (!bid.IsChecked))
                        count++;
                }
            }
            return count;
        }

        private ApplicationDbContext db;
    }
}