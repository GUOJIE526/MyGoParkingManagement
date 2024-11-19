//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using EasyPark.Models;
//using Microsoft.AspNetCore.Authorization;
//using System.Globalization;

//namespace EasyPark.Areas.MyCustomer.Controllers
//{
//    [Authorize]
//    [Area("MyCustomer")]
//    public class CustomerController : Controller
//    {
//        private readonly EasyParkContext _context;

//        public CustomerController(EasyParkContext context)
//        {
//            _context = context;
//        }

//        // GET: MyCustomer/Customer
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Customer.ToListAsync());
//        }








//        // GET: MyCustomer/Customer/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var customer = await _context.Customer
//                .FirstOrDefaultAsync(m => m.UserId == id);
//            if (customer == null)
//            {
//                return NotFound();
//            }

//            return View(customer);
//        }

//        // GET: MyCustomer/Customer/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: MyCustomer/Customer/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("UserId,Username,Password,Email,Phone,BlackCount,IsBlack")] Customer customer)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Add(customer);
//                    await _context.SaveChangesAsync();
//                    return Json(new { success = true, message = "用戶新增成功!" });
//                }
//                catch (Exception ex)
//                {
//                    return Json(new { success = false, message = "新增失敗: " + ex.Message });
//                }
//            }
//            else
//            {
//                return Json(new { success = false, message = "輸入的數據無效" });
//            }
//        }


//        // GET: MyCustomer/Customer/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var customer = await _context.Customer.FindAsync(id);
//            if (customer == null)
//            {
//                return NotFound();
//            }
//            return View(customer);
//        }

//        // POST: MyCustomer/Customer/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,Email,Phone,BlackCount,IsBlack")] Customer customer)
//        {
//            if (id != customer.UserId)
//            {
//                return Json(new { success = false, message = "用戶ID不匹配" });
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(customer);
//                    await _context.SaveChangesAsync();
//                    return Json(new { success = true, message = "用戶更新成功!" });
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CustomerExists(customer.UserId))
//                    {
//                        return Json(new { success = false, message = "用戶不存在" });
//                    }
//                    else
//                    {
//                        return Json(new { success = false, message = "更新失敗，發生並發錯誤" });
//                    }
//                }
//            }
//            else
//            {
//                return Json(new { success = false, message = "輸入的數據無效" });
//            }
//        }

//        // GET: MyCustomer/Customer/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var customer = await _context.Customer
//                .FirstOrDefaultAsync(m => m.UserId == id);
//            if (customer == null)
//            {
//                return NotFound();
//            }

//            return View(customer);
//        }

//        // POST: MyCustomer/Customer/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var customer = await _context.Customer.FindAsync(id);
//            if (customer != null)
//            {
//                _context.Customer.Remove(customer);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool CustomerExists(int id)
//        {
//            return _context.Customer.Any(e => e.UserId == id);
//        }

//    }

//}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyPark.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasyPark.Areas.MyCustomer.Controllers
{
    [Authorize]
    [Area("MyCustomer")]
    public class CustomerController : Controller
    {
        private readonly EasyParkContext _context;

        public CustomerController(EasyParkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.Customer
                .Select(c => new
                {
                    c.UserId,
                    c.Username,
                    c.Email,
                    c.Phone,
                    c.BlackCount,
                    c.IsBlack
                })
                .ToListAsync();

            return Json(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,Email,Phone,BlackCount,IsBlack")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "用戶新增成功!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "新增失敗: " + ex.Message });
                }
            }
            return Json(new { success = false, message = "輸入的數據無效" });
        }

        public IActionResult Create()
        {
            return PartialView("_CustomerCreatePartial"); // 使用部分視圖返回表單內容
        }

        // GET: MyCustomer/Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return PartialView("_CustomerEditPartial", customer); // 返回部分視圖，包含用戶資料
        }


        //----------------------------------------------------------------------------------------------------------------


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,Phone,BlackCount,IsBlack")] Customer customer)
        {
            if (id != customer.UserId)
            {
                return Json(new { success = false, message = "用戶ID不匹配" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "用戶更新成功!" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customer.Any(e => e.UserId == id))
                    {
                        return Json(new { success = false, message = "用戶不存在" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "更新失敗，發生並發錯誤" });
                    }
                }
            }
            return Json(new { success = false, message = "輸入的數據無效" });
        }


        //----------------------------------------------------------------------------------------------------------------



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null) return Json(new { success = false, message = "用戶不存在" });

            try
            {
                _context.Customer.Remove(customer);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "用戶刪除成功!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "刪除失敗: " + ex.Message });
            }
        }
       

    }
}
