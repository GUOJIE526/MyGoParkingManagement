using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace EasyPark.Areas.MyDealRecord.Controllers
{
    [Authorize]
    [Area("MyDealRecord")]
    public class DealRecordController : Controller
    {
        private readonly EasyParkContext _context;

        public DealRecordController(EasyParkContext context)
        {
            _context = context;
        }

        // GET: MyDealRecord/DealRecord
        public async Task<IActionResult> Index()
        {
            var easyParkContext = _context.DealRecord.Include(d => d.Car);
            return View(await easyParkContext.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var easyParkContext = _context.DealRecord.Include(d => d.Car).Select(d => new
            {
                dealId = d.DealId,
                amount = d.Amount.ToString("C0", CultureInfo.CurrentCulture),
                paymentTime = d.PaymentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                parkType = d.ParkType,
                carId = d.Car.LicensePlate,
            });
            return Json(easyParkContext);
        }

        // GET: MyDealRecord/DealRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealRecord = await _context.DealRecord
                .Include(d => d.Car)
                .FirstOrDefaultAsync(m => m.DealId == id);
            if (dealRecord == null)
            {
                return NotFound();
            }

            return View(dealRecord);
        }

        public IActionResult CreatePartial()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            return PartialView("_CreatePartial");
        }

        // GET: MyDealRecord/DealRecord/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId");
            return View();
        }

        // POST: MyDealRecord/DealRecord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DealId,CarId,Amount,PaymentTime,ParkType")] DealRecord dealRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dealRecord);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                // 如果 ModelState 驗證失敗，返回錯誤訊息
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }
            //ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", dealRecord.CarId);
            //return View(dealRecord);
        }

        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealRecord = await _context.DealRecord.FindAsync(id);
            if (dealRecord == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate", dealRecord.CarId);
            return PartialView("_EditPartial", dealRecord);
        }

        //// GET: MyDealRecord/DealRecord/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var dealRecord = await _context.DealRecord.FindAsync(id);
        //    if (dealRecord == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", dealRecord.CarId);
        //    return View(dealRecord);
        //}

        // POST: MyDealRecord/DealRecord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DealId,CarId,Amount,PaymentTime,ParkType")] DealRecord dealRecord)
        {
            if (id != dealRecord.DealId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dealRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealRecordExists(dealRecord.DealId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            return Json(new { success = false });
            //ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", dealRecord.CarId);
            //return View(dealRecord);
        }

        // GET: MyDealRecord/DealRecord/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealRecord = await _context.DealRecord
                .Include(d => d.Car)
                .FirstOrDefaultAsync(m => m.DealId == id);
            if (dealRecord == null)
            {
                return NotFound();
            }

            return View(dealRecord);
        }

        // POST: MyDealRecord/DealRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dealRecord = await _context.DealRecord.FindAsync(id);
            if (dealRecord != null)
            {
                _context.DealRecord.Remove(dealRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DealRecordExists(int id)
        {
            return _context.DealRecord.Any(e => e.DealId == id);
        }
    }
}
