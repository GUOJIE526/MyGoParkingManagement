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

namespace EasyPark.Areas.MyParkingLot.Controllers
{
    [Authorize]
    [Area("MyParkingLot")]
    public class ParkingLotController : Controller
    {
        private readonly EasyParkContext _context;

        public ParkingLotController(EasyParkContext context)
        {
            _context = context;
        }

        // GET: MyParkingLot/ParkingLot
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkingLots.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var parkingLots = _context.ParkingLots.Select(p => new
            {
                lotId = p.LotId,
                district = p.District,
                type = p.Type,
                lotName = p.LotName,
                location = p.Location,
                monRentalSpace = p.MonRentalSpace,
                smallCarSpace = p.SmallCarSpace,
                etcSpace = p.EtcSpace,
                motherSpace = p.MotherSpace,
                rateRules = p.RateRules,
                weekdayRate = p.WeekdayRate.ToString("C0", CultureInfo.CurrentCulture),
                holidayRate = p.HolidayRate.ToString("C0", CultureInfo.CurrentCulture),
                resDeposit = p.ResDeposit.ToString("C0", CultureInfo.CurrentCulture),
                monRentalRate = p.MonRentalRate.ToString("C0", CultureInfo.CurrentCulture),
                opendoorTime = p.OpendoorTime,
                tel = p.Tel,
                latitude = p.Latitude,
                longitude = p.Longitude,
                validSpace = p.ValidSpace,
            });
            return Json(parkingLots);
        }

        // GET: MyParkingLot/ParkingLot/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLots = await _context.ParkingLots
                .FirstOrDefaultAsync(m => m.LotId == id);
            if (parkingLots == null)
            {
                return NotFound();
            }

            return View(parkingLots);
        }

        // GET: MyParkingLot/ParkingLot/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }

        // POST: MyParkingLot/ParkingLot/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("District,Type,LotName,Location,MonRentalSpace,SmallCarSpace,EtcSpace,MotherSpace,RateRules,WeekdayRate,HolidayRate,ResDeposit,MonRentalRate,OpendoorTime,Tel,Latitude,Longitude,ValidSpace")] ParkingLots parkingLots)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingLots);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
            //return View(parkingLots);
        }

        // GET: MyParkingLot/ParkingLot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLots = await _context.ParkingLots.FindAsync(id);
            if (parkingLots == null)
            {
                return NotFound();
            }
            return View(parkingLots);
        }

        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLots = await _context.ParkingLots.FindAsync(id);
            if (parkingLots == null)
            {
                return NotFound();
            }
            return PartialView("_EditPartial", parkingLots);
        }

        // POST: MyParkingLot/ParkingLot/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("District,Type,LotName,Location,MonRentalSpace,SmallCarSpace,EtcSpace,MotherSpace,RateRules,WeekdayRate,HolidayRate,ResDeposit,MonRentalRate,OpendoorTime,Tel,Latitude,Longitude,ValidSpace")] ParkingLots parkingLots)
        {
            if (id != parkingLots.LotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingLots);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingLotsExists(parkingLots.LotId))
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
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
            //return View(parkingLots);
        }

        // GET: MyParkingLot/ParkingLot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingLots = await _context.ParkingLots
                .FirstOrDefaultAsync(m => m.LotId == id);
            if (parkingLots == null)
            {
                return NotFound();
            }

            return View(parkingLots);
        }

        // POST: MyParkingLot/ParkingLot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingLots = await _context.ParkingLots.FindAsync(id);
            if (parkingLots != null)
            {
                _context.ParkingLots.Remove(parkingLots);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingLotsExists(int id)
        {
            return _context.ParkingLots.Any(e => e.LotId == id);
        }
    }
}
