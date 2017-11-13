using Punto_de_ventas.Connection;
using Punto_de_ventas.Models;
using Punto_de_ventas.ModelsClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_ventas
{
    public partial class Form1 : Form
    {
        TextBoxEvent evento = new TextBoxEvent();
        Cliente cliente = new Cliente();
        Producto producto = new Producto();
        Empleado empleado = new Empleado();
        Venta venta = new Venta();
        Cliente reporte = new Cliente();
        List<Clientes> numCliente = new List<Clientes>();
        List<Productos> numProducto = new List<Productos>();
        List<Empleados> numEmpleado = new List<Empleados>();
        List<Ventas> numVenta = new List<Ventas>();
        private string accion = "insert", deudaActual, pago;
        private int pag = 4, pageSize = 2, maxReg, pageCount, numPagi = 1, idCliente = 0, IdRegistro = 0;
        private int idProducto = 0, idEmpleado = 0, idVenta = 0;
        private string precioT, efectivoT, fechaP = System.DateTime.Now.ToString("dd/MM/yyyy");
        private decimal precioV1, totalV1, cantidadV1, cambioV1, efectivoV1, deudaV1, deudaActualV1;
        public Form1()
        {
            InitializeComponent();
            label_FechaVenta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            venta.searchVenta(dataGridView_Venta, "", 1, pageSize);
            /*CODIGO DEL BOTON CLIENTES*/
            #region
            radioButton_IngresarCliente.Checked = true;
            radioButton_IngresarCliente.ForeColor = Color.DarkCyan;
            cliente.searchCliente(dataGridView_Cliente, "", 1, pageSize);
            cliente.getClienteReporte(dataGridView_ClienteReporte, idCliente);
            #endregion          
            new Conexion();
            /*CODIGO DEL BOTON PRODUCTOS*/
            producto.searchProducto(dataGridView_Producto, "", 1, pageSize);
            /*CODIGO DEL BOTON EMPLEADOS*/
            empleado.searchEmpleado(dataGridView_Empleado, "", 1, pageSize);
            /*CODIGO DEL BOTON VENTAS*/
            #region
            button_Ventas.Enabled = false;
            radioButton_VentaCon.Checked = true;
            radioButton_VentaCon.ForeColor = Color.SteelBlue;
            venta.searchVenta(dataGridView_Venta, "", 1, pageSize);
            producto.searchProducto(dataGridView_ProdVen, textBox_BuscarVenta.Text, 1, pageSize);
            cliente.searchCliente(dataGridView_ClieVen, textBox_BuscarVenta.Text, 1, pageSize);
            label_FechaVenta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            #endregion  
        }

        /*########################################################################################################################################
        ##########################                          CODIGO DEL PAGINADOR                                ##################################
        ########################################################################################################################################*/
        #region
        private void cargarDatos()
        {
            switch (pag)
            {
                case 1:
                    numCliente = cliente.getClientes();
                    cliente.searchCliente(dataGridView_Cliente, "", 1, pageSize);
                    maxReg = numCliente.Count();
                    break;

                case 2:
                    numProducto = producto.getProductos();
                    producto.searchProducto(dataGridView_Producto, "", 1, pageSize);
                    maxReg = numProducto.Count();
                    break;

                case 3:
                    numEmpleado = empleado.getEmpleados();
                    empleado.searchEmpleado(dataGridView_Empleado, "", 1, pageSize);
                    maxReg = numEmpleado.Count();
                    break;

                case 4:
                    numVenta = venta.getVentas();
                    venta.searchVenta(dataGridView_Venta, "", 1, pageSize);
                    maxReg = numVenta.Count();
                    break;
            }

            pageCount = (maxReg / pageSize);
            //Ajuste el numero de la pagina si la ultima pagina contiene una parte de la pagina
            if ((maxReg % pageSize) > 0)
            {
                pageCount += 1;
            }
            label_PaginasCliente.Text = "Pagina " + "1" + "/" + pageCount.ToString();
            label_PaginasPDT.Text = "Pagina" + "1" + "/" + pageCount.ToString();
            label_PaginasEmp.Text = "Pagina" + "1" + "/" + pageCount.ToString();
            label_PaginasVenta.Text = "Pagina" + "1" + "/" + pageCount.ToString();
        }
        #endregion

        /*########################################################################################################################################
        ##########################                    CODIGO DE LA SECCION CLIENTES                             ##################################
        ########################################################################################################################################*/
        #region
        private void button_Clientes_Click(object sender, EventArgs e)
        {
            pag = 1;
            restablecerCliente();
            //Se llama a la pagina 1 del tabControl1
            tabControl1.SelectedIndex = 1;
            button_Clientes.Enabled = false;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Empleados.Enabled = true;
        }

        private void radioButton_IngresarCliente_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_IngresarCliente.ForeColor = Color.DarkCyan;
            radioButton_PagosDeudas.ForeColor = Color.Black;
            textBox_Id.ReadOnly = false;
            textBox_Nombre.ReadOnly = false;
            textBox_Apellido.ReadOnly = false;
            textBox_Direccion.ReadOnly = false;
            textBox_Telefono.ReadOnly = false;
            textBox_PagoscCliente.ReadOnly = true;
            label_PagoCliente.Text = "Pagos de deudas";
            label_PagoCliente.ForeColor = Color.DarkCyan;
        }


        private void radioButton_PagosDeudas_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_PagosDeudas.ForeColor = Color.DarkCyan;
            radioButton_IngresarCliente.ForeColor = Color.Black;
            textBox_Id.ReadOnly = true;
            textBox_Nombre.ReadOnly = true;
            textBox_Apellido.ReadOnly = true;
            textBox_Direccion.ReadOnly = true;
            textBox_Telefono.ReadOnly = true;
            textBox_PagoscCliente.ReadOnly = false;
        }


        private void textBox_Nombre_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Nombre.Text == "")
            {
                label_Nombre.ForeColor = Color.Black;
            }
            else
            {
                label_Nombre.Text = "Nombre";
                label_Nombre.ForeColor = Color.Green;
            }
        }

        private void textBox_Nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.textKeyPress(e);
        }

        private void textBox_Apellido_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Apellido.Text == "")
            {
                label_Apellido.ForeColor = Color.Black;
            }
            else
            {
                label_Apellido.Text = "Apellido";
                label_Apellido.ForeColor = Color.Green;
            }
        }

        private void textBox_Apellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.textKeyPress(e);
        }

        private void textBox_Direccion_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Direccion.Text == "")
            {
                label_Direccion.ForeColor = Color.Black;
            }
            else
            {
                label_Direccion.Text = "Direccion";
                label_Direccion.ForeColor = Color.Green;
            }
        }

        private void textBox_Direccion_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox_Telefono_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Telefono.Text == "")
            {
                label_Telefono.ForeColor = Color.Black;
            }
            else
            {
                label_Telefono.Text = "Telefono";
                label_Telefono.ForeColor = Color.Green;
            }
        }

        private void textBox_Telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_PagoscCliente_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView_ClienteReporte.CurrentRow == null)
            {
                label_PagoCliente.Text = "Seleccione el cliente";
                label_PagoCliente.ForeColor = Color.Red;
                textBox_PagoscCliente.Text = "";
            }
            else
            {
                if (textBox_PagoscCliente.Text != "")
                {
                    label_PagoCliente.Text = "Pagos de deudas";
                    label_PagoCliente.ForeColor = Color.Black;
                    String deuda1;
                    Decimal deuda2, deuda3, deudaTotal;
                    deuda1 = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[3].Value);
                    deuda1 = deuda1.Replace("$", "");
                    deuda2 = Convert.ToDecimal(deuda1);

                    deuda3 = Convert.ToDecimal(textBox_PagoscCliente.Text);

                    deudaTotal = deuda2 - deuda3;
                    deudaActual = Convert.ToString(deudaTotal);
                    pago = textBox_PagoscCliente.Text;
                }
            }
        }

        private void textBox_PagoscCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberDecimalKeyPress(textBox_PagoscCliente, e);
        }

        private void textBox_Id_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Id.Text == "")
            {
                label_Id.ForeColor = Color.Black;
            }
            else
            {
                label_Id.Text = "ID";
                label_Id.ForeColor = Color.Green;
            }
        }

        private void textBox_Id_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void button_GuardarCliente_Click(object sender, EventArgs e)
        {
            if (radioButton_IngresarCliente.Checked)
            {
                guardarCliente();
            }
            else
            {
                guardarPago();
            }
        }

        private void guardarCliente()
        {
            if (textBox_Id.Text == "")
            {
                label_Id.Text = "Ingrese el ID";
                label_Id.ForeColor = Color.Red;
                textBox_Id.Focus();
            }
            else
            {
                if (textBox_Nombre.Text == "")
                {
                    label_Nombre.Text = "Ingrese el nombre completo";
                    label_Nombre.ForeColor = Color.Red;
                    textBox_Nombre.Focus();
                }
                else
                {
                    if (textBox_Apellido.Text == "")
                    {
                        label_Apellido.Text = "Ingrese el apellido";
                        label_Apellido.ForeColor = Color.Red;
                        textBox_Apellido.Focus();
                    }
                    else
                    {
                        if (textBox_Direccion.Text == "")
                        {
                            label_Direccion.Text = "Ingrese la dirección";
                            label_Direccion.ForeColor = Color.Red;
                            textBox_Direccion.Focus();
                        }
                        else
                        {
                            if (textBox_Telefono.Text == "")
                            {
                                label_Telefono.Text = "Ingrese el telefono";
                                label_Telefono.ForeColor = Color.Red;
                                textBox_Telefono.Focus();
                            }
                            else
                            {
                                string ID = textBox_Id.Text;
                                string Nombre = textBox_Nombre.Text;
                                string Apellido = textBox_Apellido.Text;
                                string Direccion = textBox_Direccion.Text;
                                string Telefono = textBox_Telefono.Text;
                                if (accion == "insert")
                                {
                                    cliente.insertCliente(ID, Nombre, Apellido, Direccion, Telefono);
                                }
                                if (accion == "update")
                                {
                                    cliente.updateCliente(ID, Nombre, Apellido, Direccion, Telefono, idCliente);
                                }
                                restablecerCliente();
                            }
                        }
                    }
                }
            }
        }

        private void guardarPago()
        {
            if (textBox_PagoscCliente.Text == "")
            {
                label_PagoCliente.Text = "Ingrese el pago";
                label_PagoCliente.ForeColor = Color.Red;
                textBox_PagoscCliente.Focus();
            }
            else
            {
                cliente.updateReporte(deudaActual, pago, idCliente);
                restablecerCliente();
            }
        }

        private void button_PrimerosClientes_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            cliente.searchCliente(dataGridView_Cliente, "", 1, pageSize);
        }

        private void button_AnteriosClientes_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                cliente.searchCliente(dataGridView_Cliente, "", numPagi, pageSize);
            }
        }
        
        private void button_SiguientesClientes_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                cliente.searchCliente(dataGridView_Cliente, "", numPagi, pageSize);
            }
        }

        private void button_UltimosClientes_Click(object sender, EventArgs e)
        {
            numPagi = pageCount;
            label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            cliente.searchCliente(dataGridView_Cliente, "", pageCount, pageSize);
        }

        private void restablecerCliente()
        {
            numPagi = 1;
            cargarDatos();
            textBox_Id.Text = "";
            textBox_Nombre.Text = "";
            textBox_Apellido.Text = "";
            textBox_Direccion.Text = "";
            textBox_Telefono.Text = "";
            textBox_PagoscCliente.Text = "";
            textBox_Id.Focus();
            textBox_BuscarCliente.Text = "";
            label_Id.ForeColor = Color.LightSlateGray;
            label_Nombre.ForeColor = Color.LightSlateGray;
            label_Apellido.ForeColor = Color.LightSlateGray;
            label_Direccion.ForeColor = Color.LightSlateGray;
            label_Telefono.ForeColor = Color.LightSlateGray;
            label_PagoCliente.ForeColor = Color.LightSlateGray;
            label_PagoCliente.Text = "Pagos de deudas ";
            radioButton_IngresarCliente.Checked = true;
            radioButton_IngresarCliente.ForeColor = Color.DarkCyan;
            accion = "insert";
            idCliente = 0;
            IdRegistro = 0;
            label_NombreRB.Text = "Nombre";
            label_ApellidoRB.Text = "Apellido";
            label_ClienteDA.Text = "$0.00";
            label_ClienteUP.Text = "$0.00";
            label_FechaPG.Text = "Fecha";
            cliente.getClienteReporte(dataGridView_ClienteReporte, idCliente);
        }

        private void button_Cancelar_Click(object sender, EventArgs e)
        {
            restablecerCliente();
        }

        private void dataGridView_Cliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Cliente.Rows.Count != 0)
            {
                dataGridViewCliente();
            }
        }

        private void dataGridView_Cliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Cliente.Rows.Count != 0)
            {
                dataGridViewCliente();
            }
        }

        private void dataGridViewCliente()
        {
            accion = "update";
            idCliente = Convert.ToInt16(dataGridView_Cliente.CurrentRow.Cells[0].Value);
            textBox_Id.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[1].Value);
            textBox_Nombre.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[2].Value);
            textBox_Apellido.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[3].Value);
            textBox_Direccion.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[4].Value);
            textBox_Telefono.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[5].Value);
            cliente.getClienteReporte(dataGridView_ClienteReporte, idCliente);
            /*********************************************************
            * NO DAR CLIC EN EL PRIMER REGISTRO, SALE UN ERROR ABAJO *
            *                EN LA SIGUIENTE LINEA                   *
            *********************************************************/
            IdRegistro = Convert.ToInt16(dataGridView_ClienteReporte.CurrentRow.Cells[0].Value);
            label_NombreRB.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[1].Value);
            label_ApellidoRB.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[2].Value);
            label_ClienteDA.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[3].Value);
            label_ClienteUP.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[5].Value);
            label_FechaPG.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[4].Value);
        }

        private void button_EliminarClientes_Click(object sender, EventArgs e)
        {
            if (idCliente > 0)
            {
                if (MessageBox.Show("¿Esta seguro de eliminar este registro?", "Eliminar registro", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cliente.deleteCliente(idCliente, IdRegistro);
                    restablecerCliente();
                }
            }
        }

        private void textBox_BuscarCliente_TextChanged(object sender, EventArgs e)
        {
            cliente.searchCliente(dataGridView_Cliente, textBox_BuscarCliente.Text, 1, pageSize);
        }

        private void button_ImprCliente_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(groupBox_ReciboCliente.Width, groupBox_ReciboCliente.Height);
            groupBox_ReciboCliente.DrawToBitmap(bm, new Rectangle(0, 0, groupBox_ReciboCliente.Width, groupBox_ReciboCliente.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }
        #endregion

        /*########################################################################################################################################
        ##########################                    CODIGO DE LA SECCION PRODUCTOS                            ##################################
        ########################################################################################################################################*/
        #region
        private void button_Productos_Click(object sender, EventArgs e)
        {
            pag = 2;
            restablecerProducto();
            //Se llama a la pagina 2 del tabControl1
            tabControl1.SelectedIndex = 2;
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = false;
            button_Empleados.Enabled = true;
        }

        private void textBox_CantidadProd_TextChanged(object sender, EventArgs e)
        {
            if (textBox_CantidadProd.Text == "")
            {
                label_CantidadProd.ForeColor = Color.Black;
            }
            else
            {
                label_CantidadProd.Text = "Cantidad";
                label_CantidadProd.ForeColor = Color.Green;
            }
        }

        private void textBox_PrecioCom_TextChanged(object sender, EventArgs e)
        {
            if (textBox_PrecioCom.Text == "")
            {
                label_PrecioCom.ForeColor = Color.Black;
            }
            else
            {
                label_PrecioCom.Text = "Precio de Compra";
                label_PrecioCom.ForeColor = Color.Green;
            }
        }

        private void textBox_PrecioVen_TextChanged(object sender, EventArgs e)
        {
            if (textBox_PrecioVen.Text == "")
            {
                label_PrecioVen.ForeColor = Color.Black;
            }
            else
            {
                label_PrecioVen.Text = "Precio de Venta";
                label_PrecioVen.ForeColor = Color.Green;
            }
        }

        private void textBox_IdProd_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox_IdProd.Text == "")
            {
                label_IdProd.ForeColor = Color.Black;
            }
            else
            {
                label_IdProd.Text = "ID";
                label_IdProd.ForeColor = Color.Green;
            }
        }

        private void textBox_Producto_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Producto.Text == "")
            {
                label_Producto.ForeColor = Color.Black;
            }
            else
            {
                label_Producto.Text = "Producto";
                label_Producto.ForeColor = Color.Green;
            }
        }

        private void textBox_IdProd_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_CantidadProd_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_PrecioCom_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberDecimalKeyPress(textBox_PrecioCom, e);
        }

        private void textBox_PrecioVen_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberDecimalKeyPress(textBox_PrecioVen, e);
        }

        private void textBox_Producto_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void guardarProducto()
        {
            if (textBox_IdProd.Text == "")
            {
                label_IdProd.Text = "Ingrese el ID";
                label_IdProd.ForeColor = Color.Red;
                textBox_IdProd.Focus();
            }
            else
            {
                if (textBox_Producto.Text == "")
                {
                    label_Producto.Text = "Ingrese el producto";
                    label_Producto.ForeColor = Color.Red;
                    textBox_Producto.Focus();
                }
                else
                {
                    if (textBox_CantidadProd.Text == "")
                    {
                        label_CantidadProd.Text = "Ingrese la cantidad";
                        label_CantidadProd.ForeColor = Color.Red;
                        textBox_CantidadProd.Focus();
                    }
                    else
                    {
                        if (textBox_PrecioCom.Text == "")
                        {
                            label_PrecioCom.Text = "Ingrese el precio de compra";
                            label_PrecioCom.ForeColor = Color.Red;
                            textBox_PrecioCom.Focus();
                        }
                        else
                        {
                            if (textBox_PrecioVen.Text == "")
                            {
                                label_PrecioVen.Text = "Ingrese el precio de venta";
                                label_PrecioVen.ForeColor = Color.Red;
                                textBox_PrecioVen.Focus();
                            }
                            else
                            {
                                string IDP = textBox_IdProd.Text;
                                string Producto = textBox_Producto.Text;
                                string Cantidad = textBox_CantidadProd.Text;
                                string PrecioCom = textBox_PrecioCom.Text;
                                string PrecioVen = textBox_PrecioVen.Text;
                                if (accion == "insert")
                                {
                                    producto.insertProducto(IDP, Producto, Cantidad, PrecioCom, PrecioVen);
                                }
                                if (accion == "update")
                                {
                                    producto.updateProducto(IDP, Producto, Cantidad, PrecioCom, PrecioVen, idProducto);
                                }
                                restablecerProducto();
                            }
                        }
                    }
                }
            }
        }

        private void restablecerProducto()
        {
            numPagi = 1;
            cargarDatos();
            textBox_IdProd.Text = "";
            textBox_Producto.Text = "";
            textBox_CantidadProd.Text = "";
            textBox_PrecioCom.Text = "";
            textBox_PrecioVen.Text = "";
            textBox_IdProd.Focus();
            textBox_BuscarProducto.Text = "";
            label_IdProd.ForeColor = Color.LightSlateGray;
            label_Producto.ForeColor = Color.LightSlateGray;
            label_CantidadProd.ForeColor = Color.LightSlateGray;
            label_PrecioCom.ForeColor = Color.LightSlateGray;
            label_PrecioVen.ForeColor = Color.LightSlateGray;
            accion = "insert";
            idProducto = 0;
        }

        private void button_GuardarPDT_Click_1(object sender, EventArgs e)
        {
            guardarProducto();
        }

        private void button_EliminarPDT_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro de eliminar este registro?", "Eliminar registro", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                producto.deleteProducto(idProducto);
                restablecerProducto();
            }
        }

        private void dataGridView_Producto_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (dataGridView_Producto.Rows.Count != 0)
            {
                dataGridViewProducto();
            }
        }



        private void dataGridView_Producto_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Producto.Rows.Count != 0)
            {
                dataGridViewProducto();
            }
        }

        private void button_CancelarPDT_Click_1(object sender, EventArgs e)
        {
            restablecerProducto();
        }

        private void textBox_BuscarProducto_TextChanged_1(object sender, EventArgs e)
        {
            producto.searchProducto(dataGridView_Producto, textBox_BuscarProducto.Text, 1, pageSize);
        }

        private void dataGridViewProducto()
        {
            accion = "update";
            idProducto = Convert.ToInt16(dataGridView_Producto.CurrentRow.Cells[0].Value);
            textBox_IdProd.Text = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[1].Value);
            textBox_Producto.Text = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[2].Value);
            textBox_CantidadProd.Text = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[3].Value);
            textBox_PrecioCom.Text = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[4].Value);
            textBox_PrecioVen.Text = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[5].Value);
        }

        private void button_UltimaPDT_Click_1(object sender, EventArgs e)
        {
            numPagi = pageCount;
            label_PaginasPDT.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            producto.searchProducto(dataGridView_Producto, "", pageCount, pageSize);
        }

        private void button_AnteriorPDT_Click_1(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label_PaginasPDT.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                producto.searchProducto(dataGridView_Producto, "", numPagi, pageSize);
            }
        }

        private void button_SiguientePDT_Click_1(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label_PaginasPDT.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                producto.searchProducto(dataGridView_Producto, "", numPagi, pageSize);
            }
        }

        private void button_PrimeroPDT_Click_1(object sender, EventArgs e)
        {
            numPagi = 1;
            label_PaginasPDT.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            producto.searchProducto(dataGridView_Producto, "", 1, pageSize);
        }
        #endregion

        /*########################################################################################################################################
        ##########################                    CODIGO DE LA SECCION EMPLEADOS                            ##################################
        ########################################################################################################################################*/
        #region  
        private void button_Empleados_Click(object sender, EventArgs e)
        {
            pag = 3;
            restablecerEmpleado();
            //Se llama a la pagina 3 del tabControl1
            tabControl1.SelectedIndex = 3;
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Empleados.Enabled = false;
        }

        private void textBox_NombreEmp_TextChanged(object sender, EventArgs e)
        {
            if (textBox_NombreEmp.Text == "")
            {
                label_NombreEmp.ForeColor = Color.Black;
            }
            else
            {
                label_NombreEmp.Text = "Nombre";
                label_NombreEmp.ForeColor = Color.Green;
            }
        }

        private void textBox_IdEmp_TextChanged(object sender, EventArgs e)
        {
            if (textBox_IdEmp.Text == "")
            {
                label_IdEmp.ForeColor = Color.Black;
            }
            else
            {
                label_IdEmp.Text = "ID";
                label_IdEmp.ForeColor = Color.Green;
            }
        }

        private void textBox_ApellidosEmp_TextChanged(object sender, EventArgs e)
        {
            if (textBox_ApellidosEmp.Text == "")
            {
                label_ApellidosEmp.ForeColor = Color.Black;
            }
            else
            {
                label_ApellidosEmp.Text = "Apellidos";
                label_ApellidosEmp.ForeColor = Color.Green;
            }
        }

        private void textBox_TelefonoEmp_TextChanged(object sender, EventArgs e)
        {
            if (textBox_TelefonoEmp.Text == "")
            {
                label_TelefonoEmp.ForeColor = Color.Black;
            }
            else
            {
                label_TelefonoEmp.Text = "Telefono";
                label_TelefonoEmp.ForeColor = Color.Green;
            }
        }

        private void textBox_CorreoEmp_TextChanged(object sender, EventArgs e)
        {
            if (textBox_CorreoEmp.Text == "")
            {
                label_CorreoEmp.ForeColor = Color.Black;
            }
            else
            {
                label_CorreoEmp.Text = "Correo";
                label_CorreoEmp.ForeColor = Color.Green;
            }
        }

        private void textBox_IdEmp_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_NombreEmp_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox_ApellidosEmp_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox_DireccionEmp_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox_TelefonoEmp_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_CorreoEmp_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void guardarEmpleado()
        {
            if (textBox_IdEmp.Text == "")
            {
                label_IdEmp.Text = "Ingrese el ID";
                label_IdEmp.ForeColor = Color.Red;
                textBox_IdEmp.Focus();
            }
            else
            {
                if (textBox_NombreEmp.Text == "")
                {
                    label_NombreEmp.Text = "Ingrese el nombre";
                    label_NombreEmp.ForeColor = Color.Red;
                    textBox_NombreEmp.Focus();
                }
                else
                {
                    if (textBox_ApellidosEmp.Text == "")
                    {
                        label_ApellidosEmp.Text = "Ingrese los apellidos";
                        label_ApellidosEmp.ForeColor = Color.Red;
                        textBox_ApellidosEmp.Focus();
                    }
                    else
                    {
                        if (textBox_DireccionEmp.Text == "")
                        {
                            label_DireccionEmp.Text = "Ingrese la direccion";
                            label_DireccionEmp.ForeColor = Color.Red;
                            textBox_DireccionEmp.Focus();
                        }
                        else
                        {
                            if (textBox_TelefonoEmp.Text == "")
                            {
                                label_TelefonoEmp.Text = "Ingrese el telefono";
                                label_TelefonoEmp.ForeColor = Color.Red;
                                textBox_TelefonoEmp.Focus();
                            }
                            else
                            {
                                if (textBox_CorreoEmp.Text == "")
                                {
                                    label_CorreoEmp.Text = "Ingrese el correo";
                                    label_CorreoEmp.ForeColor = Color.Red;
                                    textBox_CorreoEmp.Focus();
                                }
                                else
                                {
                                    string IDE = textBox_IdEmp.Text;
                                    string NombreE = textBox_NombreEmp.Text;
                                    string ApellidosE = textBox_ApellidosEmp.Text;
                                    string DireccionE = textBox_DireccionEmp.Text;
                                    string TelefonoE = textBox_TelefonoEmp.Text;
                                    string CorreoE = textBox_CorreoEmp.Text;
                                    if (accion == "insert")
                                    {
                                        empleado.insertEmpleado(IDE, NombreE, ApellidosE, DireccionE, TelefonoE, CorreoE);
                                    }
                                    if (accion == "update")
                                    {
                                        empleado.updateEmpleado(IDE, NombreE, ApellidosE, DireccionE, TelefonoE, CorreoE, idEmpleado);
                                    }
                                }
                                restablecerEmpleado();
                            }
                        }
                    }
                }
            }
        }

        private void restablecerEmpleado()
        {
            numPagi = 1;
            cargarDatos();
            textBox_IdEmp.Text = "";
            textBox_NombreEmp.Text = "";
            textBox_ApellidosEmp.Text = "";
            textBox_DireccionEmp.Text = "";
            textBox_TelefonoEmp.Text = "";
            textBox_CorreoEmp.Text = "";
            textBox_IdEmp.Focus();
            textBox_BuscarEmpleado.Text = "";
            label_IdEmp.ForeColor = Color.LightSlateGray;
            label_NombreEmp.ForeColor = Color.LightSlateGray;
            label_ApellidosEmp.ForeColor = Color.LightSlateGray;
            label_DireccionEmp.ForeColor = Color.LightSlateGray;
            label_TelefonoEmp.ForeColor = Color.LightSlateGray;
            label_CorreoEmp.ForeColor = Color.LightSlateGray;
            accion = "insert";
            idEmpleado = 0;
        }

        private void button_GuardarEmp_Click(object sender, EventArgs e)
        {
            guardarEmpleado();
        }

        private void button_EliminarEmp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro de eliminar este registro?", "Eliminar registro", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                empleado.deleteEmpleado(idEmpleado);
                restablecerEmpleado();
            }
        }

        private void button_CancelarEmp_Click(object sender, EventArgs e)
        {
            restablecerEmpleado();
        }

        private void dataGridViewEmpleado()
        {
            accion = "update";
            idEmpleado = Convert.ToInt16(dataGridView_Empleado.CurrentRow.Cells[0].Value);
            textBox_IdEmp.Text = Convert.ToString(dataGridView_Empleado.CurrentRow.Cells[1].Value);
            textBox_NombreEmp.Text = Convert.ToString(dataGridView_Empleado.CurrentRow.Cells[2].Value);
            textBox_ApellidosEmp.Text = Convert.ToString(dataGridView_Empleado.CurrentRow.Cells[3].Value);
            textBox_DireccionEmp.Text = Convert.ToString(dataGridView_Empleado.CurrentRow.Cells[4].Value);
            textBox_TelefonoEmp.Text = Convert.ToString(dataGridView_Empleado.CurrentRow.Cells[5].Value);
            textBox_CorreoEmp.Text = Convert.ToString(dataGridView_Empleado.CurrentRow.Cells[6].Value);
        }

        private void button_PrimerEmp_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            label_PaginasEmp.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            empleado.searchEmpleado(dataGridView_Empleado, "", 1, pageSize);
        }

        private void button_AnteriorEmp_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label_PaginasEmp.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                empleado.searchEmpleado(dataGridView_Empleado, "", numPagi, pageSize);
            }
        }

        private void button_SiguienteEmp_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label_PaginasEmp.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                empleado.searchEmpleado(dataGridView_Empleado, "", numPagi, pageSize);
            }
        }

        private void button_UltimaEmp_Click(object sender, EventArgs e)
        {
            numPagi = pageCount;
            label_PaginasEmp.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            empleado.searchEmpleado(dataGridView_Empleado, "", pageCount, pageSize);
        }

        private void dataGridView_Empleado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewEmpleado();
        }

        private void dataGridView_Empleado_KeyUp(object sender, KeyEventArgs e)
        {
            dataGridViewEmpleado();
        }

        private void textBox_BuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            empleado.searchEmpleado(dataGridView_Empleado, textBox_BuscarEmpleado.Text, 1, pageSize);
        }

        #endregion

        /*########################################################################################################################################
        ##########################                     CODIGO DE LA SECCION VENTAS                              ##################################
        ########################################################################################################################################*/
        #region
        private void button_Ventas_Click(object sender, EventArgs e)
        {
            radioButton_VentaCon.Checked = true;
            pag = 4;
            restablecerVenta();
            //Se llama a la pagina 0 del tabControl1
            tabControl1.SelectedIndex = 0;
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = false;
            button_Productos.Enabled = true;
            button_Empleados.Enabled = true;
            label_FechaVenta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void radioButton_VentaCon_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_VentaCon.ForeColor = Color.SteelBlue;
            radioButton_VentaCliFre.ForeColor = Color.Black;
            textBox_IdVenta.ReadOnly = false;
            textBox_VenProd.ReadOnly = true;
            textBox_VenCant.ReadOnly = false;
            textBox_VenPreUni.ReadOnly = true;
            textBox_VenClie.ReadOnly = true;
            textBox_VenEmp.ReadOnly = true;
            textBox_VenEfec.ReadOnly = false;
        }

        private void radioButton_VentaCliFre_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_VentaCon.ForeColor = Color.Black;
            radioButton_VentaCliFre.ForeColor = Color.SteelBlue;
            textBox_IdVenta.ReadOnly = false;
            textBox_VenProd.ReadOnly = true;
            textBox_VenCant.ReadOnly = false;
            textBox_VenPreUni.ReadOnly = true;
            textBox_VenClie.ReadOnly = true;
            textBox_VenEmp.ReadOnly = true;
            textBox_VenEfec.ReadOnly = false;
        }

        private void textBox_IdVenta_TextChanged(object sender, EventArgs e)
        {
            if (textBox_IdVenta.Text == "")
            {
                label_IdVenta.ForeColor = Color.Black;
            }
            else
            {
                label_IdVenta.Text = "ID";
                label_IdVenta.ForeColor = Color.Green;
            }
        }

        private void textBox_VenCant_TextChanged(object sender, EventArgs e)
        {
            if (textBox_VenCant.Text == "")
            {
                label_VenCant.ForeColor = Color.Black;
            }
            else
            {
                label_VenCant.Text = "Cantidad";
                label_VenCant.ForeColor = Color.Green;
            }
        }

        private void textBox_VenEfec_TextChanged(object sender, EventArgs e)
        {
            if (textBox_VenEfec.Text == "")
            {
                label_VenEfec.ForeColor = Color.Black;
            }
            else
            {
                label_VenEfec.Text = "Efectivo";
                label_VenEfec.ForeColor = Color.Green;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox_VenPreUni_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_IdVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_VenCant_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void textBox_VenEfec_KeyPress(object sender, KeyPressEventArgs e)
        {
            evento.numberKeyPress(e);
        }

        private void restablecerVenta()
        {
            numPagi = 1;
            cargarDatos();
            textBox_IdVenta.Text = "";
            textBox_VenProd.Text = "";
            textBox_VenCant.Text = "";
            textBox_VenPreUni.Text = "";
            textBox_VenClie.Text = "";
            textBox_VenEmp.Text = "";
            textBox_VenEfec.Text = "";
            textBox_IdVenta.Focus();
            label_VenPago.Text = "$0.00";
            label_VenCam.Text = "$0.00";
            label_VenDeu.Text = "$0.00";
            textBox_BuscarVenta.Text = "";
            label_IdVenta.ForeColor = Color.Black;
            label_VenProd.ForeColor = Color.Black;
            label_VenCant.ForeColor = Color.Black;
            label_VenPreUni.ForeColor = Color.Black;
            label_VenClie.ForeColor = Color.Black;
            label_VenEmp.ForeColor = Color.Black;
            label_VenEfec.ForeColor = Color.Black;
            accion = "insert";
            idVenta = 0;
            label_FechaVenta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void cobrarPago()
        {
            if (radioButton_VentaCon.Checked)
            {
                calcularVenta();

                string IDV = textBox_IdVenta.Text;
                string ProductoVenta = textBox_VenProd.Text;
                string CantidadVenta = textBox_VenCant.Text;
                string PrecioUnitario = textBox_VenPreUni.Text;
                string ClienteVenta = textBox_VenClie.Text;
                string EmpleadoVenta = textBox_VenEmp.Text;
                string Efectivo = textBox_VenEfec.Text;
                
                string PagoVenta = label_VenPago.Text.Replace("$", "");
                string CambioVenta = label_VenCam.Text.Replace("$", "");
                string DeudaVenta = "";
                string FechaVenta = label_FechaVenta.Text;

                int cantMen = Convert.ToInt16(textBox_VenCant.Text), cantOri = Convert.ToInt16(dataGridView_Producto.CurrentRow.Cells[3].Value), cantFin;
                string cantFina;
                cantFin = cantOri - cantMen;
                cantFina = Convert.ToString(cantFin);

                if (accion == "insert")
                {
                    string IDP = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[1].Value);
                    string Producto = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[2].Value);
                    string Cantidad = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[3].Value);
                    string PrecioCom = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[4].Value);
                    string PrecioVen = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[5].Value);
                    //string idProd;
                    //int idProdu;
                    //idProd = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[1].Value);
                    //idProdu = Convert.ToInt16(idProd);
                    venta.insertVenta(IDV, ProductoVenta, CantidadVenta, PrecioUnitario, ClienteVenta, EmpleadoVenta,
                        Efectivo, PagoVenta, CambioVenta, DeudaVenta, FechaVenta);
                    producto.updateProducto(IDP, Producto, cantFina, PrecioCom, PrecioVen, idProducto);
                }
                if (accion == "update")
                {
                    venta.updateVenta(IDV, ProductoVenta, CantidadVenta, PrecioUnitario, ClienteVenta, EmpleadoVenta,
                        Efectivo, PagoVenta, CambioVenta, DeudaVenta, FechaVenta, idVenta);
                }
                restablecerVenta();
            }
            else if (radioButton_VentaCliFre.Checked)
            {
                calcularVenta();

                string IDV = textBox_IdVenta.Text;
                string ProductoVenta = textBox_VenProd.Text;
                string CantidadVenta = textBox_VenCant.Text;
                string PrecioUnitario = textBox_VenPreUni.Text;
                string ClienteVenta = textBox_VenClie.Text;
                string EmpleadoVenta = textBox_VenEmp.Text;
                string Efectivo = textBox_VenEfec.Text;

                string PagoVenta = label_VenPago.Text.Replace("$", "");
                string CambioVenta = "";
                deudaActual = deudaActual.Replace("$", "");
                string DeudaVenta = deudaActual;
                string FechaVenta = label_FechaVenta.Text;

                int cantMen = Convert.ToInt16(textBox_VenCant.Text), cantOri = Convert.ToInt16(dataGridView_Producto.CurrentRow.Cells[3].Value), cantFin;
                string cantFina;
                cantFin = cantOri - cantMen;
                cantFina = Convert.ToString(cantFin);

                if (accion == "insert")
                {
                    string IDP = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[1].Value);
                    string Producto = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[2].Value);
                    string Cantidad = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[3].Value);
                    string PrecioCom = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[4].Value);
                    string PrecioVen = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[5].Value);
                    string idCli, idProd;
                    //int idProdu;
                    idCli = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[1].Value);
                    //idProd = Convert.ToString(dataGridView_Producto.CurrentRow.Cells[1].Value);
                    //idProdu = Convert.ToInt16(idProd);
                    venta.insertVenta(IDV, ProductoVenta, CantidadVenta, PrecioUnitario, ClienteVenta, EmpleadoVenta,
                        Efectivo, PagoVenta, CambioVenta, DeudaVenta, FechaVenta);
                    reporte.insertReporte(DeudaVenta, Efectivo, idCli, FechaVenta);
                    producto.updateProducto(IDP, Producto, cantFina, PrecioCom, PrecioVen, idProducto);
                }
                if (accion == "update")
                {
                    venta.updateVenta(IDV, ProductoVenta, CantidadVenta, PrecioUnitario, ClienteVenta, EmpleadoVenta,
                        Efectivo, PagoVenta, CambioVenta, DeudaVenta, FechaVenta, idVenta);
                }
                restablecerVenta();
            }
            
        }

        private void dataGridViewVenta()
        {
            accion = "update";
            idVenta = Convert.ToInt16(dataGridView_Cliente.CurrentRow.Cells[0].Value);
            textBox_IdVenta.Text = Convert.ToString(dataGridView_Venta.CurrentRow.Cells[1].Value);
            textBox_VenProd.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[2].Value);
            textBox_VenCant.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[3].Value);
            textBox_VenPreUni.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[4].Value);
            textBox_VenClie.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[5].Value);
            textBox_VenEmp.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[6].Value);
            textBox_VenEfec.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[7].Value);
        }

        private void dataGridViewSeleccionProducto()
        {
            textBox_VenProd.Text = Convert.ToString(dataGridView_ProdVen.CurrentRow.Cells[2].Value);
            textBox_VenPreUni.Text = Convert.ToString(dataGridView_ProdVen.CurrentRow.Cells[5].Value);
        }

        private void dataGridViewSeleccionCliente()
        {
            textBox_VenClie.Text = Convert.ToString(dataGridView_ClieVen.CurrentRow.Cells[2].Value);
        }

        private void button_VenCan_Click(object sender, EventArgs e)
        {
            restablecerVenta();
        }

        private void dataGridView_ProdVen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewSeleccionProducto();
        }

        private void dataGridView_ProdVen_KeyPress(object sender, KeyPressEventArgs e)
        {
            dataGridViewSeleccionProducto();
        }

        private void dataGridView_ClieVen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewSeleccionCliente();
        }

        private void dataGridView_ClieVen_KeyPress(object sender, KeyPressEventArgs e)
        {
            dataGridViewSeleccionCliente();
        }

        private void calcularVenta()
        {
            if (textBox_IdVenta.Text == "")
            {
                label_IdVenta.Text = "Ingrese el ID";
                label_IdVenta.ForeColor = Color.Red;
                textBox_IdVenta.Focus();
            }
            else
            {
                if (textBox_VenProd.Text == "")
                {
                    label_VenProd.Text = "Seleccione el producto";
                    label_VenProd.ForeColor = Color.Red;
                }
                else
                {
                    if (textBox_VenPreUni.Text == "")
                    {
                        label_VenPreUni.Text = "Seleccione el producto";
                        label_VenPreUni.ForeColor = Color.Red;
                    }
                    else
                    {
                        if (textBox_VenCant.Text == "")
                        {
                            label_VenCant.Text = "Ingrese la cantidad";
                            label_VenCant.ForeColor = Color.Red;
                            textBox_VenCant.Focus();
                        }
                        else
                        {
                            if ((textBox_VenClie.Text == "") && (radioButton_VentaCliFre.Checked))
                            {
                                label_VenClie.Text = "Seleccione el cliente";
                                label_VenClie.ForeColor = Color.Red;
                            }
                            else
                            {
                                if (textBox_VenEfec.Text == "")
                                {
                                    label_VenEfec.Text = "Ingrese el efectivo";
                                    label_VenEfec.ForeColor = Color.Red;
                                    textBox_VenEfec.Focus();
                                }
                                else
                                {
                                    if (radioButton_VentaCon.Checked)
                                    {
                                        precioT = Convert.ToString(dataGridView_ProdVen.CurrentRow.Cells[5].Value);
                                        efectivoT = Convert.ToString(textBox_VenEfec.Text);

                                        precioV1 = Convert.ToDecimal(precioT);
                                        cantidadV1 = Convert.ToDecimal(textBox_VenCant.Text);
                                        efectivoV1 = Convert.ToDecimal(efectivoT);

                                        totalV1 = cantidadV1 * precioV1;
                                        cambioV1 = efectivoV1 - totalV1;

                                        label_VenPago.Text = "$" + Convert.ToString(totalV1) + ".00";
                                        label_VenCam.Text = "$" + Convert.ToString(cambioV1) + ".00";

                                        string reciboProducto = Convert.ToString(dataGridView_ProdVen.CurrentRow.Cells[2].Value), totalrecibo = Convert.ToString(totalV1);
                                        string cambiorecibo = Convert.ToString(cambioV1);
                                        label_ReciboProducto.Text = reciboProducto;
                                        label_ReciboCantidad.Text = textBox_VenCant.Text;
                                        label_ReciboTotal.Text = totalrecibo;
                                        label_ReciboCambio.Text = cambiorecibo;
                                        label_ReciboDeuda.Text = "$0.00";
                                    }
                                    else if (radioButton_VentaCliFre.Checked)
                                    {
                                        precioT = Convert.ToString(dataGridView_ProdVen.CurrentRow.Cells[5].Value);
                                        efectivoT = Convert.ToString(textBox_VenEfec.Text);

                                        precioV1 = Convert.ToDecimal(precioT);
                                        cantidadV1 = Convert.ToDecimal(textBox_VenCant.Text);
                                        efectivoV1 = Convert.ToDecimal(efectivoT);

                                        totalV1 = cantidadV1 * precioV1;
                                        deudaV1 = totalV1 - efectivoV1;
                                        deudaActual = Convert.ToString(deudaV1);

                                        label_VenPago.Text = "$" + Convert.ToString(totalV1) + ".00";
                                        label_VenDeu.Text = "$" + Convert.ToString(deudaV1) + ".00";

                                        string reciboProducto = Convert.ToString(dataGridView_ProdVen.CurrentRow.Cells[2].Value), totalrecibo = Convert.ToString(totalV1);
                                        string deudarecibo = Convert.ToString(deudaV1);
                                        label_ReciboNombre.Text = Convert.ToString(dataGridView_ClieVen.CurrentRow.Cells[2].Value);
                                        label_ReciboProducto.Text = reciboProducto;
                                        label_ReciboCantidad.Text = textBox_VenCant.Text;
                                        label_ReciboTotal.Text = totalrecibo;
                                        label_ReciboCambio.Text = "$0.00";
                                        label_ReciboDeuda.Text = deudarecibo;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button_VenCal_Click(object sender, EventArgs e)
        {
            calcularVenta();
        }

        private void button_VenCobrar_Click(object sender, EventArgs e)
        {
            cobrarPago();
        }

        private void button_VentaPrimero_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            label_PaginasVenta.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            venta.searchVenta(dataGridView_Venta, "", 1, pageSize);
        }

        private void button_VentaAnterior_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label_PaginasVenta.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                venta.searchVenta(dataGridView_Venta, "", numPagi, pageSize);
            }
        }

        private void button_VentaSiguiente_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label_PaginasVenta.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
                venta.searchVenta(dataGridView_Venta, "", numPagi, pageSize);
            }
        }

        private void button_VentaUltimo_Click(object sender, EventArgs e)
        {
            numPagi = pageCount;
            label_PaginasVenta.Text = "Pagina " + numPagi.ToString() + "/" + pageCount.ToString();
            venta.searchVenta(dataGridView_Venta, "", pageCount, pageSize);
        }

        private void button_SeProdAntes_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                venta.searchVenta(dataGridView_ProdVen, "", numPagi, pageSize);
            }
        }

        private void button_SeProdSiguiente_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                venta.searchVenta(dataGridView_ProdVen, "", numPagi, pageSize);
            }
        }

        private void button_SeClieAntes_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                venta.searchVenta(dataGridView_ClieVen, "", numPagi, pageSize);
            }
        }

        private void button_SeClieSiguiente_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                venta.searchVenta(dataGridView_ClieVen, "", numPagi, pageSize);
            }
        }

        #endregion
    }
}