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
    builder.EntitySet<AspNetUserLogin>("ODAspNetUserLogins");
    builder.EntitySet<AspNetUser>("AspNetUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODAspNetUserLoginsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODAspNetUserLogins
        [EnableQuery]
        public IQueryable<AspNetUserLogin> GetODAspNetUserLogins()
        {
            return db.AspNetUserLogins;
        }

        // GET: odata/ODAspNetUserLogins(5)
        [EnableQuery]
        public SingleResult<AspNetUserLogin> GetAspNetUserLogin([FromODataUri] string key)
        {
            return SingleResult.Create(db.AspNetUserLogins.Where(aspNetUserLogin => aspNetUserLogin.LoginProvider == key));
        }

        // PUT: odata/ODAspNetUserLogins(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<AspNetUserLogin> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUserLogin aspNetUserLogin = db.AspNetUserLogins.Find(key);
            if (aspNetUserLogin == null)
            {
                return NotFound();
            }

            patch.Put(aspNetUserLogin);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserLoginExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUserLogin);
        }

        // POST: odata/ODAspNetUserLogins
        public IHttpActionResult Post(AspNetUserLogin aspNetUserLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUserLogins.Add(aspNetUserLogin);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserLoginExists(aspNetUserLogin.LoginProvider))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(aspNetUserLogin);
        }

        // PATCH: odata/ODAspNetUserLogins(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<AspNetUserLogin> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUserLogin aspNetUserLogin = db.AspNetUserLogins.Find(key);
            if (aspNetUserLogin == null)
            {
                return NotFound();
            }

            patch.Patch(aspNetUserLogin);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserLoginExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUserLogin);
        }

        // DELETE: odata/ODAspNetUserLogins(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            AspNetUserLogin aspNetUserLogin = db.AspNetUserLogins.Find(key);
            if (aspNetUserLogin == null)
            {
                return NotFound();
            }

            db.AspNetUserLogins.Remove(aspNetUserLogin);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODAspNetUserLogins(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.AspNetUserLogins.Where(m => m.LoginProvider == key).Select(m => m.AspNetUser));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserLoginExists(string key)
        {
            return db.AspNetUserLogins.Count(e => e.LoginProvider == key) > 0;
        }
    }
}
