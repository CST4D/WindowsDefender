using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using WindowsDefenderWebService.Models;

namespace WindowsDefenderWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Add all the OData routes
            AddODataRoutes(config);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }


        /// <summary>
        /// Add the all of the entities to the routing list.
        /// </summary>
        /// <param name="config">The HttpConfig class to add the routes to.</param>
        private static void AddODataRoutes(HttpConfiguration config) {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Tower>("Towers");
            builder.EntitySet<SpecialAbility>("SpecialAbilities");
            builder.EntitySet<Virus>("Virus");
            builder.EntitySet<Map>("Maps");
            builder.EntitySet<MatchHistory>("MatchHistories");
            builder.EntitySet<AspNetUser>("AspNetUsers");
            builder.EntitySet<MatchHistoryDetail>("MatchHistoryDetails");
            builder.EntitySet<AspNetUserLogin>("UserLogin");
            builder.EntitySet<Thread>("Threads");
            builder.EntitySet<ThreadPost>("ThreadPosts");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
