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
    builder.EntitySet<Tower>("ODTowers");
    builder.EntitySet<SpecialAbility>("SpecialAbilities"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODTowersController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODTowers
        [EnableQuery]
        public IQueryable<Tower> GetODTowers()
        {
            return db.Towers;
        }

        // GET: odata/ODTowers(5)
        [EnableQuery]
        public SingleResult<Tower> GetTower([FromODataUri] int key)
        {
            return SingleResult.Create(db.Towers.Where(tower => tower.TowerId == key));
        }

        // PUT: odata/ODTowers(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Tower> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tower tower = db.Towers.Find(key);
            if (tower == null)
            {
                return NotFound();
            }

            patch.Put(tower);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TowerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tower);
        }

        // POST: odata/ODTowers
        public IHttpActionResult Post(Tower tower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Towers.Add(tower);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TowerExists(tower.TowerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(tower);
        }

        // PATCH: odata/ODTowers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Tower> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tower tower = db.Towers.Find(key);
            if (tower == null)
            {
                return NotFound();
            }

            patch.Patch(tower);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TowerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tower);
        }

        // DELETE: odata/ODTowers(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Tower tower = db.Towers.Find(key);
            if (tower == null)
            {
                return NotFound();
            }

            db.Towers.Remove(tower);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODTowers(5)/SpecialAbility
        [EnableQuery]
        public SingleResult<SpecialAbility> GetSpecialAbility([FromODataUri] int key)
        {
            return SingleResult.Create(db.Towers.Where(m => m.TowerId == key).Select(m => m.SpecialAbility));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TowerExists(int key)
        {
            return db.Towers.Count(e => e.TowerId == key) > 0;
        }
    }
}
