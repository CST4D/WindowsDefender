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
    /// OData controller for the virus table. Provides GET methods.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public class VirusController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
