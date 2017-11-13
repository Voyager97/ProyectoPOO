using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
    public class Ventas
    {
        [PrimaryKey, Identity]
        public int IdVenta { set; get; }
        public string IDV { set; get; }
        public string ProductoVenta { set; get; }
        public string CantidadVenta { set; get; }
        public string PrecioUnitario { set; get; }
        public string ClienteVenta { set; get; }
        public string EmpleadoVenta { set; get; }
        public string Efectivo { set; get; }
        public string PagoVenta { set; get; }
        public string CambioVenta { set; get; }
        public string DeudaVenta { set; get; }
        public string FechaVenta { set; get; }
    }
}
