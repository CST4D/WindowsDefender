using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WindowsDefender_WebApp.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace WindowsDefender_WebApp.Controllers
{
    public class MatchmakingController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {

            DB _db = new DB();
            var claimsID = User.Identity as ClaimsIdentity;
            var userIDClaim = claimsID.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userIDValue = userIDClaim.Value;

            var matchHistory = _db.MatchHistoryDetails.Where(m => m.UserId == userIDValue);
                
            return View(matchHistory.ToList());
        }

        [Authorize]
        public ActionResult Find()
        {
            return RedirectToAction("Lobby", "Matchmaking");
        }

        [Authorize]
        public ActionResult Lobby()
        {
            return View();
        }
    }
}