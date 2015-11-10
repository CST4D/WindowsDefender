using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WindowsDefender_WebApp;

namespace WindowsDefender_WebApp.Controllers
{
    public class TowersController : Controller
    {
        private TowerDefenceEntities db = new TowerDefenceEntities();

        // GET: Towers
        public ActionResult Index()
        {
            var towers = db.Towers.Include(t => t.SpecialAbility);
            return View(towers.ToList());
        }

        // GET: Towers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tower tower = db.Towers.Find(id);
            if (tower == null)
            {
                return HttpNotFound();
            }
            return View(tower);
        }

        // GET: Towers/Create
        public ActionResult Create()
        {
            ViewBag.SpAbilityId = new SelectList(db.SpecialAbilities, "SpAbilityId", "Name");
            return View();
        }

        // POST: Towers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TowerId,Name,Description,Price,Damage,Speed,Range,LevelUnlocked,ImageUrl,SpAbilityId")] Tower tower)
        {
            if (ModelState.IsValid)
            {
                db.Towers.Add(tower);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SpAbilityId = new SelectList(db.SpecialAbilities, "SpAbilityId", "Name", tower.SpAbilityId);
            return View(tower);
        }

        // GET: Towers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tower tower = db.Towers.Find(id);
            if (tower == null)
            {
                return HttpNotFound();
            }
            ViewBag.SpAbilityId = new SelectList(db.SpecialAbilities, "SpAbilityId", "Name", tower.SpAbilityId);
            return View(tower);
        }

        // POST: Towers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TowerId,Name,Description,Price,Damage,Speed,Range,LevelUnlocked,ImageUrl,SpAbilityId")] Tower tower)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tower).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SpAbilityId = new SelectList(db.SpecialAbilities, "SpAbilityId", "Name", tower.SpAbilityId);
            return View(tower);
        }

        // GET: Towers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tower tower = db.Towers.Find(id);
            if (tower == null)
            {
                return HttpNotFound();
            }
            return View(tower);
        }

        // POST: Towers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tower tower = db.Towers.Find(id);
            db.Towers.Remove(tower);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
