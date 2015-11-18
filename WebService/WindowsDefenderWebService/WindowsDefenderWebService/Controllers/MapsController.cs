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
using WindowsDefenderWebService;

namespace WindowsDefenderWebService.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WindowsDefenderWebService;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Map>("Maps");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class MapsController : ODataController
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

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
