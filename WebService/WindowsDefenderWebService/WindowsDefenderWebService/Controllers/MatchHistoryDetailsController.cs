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
    /// 
    /// Authors
    /// </summary>
    public class MatchHistoryDetailsController : ODataController {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/MatchHistoryDetails
        [AuthorizeRequest(AccessLevel = "All")]
        [EnableQuery]
        public IQueryable<MatchHistoryDetail> GetMatchHistoryDetails() {
            return db.MatchHistoryDetails;
        }

        // GET: odata/MatchHistoryDetails(5)
        [AuthorizeRequest(AccessLevel = "All")]
        [EnableQuery]
        public SingleResult<MatchHistoryDetail> GetMatchHistoryDetail([FromODataUri] int key) {
            return SingleResult.Create(db.MatchHistoryDetails.Where(matchHistoryDetail => matchHistoryDetail.HistoryId == key));
        }

        // POST: odata/MatchHistoryDetails
        [AuthorizeRequest(AccessLevel = "All")]
        public IHttpActionResult Post(MatchHistoryDetail matchHistoryDetail) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.MatchHistoryDetails.Add(matchHistoryDetail);

            try {
                db.SaveChanges();
            } catch (DbUpdateException) {
                if (MatchHistoryDetailExists(matchHistoryDetail.HistoryId)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return Created(matchHistoryDetail);
        }

        // GET: odata/MatchHistoryDetails(5)/MatchHistory
        [AuthorizeRequest(AccessLevel = "All")]
        [EnableQuery]
        public SingleResult<MatchHistory> GetMatchHistory([FromODataUri] int key) {
            return SingleResult.Create(db.MatchHistoryDetails.Where(m => m.HistoryId == key).Select(m => m.MatchHistory));
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MatchHistoryDetailExists(int key) {
            return db.MatchHistoryDetails.Count(e => e.HistoryId == key) > 0;
        }
    }
}
