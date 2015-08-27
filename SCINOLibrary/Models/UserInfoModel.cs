using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCINOLibrary.Models
{
    public class UserInfoModel
    {
        public ApplicationUser User { get; set; }
        public string userBirthDate { get; set; }
        public int BookID { get; set; }
    }
}