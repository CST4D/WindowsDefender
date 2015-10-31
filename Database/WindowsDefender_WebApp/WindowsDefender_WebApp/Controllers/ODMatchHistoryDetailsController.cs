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
    builder.EntitySet<MatchHistoryDetail>("ODMatchHistoryDetails");
    builder.EntitySet<AspNetUser>("AspNetUsers"); 
    builder.EntitySet<MatchHistory>("MatchHistories"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ODMatchHistoryDetailsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: odata/ODMatchHistoryDetails
        [EnableQuery]
        public IQueryable<MatchHistoryDetail> GetODMatchHistoryDetails()
        {
            return db.MatchHistoryDetails;
        }

        // GET: odata/ODMatchHistoryDetails(5)
        [EnableQuery]
        public SingleResult<MatchHistoryDetail> GetMatchHistoryDetail([FromODataUri] int key)
        {
            return SingleResult.Create(db.MatchHistoryDetails.Where(matchHistoryDetail => matchHistoryDetail.HistoryId == key));
        }

        // PUT: odata/ODMatchHistoryDetails(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<MatchHistoryDetail> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MatchHistoryDetail matchHistoryDetail = db.MatchHistoryDetails.Find(key);
            if (matchHistoryDetail == null)
            {
                return NotFound();
            }

            patch.Put(matchHistoryDetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchHistoryDetailExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(matchHistoryDetail);
        }

        // POST: odata/ODMatchHistoryDetails
        public IHttpActionResult Post(MatchHistoryDetail matchHistoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MatchHistoryDetails.Add(matchHistoryDetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MatchHistoryDetailExists(matchHistoryDetail.HistoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(matchHistoryDetail);
        }

        // PATCH: odata/ODMatchHistoryDetails(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<MatchHistoryDetail> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MatchHistoryDetail matchHistoryDetail = db.MatchHistoryDetails.Find(key);
            if (matchHistoryDetail == null)
            {
                return NotFound();
            }

            patch.Patch(matchHistoryDetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchHistoryDetailExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(matchHistoryDetail);
        }

        // DELETE: odata/ODMatchHistoryDetails(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            MatchHistoryDetail matchHistoryDetail = db.MatchHistoryDetails.Find(key);
            if (matchHistoryDetail == null)
            {
                return NotFound();
            }

            db.MatchHistoryDetails.Remove(matchHistoryDetail);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ODMatchHistoryDetails(5)/AspNetUser
        [EnableQuery]
        public SingleResult<AspNetUser> GetAspNetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.MatchHistoryDetails.Where(m => m.HistoryId == key).Select(m => m.AspNetUser));
        }

        // GET: odata/ODMatchHistoryDetails(5)/MatchHistory
        [EnableQuery]
        public SingleResult<MatchHistory> GetMatchHistory([FromODataUri] int key)
        {
            return SingleResult.Create(db.MatchHistoryDetails.Where(m => m.HistoryId == key).Select(m => m.MatchHistory));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MatchHistoryDetailExists(int key)
        {
            return db.MatchHistoryDetails.Count(e => e.HistoryId == key) > 0;
        }
    }
}
