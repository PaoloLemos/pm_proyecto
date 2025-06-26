using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace proyect.Security
{
    public class PermisoAttribute : AuthorizeAttribute
    {
        public string NombrePermiso { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!(httpContext.User is CustomPrincipal user))
                return false;
            return user.HasPermission(NombrePermiso);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "controller", "Home" },
                    { "action",     "Index" }
                });
        }
    }
}