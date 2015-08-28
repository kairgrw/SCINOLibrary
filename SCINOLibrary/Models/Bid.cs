using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SCINOLibrary.Models
{
    public enum EStatus
    {
        Created,
        Processing,
        Approved,
        Rejected
    }
    public class Bid
    {
        public int ID { get; set; }

        // статус заявки (создана, в рассмотрении, одобрена/отклонена)
        [Display(Name="Статус")]
        public EStatus Status { get; set; }

        // просмотрена ли заявка адресатом
        public bool IsChecked { get; set; }

        // время оформления заявки
        [Display(Name = "Оформлена")]
        public DateTime CreateAt { get; set; }

        // время рассмотрения заявки адресатом
        [Display(Name = "Просмотрена")]
        public DateTime? CheckedAt { get; set; }

        // пользователь, оформивший заявку
        public virtual ApplicationUser UserCreate { get; set; }

        // книга, которую пользователь хочет купить
        public virtual Book BookToBuy { get; set; }
        //книга, которую пользователь хочет получить взамен
        public virtual Book WantedBook { get; set; }
        // книга, предлагаемая пользователем для обмена
        public virtual Book SuggestedBook { get; set; }
    }
}