using System;
using System.Web;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace ToolBoxPG.WebAPI.Modules
{
    public class BasicAuthenticator : IHttpModule
    {
        private const string _realm = "My Realm";

        public void Init(HttpApplication Context)
        {
            // Register event handlers
            Context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            Context.EndRequest += OnApplicationEndRequest;
        }

        private static void SetPrincipal(IPrincipal Principal)
        {
            Thread.CurrentPrincipal = Principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = Principal;
            }
        }

        // TODO: Here is where you would validate the username and password.
        private static bool CheckPassword(string Username, string Password)
        {
            return Username == "user" && Password == "password";
        }

        private static void AuthenticateUser(string Credentials)
        {
            try
            {
                Encoding Encodeur = Encoding.GetEncoding("iso-8859-1");
                Credentials = Encodeur.GetString(Convert.FromBase64String(Credentials));

                int Separator = Credentials.IndexOf(':');
                string Name = Credentials.Substring(0, Separator);
                string Password = Credentials.Substring(Separator + 1);

                if (CheckPassword(Name, Password))
                {
                    GenericIdentity Identity = new GenericIdentity(Name);
                    SetPrincipal(new GenericPrincipal(Identity, null));
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
        }

        private static void OnApplicationAuthenticateRequest(object Sender, EventArgs E)
        {
            HttpRequest Request = HttpContext.Current.Request;
            string AuthHeader = Request.Headers["Authorization"];
            if (AuthHeader == null) return;
            AuthenticationHeaderValue AuthHeaderVal = AuthenticationHeaderValue.Parse(AuthHeader);

            // RFC 2617 sec 1.2, "scheme" name is case-insensitive
            if (AuthHeaderVal.Scheme.Equals("basic",
                    StringComparison.OrdinalIgnoreCase) &&
                AuthHeaderVal.Parameter != null)
            {
                AuthenticateUser(AuthHeaderVal.Parameter);
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object Sender, EventArgs E)
        {
            HttpResponse Response = HttpContext.Current.Response;
            if (Response.StatusCode == 401)
            {
                Response.Headers.Add("WWW-Authenticate",
                    $"Basic realm=\"{_realm}\"");
            }
        }

        public void Dispose()
        {
        }
    }
}
