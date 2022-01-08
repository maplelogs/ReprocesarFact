using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Pedidos
{
    class Program
    {
       // LeecturaArchivos Lectura;
        DataTable dt;
        OpenFileDialog OpenFileDialog1;
       
        //LeerArchivo
        static void Main(string[] args)
        {
            leerarchivo();
        }
        private static void leerarchivo()
        {
            String HoraCT = DateTime.Now.ToShortDateString();
            String HoraCF = DateTime.Now.ToLongTimeString();
            String Marca = "";
            string[] separador = new string[] { "/" };
            string[] info = HoraCT.Split(separador, StringSplitOptions.RemoveEmptyEntries);
            string[] separador2 = new string[] { ":", " " };
            string[] info2 = HoraCF.Split(separador2, StringSplitOptions.RemoveEmptyEntries);
            Marca = info[0] + info[1] + info[2] + info2[0] + info2[1] + info2[2];
            LeecturaArchivos Lectura = new LeecturaArchivos();
           DataTable dt = new DataTable();
           OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            // EditarOCTxtRutaArchivo.Clear();
            //string NadroPath = @"C:\REEPROCESAR\NADRO";
            //string DepotPath = @"C:\REEPROCESAR\DEPOT";
            //string FarmacosPath = @"C:\REEPROCESAR\FARMACOS";
            //string MarzamPath = @"C:\REEPROCESAR\MARZAM";
            //string MedipacPath = @"C:\REEPROCESAR\MEDIPAC";
            List<string> Paths = new List<string>() { @"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\NADRO\facturas.txt", @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\NADRO\facturas.txt", @"\\192.168.13.30\Pedidos\REEPROCESAR\DEPOT\facturas.txt", @"\\192.168.13.30\Pedidos\REEPROCESAR\FARMACOS\facturas.txt", @"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\MARZAM\facturas.txt", @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MARZAM\facturas.txt", @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MEDIPAC\facturas.txt" };
            foreach(var ruta in Paths)
            {
                if (File.Exists(ruta.ToString()))
                {

                   // Console.WriteLine(ruta.ToString());

                    if (ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\NADRO\facturas.txt")
                    {
                        Lectura.ConfirmaNadro(ruta.ToString(), 3, Marca);

                    }
                    if (ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\NADRO\facturas.txt")
                    {
                        Lectura.ConfirmaNadro(ruta.ToString(), 2, Marca);

                    }
                    else if (ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\DEPOT\facturas.txt")
                    {
                        Lectura.ConfirmaDepot(ruta.ToString(), 2, Marca);

                    }
                    else if (ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\FARMACOS\facturas.txt")
                    {
                        Lectura.ConfirmaFarmacos(ruta.ToString(), 2, Marca);

                    }
                    else if (ruta.ToString() == @"\\192.168.13.30\PedidosREEPROCESAR\PACIFICO\MEDIPAC\facturas.txt")
                    {
                        Lectura.ConfirmaMedipac(ruta.ToString(), 2, Marca);

                    }
                    else if (ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\MARZAM\facturas.txt" || ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\MARZAM\facturagdl.txt")
                    {
                        Lectura.ConfirmaMarzam(ruta.ToString(), 2, Marca);

                    }
                    else if (ruta.ToString() == @"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MARZAM\facturas.txt")
                    {
                        Lectura.ConfirmaMarzam(ruta.ToString(), 3, Marca);

                    }


                }
            }
           
        }
        
    }
       
    }

