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
using WindowsDefenderWebService.Models;

namespace WindowsDefenderWebService.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WindowsDefenderWebService.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<MatchHistory>("MatchHistories");
    builder.EntitySet<MatchHistoryDetail>("MatchHistoryDetails"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class MatchHistoriesController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/MatchHistories
        [EnableQuery]
        public IQueryable<MatchHistory> GetMatchHistories()
        {
            return db.MatchHistories;
        }

        // GET: odata/MatchHistories(5)
        [EnableQuery]
        public SingleResult<MatchHistory> GetMatchHistory([FromODataUri] int key)
        {
            return SingleResult.Create(db.MatchHistories.Where(matchHistory => matchHistory.MatchId == key));
        }

        // PUT: odata/MatchHistories(5)
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

        // POST: odata/MatchHistories
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

        // PATCH: odata/MatchHistories(5)
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

        // GET: odata/MatchHistories(5)/MatchHistoryDetails
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
