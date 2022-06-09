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
                
                i.LecturesName = currentUser.Name;

            }

          

            return View(Courses);
        }

        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();

            var listFollowee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var Courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach(var item in listFollowee)
                {
                    if(item.FolloweeId == course.Course.LectureId)
                    {
                        Course objectCourse = course.Course;
                        objectCourse.LecturesName =System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(objectCourse.LectureId).Name;
                        Courses.Add(objectCourse);
                    }
                }
            } 
            return View(Courses);
        }
        public ActionResult Delete(int Id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Courses.Find(Id);
            context.Courses.Remove(course);
            context.SaveChanges();
            return RedirectToAction("Mine");
        }
    }
}

