using System.ComponentModel.DataAnnotations;

namespace EasyPark.Models
{
    internal class CarMetadata
    {
        public int CarId { get; set; }

        [Display(Name = "用戶")]
        public int UserId { get; set; }

        [Display(Name = "車牌號碼")]
        public string LicensePlate { get; set; } = null!;

        [Display(Name = "註冊日期")]
        public DateTime? RegisterDate { get; set; }

        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; }

        [Display(Name = "用戶")]
        public virtual Customer? User { get; set; }
    }
}