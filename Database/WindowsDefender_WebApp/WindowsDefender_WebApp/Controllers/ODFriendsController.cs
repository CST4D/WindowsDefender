using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WindowsDefender_WebApp;

namespace WindowsDefender_WebApp.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WindowsDefender_WebApp;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Friend>("ODFriends");
    builder.EntitySet<AspNetUser>("AspNetUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODFriendsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODFriends
        [EnableQuery]
        public IQueryable<Friend> GetODFriends()
        {
            return db.Friends;
        }

        // GET: odata/ODFriends(5)
        [EnableQuery]
        public SingleResult<Friend> GetFriend([FromODataUri] string key)
        {
            return SingleResult.Create(db.Friends.Where(friend => friend.UserId == key));
        }

        // PUT: odata/ODFriends(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Friend> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Friend friend = db.Friends.Find(key);
            if (friend == null)
            {
                return NotFound();
            }

            patch.Put(friend);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(friend);
        }

        // POST: odata/ODFriends
        public IHttpActionResult Post(Friend friend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Friends.Add(friend);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FriendExists(friend.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(friend);
        }

        // PATCH: odata/ODFriends(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Friend> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Friend friend = db.Friends.Find(key);
            if (friend == null)
            {
                return NotFound();
            }

            patch.Patch(friend);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(friend);
        }

        // DELETE: odata/ODFriends(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            Friend friend = db.Friends.Find(key);
            if (friend == null)
            {
                return NotFound();
            }

            db.Friends.Remove(friend);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODFriends(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.Friends.Where(m => m.UserId == key).Select(m => m.AspNetUser));
        }

        // GET: odata/ODFriends(5)/AspNetUser1
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser1([FromODataUri] string key)
        {
            return SingleResult.Create(db.Friends.Where(m => m.UserId == key).Select(m => m.AspNetUser1));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FriendExists(string key)
        {
            return db.Friends.Count(e => e.UserId == key) > 0;
        }
    }
}
