using System.ComponentModel.DataAnnotations;

namespace EasyPark.Metadata
{
    internal class ParkingLotsMetadata
    {
        [Display(Name = "編號")]
        public int LotId { get; set; }

        [Display(Name = "區域")]
        public string? District { get; set; }

        [Display(Name = "類型")]
        public string? Type { get; set; }

        [Display(Name = "停車場名稱")]
        public string? LotName { get; set; }

        [Display(Name = "地址")]
        public string? Location { get; set; }

        [Display(Name = "月租車位")]
        public int MonRentalSpace { get; set; }

        [Display(Name = "總車位")]
        public int SmallCarSpace { get; set; }

        [Display(Name = "電動車位")]
        public int EtcSpace { get; set; }

        [Display(Name = "機車車位")]
        public int MotoSpace { get; set; }

        [Display(Name = "母嬰車位")]
        public int MotherSpace { get; set; }

        [Display(Name = "收費標準")]
        public string? RateRules { get; set; }

        [Display(Name = "平日費率")]
        public int WeekdayRate { get; set; }

        [Display(Name = "假日費率")]
        public int HolidayRate { get; set; }

        [Display(Name = "預約押金")]
        public int ResDeposit { get; set; }

        [Display(Name = "月租費率")]
        public int MonRentalRate { get; set; }

        [Display(Name = "營業時間")]
        public string? OpendoorTime { get; set; }

        [Display(Name = "電話")]
        public string Tel { get; set; } = null!;

        [Display(Name = "緯度")]
        public decimal? Latitude { get; set; }

        [Display(Name = "經度")]
        public decimal? Longitude { get; set; }

        [Display(Name = "剩餘可用車位")]
        public int ValidSpace { get; set; }

        [Display(Name = "預約逾期有效時間")]
        public int ResOverdueValidTimeSet { get; set; }

    }
}