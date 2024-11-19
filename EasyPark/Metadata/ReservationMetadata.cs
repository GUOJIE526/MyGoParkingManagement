using EasyPark.Models;
using System.ComponentModel.DataAnnotations;

namespace EasyPark.Metadata
{
    internal class ReservationMetadata
    {
        public int ResId { get; set; }

        [Display(Name = "車牌號碼")]
        public int CarId { get; set; }

        [Display(Name = "停車場")]
        public int LotId { get; set; }

        [Display(Name = "填表時間")]
        public DateTime? ResTime { get; set; }

        [Display(Name = "逾期時間")]
        public DateTime? ValidUntil { get; set; }

        [Display(Name = "預約時間")]
        public DateTime? StartTime { get; set; }

        [Display(Name = "訂金付款")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "取消")]
        public bool IsCanceled { get; set; }

        [Display(Name = "超時")]
        public bool IsOverdue { get; set; }

        [Display(Name = "訂金退款")]
        public bool IsRefoundDeposit { get; set; }

        [Display(Name = "已通知")]
        public bool NotificationStatus { get; set; }

        [Display(Name = "訂單完成")]
        public bool IsFinish { get; set; }

        [Display(Name = "車牌號碼")]
        public virtual Car? Car { get; set; }

        [Display(Name = "停車場")]
        public virtual ParkingLots? Lot { get; set; }

    }
}