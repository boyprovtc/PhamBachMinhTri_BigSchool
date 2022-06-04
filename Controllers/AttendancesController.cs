using BigSchools.Models;
using Gremlin.Net.Process.Traversal;
using Microsoft.AspNet.Identity;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchools.Controllers
{
   
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var UserID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if (context.Attendances.Any(p => p.Attendee == UserID && p.CourseID == attendanceDto.Id))
            {
                return BadRequest("The Attendance already exists ! ");
            }

            var attendance = new Attendance() { CourseID = attendanceDto.Id, Attendee = User.Identity.GetUserId() };
            context.Attendances.Add(attendance);
            context.SaveChanges();
            return Ok();
        }
    }
}
