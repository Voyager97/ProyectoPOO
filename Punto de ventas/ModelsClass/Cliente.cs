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
    public class Cliente : Conexion
    {
        List<reportes_clientes> reporte;

        public List<Clientes> getClientes()
        {
            var query = from c in Cliente
                        select c;
            return query.ToList();
        }

        public void insertCliente(string id, string nombre, string apellido,
            string direccion, string telefono)
        {
            
            using (var db = new Conexion())
            {
                db.Insert(new Clientes()
                {
                    ID = id,
                    Nombre = nombre,
                    Apellido = apellido,
                    Direccion = direccion,
                    Telefono = telefono
                });
            }
        }
        public void searchCliente(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Clientes> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == "")
            {
                query = from c in Cliente select c;

            }
            else
            {
                query = from c in Cliente where c.ID.StartsWith(campo) || c.Nombre.StartsWith(campo) || c.Apellido.StartsWith(campo) select c;
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

        public void getClienteReporte(DataGridView dataGridView, int idCliente)
        {
            var query = from c in Cliente
                        join r in ReportesClientes on c.IdCliente equals r.IdCliente
                        where c.IdCliente == idCliente
                        select new
                        {
                            r.IdRegistro,
                            c.Nombre,
                            c.Apellido,
                            r.SaldoActual,
                            r.FechaActual,
                            r.UltimoPago,
                            r.FechaPago
                        };
            dataGridView.DataSource = query.ToList();
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 110;
            dataGridView.Columns[3].Width = 110;
            dataGridView.Columns[4].Width = 110;
            dataGridView.Columns[5].Width = 110;
        }

        public void updateCliente(string id, string nombre, string apellido, string direccion, string telefono, int idCliente)
        {
            string fecha = System.DateTime.Now.ToString("dd/MM/yyyy");
            Cliente.Where(c => c.IdCliente == idCliente)
                .Set(c => c.ID, id)
                .Set(c => c.Nombre, nombre)
                .Set(c => c.Apellido, apellido)
                .Set(c => c.Direccion, direccion)
                .Set(c => c.Telefono, telefono)
                .Update();
            //reporte = getReporte(idCliente);
            //ReportesClientes.Where(r => r.IdCliente == reporte[0].IdRegistro)
            //    .Set(r => r.IdCliente, reporte[0].IdCliente)
            //    .Set(r => r.SaldoActual, "$" + deudaActual)
            //    .Set(r => r.FechaActual, fecha)
            //    .Set(r => r.UltimoPago, "$" + ultimoPago)
            //    .Set(r => r.FechaPago, fecha)
            //    .Set(r => r.ID, reporte[0].ID)
            //    .Update();
        }

        public List<reportes_clientes> getReporte(int idCliente)
        {
            return ReportesClientes.Where(r => r.IdCliente == idCliente).ToList();
        }

        public void deleteCliente(int idCliente, int idRegistro)
        {
            /*Primero se elimina el dato en la tabla RegistroClientes, 
            evaluando con el dato que venga en el parametro idRegistro.
            Si el dato que esta almacenado en la columna idRegistro es igual al dato del parametro, 
            se eliminara ese registro.*/
            ReportesClientes.Where(r => r.IdRegistro == idRegistro).Delete();
            /*Despues se elimina el registro de la tabla Clientes, se evalulara el dato de la columna 
              IdCliente si es igual al dato que viene del parametro idCliente, se eliminara*/
            Cliente.Where(c => c.IdCliente == idCliente).Delete();
        }

        public void updateReporte(string deudaActual, string ultimoPago, int idCliente)
        {
            string fecha = System.DateTime.Now.ToString("dd/MM/yyyy");
            reporte = getReporte(idCliente);
            ReportesClientes.Where(r => r.IdCliente == reporte[0].IdRegistro)
                .Set(r => r.IdCliente, reporte[0].IdCliente)
                .Set(r => r.SaldoActual, "$" + deudaActual)
                .Set(r => r.FechaActual, fecha)
                .Set(r => r.UltimoPago, "$" + ultimoPago)
                .Set(r => r.FechaPago, fecha)
                .Set(r => r.ID, reporte[0].ID)
                .Update();
            //if (deudaActual == "$0.00")
            //{
            //    ReportesClientes.Where(r => r.IdRegistro == reporte[0].IdRegistro).Delete();
            //}
        }

        public void insertReporte(string deudaActual, string ultimoPago, string idCliente, string fechapa)
        {
            int pos, idCli;
            List<Clientes> cliente = getClientes();
            pos = cliente.Count;
            pos--;
            idCli = Convert.ToInt16(idCliente);
            idCli = cliente[pos].IdCliente;
            using(var db = new Conexion())
            {
                db.Insert(new reportes_clientes()
                {
                    IdCliente = idCli,
                    SaldoActual = deudaActual,
                    FechaActual = fechapa,
                    UltimoPago = "$" + ultimoPago + ".00",
                    FechaPago = fechapa,
                    ID = Convert.ToString(idCliente)
                });
            }
        }
    }
}
