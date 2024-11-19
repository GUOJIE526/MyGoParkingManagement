using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasyPark.Areas.MyCar.Controllers
{
    [Authorize]
    [Area("MyCar")]
    public class CarController : Controller
    {
        private readonly EasyParkContext _context;

        public CarController(EasyParkContext context)
        {
            _context = context;
        }

        // GET: MyCar/Car
        public async Task<IActionResult> Index()
        {
            var easyParkContext = _context.Car.Include(c => c.User);
            return View(await easyParkContext.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var easyParkContext = _context.Car.Include(c => c.User).Select(c => new
            {
                carId = c.CarId,
                userId = c.User.Username,
                licensePlate = c.LicensePlate,
                registerDate = c.RegisterDate.HasValue? c.RegisterDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "無資料",
                isActive = c.IsActive? "啟用" : "停用",
            });
            return Json(easyParkContext);
        }

        // GET: MyCar/Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: MyCar/Car/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "UserId");
            return View();
        }

        public IActionResult CreatePartial()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return PartialView("_CreatePartial");
        }

        // POST: MyCar/Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,UserId,LicensePlate,RegisterDate,IsActive")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            // 如果 ModelState 驗證失敗，返回錯誤訊息
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
            //ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "UserId", car.UserId);
            //return View(car);
        }

        // GET: MyCar/Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "UserId", car.UserId);
            return View(car);
        }

        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", car.UserId);
            return PartialView("_EditPartial", car);
        }

        // POST: MyCar/Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,UserId,LicensePlate,RegisterDate,IsActive")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
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
            // 如果 ModelState 驗證失敗，返回錯誤訊息
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
            //ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "UserId", car.UserId);
            //return View(car);
        }

        // GET: MyCar/Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: MyCar/Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                _context.Car.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.CarId == id);
        }
    }
}
