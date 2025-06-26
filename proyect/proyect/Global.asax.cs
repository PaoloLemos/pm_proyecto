using proyect.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace proyect
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BundleTable.EnableOptimizations = false;
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie?.Value == null) return;

            FormsAuthenticationTicket ticket;
            try
            {
                ticket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                FormsAuthentication.SignOut();
                return;
            }

            // Ahora sí Session está disponible
            var sesionOk = (Session != null && Session["UsuarioLogueado"] as bool? == true);
            if (!sesionOk)
            {
                FormsAuthentication.SignOut();
                return;
            }

            // Reconstruir CustomPrincipal…
            var permisos = ticket.UserData
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Trim());
            var principal = new CustomPrincipal(ticket.Name, permisos);
            HttpContext.Current.User = principal;
            System.Threading.Thread.CurrentPrincipal = principal;
        }

    }
}
