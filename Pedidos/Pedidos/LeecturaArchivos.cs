using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos
{
    class LeecturaArchivos
    {

        Instancia instancia;
        DataTable dt;
        DataTable dt2;
        public bool ConfirmaSaba(String ruta, int TablaInserta, String Marca)
        {
            string proveedor = "";
            string NoCliente = "";
            string FolioFactura = "";
            int CantFact = 0;
            float PrecioFarmacia = 0.0f;
            float Oferta = 0.0f;
            float Descuentos = 0.0f;
            float IvaMerc = 0.0f;
            float PorcentaJeOft = 0.0f;
            float DescFact = 0.0f;
            string CodigoEan = "";
            int OrdenCompra = 0;
            string FechaFact = "";
            float ImporteNetoUnit = 0.0f;
            float ImporteNetoTotal = 0.0f;
            float PrecioConOferta = 0.0f;
            float PrecioConDesc = 0.0f;
            float ImporteSinDescXCant = 0.0f;
            string sLine = "";
            int renglon = 0;
            bool resultado = false;
            int Opcion = 0;
            ArrayList arrText = new ArrayList();
            List<String> LstRenglones = new List<String>();
            try
            {
                StreamReader objReader = new StreamReader(ruta);
                dt = new DataTable();
                dt.Columns.Add("OrdenCompra", typeof(string));//
                dt.Columns.Add("FolioFactura", typeof(string));//
                dt.Columns.Add("NoCliente", typeof(string));//
                dt.Columns.Add("FechaFact", typeof(string));//
                dt.Columns.Add("Proveedor", typeof(string));//
                dt.Columns.Add("CodigoEan", typeof(string));//
                dt.Columns.Add("CantFact", typeof(string));//
                dt.Columns.Add("PrecioFarmacia", typeof(string));//
                dt.Columns.Add("PorcentajeOft", typeof(string));//
                dt.Columns.Add("PrecioOferta", typeof(string));//
                dt.Columns.Add("PrecioConOft", typeof(string));
                dt.Columns.Add("ImporteSinDesXCant", typeof(string));
                dt.Columns.Add("PorcentajeDesc", typeof(string));//
                dt.Columns.Add("PrecioDesc", typeof(string));//
                dt.Columns.Add("PrecioConDesc", typeof(string));
                dt.Columns.Add("IVAImporte", typeof(string));//
                dt.Columns.Add("ImporteNetoUnitario", typeof(string));
                dt.Columns.Add("ImporteNetoTotal", typeof(string));
                dt.Columns.Add("Opcion", typeof(string));
                dt.Columns.Add("Marca", typeof(string));
                //MIENTRAS EL RENGLON CONTENGA INFORMACION SE LE ASIGNA A LA LISTA arrText
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
                //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                foreach (string sOutput in arrText)
                {
                    if (sOutput.Trim() != "")
                    {
                        renglon += 1;
                        //CodigoEan = sOutput.Substring(85, 14).Trim();
                        //if (CodigoEan == "7501109900596")
                        //{
                        if (sOutput.Length == 141)//127
                        {
                            string OrdenCompraTxt = sOutput.Substring(109, 9);
                            long numero = 0;
                            float Decimales = 0.0f;
                            bool Convertir1 = long.TryParse(OrdenCompraTxt, out numero);
                            if (Convertir1 == true)
                            {
                                OrdenCompra = int.Parse(OrdenCompraTxt);
                                string OpcionTXT = sOutput.Substring(118, 1);
                                bool Convertir8 = long.TryParse(OpcionTXT, out numero);
                                if (Convertir8 == true)
                                {
                                    Opcion = int.Parse(OpcionTXT);
                                    string FolioFacturaTXT = sOutput.Substring(7, 7);//
                                    FolioFactura = "II-" + long.Parse(FolioFacturaTXT).ToString();
                                    NoCliente = sOutput.Substring(0, 7);//
                                    FechaFact = sOutput.Substring(119, 8);//
                                    proveedor = "P01003";//
                                    CodigoEan = sOutput.Substring(85, 14);//
                                    string CantFactTxt = sOutput.Substring(20, 7);
                                    bool Convertir2 = long.TryParse(CantFactTxt, out numero);
                                    if (Convertir2 == true)
                                    {
                                        CantFact = int.Parse(CantFactTxt);// 
                                        string PrecioFarmaciaTxt = (sOutput.Substring(27, 10));
                                        bool Convertir3 = float.TryParse(PrecioFarmaciaTxt, out Decimales);
                                        if (Convertir3 == true)
                                        {
                                            PrecioFarmacia = float.Parse(PrecioFarmaciaTxt) / 100;// 
                                            string PorcentaJeOftTxt = (sOutput.Substring(77, 4));
                                            bool Convertir4 = float.TryParse(PorcentaJeOftTxt, out Decimales);
                                            if (Convertir4 == true)
                                            {
                                                PorcentaJeOft = float.Parse(PorcentaJeOftTxt) / 100;
                                                string OfertaTxt = sOutput.Substring(37, 10);
                                                bool Convertir5 = float.TryParse(OfertaTxt, out Decimales);
                                                if (Convertir5 == true)
                                                {
                                                    Oferta = (float.Parse(OfertaTxt) / 100) / CantFact;//
                                                    PrecioConOferta = PrecioFarmacia - Oferta;
                                                    ImporteSinDescXCant = PrecioConOferta * CantFact;
                                                    string DescFactTxt = sOutput.Substring(81, 4);
                                                    bool Convertir6 = float.TryParse(DescFactTxt, out Decimales);
                                                    if (Convertir6 == true)
                                                    {
                                                        DescFact = float.Parse(DescFactTxt) / 100;
                                                        Descuentos = (float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     
                                                        string IvaMercTxt = sOutput.Substring(57, 10);
                                                        bool Convertir7 = float.TryParse(IvaMercTxt, out Decimales);
                                                        if (Convertir7 == true)
                                                        {
                                                            IvaMerc = (float.Parse(IvaMercTxt) / 100) / CantFact;
                                                            PrecioConDesc = PrecioConOferta - (Descuentos / CantFact);
                                                            ImporteNetoUnit = PrecioConDesc;
                                                            ImporteNetoTotal = PrecioConDesc * CantFact;
                                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                            OrdenCompraTxt = "";
                                                            CantFactTxt = "";
                                                            PrecioFarmaciaTxt = "";
                                                            PorcentaJeOftTxt = "";
                                                            OfertaTxt = "";
                                                            DescFactTxt = "";
                                                            IvaMercTxt = "";
                                                            OrdenCompra = 0;
                                                            FolioFactura = "";
                                                            NoCliente = "";
                                                            FechaFact = "";
                                                            CodigoEan = "";
                                                            CantFact = 0;
                                                            PrecioFarmacia = 0.0f;
                                                            PorcentaJeOft = 0.0f;
                                                            Oferta = 0.0f;
                                                            PrecioConOferta = 0.0f;
                                                            DescFact = 0.0f;
                                                            Descuentos = 0.0f;
                                                            PrecioConDesc = 0.0f;
                                                            IvaMerc = 0.0f;
                                                            ImporteNetoUnit = 0.0f;
                                                            ImporteNetoTotal = 0.0f;
                                                        }

                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }



                    // }
                }
                if (dt.Rows.Count > 0)
                {
                    instancia = new Instancia();

                    if (TablaInserta == 1)
                    {
                        resultado = instancia.InsertarConfirmacion(dt);
                        if(resultado == true)
                        {
                            File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\SABA\facturas.txt");
                        }
                    }
                    else if (TablaInserta == 2)
                    {
                        resultado = instancia.InsertarConfirmacion2(dt, 2);
                        if (resultado == true)
                        {
                            File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\SABA\facturas.txt");
                        }
                    }
                }
                else
                {
                    resultado = false;
                }

            }
            catch (Exception ex)
            {
                renglon.ToString();
                //  MessageBox.Show(ex.Message);
                resultado = false;
            }
            return resultado;
        }
        public bool ConfirmaFarmacos(String ruta, int TablaInserta, String Marca)
        {
            string proveedor = "";
            string NoCliente = "";
            string FolioFactura = "";
            int CantFact = 0;
            float PrecioFarmacia = 0.0f;
            float Oferta = 0.0f;
            float Descuentos = 0.0f;
            float IvaMerc = 0.0f;
            float PorcentaJeOft = 0.0f;
            float DescFact = 0.0f;
            string CodigoEan = "";
            int OrdenCompra = 0;
            string FechaFact = "";
            float ImporteNetoUnit = 0.0f;
            float ImporteNetoTotal = 0.0f;
            float PrecioConOferta = 0.0f;
            float PrecioConDesc = 0.0f;
            float ImporteSinDescXCant = 0.0f;

            string sLine = "";
            int renglon = 0;
            bool resultado = false;
            int Opcion = 0;
            ArrayList arrText = new ArrayList();
            StreamReader objReader = null;
            try
            {
                objReader = new StreamReader(ruta);
                dt = new DataTable();
                dt.Columns.Add("OrdenCompra", typeof(string));// dt.Columns.Add("OrdenCompra", typeof(string));//
                dt.Columns.Add("FolioFactura", typeof(string));//   dt.Columns.Add("FolioFactura", typeof(string));//
                dt.Columns.Add("NoCliente", typeof(string));//  dt.Columns.Add("NoCliente", typeof(string));//
                dt.Columns.Add("FechaFact", typeof(string));//   dt.Columns.Add("FechaFact", typeof(string));//
                dt.Columns.Add("Proveedor", typeof(string));// dt.Columns.Add("Proveedor", typeof(string));//
                dt.Columns.Add("CodigoEan", typeof(string));//   dt.Columns.Add("CodigoEan", typeof(string));//
                dt.Columns.Add("CantFact", typeof(string));// dt.Columns.Add("CantFact", typeof(string));//
                dt.Columns.Add("PrecioFarmacia", typeof(string));//  dt.Columns.Add("PrecioFarmacia", typeof(string));//
                dt.Columns.Add("PorcentajeOft", typeof(string));//    dt.Columns.Add("PorcentajeOft", typeof(string));//
                dt.Columns.Add("PrecioOferta", typeof(string));//  dt.Columns.Add("PrecioOferta", typeof(string));//
                dt.Columns.Add("PrecioConOft", typeof(string));//   dt.Columns.Add("PrecioConOft", typeof(string));
                dt.Columns.Add("ImporteSinDesXCant", typeof(string));//  dt.Columns.Add("ImporteSinDesXCant", typeof(string));
                dt.Columns.Add("PorcentajeDesc", typeof(string));//   dt.Columns.Add("PorcentajeDesc", typeof(string));//
                dt.Columns.Add("PrecioDesc", typeof(string));//    dt.Columns.Add("PrecioDesc", typeof(string));//
                dt.Columns.Add("PrecioConDesc", typeof(string));// dt.Columns.Add("PrecioConDesc", typeof(string));
                dt.Columns.Add("IVAImporte", typeof(string));//  dt.Columns.Add("IVAImporte", typeof(string));//
                dt.Columns.Add("ImporteNetoUnitario", typeof(string));// dt.Columns.Add("ImporteNetoUnitario", typeof(string));
                dt.Columns.Add("ImporteNetoTotal", typeof(string));//    dt.Columns.Add("ImporteNetoTotal", typeof(string));
                dt.Columns.Add("Opcion", typeof(string));//  dt.Columns.Add("Opcion", typeof(string));
                dt.Columns.Add("Marca", typeof(string)); // dt.Columns.Add("Marca", typeof(string)); 
                //dt.Columns.Add("UUID", typeof(string));
                //MIENTRAS EL RENGLON CONTENGA INFORMACION SE LE ASIGNA A LA LISTA arrText
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
                //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                if (TablaInserta == 1)
                {
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            if (sOutput.Length == 70)
                            {
                                renglon += 1;
                                //CodigoEan = sOutput.Substring(85, 14).Trim();
                                //if (CodigoEan == "7502214982484")
                                //{
                                string OrdenCompraTxt = sOutput.Substring(29, 9);
                                long Numero = 0;
                                bool Convertidor1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertidor1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);//
                                    string OpcionTXT = sOutput.Substring(38, 1);
                                    bool Convertidor3 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertidor3 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        NoCliente = sOutput.Substring(39, 12);//
                                        proveedor = sOutput.Substring(51, 10);//
                                        CodigoEan = sOutput.Substring(0, 15);//
                                        string CantFactTxt = sOutput.Substring(22, 7);
                                        bool Convertidor2 = long.TryParse(CantFactTxt, out Numero);
                                        if (Convertidor2 == true)
                                        {
                                            CantFact = int.Parse(CantFactTxt);//                         
                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                            OrdenCompraTxt = "";
                                            CantFactTxt = "";
                                            OrdenCompra = 0;
                                            FolioFactura = "";
                                            NoCliente = "";
                                            FechaFact = "";
                                            CodigoEan = "";
                                            CantFact = 0;
                                            PrecioFarmacia = 0.0f;
                                            PorcentaJeOft = 0.0f;
                                            Oferta = 0.0f;
                                            PrecioConOferta = 0.0f;
                                            DescFact = 0.0f;
                                            Descuentos = 0.0f;
                                            PrecioConDesc = 0.0f;
                                            IvaMerc = 0.0f;
                                            ImporteNetoUnit = 0.0f;
                                            ImporteNetoTotal = 0.0f;
                                        }
                                    }

                                }
                            }
                            //  }
                        }
                    }
                }
                else if (TablaInserta == 2)
                {
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            if (sOutput.Length == 206) //if (sOutput.Length == 246)
                            {
                                renglon += 1;
                                //CodigoEan = sOutput.Substring(85, 14).Trim();
                                //if (CodigoEan == "7502214982484")
                                //{
                                string OrdenCompraTxt = sOutput.Substring(0, 9);
                                long Numero = 0;
                                float Decimal = 0.0f;
                                bool Convertidor1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertidor1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(9, 1);
                                    bool Convertir12 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir12 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = sOutput.Substring(20, 12);//
                                        NoCliente = sOutput.Substring(32, 12);
                                        FechaFact = sOutput.Substring(44, 8);
                                        proveedor = sOutput.Substring(10, 10);
                                        CodigoEan = sOutput.Substring(52, 15);
                                        string CantFactTxt = sOutput.Substring(67, 7);
                                        bool Convertidor2 = long.TryParse(CantFactTxt, out Numero);
                                        if (Convertidor2 == true)
                                        {
                                            CantFact = int.Parse(CantFactTxt);// 
                                            string PrecioFarmaciaTxt = sOutput.Substring(74, 9);
                                            bool Convertidor3 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                            if (Convertidor3 == true)
                                            {
                                                PrecioFarmacia = float.Parse(PrecioFarmaciaTxt);//
                                                string PorcentaJeOftTxt = sOutput.Substring(83, 6);
                                                bool Convertidor4 = float.TryParse(PorcentaJeOftTxt, out Decimal);
                                                if (Convertidor4 == true)
                                                {
                                                    PorcentaJeOft = float.Parse(PorcentaJeOftTxt);
                                                    string OfertaTxt = sOutput.Substring(89, 9);
                                                    bool Convertidor5 = float.TryParse(OfertaTxt, out Decimal);
                                                    if (Convertidor5 == true)
                                                    {
                                                        Oferta = float.Parse(OfertaTxt);//

                                                        string PrecioConOfertaTxt = sOutput.Substring(98, 9);
                                                        bool Convertidor6 = float.TryParse(PrecioConOfertaTxt, out Decimal);
                                                        if (Convertidor6 == true)
                                                        {
                                                            PrecioConOferta = float.Parse(PrecioConOfertaTxt);
                                                            ImporteSinDescXCant = PrecioConOferta * CantFact;

                                                            string DescFactTxt = sOutput.Substring(107, 9);
                                                            bool Convertidor7 = float.TryParse(DescFactTxt, out Decimal);
                                                            if (Convertidor7 == true)
                                                            {
                                                                DescFact = float.Parse(DescFactTxt);

                                                                string DescuentosTxt = sOutput.Substring(116, 9);
                                                                bool Convertidor8 = float.TryParse(DescuentosTxt, out Decimal);
                                                                if (Convertidor8 == true)
                                                                {
                                                                    Descuentos = float.Parse(DescuentosTxt);//

                                                                    string PrecioConDescTxt = sOutput.Substring(125, 9);
                                                                    bool Convertidor9 = float.TryParse(PrecioConDescTxt, out Decimal);
                                                                    if (Convertidor9 == true)
                                                                    {
                                                                        PrecioConDesc = float.Parse(PrecioConDescTxt);//

                                                                        string IvaMercTxt = sOutput.Substring(179, 9);
                                                                        bool Convertidor10 = float.TryParse(IvaMercTxt, out Decimal);
                                                                        if (Convertidor10 == true)
                                                                        {
                                                                            IvaMerc = (float.Parse(IvaMercTxt)) / CantFact;
                                                                            ImporteNetoUnit = PrecioConDesc;

                                                                            string ImporteNetoTotalTxt = sOutput.Substring(188, 9);
                                                                            bool Convertidor11 = float.TryParse(ImporteNetoTotalTxt, out Decimal);
                                                                            if (Convertidor11 == true)
                                                                            {
                                                                                ImporteNetoTotal = float.Parse(ImporteNetoTotalTxt);
                                                                                //string UUID = sOutput.Substring(206, 36);
                                                                                //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca,UUID);
                                                                                dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                                                OrdenCompraTxt = "";
                                                                                CantFactTxt = "";
                                                                                PrecioFarmaciaTxt = "";
                                                                                PorcentaJeOftTxt = "";
                                                                                OfertaTxt = "";
                                                                                PrecioConOfertaTxt = "";
                                                                                OrdenCompra = 0;
                                                                                FolioFactura = "";
                                                                                NoCliente = "";
                                                                                FechaFact = "";
                                                                                CodigoEan = "";
                                                                                CantFact = 0;
                                                                                PrecioFarmacia = 0.0f;
                                                                                PorcentaJeOft = 0.0f;
                                                                                Oferta = 0.0f;
                                                                                PrecioConOferta = 0.0f;
                                                                                DescFact = 0.0f;
                                                                                Descuentos = 0.0f;
                                                                                PrecioConDesc = 0.0f;
                                                                                IvaMerc = 0.0f;
                                                                                ImporteNetoUnit = 0.0f;
                                                                                ImporteNetoTotal = 0.0f;
                                                                                //UUID = "";

                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //  }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    //-if (TablaInserta == 1)
                    //{
                    //    resultado = instancia.InsertarConfirmacion2(dt,1);
                    //}
                    //else if (TablaInserta == 2)
                    //{
                    resultado = instancia.InsertarConfirmacion2(dt, 2);
                    if (resultado == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\FARMACOS\facturas.txt");
                    }
                    //}
                }
                else
                {
                    resultado = false;
                }

            }
            catch (Exception ex)
            {

                renglon.ToString();
                //MessageBox.Show(ex.Message);
                resultado = false;
            }
            return resultado;
        }
        public bool ConfirmaDepot(String ruta, int TablaInserta, String Marca)
        {
            string proveedor = "P01010";
            string NoCliente = "";
            string FolioFactura = "";
            int CantFact = 0;
            float PrecioFarmacia = 0.0f;
            float Oferta = 0.0f;
            float Descuentos = 0.0f;
            float IvaMerc = 0.0f;
            float PorcentaJeOft = 0.0f;
            float DescFact = 0.0f;
            string CodigoEan = "";
            int OrdenCompra = 0;
            string FechaFact = "";
            float ImporteNetoUnit = 0.0f;
            float ImporteNetoTotal = 0.0f;
            float PrecioConOferta = 0.0f;
            float PrecioConDesc = 0.0f;
            float ImporteSinDescXCant = 0.0f;
            string sLine = "";
            int renglon = 0;
            int Opcion = 0;
            bool resultado = false;
            ArrayList arrText = new ArrayList();
            StreamReader objReader = null;
            try
            {
                objReader = new StreamReader(ruta);
                dt = new DataTable();
                dt.Columns.Add("OrdenCompra", typeof(string));//
                dt.Columns.Add("FolioFactura", typeof(string));//
                dt.Columns.Add("NoCliente", typeof(string));//
                dt.Columns.Add("FechaFact", typeof(string));//
                dt.Columns.Add("Proveedor", typeof(string));//
                dt.Columns.Add("CodigoEan", typeof(string));//
                dt.Columns.Add("CantFact", typeof(string));//
                dt.Columns.Add("PrecioFarmacia", typeof(string));//
                dt.Columns.Add("PorcentajeOft", typeof(string));//
                dt.Columns.Add("PrecioOferta", typeof(string));//
                dt.Columns.Add("PrecioConOft", typeof(string));
                dt.Columns.Add("ImporteSinDesXCant", typeof(string));
                dt.Columns.Add("PorcentajeDesc", typeof(string));//
                dt.Columns.Add("PrecioDesc", typeof(string));//
                dt.Columns.Add("PrecioConDesc", typeof(string));
                dt.Columns.Add("IVAImporte", typeof(string));//
                dt.Columns.Add("ImporteNetoUnitario", typeof(string));
                dt.Columns.Add("ImporteNetoTotal", typeof(string));
                dt.Columns.Add("Opcion", typeof(string));//
                dt.Columns.Add("Marca", typeof(string));//
                //dt.Columns.Add("UUID", typeof(string));//
                //MIENTRAS EL RENGLON CONTENGA INFORMACION SE LE ASIGNA A LA LISTA arrText
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
                //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON

                if (TablaInserta == 1)
                {
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            if (sOutput.Length == 40)
                            {
                                renglon += 1;
                                //CodigoEan = sOutput.Substring(85, 14).Trim();
                                //if (CodigoEan == "7502214982484")
                                //{
                                string OrdenCompraTxt = sOutput.Substring(0, 9);
                                long Numero = 0;
                                bool Convertidor1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertidor1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);//
                                    string OpcionTXT = sOutput.Substring(9, 1);
                                    bool Convertidor3 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertidor3 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        NoCliente = sOutput.Substring(10, 10);//
                                        CodigoEan = sOutput.Substring(20, 15);//
                                        string CantFactTxt = sOutput.Substring(35, 5);
                                        bool Convertidor2 = long.TryParse(CantFactTxt, out Numero);
                                        if (Convertidor2 == true)
                                        {
                                            CantFact = int.Parse(CantFactTxt);//                         
                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                            OrdenCompraTxt = "";
                                            CantFactTxt = "";
                                            OrdenCompra = 0;
                                            FolioFactura = "";
                                            NoCliente = "";
                                            FechaFact = "";
                                            CodigoEan = "";
                                            CantFact = 0;
                                            PrecioFarmacia = 0.0f;
                                            PorcentaJeOft = 0.0f;
                                            Oferta = 0.0f;
                                            PrecioConOferta = 0.0f;
                                            DescFact = 0.0f;
                                            Descuentos = 0.0f;
                                            PrecioConDesc = 0.0f;
                                            IvaMerc = 0.0f;
                                            ImporteNetoUnit = 0.0f;
                                            ImporteNetoTotal = 0.0f;
                                        }
                                    }
                                }
                            }
                            //  }
                        }
                    }
                }
                else if (TablaInserta == 2)
                {
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            if (sOutput.Length == 153)//if (sOutput.Length == 189)
                            {
                                renglon += 1;

                                //CodigoEan = sOutput.Substring(50, 15).Trim();
                                //if (CodigoEan == "7501326000864")
                                //{
                                string OrdenCompraTxt = sOutput.Substring(0, 9);
                                long Numero = 0;
                                float Decimal = 0.0f;
                                bool Convertidor1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertidor1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(9, 1);
                                    bool Convertidor3 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertidor3 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = sOutput.Substring(20, 12);//
                                        NoCliente = sOutput.Substring(32, 10);
                                        FechaFact = sOutput.Substring(42, 8);
                                        //proveedor = sOutput.Substring(10, 10);
                                        CodigoEan = sOutput.Substring(50, 15);

                                        string CantFactTxt = sOutput.Substring(65, 4);
                                        bool Convertidor2 = long.TryParse(CantFactTxt, out Numero);
                                        if (Convertidor2 == true)
                                        {
                                            CantFact = int.Parse(CantFactTxt);// 
                                            string PrecioFarmaciaTxt = sOutput.Substring(69, 9);
                                            bool Convertidor12 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                            if (Convertidor12 == true)
                                            {
                                                PrecioFarmacia = float.Parse(PrecioFarmaciaTxt);//
                                                string PorcentaJeOftTxt = sOutput.Substring(78, 6);
                                                bool Convertidor4 = float.TryParse(PorcentaJeOftTxt, out Decimal);
                                                if (Convertidor4 == true)
                                                {
                                                    PorcentaJeOft = float.Parse(PorcentaJeOftTxt);
                                                    string OfertaTxt = sOutput.Substring(84, 9);
                                                    bool Convertidor5 = float.TryParse(OfertaTxt, out Decimal);
                                                    if (Convertidor5 == true)
                                                    {
                                                        Oferta = float.Parse(OfertaTxt);//

                                                        string PrecioConOfertaTxt = sOutput.Substring(93, 9);
                                                        bool Convertidor6 = float.TryParse(PrecioConOfertaTxt, out Decimal);
                                                        if (Convertidor6 == true)
                                                        {
                                                            PrecioConOferta = float.Parse(PrecioConOfertaTxt);
                                                            ImporteSinDescXCant = PrecioConOferta * CantFact;

                                                            string DescFactTxt = sOutput.Substring(102, 6);
                                                            bool Convertidor7 = float.TryParse(DescFactTxt, out Decimal);
                                                            if (Convertidor7 == true)
                                                            {
                                                                DescFact = float.Parse(DescFactTxt);

                                                                string DescuentosTxt = sOutput.Substring(108, 9);
                                                                bool Convertidor8 = float.TryParse(DescuentosTxt, out Decimal);
                                                                if (Convertidor8 == true)
                                                                {
                                                                    Descuentos = float.Parse(DescuentosTxt);//

                                                                    string PrecioConDescTxt = sOutput.Substring(117, 9);
                                                                    bool Convertidor9 = float.TryParse(PrecioConDescTxt, out Decimal);
                                                                    if (Convertidor9 == true)
                                                                    {
                                                                        PrecioConDesc = float.Parse(PrecioConDescTxt);//

                                                                        string IvaMercTxt = sOutput.Substring(126, 9);
                                                                        bool Convertidor10 = float.TryParse(IvaMercTxt, out Decimal);
                                                                        if (Convertidor10 == true)
                                                                        {
                                                                            IvaMerc = (float.Parse(IvaMercTxt)) /*/ CantFact*/;
                                                                            ImporteNetoUnit = PrecioConDesc;

                                                                            string ImporteNetoTotalTxt = sOutput.Substring(144, 9);
                                                                            bool Convertidor11 = float.TryParse(ImporteNetoTotalTxt, out Decimal);
                                                                            if (Convertidor11 == true)
                                                                            {
                                                                                ImporteNetoTotal = float.Parse(ImporteNetoTotalTxt);
                                                                                //string UUID = sOutput.Substring(153, 36);
                                                                                //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca, UUID);
                                                                                dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                                                OrdenCompraTxt = "";
                                                                                CantFactTxt = "";
                                                                                PrecioFarmaciaTxt = "";
                                                                                PorcentaJeOftTxt = "";
                                                                                OfertaTxt = "";
                                                                                PrecioConOfertaTxt = "";
                                                                                OrdenCompra = 0;
                                                                                FolioFactura = "";
                                                                                NoCliente = "";
                                                                                FechaFact = "";
                                                                                CodigoEan = "";
                                                                                CantFact = 0;
                                                                                PrecioFarmacia = 0.0f;
                                                                                PorcentaJeOft = 0.0f;
                                                                                Oferta = 0.0f;
                                                                                PrecioConOferta = 0.0f;
                                                                                DescFact = 0.0f;
                                                                                Descuentos = 0.0f;
                                                                                PrecioConDesc = 0.0f;
                                                                                IvaMerc = 0.0f;
                                                                                ImporteNetoUnit = 0.0f;
                                                                                ImporteNetoTotal = 0.0f;
                                                                                //UUID = "";

                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                        }

                    }
                }
                if (dt.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    if (TablaInserta == 1 || TablaInserta == 2)
                    {
                        resultado = instancia.InsertarConfirmacion2(dt, 2);
                        if (resultado == true)
                        {
                            File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\DEPOT\facturas.txt");
                        }
                    }
                    else if (TablaInserta == 3)
                    {
                        resultado = instancia.InsertarConfirmacion2(dt, 2);
                        if (resultado == true)
                        {
                            File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\DEPOT\facturas.txt");
                        }
                    }

                }
                else
                {
                    resultado = false;
                }

            }
            catch (Exception ex)
            {

                renglon.ToString();
                //  MessageBox.Show(ex.Message);
                resultado = false;
            }
            return resultado;
        }
        public bool ConfirmaNadro(String ruta, int TablaInserta, String Marca)
        {
            string OrdenCompraTxt = "";
            string proveedor = "";
            string NoCliente = "";
            string FolioFactura = "";
            int CantFact = 0;
            float PrecioFarmacia = 0.0f;
            float Oferta = 0.0f;
            float Descuentos = 0.0f;
            float IvaMerc = 0.0f;
            float PorcentaJeOft = 0.0f;
            float DescFact = 0.0f;
            string CodigoEan = "";
            int OrdenCompra = 0;
            string FechaFact = "";
            float ImporteNetoUnit = 0.0f;
            float ImporteNetoTotal = 0.0f;
            float PrecioConOferta = 0.0f;
            float PrecioConDesc = 0.0f;
            float ImporteSinDescXCant = 0.0f;
            string sLine = "";
            int renglon = 0;
            bool resultado = false;
            bool resultado1 = false;
            bool resultado2 = false;
            int Opcion = 0;

            int OrdenCompraA = 0;
            String FolioFacturaA = "";
            String NoClienteA = "";
            String FechaFactA = "";
            int OpcionA = 0;


            try
            {
                instancia = new Instancia();
                dt = new DataTable();
                ArrayList arrText = new ArrayList();
                StreamReader objReader = null;


                objReader = new StreamReader(ruta);
                dt = new DataTable();

                dt.Columns.Add("OrdenCompra", typeof(string));//
                dt.Columns.Add("FolioFactura", typeof(string));//
                dt.Columns.Add("NoCliente", typeof(string));//
                dt.Columns.Add("FechaFact", typeof(string));//
                dt.Columns.Add("Proveedor", typeof(string));//
                dt.Columns.Add("CodigoEan", typeof(string));//
                dt.Columns.Add("CantFact", typeof(string));//
                dt.Columns.Add("PrecioFarmacia", typeof(string));//
                dt.Columns.Add("PorcentajeOft", typeof(string));//
                dt.Columns.Add("PrecioOferta", typeof(string));//
                dt.Columns.Add("PrecioConOft", typeof(string));
                dt.Columns.Add("ImporteSinDesXCant", typeof(string));
                dt.Columns.Add("PorcentajeDesc", typeof(string));//
                dt.Columns.Add("PrecioDesc", typeof(string));//
                dt.Columns.Add("PrecioConDesc", typeof(string));
                dt.Columns.Add("IVAImporte", typeof(string));//
                dt.Columns.Add("ImporteNetoUnitario", typeof(string));
                dt.Columns.Add("ImporteNetoTotal", typeof(string));
                dt.Columns.Add("Opcion", typeof(string));
                dt.Columns.Add("Marca", typeof(string));//

                dt2 = new DataTable();
                dt2.Columns.Add("NumOrdenCompra", typeof(string));//S
                dt2.Columns.Add("NumeroCliente", typeof(string));//S
                dt2.Columns.Add("FechaFaltante", typeof(string));//S
                dt2.Columns.Add("Codigo", typeof(string));//S
                dt2.Columns.Add("PiezasFacturadas", typeof(string));//S
                dt2.Columns.Add("PiezasFaltantes", typeof(string));//S
                dt2.Columns.Add("MotivoFaltante", typeof(string));//S
                dt2.Columns.Add("proveedor", typeof(string));//
                dt2.Columns.Add("Opcion", typeof(string));
                dt2.Columns.Add("Marca", typeof(string));
                //MIENTRAS EL RENGLON CONTENGA INFORMACION SE LE ASIGNA A LA LISTA arrText
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
                if (TablaInserta == 1)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 40 && sOutput.Contains("N"))
                            {
                                OrdenCompraTxt = "";
                                OrdenCompra = 0;
                                OrdenCompraTxt = sOutput.Substring(17, 15).Trim();
                                long Numero = 0;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    string OrdenCompraS = OrdenCompraTxt.Substring(0, OrdenCompraTxt.Length - 1);
                                    OrdenCompra = int.Parse(OrdenCompraS);
                                    string OpcionTXT = OrdenCompraTxt.Substring(OrdenCompraTxt.Length - 1, 1);
                                    bool Convertir2 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir2 == true)
                                    {

                                        Opcion = 0;
                                        Opcion = int.Parse(OpcionTXT);
                                        NoCliente = "";
                                        NoCliente = sOutput.Substring(6, 7);
                                        FolioFactura = "";
                                        FechaFact = "";
                                        proveedor = "P01002";
                                    }
                                }
                            }
                            if (sOutput.Length == 20)
                            {

                                if (sOutput.Substring(0, 14).Trim().ToString() != "")
                                {
                                    long Numero = 0;
                                    CodigoEan = sOutput.Substring(0, 14).Trim().ToString();

                                    string CantFactTxt = sOutput.Substring(14, 6);
                                    bool Convertir3 = long.TryParse(CantFactTxt, out Numero);
                                    if (Convertir3 == true)
                                    {
                                        CantFact = int.Parse(CantFactTxt);// 
                                        PrecioFarmacia = 0.0f;//                           
                                        PorcentaJeOft = 0.0f;
                                        Oferta = 0.0f;//
                                        PrecioConOferta = 0.0f;
                                        ImporteSinDescXCant = 0.0f;
                                        DescFact = 0.0f;
                                        Descuentos = 0.0f;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     
                                        IvaMerc = 0.0f;
                                        PrecioConDesc = 0.0f;
                                        ImporteNetoUnit = 0.0f;
                                        ImporteNetoTotal = 0.0f;
                                        if (CantFact > 0)
                                        {
                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                        }
                                        CantFactTxt = "";
                                        FolioFactura = "";
                                        FechaFact = "";
                                        CodigoEan = "";
                                        CantFact = 0;
                                        PrecioFarmacia = 0.0f;
                                        PorcentaJeOft = 0.0f;
                                        Oferta = 0.0f;
                                        PrecioConOferta = 0.0f;
                                        DescFact = 0.0f;
                                        Descuentos = 0.0f;
                                        PrecioConDesc = 0.0f;
                                        IvaMerc = 0.0f;
                                        ImporteNetoUnit = 0.0f;
                                        ImporteNetoTotal = 0.0f;
                                    }
                                }


                            }
                        }
                    }
                    //  }
                }
                else if (TablaInserta == 2)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        renglon++;
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            /* if (sOutput.Length == 20 && sOutput.Contains("C"))//
                             {
                                 FechaFact = "";
                                 FechaFact = sOutput.Substring(6, 8);//
                             }*/
                            if (sOutput.Length == 33 && sOutput.Contains("C:"))//
                            {
                                OrdenCompra = 0;
                                OrdenCompraTxt = "";
                                NoCliente = sOutput.Substring(3, 7);//
                                OrdenCompraTxt = sOutput.Substring(18, 15);
                                long Numero = 0;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {

                                    string OrdenCompraS = OrdenCompraTxt.Substring(0, OrdenCompraTxt.Length - 1);
                                    if (OrdenCompraS.Trim() != "")
                                    {
                                        OrdenCompra = int.Parse(OrdenCompraS);
                                        string OpcionTXT = OrdenCompraTxt.Substring(OrdenCompraTxt.Length - 1, 1);
                                        Opcion = int.Parse(OpcionTXT);
                                    }

                                }
                            }

                            if (sOutput.Length == 104 && sOutput.Contains("F"))
                            {
                                FolioFactura = "";
                                FechaFact = "";
                                FechaFact = sOutput.Substring(17, 8);//
                                FolioFactura = sOutput.Substring(91, 12);//  

                                OrdenCompraA = OrdenCompra;
                                FolioFacturaA = FolioFactura;
                                NoClienteA = NoCliente;
                                FechaFactA = FechaFact;
                                OpcionA = Opcion;

                                OrdenCompra = 0;
                                FolioFactura = "";
                                NoCliente = "";
                                FechaFact = "";
                                Opcion = 0;
                            }

                            if ((sOutput.Length == 110 || sOutput.Length == 109) && sOutput.Contains("P"))
                            {
                                //OrdenCompra,FolioFactura,NoCliente,FechaFact,Opcion,Marca
                                long Numero = 0;
                                float Decimal = 0.0f;
                                proveedor = "P01002";
                                string CantFactTxt = sOutput.Substring(9, 6);
                                bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                if (Convertir2 == true)
                                {
                                    CantFact = int.Parse(CantFactTxt);// 

                                    string PrecioFarmaciaTxt = sOutput.Substring(26, 8);
                                    bool Convertir3 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                    if (Convertir3 == true)
                                    {
                                        PrecioFarmacia = float.Parse(PrecioFarmaciaTxt) / 100;
                                        PorcentaJeOft = 0.0f;

                                        if ((sOutput.Substring(89, 14).Trim()) != "")
                                        {
                                            CodigoEan = sOutput.Substring(89, 14).Trim().ToString();//                                                
                                            Oferta = 0.0f;

                                            string PrecioPublicoTxt = sOutput.Substring(26, 8).Trim().ToString();//      
                                            bool Convertir4 = float.TryParse(PrecioPublicoTxt, out Decimal);
                                            if (Convertir4 == true)
                                            {
                                                PrecioConOferta = float.Parse(PrecioPublicoTxt) / 100;
                                                ImporteSinDescXCant = PrecioConOferta * CantFact;
                                                DescFact = 0.0f;
                                                Descuentos = 0.0f;
                                                PrecioConDesc = PrecioConOferta - Descuentos;
                                                ImporteNetoUnit = PrecioConDesc;
                                                ImporteNetoTotal = PrecioConDesc * CantFact;
                                                dt.Rows.Add(OrdenCompraA, FolioFacturaA, NoClienteA, FechaFactA, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, OpcionA, Marca);
                                                CantFactTxt = "";
                                                PrecioFarmaciaTxt = "";

                                                CodigoEan = "";
                                                CantFact = 0;
                                                PrecioFarmacia = 0.0f;
                                                PorcentaJeOft = 0.0f;
                                                Oferta = 0.0f;
                                                PrecioConOferta = 0.0f;
                                                DescFact = 0.0f;
                                                Descuentos = 0.0f;
                                                PrecioConDesc = 0.0f;
                                                IvaMerc = 0.0f;
                                                ImporteNetoUnit = 0.0f;
                                                ImporteNetoTotal = 0.0f;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                if (dt.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    resultado1 = instancia.InsertarConfirmacion2(dt, 2);
                    if (resultado1 == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\NADRO\facturas.txt");
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\NADRO\facturas.txt");
                    }

                }
                else
                {
                    resultado1 = false;
                }
                if (dt2.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    resultado2 = instancia.InsertarFaltante(dt2);
                    if (resultado == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\NADRO\facturas.txt");
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\NADRO\facturas.txt");
                    }
                }
                else
                {
                    resultado2 = false;
                }
                if (resultado1 == true || resultado2 == true)
                {
                    resultado = true;
                    if (resultado == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\NADRO\facturas.txt");
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\NADRO\facturas.txt");
                    }
                }

            }
            catch (Exception ex)
            {

                renglon.ToString();
                // MessageBox.Show(ex.Message + " Renglon No." + renglon.ToString());
                resultado = false;
            }
            return resultado;
        }
        public bool ConfirmaMedipac(String ruta, int TablaInserta, String Marca)
        {
            string proveedor = "";
            string NoCliente = "";
            string FolioFactura = "";
            int CantFact = 0;
            float PrecioFarmacia = 0.0f;
            float Oferta = 0.0f;
            float Descuentos = 0.0f;
            float IvaMerc = 0.0f;
            float PorcentaJeOft = 0.0f;
            float DescFact = 0.0f;
            string CodigoEan = "";
            int OrdenCompra = 0;
            string FechaFact = "";
            float ImporteNetoUnit = 0.0f;
            float ImporteNetoTotal = 0.0f;
            float PrecioConOferta = 0.0f;
            float PrecioConDesc = 0.0f;
            float ImporteSinDescXCant = 0.0f;
            string sLine = "";
            int renglon = 0;
            bool resultado = false;
            bool resultado1 = false;
            bool resultado2 = false;
            int Opcion = 0;
            //string UUID = "";
            ArrayList arrText = new ArrayList();
            try
            {
                StreamReader objReader = null;
                objReader = new StreamReader(ruta);

                dt = new DataTable();
                dt.Columns.Add("OrdenCompra", typeof(string));//
                dt.Columns.Add("FolioFactura", typeof(string));//
                dt.Columns.Add("NoCliente", typeof(string));//
                dt.Columns.Add("FechaFact", typeof(string));//
                dt.Columns.Add("Proveedor", typeof(string));//
                dt.Columns.Add("CodigoEan", typeof(string));//
                dt.Columns.Add("CantFact", typeof(string));//
                dt.Columns.Add("PrecioFarmacia", typeof(string));//
                dt.Columns.Add("PorcentajeOft", typeof(string));//
                dt.Columns.Add("PrecioOferta", typeof(string));//
                dt.Columns.Add("PrecioConOft", typeof(string));
                dt.Columns.Add("ImporteSinDesXCant", typeof(string));
                dt.Columns.Add("PorcentajeDesc", typeof(string));//
                dt.Columns.Add("PrecioDesc", typeof(string));//
                dt.Columns.Add("PrecioConDesc", typeof(string));
                dt.Columns.Add("IVAImporte", typeof(string));//
                dt.Columns.Add("ImporteNetoUnitario", typeof(string));
                dt.Columns.Add("ImporteNetoTotal", typeof(string));
                dt.Columns.Add("Opcion", typeof(string));
                dt.Columns.Add("Marca", typeof(string));
                //dt.Columns.Add("UUID", typeof(string));


                dt2 = new DataTable();
                dt2.Columns.Add("NumOrdenCompra", typeof(string));//S
                dt2.Columns.Add("NumeroCliente", typeof(string));//S
                dt2.Columns.Add("FechaFaltante", typeof(string));//S
                dt2.Columns.Add("Codigo", typeof(string));//S
                dt2.Columns.Add("PiezasFacturadas", typeof(string));//S
                dt2.Columns.Add("PiezasFaltantes", typeof(string));//S
                dt2.Columns.Add("MotivoFaltante", typeof(string));//S
                dt2.Columns.Add("proveedor", typeof(string));//
                dt2.Columns.Add("Opcion", typeof(string));
                dt2.Columns.Add("Marca", typeof(string));
                //MIENTRAS EL RENGLON CONTENGA INFORMACION SE LE ASIGNA A LA LISTA arrText
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
                if (TablaInserta == 1)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 89)
                            {
                                string OrdenCompraTxt = sOutput.Substring(23, 8);
                                long Numero = 0;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);

                                    string OpcionTXT = sOutput.Substring(31, 1);
                                    bool Convertir3 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir3 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = "";//
                                        NoCliente = sOutput.Substring(2, 5);
                                        FechaFact = "";
                                        proveedor = "P01004";
                                        if (sOutput.Substring(7, 13).Trim().ToString() != "")
                                        {
                                            CodigoEan = sOutput.Substring(7, 13).Trim().ToString();
                                            string CantFactTxt = sOutput.Substring(20, 3);
                                            bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                            if (Convertir2 == true)
                                            {
                                                CantFact = int.Parse(CantFactTxt);// 
                                                PrecioFarmacia = 0.0f;//                           
                                                PorcentaJeOft = 0.0f;
                                                Oferta = 0.0f;//
                                                PrecioConOferta = 0.0f;
                                                ImporteSinDescXCant = 0.0f;
                                                DescFact = 0.0f;
                                                Descuentos = 0.0f;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     
                                                IvaMerc = 0.0f;
                                                PrecioConDesc = 0.0f;
                                                ImporteNetoUnit = 0.0f;
                                                ImporteNetoTotal = 0.0f;
                                                if (CantFact > 0)
                                                {
                                                    //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion,Marca,UUID);
                                                    dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                }
                                                else if (CantFact == 0)
                                                {
                                                    dt2.Rows.Add(OrdenCompra, NoCliente, FechaFact, CodigoEan, CantFact, 0, 0, proveedor, Opcion, Marca);
                                                }
                                                OrdenCompraTxt = "";
                                                CantFactTxt = "";
                                                OrdenCompra = 0;
                                                FolioFactura = "";
                                                NoCliente = "";
                                                FechaFact = "";
                                                CodigoEan = "";
                                                CantFact = 0;
                                                PrecioFarmacia = 0.0f;
                                                PorcentaJeOft = 0.0f;
                                                Oferta = 0.0f;
                                                PrecioConOferta = 0.0f;
                                                DescFact = 0.0f;
                                                Descuentos = 0.0f;
                                                PrecioConDesc = 0.0f;
                                                IvaMerc = 0.0f;
                                                ImporteNetoUnit = 0.0f;
                                                ImporteNetoTotal = 0.0f;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //  }
                }
                else if (TablaInserta == 2)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 267)//if (sOutput.Length == 317)
                            {
                                string OrdenCompraTxt = sOutput.Substring(242, 8);
                                long Numero = 0;
                                float Decimal = 0.0f;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(250, 1);
                                    bool Convertir9 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir9 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = sOutput.Substring(8, 10);//
                                        if (FolioFactura.Contains("FY"))
                                        {
                                            NoCliente = sOutput.Substring(2, 5);//
                                            FechaFact = sOutput.Substring(18, 8);//
                                            proveedor = "P01004";

                                            if ((sOutput.Substring(75, 13).Trim()) != "")
                                            {
                                                CodigoEan = sOutput.Substring(75, 13).Trim().ToString();//
                                                string CantFactTxt = sOutput.Substring(90, 7);
                                                bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                                if (Convertir2 == true)
                                                {
                                                    CantFact = int.Parse(CantFactTxt);//   

                                                    string PrecioFarmaciaTxt = sOutput.Substring(104, 10);
                                                    bool Convertir3 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                                    if (Convertir3 == true)
                                                    {
                                                        PrecioFarmacia = float.Parse(PrecioFarmaciaTxt);//

                                                        string PorcentaJeOftTxt = sOutput.Substring(147, 6);
                                                        bool Convertir4 = float.TryParse(PorcentaJeOftTxt, out Decimal);
                                                        if (Convertir4 == true)
                                                        {
                                                            PorcentaJeOft = float.Parse(PorcentaJeOftTxt);//

                                                            string OfertaTxt = sOutput.Substring(153, 13);
                                                            bool Convertir5 = float.TryParse(OfertaTxt, out Decimal);
                                                            if (Convertir5 == true)
                                                            {
                                                                Oferta = (float.Parse(OfertaTxt)) / CantFact;//
                                                                PrecioConOferta = PrecioFarmacia - Oferta;
                                                                ImporteSinDescXCant = PrecioConOferta * CantFact;

                                                                string DescFactTxt = sOutput.Substring(166, 6);
                                                                bool Convertir6 = float.TryParse(DescFactTxt, out Decimal);
                                                                if (Convertir6 == true)
                                                                {
                                                                    DescFact = float.Parse(DescFactTxt);//

                                                                    string DescuentosTxt = sOutput.Substring(172, 13);
                                                                    bool Convertir7 = float.TryParse(DescuentosTxt, out Decimal);
                                                                    if (Convertir7 == true)
                                                                    {
                                                                        Descuentos = float.Parse(DescuentosTxt) / CantFact;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     

                                                                        string IvaMercTxt = sOutput.Substring(198, 13);
                                                                        bool Convertir8 = float.TryParse(IvaMercTxt, out Decimal);
                                                                        if (Convertir8 == true)
                                                                        {
                                                                            IvaMerc = float.Parse(IvaMercTxt) / CantFact;
                                                                            PrecioConDesc = PrecioConOferta - Descuentos;
                                                                            ImporteNetoUnit = PrecioConDesc;
                                                                            ImporteNetoTotal = PrecioConDesc * CantFact;
                                                                            //UUID = sOutput.Substring(270, 36);
                                                                            //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal,Opcion,Marca,UUID);
                                                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                                            IvaMercTxt = "";
                                                                            DescuentosTxt = "";
                                                                            DescFactTxt = "";
                                                                            OfertaTxt = "";
                                                                            PorcentaJeOftTxt = "";
                                                                            PrecioFarmaciaTxt = "";
                                                                            CantFactTxt = "";
                                                                            OrdenCompraTxt = "";
                                                                            OrdenCompra = 0;
                                                                            FolioFactura = "";
                                                                            NoCliente = "";
                                                                            FechaFact = "";
                                                                            CodigoEan = "";
                                                                            CantFact = 0;
                                                                            PrecioFarmacia = 0.0f;
                                                                            PorcentaJeOft = 0.0f;
                                                                            Oferta = 0.0f;
                                                                            PrecioConOferta = 0.0f;
                                                                            DescFact = 0.0f;
                                                                            Descuentos = 0.0f;
                                                                            PrecioConDesc = 0.0f;
                                                                            IvaMerc = 0.0f;
                                                                            ImporteNetoUnit = 0.0f;
                                                                            ImporteNetoTotal = 0.0f;
                                                                            //UUID = "";
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (TablaInserta == 3)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 267)//if (sOutput.Length == 317)
                            {
                                string OrdenCompraTxt = sOutput.Substring(242, 8);
                                long Numero = 0;
                                float Decimal = 0.0f;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(250, 1);
                                    bool Convertir9 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir9 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = sOutput.Substring(8, 10);//
                                        if (FolioFactura.Contains("FY"))
                                        {
                                            NoCliente = sOutput.Substring(2, 5);//
                                            FechaFact = sOutput.Substring(18, 8);//
                                            proveedor = "P01004";

                                            if ((sOutput.Substring(75, 13).Trim()) != "")
                                            {
                                                CodigoEan = sOutput.Substring(75, 13).Trim().ToString();//
                                                string CantFactTxt = sOutput.Substring(90, 7);
                                                bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                                if (Convertir2 == true)
                                                {
                                                    CantFact = int.Parse(CantFactTxt);//   

                                                    string PrecioFarmaciaTxt = sOutput.Substring(257, 10);//sOutput.Substring(104, 10);
                                                    bool Convertir3 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                                    if (Convertir3 == true)
                                                    {
                                                        PrecioFarmacia = float.Parse(PrecioFarmaciaTxt);//

                                                        string PorcentaJeOftTxt = "0";//sOutput.Substring(147, 6);
                                                        bool Convertir4 = float.TryParse(PorcentaJeOftTxt, out Decimal);
                                                        if (Convertir4 == true)
                                                        {
                                                            PorcentaJeOft = 0.0f;//float.Parse(PorcentaJeOftTxt);//

                                                            string OfertaTxt = "0";//sOutput.Substring(153, 13);
                                                            bool Convertir5 = float.TryParse(OfertaTxt, out Decimal);
                                                            if (Convertir5 == true)
                                                            {
                                                                Oferta = (float.Parse(OfertaTxt)) / CantFact;//
                                                                PrecioConOferta = PrecioFarmacia - Oferta;
                                                                ImporteSinDescXCant = PrecioConOferta * CantFact;

                                                                string DescFactTxt = "0";//sOutput.Substring(166, 6);
                                                                bool Convertir6 = float.TryParse(DescFactTxt, out Decimal);
                                                                if (Convertir6 == true)
                                                                {
                                                                    DescFact = 0.0f;//float.Parse(DescFactTxt);//

                                                                    string DescuentosTxt = "0";//sOutput.Substring(172, 13);
                                                                    bool Convertir7 = float.TryParse(DescuentosTxt, out Decimal);
                                                                    if (Convertir7 == true)
                                                                    {
                                                                        Descuentos = float.Parse(DescuentosTxt) / CantFact;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     

                                                                        string IvaMercTxt = sOutput.Substring(198, 13);
                                                                        bool Convertir8 = float.TryParse(IvaMercTxt, out Decimal);
                                                                        if (Convertir8 == true)
                                                                        {
                                                                            IvaMerc = float.Parse(IvaMercTxt) / CantFact;
                                                                            PrecioConDesc = PrecioConOferta - Descuentos;
                                                                            ImporteNetoUnit = PrecioConDesc;
                                                                            ImporteNetoTotal = PrecioConDesc * CantFact;
                                                                            //UUID = sOutput.Substring(270, 36);
                                                                            //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal,Opcion,Marca,UUID);
                                                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                                            IvaMercTxt = "";
                                                                            DescuentosTxt = "";
                                                                            DescFactTxt = "";
                                                                            OfertaTxt = "";
                                                                            PorcentaJeOftTxt = "";
                                                                            PrecioFarmaciaTxt = "";
                                                                            CantFactTxt = "";
                                                                            OrdenCompraTxt = "";
                                                                            OrdenCompra = 0;
                                                                            FolioFactura = "";
                                                                            NoCliente = "";
                                                                            FechaFact = "";
                                                                            CodigoEan = "";
                                                                            CantFact = 0;
                                                                            PrecioFarmacia = 0.0f;
                                                                            PorcentaJeOft = 0.0f;
                                                                            Oferta = 0.0f;
                                                                            PrecioConOferta = 0.0f;
                                                                            DescFact = 0.0f;
                                                                            Descuentos = 0.0f;
                                                                            PrecioConDesc = 0.0f;
                                                                            IvaMerc = 0.0f;
                                                                            ImporteNetoUnit = 0.0f;
                                                                            ImporteNetoTotal = 0.0f;
                                                                            //UUID = "";
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    if (TablaInserta == 1 || TablaInserta == 2)
                    {
                        resultado = instancia.InsertarConfirmacion2(dt, 2);
                        if (resultado == true)
                        {
                            File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MEDIPAC\facturas.txt");
                        }
                    }
                    else if (TablaInserta == 3)
                    {
                        resultado = instancia.InsertarConfirmacion2(dt, 2);
                        if (resultado == true)
                        {
                            File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MEDIPAC\facturas.txt");
                        }
                    }

                }
                else
                {
                    resultado1 = false;
                }
                if (dt2.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    resultado2 = instancia.InsertarFaltante(dt2);
                    if (resultado2 == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MEDIPAC\facturas.txt");
                    }
                }
                else
                {
                    resultado2 = false;
                }
                if (resultado1 == true || resultado2 == true)
                {
                    resultado = true;
                    File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MEDIPAC\facturas.txt");
                    
                }
            }
            catch (Exception ex)
            {

                renglon.ToString();
                // MessageBox.Show(ex.Message);
                resultado = false;
            }
            return resultado;
        }
        public bool ConfirmaMarzam(String ruta, int TablaInserta, String Marca)
        {
            string proveedor = "";
            string NoCliente = "";
            string FolioFactura = "";
            int CantFact = 0;
            float PrecioFarmacia = 0.0f;
            float Oferta = 0.0f;
            float Descuentos = 0.0f;
            float IvaMerc = 0.0f;
            float PorcentaJeOft = 0.0f;
            float DescFact = 0.0f;
            string CodigoEan = "";
            int OrdenCompra = 0;
            string FechaFact = "";
            float ImporteNetoUnit = 0.0f;
            float ImporteNetoTotal = 0.0f;
            float PrecioConOferta = 0.0f;
            float PrecioConDesc = 0.0f;
            float ImporteSinDescXCant = 0.0f;
            string sLine = "";
            //string UUID = "";
            int renglon = 0;
            bool resultado = false;
            bool resultado1 = false;
            bool resultado2 = false;
            ArrayList arrText = new ArrayList();
            int Opcion = 0;
            try
            {

                StreamReader objReader = null;
                objReader = new StreamReader(ruta);
                dt = new DataTable();
                dt.Columns.Add("OrdenCompra", typeof(string));//
                dt.Columns.Add("FolioFactura", typeof(string));//
                dt.Columns.Add("NoCliente", typeof(string));//
                dt.Columns.Add("FechaFact", typeof(string));//
                dt.Columns.Add("Proveedor", typeof(string));//
                dt.Columns.Add("CodigoEan", typeof(string));//
                dt.Columns.Add("CantFact", typeof(string));//
                dt.Columns.Add("PrecioFarmacia", typeof(string));//
                dt.Columns.Add("PorcentajeOft", typeof(string));//
                dt.Columns.Add("PrecioOferta", typeof(string));//
                dt.Columns.Add("PrecioConOft", typeof(string));
                dt.Columns.Add("ImporteSinDesXCant", typeof(string));
                dt.Columns.Add("PorcentajeDesc", typeof(string));//
                dt.Columns.Add("PrecioDesc", typeof(string));//
                dt.Columns.Add("PrecioConDesc", typeof(string));
                dt.Columns.Add("IVAImporte", typeof(string));//
                dt.Columns.Add("ImporteNetoUnitario", typeof(string));
                dt.Columns.Add("ImporteNetoTotal", typeof(string));
                dt.Columns.Add("Opcion", typeof(string));
                dt.Columns.Add("Marca", typeof(string));
                //dt.Columns.Add("UUID", typeof(string));
                dt2 = new DataTable();
                dt2.Columns.Add("NumOrdenCompra", typeof(string));//S
                dt2.Columns.Add("NumeroCliente", typeof(string));//S
                dt2.Columns.Add("FechaFaltante", typeof(string));//S
                dt2.Columns.Add("Codigo", typeof(string));//S
                dt2.Columns.Add("PiezasFacturadas", typeof(string));//S
                dt2.Columns.Add("PiezasFaltantes", typeof(string));//S
                dt2.Columns.Add("MotivoFaltante", typeof(string));//S
                dt2.Columns.Add("proveedor", typeof(string));//
                dt2.Columns.Add("Opcion", typeof(string));
                dt2.Columns.Add("Marca", typeof(string));
                //MIENTRAS EL RENGLON CONTENGA INFORMACION SE LE ASIGNA A LA LISTA arrText
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();
                if (TablaInserta == 1)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 89)
                            {
                                string OrdenCompraTxt = sOutput.Substring(23, 8);
                                long Numero = 0;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(31, 1);
                                    bool Convertir3 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir3 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = "";//
                                        NoCliente = sOutput.Substring(2, 5);
                                        FechaFact = "";
                                        proveedor = "P01005";
                                        if (sOutput.Substring(7, 13).Trim().ToString() != "")
                                        {
                                            CodigoEan = sOutput.Substring(7, 13).Trim().ToString();
                                            string CantFactTxt = sOutput.Substring(20, 3);
                                            bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                            if (Convertir2 == true)
                                            {
                                                CantFact = int.Parse(CantFactTxt);// 
                                                PrecioFarmacia = 0.0f;//                           
                                                PorcentaJeOft = 0.0f;
                                                Oferta = 0.0f;//
                                                PrecioConOferta = 0.0f;
                                                ImporteSinDescXCant = 0.0f;
                                                DescFact = 0.0f;
                                                Descuentos = 0.0f;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     
                                                IvaMerc = 0.0f;
                                                PrecioConDesc = 0.0f;
                                                ImporteNetoUnit = 0.0f;
                                                ImporteNetoTotal = 0.0f;

                                                if (CantFact > 0)
                                                {
                                                    dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                }
                                                else if (CantFact == 0)
                                                {
                                                    dt2.Rows.Add(OrdenCompra, NoCliente, FechaFact, CodigoEan, CantFact, 0, 0, proveedor, Opcion, Marca);
                                                }
                                                CantFactTxt = "";
                                                OrdenCompraTxt = "";
                                                OrdenCompra = 0;
                                                FolioFactura = "";
                                                NoCliente = "";
                                                FechaFact = "";
                                                CodigoEan = "";
                                                CantFact = 0;
                                                PrecioFarmacia = 0.0f;
                                                PorcentaJeOft = 0.0f;
                                                Oferta = 0.0f;
                                                PrecioConOferta = 0.0f;
                                                DescFact = 0.0f;
                                                Descuentos = 0.0f;
                                                PrecioConDesc = 0.0f;
                                                IvaMerc = 0.0f;
                                                ImporteNetoUnit = 0.0f;
                                                ImporteNetoTotal = 0.0f;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //  }
                }
                else if (TablaInserta == 2)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 267)//if (sOutput.Length == 318)
                            {
                                string OrdenCompraTxt = sOutput.Substring(242, 8);
                                long Numero = 0;
                                float Decimal = 0.0f;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(250, 1);
                                    bool Convertir9 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir9 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = sOutput.Substring(8, 10);//
                                        if (FolioFactura.Contains("FC") || FolioFactura.Contains("FJ"))
                                        {
                                            NoCliente = sOutput.Substring(2, 5);//
                                            FechaFact = sOutput.Substring(18, 8);//
                                            proveedor = "P01005";
                                            if ((sOutput.Substring(75, 13).Trim()) != "")
                                            {
                                                CodigoEan = sOutput.Substring(75, 13).Trim().ToString();//
                                                string CantFactTxt = sOutput.Substring(90, 7);
                                                bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                                if (Convertir2 == true)
                                                {
                                                    CantFact = int.Parse(CantFactTxt);//   

                                                    string PrecioFarmaciaTxt = sOutput.Substring(104, 10);
                                                    bool Convertir3 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                                    if (Convertir3 == true)
                                                    {
                                                        PrecioFarmacia = float.Parse(PrecioFarmaciaTxt);//

                                                        string PorcentaJeOftTxt = sOutput.Substring(147, 6);
                                                        bool Convertir4 = float.TryParse(PorcentaJeOftTxt, out Decimal);
                                                        if (Convertir4 == true)
                                                        {
                                                            PorcentaJeOft = float.Parse(PorcentaJeOftTxt);//

                                                            string OfertaTxt = sOutput.Substring(153, 13);
                                                            bool Convertir5 = float.TryParse(OfertaTxt, out Decimal);
                                                            if (Convertir5 == true)
                                                            {
                                                                Oferta = (float.Parse(OfertaTxt)) / CantFact;//
                                                                PrecioConOferta = PrecioFarmacia - Oferta;
                                                                ImporteSinDescXCant = PrecioConOferta * CantFact;

                                                                string DescFactTxt = sOutput.Substring(166, 6);
                                                                bool Convertir6 = float.TryParse(DescFactTxt, out Decimal);
                                                                if (Convertir6 == true)
                                                                {
                                                                    DescFact = float.Parse(DescFactTxt);//

                                                                    string DescuentosTxt = sOutput.Substring(172, 13);
                                                                    bool Convertir7 = float.TryParse(DescuentosTxt, out Decimal);
                                                                    if (Convertir7 == true)
                                                                    {
                                                                        Descuentos = float.Parse(DescuentosTxt) / CantFact;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     

                                                                        string IvaMercTxt = sOutput.Substring(198, 13);
                                                                        bool Convertir8 = float.TryParse(IvaMercTxt, out Decimal);
                                                                        if (Convertir8 == true)
                                                                        {
                                                                            IvaMerc = float.Parse(IvaMercTxt) / CantFact;
                                                                            PrecioConDesc = PrecioConOferta - Descuentos;
                                                                            ImporteNetoUnit = PrecioConDesc;
                                                                            ImporteNetoTotal = PrecioConDesc * CantFact;
                                                                            //UUID = sOutput.Substring(270, 36);
                                                                            //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca,UUID);
                                                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                                            OrdenCompraTxt = "";
                                                                            CantFactTxt = "";
                                                                            PrecioFarmaciaTxt = "";
                                                                            PorcentaJeOftTxt = "";
                                                                            OfertaTxt = "";
                                                                            DescFactTxt = "";
                                                                            DescuentosTxt = "";
                                                                            IvaMercTxt = "";
                                                                            OrdenCompra = 0;
                                                                            FolioFactura = "";
                                                                            NoCliente = "";
                                                                            FechaFact = "";
                                                                            CodigoEan = "";
                                                                            CantFact = 0;
                                                                            PrecioFarmacia = 0.0f;
                                                                            PorcentaJeOft = 0.0f;
                                                                            Oferta = 0.0f;
                                                                            PrecioConOferta = 0.0f;
                                                                            DescFact = 0.0f;
                                                                            Descuentos = 0.0f;
                                                                            PrecioConDesc = 0.0f;
                                                                            IvaMerc = 0.0f;
                                                                            ImporteNetoUnit = 0.0f;
                                                                            ImporteNetoTotal = 0.0f;
                                                                            //UUID = "";

                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (TablaInserta == 3)
                {
                    //COMIENZA LECTURA DE RENGLONES PARA REALIZAR LAS SEPARACIONES EN BASE A LA LONGITUD DEL CAMPO 
                    //SE ASIGNAN LOS DATOS YA SEPARADOS A LAS COLUMNAS CORRESPONDIENTES DE UN DATATABLE PARA QUE 
                    //ESTE SE INSERTE TAL Y COMO ESTA DE UN SOLO JALON
                    foreach (string sOutput in arrText)
                    {
                        if (sOutput.Trim() != "")
                        {
                            //CodigoEan = sOutput.Substring(85, 14).Trim();
                            //if (CodigoEan == "7502214982484")
                            //{
                            if (sOutput.Length == 267)//if (sOutput.Length == 318)
                            {
                                string OrdenCompraTxt = sOutput.Substring(242, 8);
                                long Numero = 0;
                                float Decimal = 0.0f;
                                bool Convertir1 = long.TryParse(OrdenCompraTxt, out Numero);
                                if (Convertir1 == true)
                                {
                                    OrdenCompra = int.Parse(OrdenCompraTxt);
                                    string OpcionTXT = sOutput.Substring(250, 1);
                                    bool Convertir9 = long.TryParse(OpcionTXT, out Numero);
                                    if (Convertir9 == true)
                                    {
                                        Opcion = int.Parse(OpcionTXT);
                                        FolioFactura = sOutput.Substring(8, 10);//
                                        if (FolioFactura.Contains("FC") || FolioFactura.Contains("FJ"))
                                        {
                                            NoCliente = sOutput.Substring(2, 5);//
                                            FechaFact = sOutput.Substring(18, 8);//
                                            proveedor = "P01005";
                                            if ((sOutput.Substring(75, 13).Trim()) != "")
                                            {
                                                CodigoEan = sOutput.Substring(75, 13).Trim().ToString();//
                                                string CantFactTxt = sOutput.Substring(90, 7);
                                                bool Convertir2 = long.TryParse(CantFactTxt, out Numero);
                                                if (Convertir2 == true)
                                                {
                                                    CantFact = int.Parse(CantFactTxt);//   

                                                    string PrecioFarmaciaTxt = sOutput.Substring(257, 10);//sOutput.Substring(104, 10);
                                                    bool Convertir3 = float.TryParse(PrecioFarmaciaTxt, out Decimal);
                                                    if (Convertir3 == true)
                                                    {
                                                        PrecioFarmacia = float.Parse(PrecioFarmaciaTxt);//

                                                        string PorcentaJeOftTxt = "0";//sOutput.Substring(147, 6);
                                                        bool Convertir4 = float.TryParse(PorcentaJeOftTxt, out Decimal);
                                                        if (Convertir4 == true)
                                                        {
                                                            PorcentaJeOft = 0.0f;//float.Parse(PorcentaJeOftTxt);//

                                                            string OfertaTxt = "0";//sOutput.Substring(153, 13);
                                                            bool Convertir5 = float.TryParse(OfertaTxt, out Decimal);
                                                            if (Convertir5 == true)
                                                            {
                                                                Oferta = (float.Parse(OfertaTxt)) / CantFact;//
                                                                PrecioConOferta = PrecioFarmacia - Oferta;
                                                                ImporteSinDescXCant = PrecioConOferta * CantFact;

                                                                string DescFactTxt = "0";//sOutput.Substring(166, 6);
                                                                bool Convertir6 = float.TryParse(DescFactTxt, out Decimal);
                                                                if (Convertir6 == true)
                                                                {
                                                                    DescFact = 0.0f;//float.Parse(DescFactTxt);//

                                                                    string DescuentosTxt = "0";//sOutput.Substring(172, 13);
                                                                    bool Convertir7 = float.TryParse(DescuentosTxt, out Decimal);
                                                                    if (Convertir7 == true)
                                                                    {
                                                                        Descuentos = float.Parse(DescuentosTxt) / CantFact;//(float)Math.Round(((PrecioConOferta * CantFact) * (DescFact / 100)), 2);//     

                                                                        string IvaMercTxt = sOutput.Substring(198, 13);
                                                                        bool Convertir8 = float.TryParse(IvaMercTxt, out Decimal);
                                                                        if (Convertir8 == true)
                                                                        {
                                                                            IvaMerc = float.Parse(IvaMercTxt) / CantFact;
                                                                            PrecioConDesc = PrecioConOferta - Descuentos;
                                                                            ImporteNetoUnit = PrecioConDesc;
                                                                            ImporteNetoTotal = PrecioConDesc * CantFact;
                                                                            //UUID = sOutput.Substring(270, 36);
                                                                            //dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca,UUID);
                                                                            dt.Rows.Add(OrdenCompra, FolioFactura, NoCliente, FechaFact, proveedor, CodigoEan, CantFact, PrecioFarmacia, PorcentaJeOft, Oferta, PrecioConOferta, ImporteSinDescXCant, DescFact, Descuentos, PrecioConDesc, IvaMerc, ImporteNetoUnit, ImporteNetoTotal, Opcion, Marca);
                                                                            OrdenCompraTxt = "";
                                                                            CantFactTxt = "";
                                                                            PrecioFarmaciaTxt = "";
                                                                            PorcentaJeOftTxt = "";
                                                                            OfertaTxt = "";
                                                                            DescFactTxt = "";
                                                                            DescuentosTxt = "";
                                                                            IvaMercTxt = "";
                                                                            OrdenCompra = 0;
                                                                            FolioFactura = "";
                                                                            NoCliente = "";
                                                                            FechaFact = "";
                                                                            CodigoEan = "";
                                                                            CantFact = 0;
                                                                            PrecioFarmacia = 0.0f;
                                                                            PorcentaJeOft = 0.0f;
                                                                            Oferta = 0.0f;
                                                                            PrecioConOferta = 0.0f;
                                                                            DescFact = 0.0f;
                                                                            Descuentos = 0.0f;
                                                                            PrecioConDesc = 0.0f;
                                                                            IvaMerc = 0.0f;
                                                                            ImporteNetoUnit = 0.0f;
                                                                            ImporteNetoTotal = 0.0f;
                                                                            //UUID = "";

                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    resultado = instancia.InsertarConfirmacion2(dt, 2);
                    if (resultado == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MARZAM\facturas.txt");
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\MARZAM\facturas.txt");
                    }
                }
                else
                {
                    resultado1 = false;
                }
                if (dt2.Rows.Count > 0)
                {
                    instancia = new Instancia();
                    resultado2 = instancia.InsertarFaltante(dt2);
                    if (resultado == true)
                    {
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\MARZAM\facturas.txt");
                        File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MARZAM\facturas.txt");
                    }
                }
                else
                {
                    resultado2 = false;
                }
                if (resultado1 == true || resultado2 == true)
                {
                    resultado = true;
                    File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\CENTRO\MARZAM\facturas.txt");
                    File.Delete(@"\\192.168.13.30\Pedidos\REEPROCESAR\PACIFICO\MARZAM\facturas.txt");

                }

            }
            catch (Exception ex)
            {

                renglon.ToString();
                // MessageBox.Show(ex.Message);
                resultado = false;
            }
            return resultado;
        }
    }
}
