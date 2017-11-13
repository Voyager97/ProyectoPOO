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
    public class Empleado : Conexion
    {
        public List<Empleados> getEmpleados()
        {
            var query = from d in Empleado
                        select d;
            return query.ToList();
        }

        public void insertEmpleado(string ide, string nombreE, string apellidosE,
            string direccionE, string telefonoE, string correoE)
        {
            using (var db = new Conexion())
            {
                db.Insert(new Empleados()
                {
                    IDE = ide,
                    NombreE = nombreE,
                    ApellidosE = apellidosE,
                    DireccionE = direccionE,
                    TelefonoE = telefonoE,
                    CorreoE = correoE
                });
            }
        }

        public void searchEmpleado(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Empleados> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == "")
            {
                query = from d in Empleado select d;

            }
            else
            {
                query = from d in Empleado where d.IDE.StartsWith(campo) || d.NombreE.StartsWith(campo) || d.ApellidosE.StartsWith(campo) select d;
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 150;
            dataGridView.Columns[3].Width = 150;
            dataGridView.Columns[4].Width = 150;
            dataGridView.Columns[5].Width = 150;
            dataGridView.Columns[6].Width = 150;

            dataGridView.Columns[1].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridView.Columns[5].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        public void updateEmpleado(string ide, string nombreE, string apellidosE, string direccionE, string telefonoE, string correoE, int idEmpleado)
        {
            Empleado.Where(d => d.IdEmpleado == idEmpleado)
                .Set(d => d.IDE, ide)
                .Set(d => d.NombreE, nombreE)
                .Set(d => d.ApellidosE, apellidosE)
                .Set(d => d.DireccionE, direccionE)
                .Set(d => d.TelefonoE, telefonoE)
                .Set(d => d.CorreoE, correoE)
                .Update();
        }

        public void deleteEmpleado(int idEmpleado)
        {
            /*Se elimina el registro de la tabla Empleados, se evalulara el dato de la columna 
              IdEmpleado si es igual al dato que viene del parametro idEmpleadp, se eliminara*/
            Empleado.Where(d => d.IdEmpleado == idEmpleado).Delete();
        }
    }
}
