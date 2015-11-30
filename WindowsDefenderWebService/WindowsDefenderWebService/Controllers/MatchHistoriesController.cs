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
using WindowsDefenderWebService.Authorization;

namespace WindowsDefenderWebService.Controllers {
    /// <summary>
    /// OData controller for the match histories table
    /// 
    /// Authors: Gerald Becker, Wilson Carpenter
    /// </summary>
    public class MatchHistoriesController : ODataController {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/MatchHistories
        [AuthorizeRequest(AccessLevel = "All")]
        [EnableQuery]
        public IQueryable<MatchHistory> GetMatchHistories() {
            return db.MatchHistories;
        }

        // GET: odata/MatchHistories(5)
        [AuthorizeRequest(AccessLevel = "All")]
        [EnableQuery]
        public SingleResult<MatchHistory> GetMatchHistory([FromODataUri] int key) {
            return SingleResult.Create(db.MatchHistories.Where(matchHistory => matchHistory.MatchId == key));
        }

        // POST: odata/MatchHistories
        [AuthorizeRequest(AccessLevel = "All")]
        public IHttpActionResult Post(MatchHistory matchHistory) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.MatchHistories.Add(matchHistory);

            try {
                db.SaveChanges();
            } catch (DbUpdateException) {
                if (MatchHistoryExists(matchHistory.MatchId)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return Created(matchHistory);
        }

        // GET: odata/MatchHistories(5)/MatchHistoryDetails
        [AuthorizeRequest(AccessLevel = "All")]
        [EnableQuery]
        public IQueryable<MatchHistoryDetail> GetMatchHistoryDetails([FromODataUri] int key) {
            return db.MatchHistories.Where(m => m.MatchId == key).SelectMany(m => m.MatchHistoryDetails);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MatchHistoryExists(int key) {
            return db.MatchHistories.Count(e => e.MatchId == key) > 0;
        }
    }
}
