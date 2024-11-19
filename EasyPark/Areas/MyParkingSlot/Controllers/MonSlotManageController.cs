using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using EasyPark.Areas.MyParkingSlot.ViewModels;

namespace EasyPark.Areas.MyParkingSlot.Controllers
{
    [Area("MyParkingSlot")]
    public class MonSlotManageController : Controller
    {
        private readonly EasyParkContext _context;

        public MonSlotManageController(EasyParkContext context)
        {
            _context = context;
        }

        // GET: MyParkingSlot/MonSlotManage
        public async Task<IActionResult> Index()
        {
            // 根據 parkingLotId 從資料庫抓取數據
            var defaultLotId = 54;

            var parkingSlots = await _context.ParkingSlot
                .Where(ps => ps.LotId == defaultLotId)  // 使用預設的 LotId = 54
                .Include(ps => ps.Lot) // join到 ParkingLot，要查詢停車場資訊
                .ToListAsync();

            var monApplyList = await _context.MonApplyList
                .Where(ma => ma.LotId == defaultLotId)  // 使用預設的 LotId = 54
                .Include(ma => ma.Lot) // join到 ParkingLot，要查詢停車場資訊
                .Include(ma => ma.Car) // join到 Car，要查詢車輛資訊（車牌）
                .ToListAsync();

            var viewModel = new ParkingSlotManageViewModel
            {
                ParkingLots = await _context.ParkingLots.ToListAsync(), // 載入所有停車場資料
                ParkingSlots = parkingSlots,
                MonApplyList = monApplyList,
                LotName = await _context.ParkingLots
                    .Where(n => n.LotId == defaultLotId)
                    .Select(n => n.LotName)
                    .FirstOrDefaultAsync(),
                Location = await _context.ParkingLots
                    .Where(n => n.LotId == defaultLotId)
                    .Select(n => n.Location)
                    .FirstOrDefaultAsync(),
                TotalLots = await _context.ParkingSlot
                    .Where(n => n.LotId == defaultLotId)
                    .CountAsync(), // 使用預設的 LotId = 1
                Rentedlots = await _context.ParkingSlot
                    .Where(n => n.IsRented == true && n.LotId == defaultLotId) // 使用預設的 LotId = 54
                    .CountAsync(),
                WaitingApplicants = _context.MonApplyList.Where(n => n.LotId == defaultLotId && n.ApplyStatus == "pending").Count(),
            };

            return View(viewModel);

        }

        public async Task<IActionResult> LoadParkingLotStatus(int? id)
        {
            if (id == null) return BadRequest();

            //根據 parkingLotId 從資料庫抓數據
            var parkingSlots = await _context.ParkingSlot
                .Where(ps => ps.LotId == id) 
                .Include(ps => ps.Lot) 
                .ToListAsync();

            var monApplyList = await _context.MonApplyList
                .Where(ma => ma.LotId == id) 
                .Include(ma => ma.Lot) 
                .Include(ma => ma.Car) 
                .ToListAsync();

            var viewModel = new ParkingSlotManageViewModel
            {
                ParkingLots = _context.ParkingLots,
                ParkingSlots = parkingSlots,
                MonApplyList = monApplyList,
                LotName = _context.ParkingLots.Where(n => n.LotId == id).Select(n => n.LotName).FirstOrDefault(),
                Location = _context.ParkingLots.Where(n => n.LotId == id).Select(n => n.Location).FirstOrDefault(),
                TotalLots = _context.ParkingSlot.Where(n => n.LotId == id).Count(), //該停車場月租車位數
                Rentedlots = _context.ParkingSlot.Where(n => n.IsRented == true && n.LotId == id).Count(),   //該停車場出租車位數(要改嗎?)
                WaitingApplicants = _context.MonApplyList.Where(n => n.LotId == id && n.ApplyStatus == "pending").Count(),
            };

            return PartialView("_ParkingLotStatusPartial", viewModel); //這可能到時候要改
        }


        // GET: MyParkingSlot/MonSlotManage/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId");
            ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotId", "LotId");

            var viewModel = new MonthlyApplyViewModel
            {
                ApplyDate = DateTime.Now.Date,  // 設置開始日期為當前日期
            };

            ViewData["LotName"] = new SelectList(_context.ParkingLots, "LotName", "LotName");
            return View(viewModel);

        }

        // POST: MyParkingSlot/MonSlotManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyApplyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lotId = _context.ParkingLots
                    .Where(n => n.LotName == viewModel.LotName)
                    .Select(n => n.LotId)
                    .FirstOrDefault();

                var carId = _context.Car
                   .Where(n => n.LicensePlate == viewModel.LicensePlate)
                   .Select(n => n.CarId)
                   .FirstOrDefault();

                var MonApplyList = new MonApplyList
                {
                    LotId = lotId,
                    CarId = carId,
                    ApplyDate = viewModel.ApplyDate,
                    ApplyStatus = "pending"
                };

                _context.Add(MonApplyList);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

        }



        //// GET: MyParkingSlot/MonSlotManage/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var monApplyList = await _context.MonApplyList
        //        .Include(m => m.Car)
        //        .Include(m => m.Lot)
        //        .FirstOrDefaultAsync(m => m.ApplyId == id);
        //    if (monApplyList == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(monApplyList);
        //}


        // GET: MyParkingSlot/MonSlotManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monApplyList = await _context.MonApplyList.FindAsync(id);
            if (monApplyList == null)
            {
                return NotFound();
            }

            string lotName = await _context.ParkingLots.Where(lot => lot.LotId == monApplyList.LotId).Select(lot => lot.LotName).FirstOrDefaultAsync();

            string license = await _context.Car.Where(car => car.CarId == monApplyList.CarId).Select(car => car.LicensePlate).FirstOrDefaultAsync();
            var viewModel = new MonthlyApplyViewModel
            {
                ApplyId = monApplyList.ApplyId,
                ApplyDate = (DateTime)monApplyList.ApplyDate,
                LotName = lotName,
                LicensePlate = license,
                ApplyStatus = monApplyList.ApplyStatus

            };
            //新增的
            ViewData["LotName"] = new SelectList(_context.ParkingLots, "LotName", "LotName");

            return View(viewModel);
        }


        //★如果要寫抽籤可以參考這個
        // POST: MyParkingSlot/MonSlotManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonthlyApplyViewModel viewModel)
        {
            if (id != viewModel.ApplyId)
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
                    var MonApplyList = new MonApplyList
                    {
                        ApplyId = viewModel.ApplyId,
                        ApplyDate = viewModel.ApplyDate,
                        LotId = lotId,
                        CarId = carId,
                        ApplyStatus = viewModel.ApplyStatus,

                    };

                    if (viewModel.ApplyStatus == "accepted")
                    {
                        int SlotId = _context.ParkingSlot.Where(n => n.LotId == lotId && n.IsRented == false).Select(n => n.SlotId).FirstOrDefault();
                        var MonthlyRental = new MonthlyRental
                        {
                            LotId = lotId,

                            //SlotId = SlotId,

                            CarId = carId,

                            StartDate = DateTime.Now.Date,  // 設置開始日期為當前日期
                            EndDate = DateTime.Now.Date.AddMonths(1),  // 預設結束日期為開始日期加一個月

                            //預設值
                            Amount = 3000
                        };

                        _context.Add(MonthlyRental);
                        //改變分配到的車位狀態(isrented變成true)
                        var changeSlot = _context.ParkingSlot.Find(SlotId);
                        changeSlot.IsRented = true;
                        _context.Update(changeSlot);

                    }

                    _context.Update(MonApplyList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonApplyListExists(viewModel.ApplyId))
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



        // GET: MyParkingSlot/MonSlotManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monApplyList = await _context.MonApplyList
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .FirstOrDefaultAsync(m => m.ApplyId == id);
            if (monApplyList == null)
            {
                return NotFound();
            }

            return View(monApplyList);
        }

        // POST: MyParkingSlot/MonSlotManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monApplyList = await _context.MonApplyList.FindAsync(id);
            if (monApplyList != null)
            {
                _context.MonApplyList.Remove(monApplyList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonApplyListExists(int id)
        {
            return _context.MonApplyList.Any(e => e.ApplyId == id);
        }
    }
}
