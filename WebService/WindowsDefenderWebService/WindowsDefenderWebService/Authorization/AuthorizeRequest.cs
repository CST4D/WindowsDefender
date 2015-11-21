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
        private const string USERNAME = "compcst";
        private const string PASSWORD = "Magicismagenta99";

        /// <summary>
        /// The access level required for the operation.
        /// </summary>
        public string AccessLevel { get; set; }

        /// <summary>
        /// Checks login information to see if it is valid.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns>True if authorization is successful, false if unsuccessful</returns>
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext) {
            AuthenticationHeaderValue authHeaders = actionContext.Request.Headers.Authorization;
            if (authHeaders == null)
                return false;

            string authValue = authHeaders.Parameter;
            string[] login = Base64Decode(authValue);

            if (login[0] == USERNAME && login[1] == PASSWORD) {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Decode a base-64 encoded string to get a username and password, separated by ':'.
        /// </summary>
        /// <param name="base64EncodedData">String to decode.</param>
        /// <returns>Username and password, decoded.</returns>
        public static string[] Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            string decoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            string[] keyValuePairs = decoded.Split(':');
            return keyValuePairs;
        }
        /// <summary>
        /// Encode a plaintext string into a Base-64 encoded string.
        /// </summary>
        /// <param name="plainText">The plaintext string to be encoded.</param>
        /// <returns>A Base-64 encoded string.</returns>
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}