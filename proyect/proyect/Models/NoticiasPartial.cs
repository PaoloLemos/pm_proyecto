using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace proyect.Models
{
    public partial class Noticias
    {
        [NotMapped]
        public HttpPostedFileBase ImagenFile { get; set; }
    }
}