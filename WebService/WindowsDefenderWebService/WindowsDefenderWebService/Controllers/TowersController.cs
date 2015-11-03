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
    builder.EntitySet<Tower>("Towers");
    builder.EntitySet<SpecialAbility>("SpecialAbilities"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TowersController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Towers
        [EnableQuery]
        public IQueryable<Tower> GetTowers()
        {
            return db.Towers;
        }

        // GET: odata/Towers(5)
        [EnableQuery]
        public SingleResult<Tower> GetTower([FromODataUri] int key)
        {
            return SingleResult.Create(db.Towers.Where(tower => tower.TowerId == key));
        }

        // GET: odata/Towers(5)/SpecialAbility
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
