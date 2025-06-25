using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace proyect.Models
{
    public partial class VozDelEsteEntities
    {
        public override int SaveChanges()
        {
            // Para cada Usuario añadido
            var añadidos = ChangeTracker
                .Entries<Usuarios>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            foreach (var u in añadidos)
            {
                if (!string.IsNullOrWhiteSpace(u.Contrasena))
                    u.Contrasena = Crypto.HashPassword(u.Contrasena);
            }

            // Para cada Usuario modificado donde cambiaron la contraseña
            var modificados = ChangeTracker
                .Entries<Usuarios>()
                .Where(e => e.State == EntityState.Modified && e.Property("Contrasena").IsModified)
                .Select(e => e.Entity);

            foreach (var u in modificados)
            {
                if (!string.IsNullOrWhiteSpace(u.Contrasena))
                    u.Contrasena = Crypto.HashPassword(u.Contrasena);
            }

            return base.SaveChanges();
        }
    }
}