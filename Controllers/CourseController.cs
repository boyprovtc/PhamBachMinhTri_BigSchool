using BigSchools.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchools.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Create()
        {

            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.Listcategory = context.Categories.ToList();

            return View(objCourse);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Course objCourse)
        {

            BigSchoolContext context = new BigSchoolContext();

            ModelState.Remove("LectureId");
            if (!ModelState.IsValid)
            {
                objCourse.Listcategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LectureId = user.Id;
            
            context.Courses.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();

            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();

            var courses = new List<Course>();

            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LectureId = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(objCourse.LectureId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }


        public ActionResult Mine()
        {

            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


            var Courses = context.Courses.Where(c => c.LectureId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in Courses)
            {
                
                i.LectureId = currentUser.Name;

            }

          

            return View(Courses);
        }
    }
}