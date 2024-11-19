using EasyPark.Models;
using System.ComponentModel.DataAnnotations;

namespace EasyPark.Metadata
{
    internal class DealRecordMetadata
    {
        public int DealId { get; set; }

        [Display(Name = "車牌號碼")]
        public int CarId { get; set; }

        [Display(Name = "交易金額")]
        public int Amount { get; set; }

        [Display(Name = "交易時間")]
        public DateTime PaymentTime { get; set; }

        [Display(Name = "交易類型")]
        public string ParkType { get; set; } = null!;

        [Display(Name = "車牌號碼")]
        public virtual Car? Car { get; set; }
    }
}