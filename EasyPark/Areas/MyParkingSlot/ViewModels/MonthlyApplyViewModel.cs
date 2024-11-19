using EasyPark.PartailClass;
using System.ComponentModel.DataAnnotations;

namespace EasyPark.Areas.MyParkingSlot.ViewModels
{
    public class MonthlyApplyViewModel
    {
        public int ApplyId { get; set; }

        //申請表單上欄位

        [Required(ErrorMessage = "請選擇申請日期")]
        [Display(Name = "申請日期")]
        public DateTime ApplyDate { get; set; }

        [Required(ErrorMessage = "請選擇停車場")]
        [Display(Name = "停車場名稱")]
        public string LotName { get; set; }

        [Required(ErrorMessage = "請輸入車牌號碼")]
        [CarExists]
        [Display(Name = "車牌號碼")]
        public string LicensePlate { get; set; }

        //其他
        public string? ApplyStatus { get; set; }

    }
}
