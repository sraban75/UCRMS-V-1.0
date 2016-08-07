using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using UCRMS_V_1._0.Models.MyContext;
using UCRMS_V_1._0.Models.MyModels;

namespace UCRMS_V_1._0.Controllers.MyControllers
{
    public class AllocateClassRoomsController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: AllocateClassRooms
        public async Task<ActionResult> ViewAllocations()
        {
            var allocateClassRooms = db.AllocateClassRooms.Include(a => a.Course).Include(a => a.Day).Include(a => a.Department).Include(a => a.Room);
            return View(await allocateClassRooms.ToListAsync());
        }

        // GET: AllocateClassRooms/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AllocateClassRoom allocateClassRoom = await db.AllocateClassRooms.FindAsync(id);
        //    if (allocateClassRoom == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(allocateClassRoom);
        //}

        // GET: AllocateClassRooms/Allocate
        public ActionResult Allocate()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name");
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name");
            return View();
        }

        // POST: AllocateClassRooms/Allocate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Allocate( [Bind(Include = "AllocateClassRoomId,DepartmentId,CourseId,RoomId,DayId,From,To")] AllocateClassRoom allocateClassRoom )
        {
            if (ModelState.IsValid)
            {
                List<AllocateClassRoom> allocateClassRooms =
                    db.AllocateClassRooms.Where(
                        t =>
                            (t.RoomId == allocateClassRoom.RoomId) && (t.DayId == allocateClassRoom.DayId) &&
                            ((t.From <= allocateClassRoom.From && t.To >= allocateClassRoom.From) ||
                            (t.From <= allocateClassRoom.To && t.To >= allocateClassRoom.To) ||
                            (t.From >= allocateClassRoom.From && t.From <= allocateClassRoom.To) ||
                            (t.To >= allocateClassRoom.From && t.To <= allocateClassRoom.To)
                            )).ToList();

                if (allocateClassRooms.Count == 0)
                {
                    db.AllocateClassRooms.Add(allocateClassRoom);
                    db.SaveChanges();
                    TempData["Msg"] = "Class Allocate Successfull";
                }
                else
                {
                    TempData["Msgf"] = "Can't Allocate Class in Same Room During other class";
                }

                return RedirectToAction("Allocate");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", allocateClassRoom.CourseId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", allocateClassRoom.DayId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", allocateClassRoom.DepartmentId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", allocateClassRoom.RoomId);
            return View(allocateClassRoom);
        }
        //GET: AllocateClassRooms/LoadCourse
        public ActionResult LoadCourse( int? departmentId )
        {

            var courseList = db.Courses.Where(m => m.DepartmentId == departmentId).ToList();
            //var assignCourse = db.AssignCourses.Where(m => m.DepartmentId == departmentId).ToList();


            ViewBag.CourseId = new SelectList(courseList, "CourseId", "Name");


            return PartialView("LoadCourse");
        }

        // GET: AllocateClassRooms/UnAllocate
        public ActionResult UnallocateClassRoom()
        {
            var rooms = from p in db.AllocateClassRooms where p.Status.Equals(false) select p;

            foreach (var roomAllocation in rooms)
            {
                db.AllocateClassRooms.Remove(roomAllocation);
            }
            db.SaveChanges();
            return View();
        }

        // GET: /AllocateClassRoom/ViewStudents
        public ActionResult ViewSchedule()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            return View();
        }
        
        public JsonResult GetCourseByDept( int? departmentId )
        {
            var courseList = db.Courses
                .Where(x => x.DepartmentId == departmentId)
                .Select(x => new
                {
                    x.CourseId,
                    x.Code,
                    x.Name,

                }).ToList();

            return Json(courseList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetScheduleInfo( int? courseId )
        {

            var courseList = db.AllocateClassRooms
                        .Where(x => x.CourseId == courseId)
                        .Select(x => new
                        {
                            x.From,
                            x.To,
                            x.Room.Name,
                            DayName=x.Day.Name,
                          //  Semester = x.Course.Semester.Name,
                            Schedule = "R. No :" + x.Room.Name + ", " +
                            x.Day.Name + " " + x.From + " - " + x.To
                        }).ToList();

            return Json(courseList, JsonRequestBehavior.AllowGet);

        }



        //// GET: AllocateClassRooms/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AllocateClassRoom allocateClassRoom = await db.AllocateClassRooms.FindAsync(id);
        //    if (allocateClassRoom == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", allocateClassRoom.CourseId);
        //    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", allocateClassRoom.DayId);
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", allocateClassRoom.DepartmentId);
        //    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", allocateClassRoom.RoomId);
        //    return View(allocateClassRoom);
        //}

        //// POST: AllocateClassRooms/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "AllocateClassRoomId,DepartmentId,CourseId,RoomId,DayId,From,To,Status")] AllocateClassRoom allocateClassRoom)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(allocateClassRoom).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewStudents");
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", allocateClassRoom.CourseId);
        //    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", allocateClassRoom.DayId);
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", allocateClassRoom.DepartmentId);
        //    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", allocateClassRoom.RoomId);
        //    return View(allocateClassRoom);
        //}

        //// GET: AllocateClassRooms/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AllocateClassRoom allocateClassRoom = await db.AllocateClassRooms.FindAsync(id);
        //    if (allocateClassRoom == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(allocateClassRoom);
        //}

        //// POST: AllocateClassRooms/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    AllocateClassRoom allocateClassRoom = await db.AllocateClassRooms.FindAsync(id);
        //    db.AllocateClassRooms.Remove(allocateClassRoom);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("ViewStudents");
        //}

        protected override void Dispose( bool disposing )
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
