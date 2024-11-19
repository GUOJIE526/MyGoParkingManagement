using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using EasyPark.Areas.MyMonthlyRental.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EasyPark.Areas.MyMonthlyRental.Controllers
{
    [Authorize]
    [Area("MyMonthlyRental")]
    public class MonthlyRentalsController : Controller
    {
        private readonly EasyParkContext _context;

        public MonthlyRentalsController(EasyParkContext context)
        {
            _context = context;
        }

        //---------20241107 註解掉的就是過去有的(主要差別在於能不能選車位號碼)

        // GET: MyMonthlyRental/MonthlyRentals
        public async Task<IActionResult> Index()
        {
            var easyParkContext = _context.MonthlyRental.Include(m => m.Car).Include(m => m.Lot);
            return View(await easyParkContext.ToListAsync());

        }

        //Ajax請求:用於產生月租訂單表格
        public async Task<IActionResult> GetRentalData()
        {
            var data = await _context.MonthlyRental
                .Select(m => new
                {
                    Id = m.RenId, // 確保這裡提供 ID
                    lotName = m.Lot.LotName,
                    //slotNumber = m.Slot.SlotNumber,
                    plate = m.Car.LicensePlate,
                    startDate = m.StartDate.HasValue ? m.StartDate.Value.ToString("yyyy-MM-dd") : null,
                    endDate = m.EndDate.HasValue ? m.EndDate.Value.ToString("yyyy-MM-dd") : null,
                    amount = m.Amount,
                    //paymentTime = m.PaymentTime,
                    paymentStatus = m.PaymentStatus == true ? "已付款" : "未付款"
                    //notificationStatus = m.NotificationStatus

                })
                .ToListAsync();

            return Json(data);
        }


        // GET: MyMonthlyRental/MonthlyRentals/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new MonthlyRentalViewModel
            {
                StartDate = DateTime.Now.Date,  // 設置開始日期為當前日期
                EndDate = DateTime.Now.Date.AddMonths(1)  // 預設結束日期為開始日期加一個月
            };
            ViewData["LotName"] = new SelectList(_context.ParkingLots, "LotName", "LotName");
            // 建立空白下拉式選單(車位號碼)
            //ViewData["SlotNumber"] = new SelectList(new List<SelectListItem>());  
            return View(viewModel);

        }


        // POST: MyMonthlyRental/MonthlyRentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyRentalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var lotId = _context.ParkingLots
                    .Where(n => n.LotName == viewModel.LotName)
                    .Select(n => n.LotId)
                    .FirstOrDefault();

                //20241107 Slot Number改成沒有指定(優先分配該停車場第一個空位)
                var SlotId = _context.ParkingSlot
                    .Where(n => n.LotId == lotId && n.IsRented == false)
                    .Select(n => n.SlotId)
                    .FirstOrDefault();

                var monthlyRental = new MonthlyRental
                {
                    LotId = lotId,
                    //SlotId = SlotId,   //20241107 原本有指定車位後來沒有
                    CarId = _context.Car
                    .Where(n => n.LicensePlate == viewModel.LicensePlate)
                    .Select(n => n.CarId)
                    .FirstOrDefault(),

                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    Amount = 3000, //預設值
                    PaymentStatus = true,
                };

                // 使用 slotId 查詢車位
                var updateSlot = _context.ParkingSlot
                    .FirstOrDefault(slot => slot.SlotId == SlotId);
                //更新該車位狀態為被預訂
                updateSlot.IsRented = true;

                //查詢停車場(要變更月租車位剩餘數量)
                var updateLot = _context.ParkingLots
                    .FirstOrDefault(lot => lot.LotId == lotId);

                //月租車位數-1
                updateLot.MonRentalSpace -= 1;

                //更新資料表
                await _context.SaveChangesAsync();

                _context.Add(monthlyRental);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

        }


        // GET: MyMonthlyRental/MonthlyRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var monthlyRental = await _context.MonthlyRental.FindAsync(id);
            if (monthlyRental == null)
            {
                return NotFound();
            }

            string lotName = await _context.ParkingLots.Where(lot => lot.LotId == monthlyRental.LotId).Select(lot => lot.LotName).FirstOrDefaultAsync();
            //string slotNumber = await _context.ParkingSlot.Where(slot => slot.SlotId == monthlyRental.SlotId).Select(slot => slot.SlotNumber).FirstOrDefaultAsync();
            string license = await _context.Car.Where(car => car.CarId == monthlyRental.CarId).Select(car => car.LicensePlate).FirstOrDefaultAsync();
            var viewModel = new MonthlyRentalViewModel
            {
                RenId = monthlyRental.RenId,
                LotName = lotName,
                //SlotNumber = slotNumber,
                LicensePlate = license,
                StartDate = (DateTime)monthlyRental.StartDate,
                EndDate = (DateTime)monthlyRental.EndDate,
                Amount = (int)monthlyRental.Amount,
                //PaymentTime = monthlyRental.PaymentTime,
                PaymentStatus = monthlyRental.PaymentStatus,
                //NotificationStatus = monthlyRental.NotificationStatus,

            };
            //新增的
            ViewData["LotName"] = new SelectList(_context.ParkingLots, "LotName", "LotName");
            // 建立空白下拉式選單(車位號碼)
            //ViewData["SlotNumber"] = new SelectList(new List<SelectListItem>());  

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonthlyRentalViewModel viewModel)
        {
            if (id != viewModel.RenId)
            {
                return NotFound();
            }

            var lotId = _context.ParkingLots
                    .Where(n => n.LotName == viewModel.LotName)
                    .Select(n => n.LotId)
                    .FirstOrDefault();

            var carId = _context.Car
                    .Where(n => n.LicensePlate == viewModel.LicensePlate)
                    .Select(n => n.CarId)
                    .FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    // 從 ViewModel 轉換為 MonthlyRental
                    var monthlyRental = new MonthlyRental
                    {
                        RenId = viewModel.RenId,
                        LotId = lotId,
                        CarId = carId,
                        //SlotId = SlotId,
                        StartDate = viewModel.StartDate,
                        EndDate = viewModel.EndDate,
                        //RenewalStatus = false,
                        //NotificationStatus = viewModel.NotificationStatus,
                        Amount = viewModel.Amount,
                        //PaymentTime = viewModel.PaymentTime,
                        PaymentStatus = viewModel.PaymentStatus
                    };

                    _context.Update(monthlyRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyRentalExists(viewModel.RenId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

        }
        

        private bool MonthlyRentalExists(int id)
        {
            return _context.MonthlyRental.Any(e => e.RenId == id);
        }
    }
}
