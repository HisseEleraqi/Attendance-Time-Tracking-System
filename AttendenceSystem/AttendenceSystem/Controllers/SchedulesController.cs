using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttendenceSystem.Data;
using AttendenceSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AttendenceSystem.CustomFilter;

namespace AttendenceSystem.Controllers
{
    [AuthFilter]

    //Only Supervisor
    [Authorize(Roles = "Supervisor")]
    public class SchedulesController : Controller
    {
        DataContext db = new DataContext();
        private string _trackName;
        private int _trackId;

        // GET: Schedules
        public IActionResult Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Id;
            _trackName = trackName;
            _trackId = trackId;

            ViewData["TrackName"] = _trackName;

            var Schedules = db.Schedules.Include(s => s.Tracks).Where(a => a.TrackId == _trackId).ToList();
            return View(Schedules);
        }

        // GET: Schedules/Details/id
        public IActionResult Details(int? id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id2 = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Id;
            _trackName = trackName;
            _trackId = trackId;
            if (id == null)
            {
                return NotFound();
            }

            var schedule =  db.Schedules
                .Include(s => s.Tracks)
                .FirstOrDefault(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Id;
            _trackName = trackName;
            _trackId = trackId;

            ViewData["TrackName"] = _trackName;

            return View();
        }

        // POST: Schedules/Create
        [HttpPost]
        public IActionResult Create(Schedule schedule)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Id;
            _trackName = trackName;
            _trackId = trackId;

            ViewData["TrackName"] = _trackName;

            schedule.TrackId = _trackId;
            if (ModelState.IsValid)
            {   
                db.Add(schedule);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrackName"] = _trackName;
            return View(schedule);
        }

        // GET: Schedules/Edit/id
        public IActionResult Edit(int? id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id2 = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Id;
            _trackName = trackName;
            _trackId = trackId;
            if (id == null)
            {
                return NotFound();
            }

            var schedule =  db.Schedules.Find(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        [HttpPost]
        public IActionResult Edit(int id, Schedule schedule)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id2 = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Id;
            _trackName = trackName;
            _trackId = trackId;

            ViewData["TrackName"] = _trackName;


            if (id != schedule.Id)
            {
                return NotFound();
            }
            schedule.TrackId = _trackId;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(schedule);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public IActionResult Delete(int? id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id2 = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Id;
            _trackName = trackName;
            _trackId = trackId;
            if (id == null)
            {
                return NotFound();
            }

            var schedule =  db.Schedules
                .Include(s => s.Tracks)
                .FirstOrDefault(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id2 = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Id;
            _trackName = trackName;
            _trackId = trackId;
            var schedule =  db.Schedules.Find(id);
            if (schedule != null)
            {
                db.Schedules.Remove(schedule);
            }

             db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return false;
            int id2 = int.Parse(userIdClaim);
            string trackName = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Name;
            int trackId = db.Tracks.FirstOrDefault(a => a.SupervisorId == id2).Id;
            _trackName = trackName;
            _trackId = trackId;
            return db.Schedules.Any(e => e.Id == id);
        }
    }
}
