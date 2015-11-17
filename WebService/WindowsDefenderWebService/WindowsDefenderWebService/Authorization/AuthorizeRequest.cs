using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace WindowsDefenderWebService.Authorization {
    /// <summary>
    /// Authorization class for access of CRUD methods of the controllers.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public class AuthorizeRequest : AuthorizeAttribute {
        public string AccessLevel { get; set; }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext) {
            AuthenticationHeaderValue authHeaders = actionContext.Request.Headers.Authorization;
            if (authHeaders == null)
                return false;

            string authValue = authHeaders.Parameter;
            string[] login = Base64Decode(authValue);

            if (login[0] == "compcst" && login[1] == "Magicismagenta99") {
                return true;
            }

            return false;
        }

        public static string[] Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            string decoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            string[] keyValuePairs = decoded.Split(':');
            return keyValuePairs;
        }
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}