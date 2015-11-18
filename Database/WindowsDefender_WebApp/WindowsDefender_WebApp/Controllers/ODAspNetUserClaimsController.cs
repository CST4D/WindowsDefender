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
    builder.EntitySet<AspNetUserClaim>("ODAspNetUserClaims");
    builder.EntitySet<AspNetUser>("AspNetUsers"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODAspNetUserClaimsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODAspNetUserClaims
        [EnableQuery]
        public IQueryable<AspNetUserClaim> GetODAspNetUserClaims()
        {
            return db.AspNetUserClaims;
        }

        // GET: odata/ODAspNetUserClaims(5)
        [EnableQuery]
        public SingleResult<AspNetUserClaim> GetAspNetUserClaim([FromODataUri] int key)
        {
            return SingleResult.Create(db.AspNetUserClaims.Where(aspNetUserClaim => aspNetUserClaim.Id == key));
        }

        // PUT: odata/ODAspNetUserClaims(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<AspNetUserClaim> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUserClaim aspNetUserClaim = db.AspNetUserClaims.Find(key);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            patch.Put(aspNetUserClaim);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserClaimExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUserClaim);
        }

        // POST: odata/ODAspNetUserClaims
        public IHttpActionResult Post(AspNetUserClaim aspNetUserClaim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUserClaims.Add(aspNetUserClaim);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserClaimExists(aspNetUserClaim.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(aspNetUserClaim);
        }

        // PATCH: odata/ODAspNetUserClaims(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<AspNetUserClaim> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUserClaim aspNetUserClaim = db.AspNetUserClaims.Find(key);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            patch.Patch(aspNetUserClaim);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserClaimExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUserClaim);
        }

        // DELETE: odata/ODAspNetUserClaims(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            AspNetUserClaim aspNetUserClaim = db.AspNetUserClaims.Find(key);
            if (aspNetUserClaim == null)
            {
                return NotFound();
            }

            db.AspNetUserClaims.Remove(aspNetUserClaim);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODAspNetUserClaims(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.AspNetUserClaims.Where(m => m.Id == key).Select(m => m.AspNetUser));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserClaimExists(int key)
        {
            return db.AspNetUserClaims.Count(e => e.Id == key) > 0;
        }
    }
}
