using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using WindowsDefender_WebApp;

namespace WindowsDefender_WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config) {
            AddODataRoutes(config);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void AddODataRoutes(HttpConfiguration config) {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            //Towers
            builder.EntitySet<Tower>("ODTowers");
            //builder.EntitySet<SpecialAbility>("SpecialAbilities");

            builder.EntitySet<Virus>("ODVirus");
            builder.EntitySet<SpecialAbility>("ODSpecialAbilities");

            builder.EntitySet<AspNetRole>("ODAspNetRoles");
            builder.EntitySet<AspNetUser>("ODAspNetUsers");
            builder.EntitySet<AspNetUserClaim>("ODAspNetUserClaims");
            builder.EntitySet<AspNetUserLogin>("ODAspNetUserLogins");
            builder.EntitySet<AspNetUserRole>("ODAspNetUserRoles");
            builder.EntitySet<Friend>("ODFriends");
            builder.EntitySet<Map>("ODMaps");
            builder.EntitySet<MatchHistoryDetail>("ODMatchHistoryDetails");
            builder.EntitySet<MatchHistory>("ODMatchHistories");
            builder.EntitySet<Thread>("ODThreads");
            builder.EntitySet<ThreadPost>("ODThreadPosts");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }

        
    }
}
