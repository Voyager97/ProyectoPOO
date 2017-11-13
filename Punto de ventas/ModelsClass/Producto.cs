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
    public class Producto : Conexion
    {
        public List<Productos> getProductos()
        {
            var query = from d in Producto
                        select d;
            return query.ToList();
        }

        public void insertProducto(string idp, string producto, string cantidad,
            string preciocom, string precioven)
        {
            using (var db = new Conexion())
            {
                db.Insert(new Productos()
                {
                    IDP = idp,
                    Producto = producto,
                    Cantidad = cantidad,
                    PrecioCom = preciocom,
                    PrecioVen = precioven
                });
            }
        }

        public void searchProducto(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Productos> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == "")
            {
                query = from d in Producto select d;

            }
            else
            {
                query = from d in Producto where d.IDP.StartsWith(campo) || d.Producto.StartsWith(campo) || d.Cantidad.StartsWith(campo) select d;
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 150;
            dataGridView.Columns[3].Width = 150;
            dataGridView.Columns[4].Width = 150;
            dataGridView.Columns[5].Width = 150;

            dataGridView.Columns[1].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[5].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        public void updateProducto(string idp, string producto, string cantidad, string preciocom, string precioven, int idProducto)
        {
            Producto.Where(d => d.IdProducto == idProducto)
                .Set(d => d.IDP, idp)
                .Set(d => d.Producto, producto)
                .Set(d => d.Cantidad, cantidad)
                .Set(d => d.PrecioCom, preciocom)
                .Set(d => d.PrecioVen, precioven)
                .Update();
        }

        public void deleteProducto(int idProducto)
        {
            /*Se elimina el registro de la tabla Productos, se evalulara el dato de la columna 
              IdProducto si es igual al dato que viene del parametro idProducto, se eliminara*/
            Producto.Where(d => d.IdProducto == idProducto).Delete();
        }

        public void actualizarProducto(int idProducto, int cantidadFin)
        {
            string cantidadFina;
            idProducto = Convert.ToInt16(idProducto);
            cantidadFina = Convert.ToString(cantidadFin);
            Producto.Where(d => d.IdProducto == idProducto)
                .Set(d => d.Cantidad, cantidadFina)
                .Update();
        }
    }
}
