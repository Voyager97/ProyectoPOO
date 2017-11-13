using LinqToDB;
using Punto_de_ventas.Connection;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_ventas.ModelsClass
{
    public class Venta : Conexion
    {
        public List<Ventas> getVentas()
        {
            var query = from d in Venta
                        select d;
            return query.ToList();
        }

        public void insertVenta(string idv, string productoventa, string cantidadventa, string preciounitario, string clienteventa,
             string empleadoventa, string efectivo, string pagoventa, string cambioventa, string deudaventa, string fechaventa)
        {
            using (var db = new Conexion())
            {
                db.Insert(new Ventas()
                {
                    IDV = idv,
                    ProductoVenta = productoventa,
                    CantidadVenta = cantidadventa,
                    PrecioUnitario = preciounitario,
                    ClienteVenta = clienteventa,
                    EmpleadoVenta = empleadoventa,
                    Efectivo = efectivo,
                    PagoVenta = pagoventa,
                    CambioVenta = cambioventa,
                    DeudaVenta = deudaventa,
                    FechaVenta = fechaventa
                });
            }
        }

        public void searchVenta(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Ventas> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == "")
            {
                query = from d in Venta select d;

            }
            else
            {
                query = from d in Venta where d.IDV.StartsWith(campo) || d.ProductoVenta.StartsWith(campo) || d.ClienteVenta.StartsWith(campo) || d.EmpleadoVenta.StartsWith(campo) select d;
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 150;
            dataGridView.Columns[3].Width = 80;
            dataGridView.Columns[4].Width = 80;
            dataGridView.Columns[5].Width = 150;
            dataGridView.Columns[6].Width = 150;
            dataGridView.Columns[7].Width = 80;
            dataGridView.Columns[8].Width = 80;
            dataGridView.Columns[9].Width = 80;
            dataGridView.Columns[10].Width = 80;
            dataGridView.Columns[11].Width = 90;
            

            dataGridView.Columns[1].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[5].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[7].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[9].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[11].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        public void updateVenta(string idv, string productoventa, string cantidadventa, string preciounitario, string clienteventa, string empleadoventa,
            string efectivo, string pagoventa, string cambioventa, string deudaventa, string fechaventa, int idVenta)
        {
            Venta.Where(d => d.IdVenta == idVenta)
                .Set(d => d.IDV, idv)
                .Set(d => d.ProductoVenta, productoventa)
                .Set(d => d.CantidadVenta, cantidadventa)
                .Set(d => d.PrecioUnitario, preciounitario)
                .Set(d => d.ClienteVenta, clienteventa)
                .Set(d => d.EmpleadoVenta, empleadoventa)
                .Set(d => d.Efectivo, efectivo)
                .Set(d => d.PagoVenta, pagoventa)
                .Set(d => d.CambioVenta, cambioventa)
                .Set(d => d.DeudaVenta, deudaventa)
                .Set(d => d.FechaVenta, fechaventa)
                .Update();
        }

        public void deleteVenta(int idVenta)
        {
            /*Se elimina el registro de la tabla Ventas, se evalulara el dato de la columna 
              IdVenta si es igual al dato que viene del parametro idVenta, se eliminara*/
            Venta.Where(d => d.IdVenta == idVenta).Delete();
        }
    }
}
