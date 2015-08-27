using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCINOLibrary.Models
{
    public class MyBooksModel
    {
        public List<Book> Books { get; set; }
        public List<Bid> Bids { get; set; }
        public MyBooksModel()
        {
            Books = new List<Book>();
            Bids = new List<Bid>();
        }
    }
}