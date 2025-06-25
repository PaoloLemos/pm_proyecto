using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyect.Filters
{
    // Permite usar [RolAuthorize("admin","editor")] sobre controladores o acciones
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RolAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _rolesPermitidos;

        // Aquí defines qué roles están permitidos
        public RolAuthorizeAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        // Se ejecuta antes de la acción: devuelve true si el rol en Session está en la lista
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var rol = httpContext.Session["UsuarioRol"] as string;
            if (rol == null) return false;
            // comparamos en minúsculas para que case con tus valores
            return _rolesPermitidos.Contains(rol.ToLower());
        }

        // Si no está autorizado, redirige al login
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary {
            { "controller", "LogIn" },
            { "action",     "SolicitarDatosView" }
                });
        }
    }
}
