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
    builder.EntitySet<Thread>("ODThreads");
    builder.EntitySet<ThreadPost>("ThreadPosts"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODThreadsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODThreads
        [EnableQuery]
        public IQueryable<Thread> GetODThreads()
        {
            return db.Threads;
        }

        // GET: odata/ODThreads(5)
        [EnableQuery]
        public SingleResult<Thread> GetThread([FromODataUri] int key)
        {
            return SingleResult.Create(db.Threads.Where(thread => thread.ThreadId == key));
        }

        // PUT: odata/ODThreads(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Thread> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Thread thread = db.Threads.Find(key);
            if (thread == null)
            {
                return NotFound();
            }

            patch.Put(thread);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(thread);
        }

        // POST: odata/ODThreads
        public IHttpActionResult Post(Thread thread)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Threads.Add(thread);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ThreadExists(thread.ThreadId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(thread);
        }

        // PATCH: odata/ODThreads(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Thread> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Thread thread = db.Threads.Find(key);
            if (thread == null)
            {
                return NotFound();
            }

            patch.Patch(thread);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(thread);
        }

        // DELETE: odata/ODThreads(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Thread thread = db.Threads.Find(key);
            if (thread == null)
            {
                return NotFound();
            }

            db.Threads.Remove(thread);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODThreads(5)/ThreadPosts
        [EnableQuery]
        public IQueryable<ThreadPost> GetThreadPosts([FromODataUri] int key)
        {
            return db.Threads.Where(m => m.ThreadId == key).SelectMany(m => m.ThreadPosts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThreadExists(int key)
        {
            return db.Threads.Count(e => e.ThreadId == key) > 0;
        }
    }
}
