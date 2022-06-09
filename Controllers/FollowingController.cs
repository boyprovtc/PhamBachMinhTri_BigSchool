using BigSchools.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchools.Controllers
{
    public class FollowingController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            //user login laf nguoi theo doi, follow.FolloweeId la nguoi dc theo doi
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Login First !!!");
            if (userID == follow.FolloweeId)
                return BadRequest("Can not follow your-self !");

            BigSchoolContext context = new BigSchoolContext();
            Following find = context.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId);

            if (find != null)
            {
               // return BadRequest("The already following exists ! ");
               context.Followings.Remove(context.Followings.SingleOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                context.SaveChanges();
                return Ok("cancel");

            }

            //set object follow
            follow.FollowerId = userID;
            context.Followings.Add(follow);
            context.SaveChanges();


            return Ok();
            }
        }
    }
