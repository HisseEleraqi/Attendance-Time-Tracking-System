using AttendenceSystem.Data;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;

namespace AttendenceSystem.Controllers
{
    //Only Supervisor
   [Authorize(Roles = "Supervisor")]
    public class PermissionController : Controller



    {
        private readonly DataContext _db = new DataContext();
        private string _trackName;
        private int _trackId;

        
        public IActionResult StudentPermissions()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return NotFound();
            int id = int.Parse(userIdClaim);
            string trackName = _db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Name;
            int trackId = _db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Id;
            _trackName = trackName;
            _trackId = trackId;
            ViewData["TrackName"] = _trackName;
            var studentPermissions = _db.Permisions.Where(p => p.Student.TrackID == _trackId).ToList();

            return View(studentPermissions);
        }

        
        public IActionResult AcceptPermission(int permissionId)
        {
            var permission = _db.Permisions.FirstOrDefault(p => p.Id == permissionId);
            if (permission != null)
            {
                // Update permission status to "Approved"
                permission.Status = PermisionStatus.Approved;
                _db.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("StudentPermissions");
        }

        public IActionResult RejectPermission(int permissionId)
        {
            var permission = _db.Permisions.FirstOrDefault(p => p.Id == permissionId);
            if (permission != null)
            {
                // Update permission status to "Rejected"
                permission.Status = PermisionStatus.Rejected;
                _db.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("StudentPermissions");
        }


    }
}
