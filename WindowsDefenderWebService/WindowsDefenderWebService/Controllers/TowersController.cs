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
    /// OData controller for the Towers table. Provides only GET methods.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
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
