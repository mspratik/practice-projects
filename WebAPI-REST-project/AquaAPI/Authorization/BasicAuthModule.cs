using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using AquaAPI.DAL;
using AquaAPI.Models;
using System.Net;

namespace AquaAPI.Authorization
{
    public class BasicAuthModule : IHttpModule
    {
        #region Public Methods

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        #endregion

        #region Private Methods

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool CheckPassword(string username, string password)
        {
            bool verified = false;

            try 
            {
                List<UserInfo> users = DbManager.GetUsers();
                if (users != null && users.Count > 0)
                {
                    foreach(UserInfo user in users)
                    {
                        if (user.Email.Equals(username) && user.Password.Equals(password))
                        {
                            verified = true;
                            break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return verified;
        }

        private static void AuthenticateUser(string credentials)
        {
            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                credentials = encoding.GetString(Convert.FromBase64String(credentials));

                int separator = credentials.IndexOf(':');
                string name = credentials.Substring(0, separator);
                string password = credentials.Substring(separator + 1);

                if (CheckPassword(name, password))
                {
                    var identity = new GenericIdentity(name);
                    SetPrincipal(new GenericPrincipal(identity, null));
                }
                else
                {
                    // Invalid username or password.
                    HttpContext.Current.Response.StatusCode = 401;
                }
            }
            catch (FormatException)
            {
                // Credentials were not formatted correctly.
                HttpContext.Current.Response.StatusCode = 401;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                HttpContext.Current.Response.Status = ex.Message.ToString();
            }
        }

        #endregion

        #region Event Handlers

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("Authenticate",
                    string.Format("Not Authorized"));
            }
        }

        #endregion
    }
}