using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using System.Diagnostics;

namespace EasyPark.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly EasyParkContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, EasyParkContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var monthlySum = _context.MonthlyRental
                .Where(s => s.PaymentStatus == true)
                .Select(s => s.Amount)
                .Sum();
            var EntryExitSum = _context.EntryExitManagement
               .Where(s => s.PaymentStatus == true)
               .Select(s => s.Amount)
               .Sum();
            var MonthlyApply = _context.MonApplyList
                .Where(s => s.ApplyStatus == "pending")
                .Count();
            var ParkingLot = await _context.ParkingLots
                .Select(p => new ParkingLots
                {
                    LotName = p.LotName,
                    SmallCarSpace = p.SmallCarSpace,
                }).OrderByDescending(p => p.SmallCarSpace).Take(5).ToListAsync();

            var overtimecount = _context.Reservation.Where(s => s.IsOverdue == false).Count();
            var AllReservationCount = _context.Reservation.Count();
            decimal CompeleteRate = 0;
            if (AllReservationCount > 0)
            {
                CompeleteRate += (decimal)overtimecount / AllReservationCount;
                ViewData["CompeleteRate"] = Math.Round(decimal.Round(CompeleteRate, 2) * 100);
            }
            else
            {
                ViewData["CompeleteRate"] = '0';
            }
            ViewData["monthlySum"] = monthlySum;
            ViewData["EntryExitSum"] = EntryExitSum;
            ViewData["MonthlyApply"] = MonthlyApply;

            return View(ParkingLot);
        }
       

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> GetSlotData()
        {
            // ���Ӱ��������աA�p��C�Ӱ��������X���v
            var data = await _context.ParkingSlot
                .GroupBy(m => m.Lot.LotName)
                .Select(g => new
                {
                    LotName = g.Key,
                    RentedCount = g.Count(p => p.IsRented == true),
                    TotalCount = g.Count(),
                    Percentage = g.Count(p => p.IsRented == true) * 100.0 / g.Count()
                })
                .OrderByDescending(x=>x.Percentage)
                .Take(5)
                .ToListAsync();
            return Json(data); // ��^JSON�ƾ�
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
