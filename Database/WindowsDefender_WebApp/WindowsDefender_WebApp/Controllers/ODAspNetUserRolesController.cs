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
    builder.EntitySet<AspNetUserRole>("ODAspNetUserRoles");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODAspNetUserRolesController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODAspNetUserRoles
        [EnableQuery]
        public IQueryable<AspNetUserRole> GetODAspNetUserRoles()
        {
            return db.AspNetUserRoles;
        }

        // GET: odata/ODAspNetUserRoles(5)
        [EnableQuery]
        public SingleResult<AspNetUserRole> GetAspNetUserRole([FromODataUri] string key)
        {
            return SingleResult.Create(db.AspNetUserRoles.Where(aspNetUserRole => aspNetUserRole.UserId == key));
        }

        // PUT: odata/ODAspNetUserRoles(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<AspNetUserRole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find(key);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }

            patch.Put(aspNetUserRole);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserRoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUserRole);
        }

        // POST: odata/ODAspNetUserRoles
        public IHttpActionResult Post(AspNetUserRole aspNetUserRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUserRoles.Add(aspNetUserRole);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserRoleExists(aspNetUserRole.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(aspNetUserRole);
        }

        // PATCH: odata/ODAspNetUserRoles(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<AspNetUserRole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find(key);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }

            patch.Patch(aspNetUserRole);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserRoleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(aspNetUserRole);
        }

        // DELETE: odata/ODAspNetUserRoles(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Find(key);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }

            db.AspNetUserRoles.Remove(aspNetUserRole);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserRoleExists(string key)
        {
            return db.AspNetUserRoles.Count(e => e.UserId == key) > 0;
        }
    }
}
