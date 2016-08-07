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
    public class SaveResultsController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: SaveResults/ViewResult
        public ActionResult ViewResult( int? id )
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");

            //            var studentresults = db.SaveResults.Where(s=>s.StudentId==id);
            var courseList = db.Courses.Where(s => s.CourseId == id);
            return View(courseList);
        }

        //// GET: SaveResults/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SaveResult saveResult = await db.SaveResults.FindAsync(id);
        //    if (saveResult == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(saveResult);
        //}

        // GET: SaveResults/SaveResult
        public ActionResult SaveResult()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name");
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo");
            return View();
        }

        // POST: SaveResults/SaveResult
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveResult( [Bind(Include = "SaveResultId,StudentId,CourseId,GradeId")] SaveResult saveResult )
        {
            if (ModelState.IsValid)
            {
                db.SaveResults.Add(saveResult);
                await db.SaveChangesAsync();
                TempData["Msg"] = "Successfully Saved Student Result";
                return RedirectToAction("SaveResult");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name", saveResult.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "Name", saveResult.GradeId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegNo", saveResult.StudentId);
            return View(saveResult);
        }

        //GET: SaveResults/LoadStudentInfo
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

        //GET: SaveResults/LoadCourses
        public ActionResult LoadCourse( int? studentId )
        {

            Student aStudent = db.Students.FirstOrDefault(s => s.StudentId == studentId);

            var enrollCourses = db.EnrollCourses.Where(t => t.StudentId == studentId).ToList();

            List<Course> courseList = new List<Course>();
            foreach (var enrollCourse in enrollCourses)
            {
                courseList.Add(db.Courses.FirstOrDefault(t => t.CourseId == enrollCourse.CourseId));
            }


            ViewBag.CourseId = new SelectList(courseList, "CourseId", "Name");

            return PartialView("LoadCourse");
        }

        //GET: SaveResults/LoadResult
        public PartialViewResult LoadResult( int studentId )
        {
            var students = db.Students.Where(s => s.StudentId == studentId);
            var enrollCourses = db.EnrollCourses.Where(t => t.StudentId == studentId).ToList();
            var studentResults = db.SaveResults.Where(s => s.StudentId == studentId).ToList();
            List<Course> courseList = new List<Course>();
            foreach (var enrollCourse in enrollCourses)
            {
                courseList.Add(db.Courses.FirstOrDefault(t => t.CourseId == enrollCourse.CourseId));
            }
            foreach (var course in courseList)
            {
                foreach (var studentResult in studentResults)
                {
                    if (course.CourseId == studentResult.CourseId)
                    {
                        course.Grade = studentResult.Grade.Name;
                    }
                }
            }

            return PartialView("LoadResult", courseList);
        }

        //GET: SaveResult/GeneratePDF
        public ActionResult GeneratePdf( int? studentId )
        {
            return new Rotativa.ActionAsPdf("MakePdf", new { studentId = studentId });
        }

        //GET: SaveResult/MakePDF
        public ActionResult MakePdf( int? studentId )
        {
            Student aStudent = new Student();
            Student desiredStudent = db.Students.FirstOrDefault(t => t.StudentId == studentId);
            if (desiredStudent != null)
            {
                aStudent.Name = desiredStudent.Name;
                aStudent.Email = desiredStudent.Email;
                aStudent.RegNo = desiredStudent.RegNo;
                aStudent.Department = desiredStudent.Department;

            }

            aStudent.ResultDate = DateTime.Now;

            var enrollCourses = db.EnrollCourses.Where(t => t.StudentId == studentId).ToList();
            var studentResults = db.SaveResults.Where(s => s.StudentId == studentId).ToList();
            List<Course> courseList = new List<Course>();
            foreach (var enrollCourse in enrollCourses)
            {
                courseList.Add(db.Courses.FirstOrDefault(t => t.CourseId == enrollCourse.CourseId));
            }
            foreach (var course in courseList)
            {
                foreach (var studentResult in studentResults)
                {
                    if (course.CourseId == studentResult.CourseId)
                    {
                        course.Grade = studentResult.Grade.Name;
                    }
                }
            }
            aStudent.Courses = courseList;


            return View(aStudent);
        }

        //// GET: SaveResults/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SaveResult saveResult = await db.SaveResults.FindAsync(id);
        //    if (saveResult == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", saveResult.CourseId);
        //    ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "Name", saveResult.GradeId);
        //    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", saveResult.StudentId);
        //    return View(saveResult);
        //}

        //// POST: SaveResults/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "SaveResultId,StudentId,CourseId,GradeId")] SaveResult saveResult)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(saveResult).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewResult");
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", saveResult.CourseId);
        //    ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "Name", saveResult.GradeId);
        //    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Name", saveResult.StudentId);
        //    return View(saveResult);
        //}

        //// GET: SaveResults/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SaveResult saveResult = await db.SaveResults.FindAsync(id);
        //    if (saveResult == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(saveResult);
        //}

        //// POST: SaveResults/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    SaveResult saveResult = await db.SaveResults.FindAsync(id);
        //    db.SaveResults.Remove(saveResult);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("ViewResult");
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
