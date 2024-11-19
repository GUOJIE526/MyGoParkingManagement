using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasyPark.Areas.MyReservation.Controllers
{
    [Authorize]
    [Area("MyReservation")]
    public class ReservationController : Controller
    {
        private readonly EasyParkContext _context;

        public ReservationController(EasyParkContext context)
        {
            _context = context;
        }

        // GET: MyReservation/Reservation
        public async Task<IActionResult> Index()
        {
            var easyParkContext = _context.Reservation.Include(r => r.Car).Include(r => r.Lot);
            return View(await easyParkContext.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var EasyParkContext = _context.Reservation.Include(r => r.Car).Include(r => r.Lot).Select(r => new
            {
                resId = r.ResId,
                lotId = r.Lot.LotName,
                carId = r.Car.LicensePlate,
                resTime = r.ResTime.HasValue ? r.ResTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                validUntil = r.ValidUntil.HasValue ? r.ValidUntil.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                startTime = r.StartTime.HasValue ? r.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                paymentStatus = r.PaymentStatus? "已付訂金" : "未訂金",
                isCanceled = r.IsCanceled ? "已取消" : "未取消",
                isOverdue = r.IsOverdue ? "已逾期" : "未逾期",
                isRefoundDeposit = r.IsRefoundDeposit ? "已退訂金" : "未退訂金",
                notificationStatus = r.NotificationStatus ? "已通知" : "未通知",
                isFinish = r.IsFinish ? "已完成" : "未完成"
            });
            return Json(EasyParkContext);
        }

        // GET: MyReservation/Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Lot)
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public IActionResult CreatePartial()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotId", "LotName");
            ViewData["PaymentStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "已付訂金", Value = "True"},
                new SelectListItem {Text = "未付訂金", Value = "False"},
            }, "Value", "Text");
            ViewData["IsCanceled"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "取消", Value = "True"},
                new SelectListItem {Text = "未取消", Value="False"},
            }, "Value", "Text");
            ViewData["IsOverdue"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "逾時", Value = "True"},
                new SelectListItem {Text = "未逾時", Value="False"},
            }, "Value", "Text");
            ViewData["IsRefoundDeposit"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "退款", Value = "True"},
                new SelectListItem {Text = "未退款", Value="False"},
            }, "Value", "Text");
            ViewData["NotificationStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "通知", Value = "True"},
                new SelectListItem {Text = "未通知", Value="False"},
            }, "Value", "Text");
            ViewData["IsFinish"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "完成", Value = "True"},
                new SelectListItem {Text = "未完成", Value="False"},
            }, "Value", "Text");
            return PartialView("_CreatePartial");
        }

        // GET: MyReservation/Reservation/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotId", "LotName");
            ViewData["PaymentStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "已付訂金", Value = "True"},
                new SelectListItem {Text = "未付訂金", Value = "False"},
            }, "Value", "Text");
            ViewData["IsCanceled"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "取消", Value = "True"},
                new SelectListItem {Text = "未取消", Value="False"},
            }, "Value", "Text");
            ViewData["IsOverdue"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "逾時", Value = "True"},
                new SelectListItem {Text = "未逾時", Value="False"},
            }, "Value", "Text");
            ViewData["IsRefoundDeposit"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "退款", Value = "True"},
                new SelectListItem {Text = "未退款", Value="False"},
            }, "Value", "Text");
            ViewData["NotificationStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "通知", Value = "True"},
                new SelectListItem {Text = "未通知", Value="False"},
            }, "Value", "Text");
            ViewData["IsFinish"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "完成", Value = "True"},
                new SelectListItem {Text = "未完成", Value="False"},
            }, "Value", "Text");
            return View();
        }

        // POST: MyReservation/Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResId,CarId,LotId,ResTime,ValidUntil,StartTime,PaymentStatus,IsCanceled,IsOverdue,IsRefoundDeposit,NotificationStatus,IsFinish,Car,Lot")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
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
            //ViewData["CarId"] = new SelectList(_context.Car, "LicensePlate", "LicensePlate", reservation.CarId);
            //ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotName", "LotName", reservation.LotId);
            //return View(reservation);
        }

        public async  Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate", reservation.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotId", "LotName", reservation.LotId);
            return PartialView("_EditPartial", reservation);
        }

        // GET: MyReservation/Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", reservation.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotId", "LotId", reservation.LotId);
            return View(reservation);
        }

        // POST: MyReservation/Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResId,CarId,LotId,ResTime,ValidUntil,StartTime,PaymentStatus,IsCanceled,IsOverdue,IsRefoundDeposit,NotificationStatus,IsFinish")] Reservation reservation)
        {
            if (id != reservation.ResId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ResId))
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
            else
            {
                return Json(new { success = false });
            }
            //ViewData["CarId"] = new SelectList(_context.Car, "CarId", "CarId", reservation.CarId);
            //ViewData["LotId"] = new SelectList(_context.ParkingLots, "LotId", "LotId", reservation.LotId);
            //return View(reservation);
        }

        // GET: MyReservation/Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Lot)
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: MyReservation/Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ResId == id);
        }
    }
}
