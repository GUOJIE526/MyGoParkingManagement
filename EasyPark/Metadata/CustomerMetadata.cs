using System.ComponentModel.DataAnnotations;

namespace EasyPark.Models
{
    internal class CustomerMetadata
    {
        public int UserId { get; set; }

        [Display(Name = "用戶")]
        [Required(ErrorMessage = "用戶必填寫")]
        public string Username { get; set; }

        [Display(Name = "密碼")]
        public string? Password { get; set; }

        [Display(Name = "信箱")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string? Email { get; set; }

        [Display(Name = "電話")]
        [Phone(ErrorMessage = "請輸入有效的電話號碼")]
        public string? Phone { get; set; }

        [Display(Name = "逾時次數")]
        public int BlackCount { get; set; } = 0;

        [Display(Name = "黑名單狀態")]
        public bool IsBlack { get; set; } = false;


    }
}