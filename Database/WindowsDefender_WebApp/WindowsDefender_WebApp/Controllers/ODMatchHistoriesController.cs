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
    builder.EntitySet<MatchHistory>("ODMatchHistories");
    builder.EntitySet<AspNetUser>("AspNetUsers"); 
    builder.EntitySet<MatchHistoryDetail>("MatchHistoryDetails"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODMatchHistoriesController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODMatchHistories
        [EnableQuery]
        public IQueryable<MatchHistory> GetODMatchHistories()
        {
            return db.MatchHistories;
        }

        // GET: odata/ODMatchHistories(5)
        [EnableQuery]
        public SingleResult<MatchHistory> GetMatchHistory([FromODataUri] int key)
        {
            return SingleResult.Create(db.MatchHistories.Where(matchHistory => matchHistory.MatchId == key));
        }

        // PUT: odata/ODMatchHistories(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<MatchHistory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MatchHistory matchHistory = db.MatchHistories.Find(key);
            if (matchHistory == null)
            {
                return NotFound();
            }

            patch.Put(matchHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchHistoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(matchHistory);
        }

        // POST: odata/ODMatchHistories
        public IHttpActionResult Post(MatchHistory matchHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MatchHistories.Add(matchHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MatchHistoryExists(matchHistory.MatchId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(matchHistory);
        }

        // PATCH: odata/ODMatchHistories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<MatchHistory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MatchHistory matchHistory = db.MatchHistories.Find(key);
            if (matchHistory == null)
            {
                return NotFound();
            }

            patch.Patch(matchHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchHistoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(matchHistory);
        }

        // DELETE: odata/ODMatchHistories(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            MatchHistory matchHistory = db.MatchHistories.Find(key);
            if (matchHistory == null)
            {
                return NotFound();
            }

            db.MatchHistories.Remove(matchHistory);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODMatchHistories(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.MatchHistories.Where(m => m.MatchId == key).Select(m => m.AspNetUser));
        }

        // GET: odata/ODMatchHistories(5)/MatchHistoryDetails
        [EnableQuery]
        public IQueryable<MatchHistoryDetail> GetMatchHistoryDetails([FromODataUri] int key)
        {
            return db.MatchHistories.Where(m => m.MatchId == key).SelectMany(m => m.MatchHistoryDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MatchHistoryExists(int key)
        {
            return db.MatchHistories.Count(e => e.MatchId == key) > 0;
        }
    }
}
