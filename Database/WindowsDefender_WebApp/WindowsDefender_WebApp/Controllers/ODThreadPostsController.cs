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
    builder.EntitySet<ThreadPost>("ODThreadPosts");
    builder.EntitySet<AspNetUser>("AspNetUsers"); 
    builder.EntitySet<Thread>("Threads"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODThreadPostsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODThreadPosts
        [EnableQuery]
        public IQueryable<ThreadPost> GetODThreadPosts()
        {
            return db.ThreadPosts;
        }

        // GET: odata/ODThreadPosts(5)
        [EnableQuery]
        public SingleResult<ThreadPost> GetThreadPost([FromODataUri] int key)
        {
            return SingleResult.Create(db.ThreadPosts.Where(threadPost => threadPost.PostId == key));
        }

        // PUT: odata/ODThreadPosts(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ThreadPost> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ThreadPost threadPost = db.ThreadPosts.Find(key);
            if (threadPost == null)
            {
                return NotFound();
            }

            patch.Put(threadPost);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadPostExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(threadPost);
        }

        // POST: odata/ODThreadPosts
        public IHttpActionResult Post(ThreadPost threadPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ThreadPosts.Add(threadPost);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ThreadPostExists(threadPost.PostId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(threadPost);
        }

        // PATCH: odata/ODThreadPosts(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ThreadPost> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ThreadPost threadPost = db.ThreadPosts.Find(key);
            if (threadPost == null)
            {
                return NotFound();
            }

            patch.Patch(threadPost);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadPostExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(threadPost);
        }

        // DELETE: odata/ODThreadPosts(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ThreadPost threadPost = db.ThreadPosts.Find(key);
            if (threadPost == null)
            {
                return NotFound();
            }

            db.ThreadPosts.Remove(threadPost);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODThreadPosts(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.ThreadPosts.Where(m => m.PostId == key).Select(m => m.AspNetUser));
        }

        // GET: odata/ODThreadPosts(5)/AspNetUser1
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser1([FromODataUri] int key)
        {
            return SingleResult.Create(db.ThreadPosts.Where(m => m.PostId == key).Select(m => m.AspNetUser1));
        }

        // GET: odata/ODThreadPosts(5)/Thread
        [EnableQuery]
        public SingleResult<Thread> GetThread([FromODataUri] int key)
        {
            return SingleResult.Create(db.ThreadPosts.Where(m => m.PostId == key).Select(m => m.Thread));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThreadPostExists(int key)
        {
            return db.ThreadPosts.Count(e => e.PostId == key) > 0;
        }
    }
}
