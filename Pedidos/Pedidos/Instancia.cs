using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos
{
    class Instancia
    {

        Consulta consulta;
        public DataTable Editar_OC_Factura(string Marca, int Realizar)
        {
            Consulta consulta = new Consulta();
            DataTable dt = new DataTable();
            dt = consulta.Editar_OC_Factura(Marca, Realizar);
            return dt;
        }
        public bool EliminaConfiramcion(int ID, string Marca)
        {
            bool resultado = false;
            Consulta consulta = new Consulta();
            resultado = consulta.EliminaConfiramcion(ID, Marca);
            return resultado;
        }
        public bool InsertarConfirmacion(DataTable dt)
        {
            bool Resultado = false;
            Consulta consulta = new Consulta();
            Resultado = consulta.InsertarConfirmacion(dt);
            return Resultado;
        }
        public bool InsertarConfirmacion2(DataTable dt, int ID)
        {
            bool Resultado = false;
            Consulta consulta = new Consulta();
            Resultado = consulta.InsertarConfirmacion2(dt, ID);
            return Resultado;
        }
        public bool InsertarFaltante(DataTable dt)
        {
            bool Resultado = false;
            Consulta consulta = new Consulta();
            Resultado = consulta.InsertarFaltante(dt);
            return Resultado;
        }
    }
}
