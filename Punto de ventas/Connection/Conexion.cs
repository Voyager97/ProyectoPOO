using LinqToDB;
using LinqToDB.Data;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Connection
{
    public class Conexion : DataConnection
    {
        public Conexion() : base("ComCel") { }
        public ITable<Clientes> Cliente { get { return GetTable<Clientes>(); } }
        public ITable<reportes_clientes> ReportesClientes { get { return GetTable<reportes_clientes>(); } }
        public ITable<Productos> Producto { get { return GetTable<Productos>(); } }
        public ITable<Empleados> Empleado { get { return GetTable<Empleados>(); } }
        public ITable<Ventas> Venta { get { return GetTable<Ventas>(); } }
    }
}
