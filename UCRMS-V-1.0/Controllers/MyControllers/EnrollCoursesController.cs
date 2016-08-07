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
    public class EnrollCoursesController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: EnrollCourses
        public async Task<ActionResult> ViewEnrollCourses()
        {
            var enrollCourses = db.EnrollCourses.Include(e => e.Course).Include(e => e.Student);
            return View(await enrollCourses.ToListAsync());
        }

        //// GET: EnrollCourses/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EnrollCourse enrollCourse = await db.EnrollCourses.FindAsync(id);
        //    if (enrollCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(enrollCourse);
        //}

        // GET: EnrollCourses/Enroll
        public ActionResult Enroll()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            return View();
        }

        // POST: EnrollCourses/Enroll
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll([Bind(Include = "EnrollCourseId,StudentId,CourseId,Date")] EnrollCourse enrollCourse)
        {
            if (ModelState.IsValid)
            {
                db.EnrollCourses.Add(enrollCourse);
                await db.SaveChangesAsync();
                TempData["Msg"] = "Successfully enrolled in a Course";

                return RedirectToAction("Enroll");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", enrollCourse.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo", enrollCourse.StudentId);
            return View(enrollCourse);
        }


        //GET: EnrollCourses/LoadStudentInfo
        public PartialViewResult LoadStudentInfo( int? studentId )
        {
            if (studentId != null)
            {
                Student aStudent = db.Students.FirstOrDefault(s => s.StudentId == studentId);

                ViewBag.Name = aStudent.Name;
                ViewBag.Email = aStudent.Email;
                ViewBag.Department = aStudent.Department.Name;
                return PartialView("LoadStudentInfo");
            }
            return PartialView("LoadStudentInfo");
        }


        //GET: EnrollCourses/LoadCourse
        public ActionResult LoadCourse( int? studentId )
        {

            Student aStudent = db.Students.FirstOrDefault(s => s.StudentId == studentId);
            var courseList = db.Courses.Where(t => t.DepartmentId == aStudent.DepartmentId).ToList();

            var enrollCourses = db.EnrollCourses.Where(t => t.StudentId == studentId).ToList();
            foreach (var enrollCourse in enrollCourses)
            {
                courseList.Remove(db.Courses.FirstOrDefault(c => c.CourseId == enrollCourse.CourseId));
            }

            ViewBag.CourseId = new SelectList(courseList, "CourseId", "Name");

           return PartialView("LoadCourse");
           
        }
        //// GET: EnrollCourses/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EnrollCourse enrollCourse = await db.EnrollCourses.FindAsync(id);
        //    if (enrollCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", enrollCourse.CourseId);
        //    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", enrollCourse.StudentId);
        //    return View(enrollCourse);
        //}

        //// POST: EnrollCourses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "EnrollCourseId,StudentId,CourseId,Date")] EnrollCourse enrollCourse)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(enrollCourse).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewEnrollCourses");
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", enrollCourse.CourseId);
        //    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", enrollCourse.StudentId);
        //    return View(enrollCourse);
        //}

        //// GET: EnrollCourses/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EnrollCourse enrollCourse = await db.EnrollCourses.FindAsync(id);
        //    if (enrollCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(enrollCourse);
        //}

        //// POST: EnrollCourses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    EnrollCourse enrollCourse = await db.EnrollCourses.FindAsync(id);
        //    db.EnrollCourses.Remove(enrollCourse);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("ViewEnrollCourses");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
