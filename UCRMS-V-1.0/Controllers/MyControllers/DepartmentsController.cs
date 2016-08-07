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
    public class DepartmentsController : Controller
    {
        private UcrmsDbContext db = new UcrmsDbContext();

        // GET: /ViewAllDepartment/
        public async Task<ActionResult> ViewDepartments()
        {
            return View(await db.Departments.ToListAsync());
        }



        // GET: /Department/Save
        public ActionResult Save()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save( [Bind(Include = "DepartmentId,Code,Name")] Department department )
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                TempData["Msg"] = "Department Saved Successfully";
                return RedirectToAction("Save");
            }

            return View(department);
        }



        public JsonResult IsCodeAvailble( string code )
        {
            var departmentCode = db.Departments.FirstOrDefault(x => x.Code == code);
            if (departmentCode != null)
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
            var departmentName = db.Departments.FirstOrDefault(x => x.Name == name);

            if (departmentName != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
        }
    }
}
