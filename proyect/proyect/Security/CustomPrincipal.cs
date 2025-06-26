using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;


namespace proyect.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        private readonly HashSet<string> _permisos;

        public CustomPrincipal(string username, IEnumerable<string> permisos)
        {
            Identity = new GenericIdentity(username);
            _permisos = new HashSet<string>(permisos);
        }

        public bool IsInRole(string role) => false; // no usamos roles

        public bool HasPermission(string permiso) =>
            _permisos.Contains(permiso);
    }
}