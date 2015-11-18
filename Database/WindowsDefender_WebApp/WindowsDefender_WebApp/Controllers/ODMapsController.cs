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
    builder.EntitySet<Map>("ODMaps");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODMapsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODMaps
        [EnableQuery]
        public IQueryable<Map> GetODMaps()
        {
            return db.Maps;
        }

        // GET: odata/ODMaps(5)
        [EnableQuery]
        public SingleResult<Map> GetMap([FromODataUri] int key)
        {
            return SingleResult.Create(db.Maps.Where(map => map.MapId == key));
        }

        // PUT: odata/ODMaps(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Map> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Map map = db.Maps.Find(key);
            if (map == null)
            {
                return NotFound();
            }

            patch.Put(map);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(map);
        }

        // POST: odata/ODMaps
        public IHttpActionResult Post(Map map)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Maps.Add(map);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MapExists(map.MapId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(map);
        }

        // PATCH: odata/ODMaps(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Map> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Map map = db.Maps.Find(key);
            if (map == null)
            {
                return NotFound();
            }

            patch.Patch(map);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(map);
        }

        // DELETE: odata/ODMaps(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Map map = db.Maps.Find(key);
            if (map == null)
            {
                return NotFound();
            }

            db.Maps.Remove(map);
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

        private bool MapExists(int key)
        {
            return db.Maps.Count(e => e.MapId == key) > 0;
        }
    }
}
