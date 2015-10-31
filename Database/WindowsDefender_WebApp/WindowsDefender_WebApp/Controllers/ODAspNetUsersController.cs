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
    builder.EntitySet<AspNetUser>("ODAspNetUsers");
    builder.EntitySet<Friend>("Friends"); 
    builder.EntitySet<MatchHistory>("MatchHistories"); 
    builder.EntitySet<MatchHistoryDetail>("MatchHistoryDetails"); 
    builder.EntitySet<ThreadPost>("ThreadPosts"); 
    builder.EntitySet<AspNetUserClaim>("AspNetUserClaims"); 
    builder.EntitySet<AspNetUserLogin>("AspNetUserLogins"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODAspNetUsersController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODAspNetUsers
        [EnableQuery]
        public IQueryable<AspNetUser> GetODAspNetUsers()
        {
            return db.AspNetUsers;
        }

        // GET: odata/ODAspNetUsers(5)
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.AspNetUsers.Where(aspNetUser => aspNetUser.Id == key));
        }

        // PUT: odata/ODAspNetUsers(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<AspNetUser> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUser aspNetUser = db.AspNetUsers.Find(key);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            patch.Put(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUser);
        }

        // POST: odata/ODAspNetUsers
        public IHttpActionResult Post(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUsers.Add(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspNetUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(aspNetUser);
        }

        // PATCH: odata/ODAspNetUsers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<AspNetUser> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUser aspNetUser = db.AspNetUsers.Find(key);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            patch.Patch(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUser);
        }

        // DELETE: odata/ODAspNetUsers(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(key);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODAspNetUsers(5)/Friends
        [EnableQuery]
        public IQueryable<Friend> GetFriends([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.Friends);
        }

        // GET: odata/ODAspNetUsers(5)/Friends1
        [EnableQuery]
        public IQueryable<Friend> GetFriends1([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.Friends1);
        }

        // GET: odata/ODAspNetUsers(5)/MatchHistories
        [EnableQuery]
        public IQueryable<MatchHistory> GetMatchHistories([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.MatchHistories);
        }

        // GET: odata/ODAspNetUsers(5)/MatchHistoryDetails
        [EnableQuery]
        public IQueryable<MatchHistoryDetail> GetMatchHistoryDetails([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.MatchHistoryDetails);
        }

        // GET: odata/ODAspNetUsers(5)/ThreadPosts
        [EnableQuery]
        public IQueryable<ThreadPost> GetThreadPosts([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.ThreadPosts);
        }

        // GET: odata/ODAspNetUsers(5)/ThreadPosts1
        [EnableQuery]
        public IQueryable<ThreadPost> GetThreadPosts1([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.ThreadPosts1);
        }

        // GET: odata/ODAspNetUsers(5)/AspNetUserClaims
        [EnableQuery]
        public IQueryable<AspNetUserClaim> GetAspNetUserClaims([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.AspNetUserClaims);
        }

        // GET: odata/ODAspNetUsers(5)/AspNetUserLogins
        [EnableQuery]
        public IQueryable<AspNetUserLogin> GetAspNetUserLogins([FromODataUri] string key)
        {
            return db.AspNetUsers.Where(m => m.Id == key).SelectMany(m => m.AspNetUserLogins);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserExists(string key)
        {
            return db.AspNetUsers.Count(e => e.Id == key) > 0;
        }
    }
}
