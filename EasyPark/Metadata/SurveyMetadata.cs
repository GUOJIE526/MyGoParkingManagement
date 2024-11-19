using System.ComponentModel.DataAnnotations;
using EasyPark.Models;

namespace EasyPark.Metadata
{
    internal class SurveyMetadata
    {
        public int Id { get; set; }
        [Display(Name = "用戶")]
        public int UserId { get; set; }
        [Display(Name = "問題")]
        public string Question { get; set; } = null!;
        [Display(Name = "回覆訊息")]
        public string? ReplyMessage { get; set; }
        [Display(Name = "是否回覆")]
        public bool IsReplied { get; set; }
        [Display(Name = "發問時間")]
        public DateTime SubmittedAt { get; set; }
        [Display(Name = "回覆時間")]
        public DateTime? RepliedAt { get; set; }
        [Display(Name = "回覆狀態")]
        public string Status { get; set; } = null!;
        [Display(Name = "信箱")]
        public virtual Customer User { get; set; } = null!;
    }
}