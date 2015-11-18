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

    public class VirusController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/Virus
        [EnableQuery]
        public IQueryable<Virus> GetVirus()
        {
            return db.Viruses;
        }

        // GET: odata/Virus(5)
        [EnableQuery]
        public SingleResult<Virus> GetVirus([FromODataUri] int key)
        {
            return SingleResult.Create(db.Viruses.Where(virus => virus.VirusId == key));
        }

        // PUT: odata/Virus(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Virus> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Virus virus = db.Viruses.Find(key);
            if (virus == null)
            {
                return NotFound();
            }

            patch.Put(virus);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VirusExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(virus);
        }

        // POST: odata/Virus
        public IHttpActionResult Post(Virus virus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Viruses.Add(virus);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VirusExists(virus.VirusId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(virus);
        }

        // PATCH: odata/Virus(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Virus> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Virus virus = db.Viruses.Find(key);
            if (virus == null)
            {
                return NotFound();
            }

            patch.Patch(virus);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VirusExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(virus);
        }

        // DELETE: odata/Virus(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Virus virus = db.Viruses.Find(key);
            if (virus == null)
            {
                return NotFound();
            }

            db.Viruses.Remove(virus);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Virus(5)/SpecialAbility
        [EnableQuery]
        public SingleResult<SpecialAbility> GetSpecialAbility([FromODataUri] int key)
        {
            return SingleResult.Create(db.Viruses.Where(m => m.VirusId == key).Select(m => m.SpecialAbility));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VirusExists(int key)
        {
            return db.Viruses.Count(e => e.VirusId == key) > 0;
        }
    }
}
