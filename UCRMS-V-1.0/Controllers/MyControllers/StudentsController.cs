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
    public class StudentsController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: Students
        public async Task<ActionResult> ViewStudents()
        {
            var students = db.Students.Include(s => s.Department);
            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = await db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student);
        //}

        // GET: Students/Register
        public ActionResult Register()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            DateTime dateTime = DateTime.Now;
            ViewBag.Date = dateTime;
            return View();
        }

        // POST: Students/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register( [Bind(Include = "StudentId,Name,Email,Date,ContactNo,Address,DepartmentId")] Student student )
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                student.RegNo = GetRegNo(student);
                await db.SaveChangesAsync();
                TempData["Msg"] = "New Student Successfully Registerd!";
                return RedirectToAction("Register");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", student.DepartmentId);
            DateTime dateTime = DateTime.Now;
            ViewBag.Date = dateTime;
            return View(student);
        }
        //GET: Students/RegNo
        public string GetRegNo( Student student )
        {
            int id = db.Students.Count(t => (t.DepartmentId == student.DepartmentId) && (t.Date.Year == student.Date.Year)) + 1;

            Department department = db.Departments.FirstOrDefault(d => d.DepartmentId == student.DepartmentId);
            string deptName = department.Name;
            string regId = deptName + "-" + student.Date.Year + "-";
            int len = 3 - id.ToString().Length;
            string addZero = "";
            for (int i = 0; i < len; i++)
            {
                addZero = "0" + addZero;
            }
            return regId + addZero + id;
        }
        //GET: Students/IsEmailAvailable
        public JsonResult IsEmailAvailable( string email )
        {
            var studentEmail = db.Students.FirstOrDefault(x => x.Email == email);

            if (studentEmail != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
        }


        //// GET: Students/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = await db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", student.DepartmentId);
        //    return View(student);
        //}

        //// POST: Students/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "StudentId,Name,Email,Date,ContactNo,Address,DepartmentId,ResultDate,RegNo")] Student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(student).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewStudents");
        //    }
        //    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", student.DepartmentId);
        //    return View(student);
        //}

        //// GET: Students/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = await db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student);
        //}

        //// POST: Students/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Student student = await db.Students.FindAsync(id);
        //    db.Students.Remove(student);
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
