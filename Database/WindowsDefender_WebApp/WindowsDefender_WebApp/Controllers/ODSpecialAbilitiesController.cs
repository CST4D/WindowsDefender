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
    builder.EntitySet<SpecialAbility>("ODSpecialAbilities");
    builder.EntitySet<Tower>("Towers"); 
    builder.EntitySet<Virus>("Viruses"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODSpecialAbilitiesController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODSpecialAbilities
        [EnableQuery]
        public IQueryable<SpecialAbility> GetODSpecialAbilities()
        {
            return db.SpecialAbilities;
        }

        // GET: odata/ODSpecialAbilities(5)
        [EnableQuery]
        public SingleResult<SpecialAbility> GetSpecialAbility([FromODataUri] int key)
        {
            return SingleResult.Create(db.SpecialAbilities.Where(specialAbility => specialAbility.SpAbilityId == key));
        }

        // PUT: odata/ODSpecialAbilities(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<SpecialAbility> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SpecialAbility specialAbility = db.SpecialAbilities.Find(key);
            if (specialAbility == null)
            {
                return NotFound();
            }

            patch.Put(specialAbility);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialAbilityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(specialAbility);
        }

        // POST: odata/ODSpecialAbilities
        public IHttpActionResult Post(SpecialAbility specialAbility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SpecialAbilities.Add(specialAbility);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SpecialAbilityExists(specialAbility.SpAbilityId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(specialAbility);
        }

        // PATCH: odata/ODSpecialAbilities(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<SpecialAbility> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SpecialAbility specialAbility = db.SpecialAbilities.Find(key);
            if (specialAbility == null)
            {
                return NotFound();
            }

            patch.Patch(specialAbility);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialAbilityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(specialAbility);
        }

        // DELETE: odata/ODSpecialAbilities(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            SpecialAbility specialAbility = db.SpecialAbilities.Find(key);
            if (specialAbility == null)
            {
                return NotFound();
            }

            db.SpecialAbilities.Remove(specialAbility);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODSpecialAbilities(5)/Towers
        [EnableQuery]
        public IQueryable<Tower> GetTowers([FromODataUri] int key)
        {
            return db.SpecialAbilities.Where(m => m.SpAbilityId == key).SelectMany(m => m.Towers);
        }

        // GET: odata/ODSpecialAbilities(5)/Viruses
        [EnableQuery]
        public IQueryable<Virus> GetViruses([FromODataUri] int key)
        {
            return db.SpecialAbilities.Where(m => m.SpAbilityId == key).SelectMany(m => m.Viruses);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialAbilityExists(int key)
        {
            return db.SpecialAbilities.Count(e => e.SpAbilityId == key) > 0;
        }
    }
}
