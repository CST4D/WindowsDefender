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
    builder.EntitySet<SpecialAbility>("SpecialAbilities");
    builder.EntitySet<Tower>("Towers"); 
    builder.EntitySet<Virus>("Viruses"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SpecialAbilitiesController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/SpecialAbilities
        [EnableQuery]
        public IQueryable<SpecialAbility> GetSpecialAbilities()
        {
            return db.SpecialAbilities;
        }

        // GET: odata/SpecialAbilities(5)
        [EnableQuery]
        public SingleResult<SpecialAbility> GetSpecialAbility([FromODataUri] int key)
        {
            return SingleResult.Create(db.SpecialAbilities.Where(specialAbility => specialAbility.SpAbilityId == key));
        }

        // GET: odata/SpecialAbilities(5)/Towers
        [EnableQuery]
        public IQueryable<Tower> GetTowers([FromODataUri] int key)
        {
            return db.SpecialAbilities.Where(m => m.SpAbilityId == key).SelectMany(m => m.Towers);
        }

        // GET: odata/SpecialAbilities(5)/Viruses
        [EnableQuery]
        public IQueryable<Virus> GetViruses([FromODataUri] int key)
        {
            return db.SpecialAbilities.Where(m => m.SpAbilityId == key).SelectMany(m => m.Viruses);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialAbilityExists(int key)
        {
            return db.SpecialAbilities.Count(e => e.SpAbilityId == key) > 0;
        }
    }
}
