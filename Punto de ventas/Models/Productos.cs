using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
    public class Productos
    {
        [PrimaryKey, Identity]
        public int IdProducto { set; get; }
        public string IDP { set; get; }
        public string Producto { set; get; }
        public string Cantidad { set; get; }
        public string PrecioCom { set; get; }
        public string PrecioVen { set; get; }
    }
}
