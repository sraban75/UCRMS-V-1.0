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
using UCRMS_V_1._0.Models.MyContext;
using UCRMS_V_1._0.Models.MyModels;

namespace UCRMS_V_1._0.Controllers
{
    public class AssignCoursesController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: AssignCourses
        public async Task<ActionResult> ViewAssigningCourses()
        {
            var assignCourses = db.AssignCourses.Include(a => a.Course).Include(a => a.Department).Include(a => a.Teacher);
            return View(await assignCourses.ToListAsync());
        }



        // GET: AssignCourses/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AssignCourse assignCourse = await db.AssignCourses.FindAsync(id);
        //    if (assignCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(assignCourse);
        //}

        // GET: AssignCourses/Assign
        public ActionResult Assign()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name");
            return View();
        }

        // POST: AssignCourses/Assign
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign([Bind(Include = "AssignCourseId,DepartmentId,TeacherId,CreditToBeTaken,RemainigCredit,CourseId,CourseName,CourseCredit")] AssignCourse assignCourse)
        {
            if (ModelState.IsValid)
            {
                assignCourse.Status = true;
                Teacher aTeacher = db.Teachers.FirstOrDefault(t => t.TeacherId == assignCourse.TeacherId);
                Course aCourse = db.Courses.FirstOrDefault(c => c.CourseId == assignCourse.CourseId);
                assignCourse.CreditToBeTaken = aTeacher.TakenCredit;

                List<AssignCourse> assignCourses = db.AssignCourses.Where(a => a.TeacherId == aTeacher.TeacherId).ToList();
                AssignCourse assign = null;
                if (assignCourses.Count() != 0)
                {
                    assign = assignCourses.Last();
                    assignCourse.RemainigCredit = assign.RemainigCredit;
                }
                else
                {
                    assignCourse.RemainigCredit = aTeacher.TakenCredit;
                }
                if (assignCourse.RemainigCredit < aCourse.Credit)
                {
                    if (assignCourse.RemainigCredit < aCourse.Credit)
                    {
                        Session["Teacher"] = aTeacher;
                        Session["Course"] = aCourse;
                        Session["AssignedCourse"] = assignCourse;
                        Session["AssignedCourseCheck"] = assign;
                        return RedirectToAction("AskToAssign");
                    }

                    return RedirectToAction("Assign");

                }
                assignCourse.RemainigCredit = assignCourse.RemainigCredit - aCourse.Credit;


                aCourse.AssignTo = aTeacher.TeacherId;
                db.AssignCourses.Add(assignCourse);
                db.SaveChanges();
                TempData["Successfull"] = "Course Assign Successfull";
                return RedirectToAction("Assign");
            }

            //ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", assignCourse.CourseId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", assignCourse.DepartmentId);
            //ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", assignCourse.TeacherId);
            return View(assignCourse);
        }
        //GET: UnAssignCourse
        public ActionResult UnAssignCourse()
        {
            // var list = from p in db.AssignCourses where p.Status.Equals(true) select p;
            var list = from p in db.Courses where p.Status.Equals(true) select p;

            foreach (var course in list)
            {
                course.Status = false;
                //db.AssignCourses.Remove(course);
            }

            //var list2 = from p2 in db.Courses where p2.AssignTo != null select p2;
            //foreach (var assign in list2)
            //{

            //}

            db.SaveChanges();
            return View();
        }

        //GET: ViewCourseStatics
        public ActionResult ViewCourseStatics(int? id)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            var courses = db.Courses.Where(c => c.CourseId == id);

            //var assigncourses = db.AssignCourses.Include(a => a.Course).Include(a => a.Department).Include(a => a.Teacher).Where(t=>t.DepartmentId==departmentId);

            return View(courses);
        }
        //GET: LoadCourseStatics
        //public PartialViewResult LoadCourseStatics( int? departmentId )
        //{

        //       var courseList = db.Courses.Join(db.Teachers,x=>x.AssignTo,y=>y.TeacherId,(x,y)=>new
        //        {
        //            Course=x,Teacher=y
        //        })
        //        .Select(x=> new
        //        {
        //            x.Course.Code,
        //            x.Course.Name,
        //            x.Course.Semester,
        //            Teacher=x.Teacher.Name
        //        }).ToList();
        //    return PartialView("LoadCourseStatics", courseList);
        //}

        [HttpGet]
        public JsonResult GetCourseStatics(int? departmentId)
        {
            var courseList = db.Courses.GroupJoin(db.Teachers, x => x.AssignTo, y => y.TeacherId, (x, y)
                => new { Course = x, Teacher1 = y.DefaultIfEmpty().Select(a => string.IsNullOrEmpty(a.Name) ? "Not Assigned Yet" : a.Name).FirstOrDefault() })
                .Where(x => x.Course.DepartmentId == departmentId)
                //.Where( x => (x.Course.DepartmentId == departmentId) && (x.Course.Status==false))
             .Select(x => new
             {
                 x.Course.Code,
                 x.Course.Name,
                 Semester = x.Course.Semester.Name,
                 Teacher = x.Teacher1
             }).ToList();
            return Json(courseList, JsonRequestBehavior.AllowGet);
        }
        //Get: LoadTeachers
        public ActionResult LoadTeacher(int? departmentId)
        {

            var teachers = db.Teachers.Where(m => m.DepartmentId == departmentId).ToList();
            ViewBag.TeacherId = new SelectList(teachers, "TeacherId", "Name");

            return PartialView("LoadTeacher");
        }
        //Get: LoadTeacherInfo
        public PartialViewResult LoadTeacherInfo(int? teacherId)
        {
            if (teacherId != null)
            {
                Teacher aTeacher = db.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);

                ViewBag.TakenCredit = aTeacher.TakenCredit;
                List<AssignCourse> assignCourses = db.AssignCourses.Where(m => m.TeacherId == teacherId).ToList();

                AssignCourse assign = null;

                if (assignCourses.Count != 0)
                {
                    assign = assignCourses.Last();
                }

                if (assign == null)
                {
                    ViewBag.RemaingCredit = aTeacher.TakenCredit;
                }
                else
                {
                    ViewBag.RemaingCredit = assign.RemainigCredit;
                }
                return PartialView("LoadTeacherInfo");
            }
            return PartialView("LoadTeacherInfo");
        }
        //Get: LoadCourses
        public ActionResult LoadCourse(int? departmentId)
        {

            var courseList = db.Courses.Where(m => m.DepartmentId == departmentId).ToList();
            var assignCourse = db.AssignCourses.Where(m => m.DepartmentId == departmentId).ToList();


            foreach (var assign in assignCourse)
            {
                courseList.Remove(db.Courses.FirstOrDefault(t => t.CourseId == assign.CourseId));
            }


            ViewBag.CourseId = new SelectList(courseList, "CourseId", "Code");

            return PartialView("LoadCourse");
        }
        //Get: LoadCourseInfo
        public PartialViewResult LoadCourseinfo(int? courseId)
        {
            if (courseId != null)
            {
                Course aCourse = db.Courses.FirstOrDefault(t => t.CourseId == courseId);

                ViewBag.Name = aCourse.Name;
                ViewBag.Credit = aCourse.Credit;
                return PartialView("LoadCourseinfo");
            }
            return PartialView("LoadCourseinfo");
        }




        //// GET: AssignCourses/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AssignCourse assignCourse = await db.AssignCourses.FindAsync(id);
        //    if (assignCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", assignCourse.CourseId);
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", assignCourse.DepartmentId);
        //    ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", assignCourse.TeacherId);
        //    return View(assignCourse);
        //}

        //// POST: AssignCourses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "AssignCourseId,DepartmentId,TeacherId,CreditToBeTaken,RemaingCredit,CourseId,Status")] AssignCourse assignCourse)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(assignCourse).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewAssigningCourses");
        //    }
        //    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", assignCourse.CourseId);
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", assignCourse.DepartmentId);
        //    ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", assignCourse.TeacherId);
        //    return View(assignCourse);
        //}

        //// GET: AssignCourses/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AssignCourse assignCourse = await db.AssignCourses.FindAsync(id);
        //    if (assignCourse == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(assignCourse);
        //}

        //// POST: AssignCourses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    AssignCourse assignCourse = await db.AssignCourses.FindAsync(id);
        //    db.AssignCourses.Remove(assignCourse);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("ViewAssigningCourses");
        //}



        public ActionResult AskToAssign()
        {
            Teacher aTeacher = (Teacher)Session["Teacher"];
            Course aCourse = (Course)Session["Course"];
            AssignCourse assign = (AssignCourse)Session["AssignedCourseCheck"];
            double credit = 0.0;
            if (assign == null)
                credit = aTeacher.TakenCredit;
            else
            {
                credit = assign.RemainigCredit;
            }
            if (credit < aCourse.Credit)
            {
                ViewBag.Message = aTeacher.Name
                + " Credit Limit Is Over. And The Course Credit  : " + aCourse.Code
                + " Is " + aCourse.Credit
                + "  ! Still You Want To Assign This Course To This Teacher ?";
            }
            else
            {
                ViewBag.Message = aTeacher.Name
                + " has only " + credit
                + " Credits Remaining . But, The Credit  : " + aCourse.Code
                + " Is " + aCourse.Credit
                + "  ! Still You Want To Assign This Course To This Teacher ?";
            }

            return View();
        }
        //GET: AssignConfirmed
        public ActionResult AssignConfirmed()
        {
            Teacher aTeacher = (Teacher)Session["Teacher"];

            AssignCourse aAssignCourse = (AssignCourse)Session["AssignedCourse"];
            AssignCourse assign = (AssignCourse)Session["AssignedCourseCheck"];
            Course aCourse = db.Courses.FirstOrDefault(c => c.CourseId == aAssignCourse.CourseId);


            aAssignCourse.CreditToBeTaken = aTeacher.TakenCredit;
            if (assign == null)
            {
                aAssignCourse.RemainigCredit = aTeacher.TakenCredit - aCourse.Credit;
            }
            else
            {
                aAssignCourse.RemainigCredit = assign.RemainigCredit - aCourse.Credit;
            }

            aCourse.AssignTo = aTeacher.TeacherId;

            db.AssignCourses.Add(aAssignCourse);
            db.SaveChanges();
            TempData["success"] = "Course Is Assigned Successfully!!!";
            return View();
        }

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
