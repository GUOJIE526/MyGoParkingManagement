using EasyPark.Models;
using System.ComponentModel.DataAnnotations;

namespace EasyPark.Areas.MyCoupon.Metadatas
{
    public class CouponMetadatas
    {
        public int CouponId { get; set; }

        [Display(Name = "優惠碼序號")]
        public string CouponCode { get; set; } = null!;

        [Display(Name = "優惠金額")]
        public int? DiscountAmount { get; set; }

        [Display(Name = "生效時間")]
        public DateTime ValidFrom { get; set; }

        [Display(Name = "過期時間")]
        public DateTime ValidUntil { get; set; }

        [Display(Name = "是否使用")]
        public bool IsUsed { get; set; }

        [Display(Name = "用戶ID")]
        public int? UserId { get; set; }

        public virtual ICollection<Transactions> Transactions { get; set; } = new List<Transactions>();

        [Display(Name = "使用者")]
        public virtual Customer? User { get; set; }

    }
}
