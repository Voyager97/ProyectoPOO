using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
    public class Empleados
    {
        [PrimaryKey, Identity]
        public int IdEmpleado { set; get; }
        public string IDE { set; get; }
        public string NombreE { set; get; }
        public string ApellidosE { set; get; }
        public string DireccionE { set; get; }
        public string TelefonoE { set; get; }
        public string CorreoE { set; get; }
    }
}
