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
    builder.EntitySet<Virus>("Virus");
    builder.EntitySet<SpecialAbility>("SpecialAbilities"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
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
