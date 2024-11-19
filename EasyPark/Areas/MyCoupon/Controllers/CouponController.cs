using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasyPark.Areas.MyCoupon.Controllers
{
    [Authorize]
    [Area("MyCoupon")]
    public class CouponController : Controller
    {
        private readonly EasyParkContext _context;

        public CouponController(EasyParkContext context)
        {
            _context = context;
        }

        // GET: MyCoupon/Coupon
        public async Task<IActionResult> Index()
        {
            var easyParkContext = _context.Coupon.Include(c => c.User);
            return View(await easyParkContext.ToListAsync());
        }

        //取得DataTable要得資料
        public IActionResult GetCouponAllData()
        {
            var data = _context.Coupon.Include(c => c.User)
                .Select(c => new {
                    userId = c.UserId,
                    userName = c.User != null ? c.User.Username : "代填",
                    couponId = c.CouponId,
                    couponCode = c.CouponCode,
                    discountAmount = c.DiscountAmount,
                    validFrom = c.ValidFrom.ToString("yyyy/M/d tt hh:mm:ss"),
                    validUntil = c.ValidUntil.ToString("yyyy/M/d tt hh:mm:ss"),
                    isUsed = c.IsUsed ? "已使用" : "未使用",
                    //transactions = c.Transactions,
                });


            return Json(data); // 返回 JSON 格式的數據
        }

        // GET: MyCoupon/Coupon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CouponId == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }


        public IActionResult CreatePartial()
        {
            ViewData["isUsed"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "未使用", Value = "false" },
                new SelectListItem { Text = "已使用", Value = "true" }
            }, "Value", "Text");

            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return PartialView("_CreatePartial");
        }

        // GET: MyCoupon/Coupon/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username");
            return View();
        }

        // POST: MyCoupon/Coupon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CouponId,CouponCode,DiscountAmount,ValidFrom,ValidUntil,IsUsed,UserId")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coupon);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewData["isUsed"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "未使用", Value = "False" },
                new SelectListItem { Text = "已使用", Value = "True" }
            }, "Value", "Text");

            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", coupon.UserId);
            return Json(new { success = false, message = "資料有誤" });
        }


        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            ViewData["isUsed"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "未使用", Value = "False" },
                new SelectListItem { Text = "已使用", Value = "True" }
            }, "Value", "Text");
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", coupon.UserId);
            return PartialView("_EditPartial", coupon);
        }


        // GET: MyCoupon/Coupon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "Username", coupon.UserId);
            return View(coupon);
        }

        // POST: MyCoupon/Coupon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CouponId,CouponCode,DiscountAmount,ValidFrom,ValidUntil,IsUsed,UserId")] Coupon coupon)
        {
            if (id != coupon.CouponId)
            {
                return Json(new { success = false, message = "不存在的ID" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!CouponExists(coupon.CouponId))
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                    else
                    {
                        return Json(new { success = false, message = ex.Message });
                        //throw;
                    }
                }
                return Json(new { success = true });
            }
            ViewData["isUsed"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "未使用", Value = "False" },
                new SelectListItem { Text = "已使用", Value = "True" }
            }, "Value", "Text");
            ViewData["UserId"] = new SelectList(_context.Customer, "UserId", "UserId", coupon.UserId);
            return Json(new { success = true });
        }

        // GET: MyCoupon/Coupon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CouponId == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // POST: MyCoupon/Coupon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon != null)
            {
                _context.Coupon.Remove(coupon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CouponExists(int id)
        {
            return _context.Coupon.Any(e => e.CouponId == id);
        }
    }
}
