using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UCRMS_V_1._0.Models.MyContext;
using UCRMS_V_1._0.Models.MyModels;

namespace UCRMS_V_1._0.Controllers.MyControllers
{
    public class CoursesController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: Courses
        public async Task<ActionResult> ViewCourses()
        {
            var courses = db.Courses.Include(c => c.Department).Include(c => c.Semester);
            return View(await courses.ToListAsync());
        }

        //// GET: Courses/Details/5
        //public async Task<ActionResult> Details( int? id )
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Course course = await db.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(course);
        //}

        // GET: Courses/Save
        public ActionResult Save()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
            return View();
        }

        // POST: Courses/Save
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save( [Bind(Include = "CourseId,Code,Name,Credit,Description,DepartmentId,SemesterId")] Course course )
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                db.SaveChanges();
                TempData["Msg"] = "Course Successfully Saved";
                return RedirectToAction("Save");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name", course.SemesterId);
            return View(course);
        }

        //GET: Validation

        public JsonResult IsCodeAvailble( string code )
        {
            var courseCode = db.Courses.FirstOrDefault(x => x.Code == code);
            if (courseCode != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult IsNameAvailble( string name )
        {
            var courseName = db.Courses.FirstOrDefault(x => x.Name == name);

            if (courseName != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
        }



        // GET: Courses/Edit/5
        //public async Task<ActionResult> Edit( int? id )
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Course course = await db.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
        //    ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name", course.SemesterId);
        //    return View(course);
        //}

        //// POST: Courses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit( [Bind(Include = "CourseId,Code,Name,Credit,Description,DepartmentId,SemesterId,AssignTo,Grade,Status,ScheduleInfo")] Course course )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(course).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewCourses");
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
        //    ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name", course.SemesterId);
        //    return View(course);
        //}

        //// GET: Courses/Delete/5
        //public async Task<ActionResult> Delete( int? id )
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Course course = await db.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(course);
        //}

        //// POST: Courses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed( int id )
        //{
        //    Course course = await db.Courses.FindAsync(id);
        //    db.Courses.Remove(course);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("ViewCourses");
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
