using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos
{
    class Consulta
    {

        private SqlConnection conexion5 = new SqlConnection("Data Source=192.168.13.4;Initial Catalog=Opesys_Desarrollo;User Id=SA;Password=S1ST3M45AD;Application Name=APP;");//Connection Timeout=30

        public DataTable Editar_OC_Factura(String Marca, int Realizar)
        {
            DataTable tabla = null;
            try
            {
                //  conexion3.Open();
                conexion5.Open();
                //string consulta = "SpEditar_OC_Factura";
                string consulta = "[SpEditar_OC_Factura4]";
                SqlCommand comandocargar = new SqlCommand(consulta, conexion5);
                //SqlCommand comandocargar = new SqlCommand(consulta, conexion3);
                comandocargar.Parameters.AddWithValue("@Marca", Marca);
                comandocargar.Parameters.AddWithValue("@Realizar", Realizar);
                comandocargar.CommandTimeout = 800;
                comandocargar.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dainterno = new SqlDataAdapter(comandocargar);
                tabla = new DataTable();
                dainterno.Fill(tabla);

            }
            catch (Exception e)
            {
                Console.WriteLine("error=Consultas.Editar_OC_Factura()-" + e.ToString());
                //MessageBox.Show(e.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabla = null;
            }
            finally
            {
                // conexion3.Close();
                conexion5.Close();
            }

            return tabla;
        }
        public bool InsertarConfirmacion(DataTable dt)
        {
            bool Resultado = false;
            SqlBulkCopy sbc = null;
            try
            {
                conexion5.Open();
                //conexion3.Open();
                //using (sbc = new SqlBulkCopy(conexion3))
                using (sbc = new SqlBulkCopy(conexion5))
                {
                    sbc.DestinationTableName = "dbo.ConfirmacionDePedidos";
                    sbc.BatchSize = 1000;
                    sbc.ColumnMappings.Add("OrdenCompra", "NumOrdenCompra");
                    sbc.ColumnMappings.Add("FolioFactura", "NumeroFactura");
                    sbc.ColumnMappings.Add("NoCliente", "NumeroCliente");
                    sbc.ColumnMappings.Add("FechaFact", "FechaFactura");
                    sbc.ColumnMappings.Add("Proveedor", "Proveedor");
                    sbc.ColumnMappings.Add("CodigoEan", "Codigo");
                    sbc.ColumnMappings.Add("CantFact", "PizasFact");
                    sbc.ColumnMappings.Add("PrecioFarmacia", "PrecioFarmacia");
                    sbc.ColumnMappings.Add("PorcentajeOft", "PorcentajeOferta");
                    sbc.ColumnMappings.Add("PrecioOferta", "PrecioOferta");
                    sbc.ColumnMappings.Add("PrecioConOft", "PrecioConOferta");
                    sbc.ColumnMappings.Add("ImporteSinDesXCant", "ImporteSinDescXCant");
                    sbc.ColumnMappings.Add("PorcentajeDesc", "PorcentajeDesc");
                    sbc.ColumnMappings.Add("PrecioDesc", "PrecioDescuento");
                    sbc.ColumnMappings.Add("PrecioConDesc", "PrecioConDesc");
                    sbc.ColumnMappings.Add("IVAImporte", "IVAImporte");
                    sbc.ColumnMappings.Add("ImporteNetoUnitario", "ImporteNetoUnitario");
                    sbc.ColumnMappings.Add("ImporteNetoTotal", "ImporteNetoTotal");
                    sbc.ColumnMappings.Add("Opcion", "Opcion");
                    sbc.ColumnMappings.Add("UUID", "UUID");//UUID
                    sbc.NotifyAfter = dt.Rows.Count;
                    sbc.WriteToServer(dt);
                    sbc.Close();
                    Resultado = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error=Consultas.InsertarConfirmacion()-" + e.ToString());
                // MessageBox.Show(e.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Resultado = false;
            }
            finally
            {
                if (sbc != null)
                {
                    sbc.Close();
                }
                conexion5.Close();
                //conexion3.Close();
            }
            return Resultado;
        }
        public bool EliminaConfiramcion(int TablaEliminar, string Marca)
        {
            SqlCommand comandocargar;
            bool Resultado = false;
            string Elimina = "";
            DataTable tabla = null;
            try
            {
                conexion5.Open();
                //conexion3.Open();
                if (TablaEliminar == 1)
                {
                    Elimina = "delete dbo.ConfirmacionDePedidos";
                }
                else if (TablaEliminar == 2)
                {
                    Elimina = "delete dbo.ConfirmacionDePedidos2 where Marca = @Marca";
                }
                comandocargar = new SqlCommand(Elimina, conexion5);
                // comandocargar = new SqlCommand(Elimina, conexion3);
                tabla = new DataTable();
                tabla.Columns.Add("Elimino", typeof(int));
                comandocargar.Parameters.AddWithValue("@Marca", Marca);
                tabla.Rows.Add(comandocargar.ExecuteNonQuery());
                if (int.Parse(tabla.Rows[0][0].ToString()) >= 1)
                {
                    Resultado = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error=Consultas.EliminaConfiramcion()-" + e.ToString());
                // MessageBox.Show(e.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Resultado = false;
            }
            finally
            {
                conexion5.Close();
                //conexion3.Close();
            }
            return Resultado;
        }
        public bool InsertarConfirmacion2(DataTable dt, int ID)
        {
            bool Resultado = false;
            SqlBulkCopy sbc = null;
            try
            {
                conexion5.Open();
                // conexion3.Open();
                if (ID == 1)
                {
                    using (sbc = new SqlBulkCopy(conexion5))
                    //using (sbc = new SqlBulkCopy(conexion3))

                    {
                        sbc.DestinationTableName = "dbo.ConfirmacionDePedidos2";
                        sbc.BatchSize = 1000;
                        sbc.ColumnMappings.Add("OrdenCompra", "NumOrdenCompra");//NumOrdenCompra--------------------------------
                        sbc.ColumnMappings.Add("FolioFactura", "NumeroFactura");//-NumeroFactura--------------------------------
                        sbc.ColumnMappings.Add("FechaFact", "FechaFactura");//-FechaFactura--------------------------------
                        sbc.ColumnMappings.Add("Proveedor", "Proveedor");//-Proveedor--------------------------------
                        sbc.ColumnMappings.Add("CodigoEan", "Codigo");//-Codigo--------------------------------
                        sbc.ColumnMappings.Add("CantFact", "PizasFact");//-PizasFact--------------------------------
                        sbc.ColumnMappings.Add("PrecioFarmacia", "PrecioFarmacia");//-PrecioFarmacia--------------------------------
                        sbc.ColumnMappings.Add("PorcentajeOft", "PorcentajeOferta");//-PorcentajeOferta--------------------------------
                        sbc.ColumnMappings.Add("PrecioOferta", "PrecioOferta");//-PrecioOferta--------------------------------
                        sbc.ColumnMappings.Add("PrecioConOft", "PrecioConOferta");//-PrecioConOferta--------------------------------
                        sbc.ColumnMappings.Add("ImporteSinDesXCant", "ImporteSinDescXCant");//-ImporteSinDescXCant--------------------------------
                        sbc.ColumnMappings.Add("PorcentajeDesc", "PorcentajeDesc");//-PorcentajeDesc--------------------------------
                        sbc.ColumnMappings.Add("PrecioDesc", "PrecioDescuento");//-PrecioDescuento--------------------------------
                        sbc.ColumnMappings.Add("PrecioConDesc", "PrecioConDesc");//-PrecioConDesc--------------------------------                     
                        sbc.ColumnMappings.Add("ImporteNetoUnitario", "ImporteNetoUnitario");//-ImporteNetoUnitario--------------------------------
                        sbc.ColumnMappings.Add("ImporteNetoTotal", "ImporteNetoTotal");//-ImporteNetoTotal--------------------------------
                        sbc.ColumnMappings.Add("DescuentoComercial", "DescuentoComercial");//-DescuentoComercial--------------------------------
                        sbc.ColumnMappings.Add("DescuentoComercialCal", "DescuentoComercialCal");//-DescuentoComercialCal--------------------------------
                        sbc.ColumnMappings.Add("CostoDiaCalculado", "CostoDiaCalculado");//-CostoDiaCalculado--------------------------------
                        sbc.ColumnMappings.Add("CostoFacturaCalculado", "CostoFacturaCalculado");//-CostoFacturaCalculado--------------------------------
                        sbc.ColumnMappings.Add("Bonificacion", "Bonificacion");//-Bonificacion--------------------------------
                        sbc.ColumnMappings.Add("DescuentoLineal", "DescuentoLineal");//-DescuentoLineal--------------------------------
                        sbc.ColumnMappings.Add("Sucursal", "Sucursal");//-Sucursal--------------------------------
                        sbc.ColumnMappings.Add("Marca", "Marca");//-Marca--------------------------------
                        sbc.ColumnMappings.Add("ID", "ID");//-ID--------------------------------
                        sbc.ColumnMappings.Add("Almacen", "Almacen");//-Almacen--------------------------------
                        sbc.ColumnMappings.Add("FechaPedido", "FechaPedido");//-FechaPedido--------------------------------
                        sbc.ColumnMappings.Add("Usuario", "Usuario");//-FechaPedido--------------------------------
                        sbc.ColumnMappings.Add("Opcion", "Opcion");//-FechaPedido--------------------------------
                        //sbc.ColumnMappings.Add("UUID", "UUID");//-UUID---
                        sbc.NotifyAfter = dt.Rows.Count;
                        sbc.WriteToServer(dt);
                        sbc.Close();
                        Resultado = true;

                    }
                }

                if (ID == 2)
                {
                    using (sbc = new SqlBulkCopy(conexion5))
                    //using (sbc = new SqlBulkCopy(conexion3))
                    {
                        sbc.DestinationTableName = "dbo.ConfirmacionDePedidos2";
                        sbc.BatchSize = 1000;
                        sbc.ColumnMappings.Add("OrdenCompra", "NumOrdenCompra");
                        sbc.ColumnMappings.Add("FolioFactura", "NumeroFactura");
                        sbc.ColumnMappings.Add("NoCliente", "NumeroCliente");
                        sbc.ColumnMappings.Add("FechaFact", "FechaFactura");
                        sbc.ColumnMappings.Add("Proveedor", "Proveedor");
                        sbc.ColumnMappings.Add("CodigoEan", "Codigo");
                        sbc.ColumnMappings.Add("CantFact", "PizasFact");
                        sbc.ColumnMappings.Add("PrecioFarmacia", "PrecioFarmacia");
                        sbc.ColumnMappings.Add("PorcentajeOft", "PorcentajeOferta");
                        sbc.ColumnMappings.Add("PrecioOferta", "PrecioOferta");
                        sbc.ColumnMappings.Add("PrecioConOft", "PrecioConOferta");
                        sbc.ColumnMappings.Add("ImporteSinDesXCant", "ImporteSinDescXCant");
                        sbc.ColumnMappings.Add("PorcentajeDesc", "PorcentajeDesc");
                        sbc.ColumnMappings.Add("PrecioDesc", "PrecioDescuento");
                        sbc.ColumnMappings.Add("PrecioConDesc", "PrecioConDesc");
                        sbc.ColumnMappings.Add("IVAImporte", "IVAImporte");
                        sbc.ColumnMappings.Add("ImporteNetoUnitario", "ImporteNetoUnitario");
                        sbc.ColumnMappings.Add("ImporteNetoTotal", "ImporteNetoTotal");
                        sbc.ColumnMappings.Add("Opcion", "Opcion");
                        sbc.ColumnMappings.Add("Marca", "Marca");
                        //sbc.ColumnMappings.Add("UUID", "UUID");
                        sbc.NotifyAfter = dt.Rows.Count;
                        sbc.WriteToServer(dt);
                        sbc.Close();
                        Resultado = true;

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error=Consultas.InsertarConfirmacion()-" + e.ToString());
                //MessageBox.Show(e.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Resultado = false;
            }

            finally
            {
                if (sbc != null)
                {
                    sbc.Close();
                }
                conexion5.Close();
                //conexion3.Close();
            }
            return Resultado;
        }
        public bool InsertarFaltante(DataTable dt)
        {
            bool Resultado = false;
            SqlBulkCopy sbc = null;
            try
            {
                conexion5.Open();
                //conexion3.Open();
                using (sbc = new SqlBulkCopy(conexion5))
                //using (sbc = new SqlBulkCopy(conexion3))
                {
                    sbc.DestinationTableName = "dbo.ConfirmacionFaltantes";
                    sbc.BatchSize = 1000;
                    sbc.ColumnMappings.Add("NumOrdenCompra", "NumOrdenCompra");
                    sbc.ColumnMappings.Add("NumeroCliente", "NumeroCliente");
                    sbc.ColumnMappings.Add("FechaFaltante", "FechaFaltante");
                    sbc.ColumnMappings.Add("Codigo", "Codigo");
                    sbc.ColumnMappings.Add("PiezasFacturadas", "PiezasFacturadas");
                    sbc.ColumnMappings.Add("PiezasFaltantes", "PiezasFaltantes");
                    sbc.ColumnMappings.Add("MotivoFaltante", "MotivoFaltante");
                    sbc.ColumnMappings.Add("proveedor", "proveedor");
                    sbc.ColumnMappings.Add("Opcion", "Opcion");
                    sbc.ColumnMappings.Add("Marca", "Marca");
                    sbc.NotifyAfter = dt.Rows.Count;
                    sbc.WriteToServer(dt);
                    sbc.Close();
                    Resultado = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error=Consultas.InsertarFaltante()-" + e.ToString());
                //MessageBox.Show(e.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Resultado = false;
            }
            finally
            {
                if (sbc != null)
                {
                    sbc.Close();
                }
                conexion5.Close();
                //conexion3.Close();
            }
            return Resultado;
        }
    }
}
