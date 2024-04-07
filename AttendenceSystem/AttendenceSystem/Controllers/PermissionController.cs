using AttendenceSystem.Data;
using AttendenceSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace AttendenceSystem.Controllers
{
    public class PermissionController : Controller



    {
        private readonly DataContext _db = new DataContext();
        private string _trackName;
        private int _trackId;
        public PermissionController()
        {
            RetrieveTrackInfo();
        }
        private void RetrieveTrackInfo()
        {
            int id = 3; // Get the id from the session
            string trackName = _db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Name;
            int trackId = _db.Tracks.FirstOrDefault(a => a.SupervisorId == id).Id;
            _trackName = trackName;
            _trackId = trackId;

        }

        
        public IActionResult StudentPermissions()
        {
            /* _db.Permisions.Add(new Permision
             {
                 Type=PermisionType.Late,
                 Status=PermisionStatus.Pending,
                 StudentId =5,
                 IsApproved=false,
                 Date=new DateOnly (2024,4,5),
                 Reason="maryam"

             });
             _db.SaveChanges();*/
            // Retrieve student permissions from the database

            ViewData["TrackName"] = _trackName;
            var studentPermissions = _db.Permisions.ToList();

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
