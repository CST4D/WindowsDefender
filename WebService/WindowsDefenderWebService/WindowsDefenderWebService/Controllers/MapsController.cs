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
    /// <summary>
    /// OData controller for the maps table. Provides only GET methods.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public class MapsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Maps
        [EnableQuery]
        public IQueryable<Map> GetMaps()
        {
            return db.Maps;
        }

        // GET: odata/Maps(5)
        [EnableQuery]
        public SingleResult<Map> GetMap([FromODataUri] int key)
        {
            return SingleResult.Create(db.Maps.Where(map => map.MapId == key));
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
