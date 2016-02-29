using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AytoLosLLanos.Models;

namespace AytoLosLLanos.Controllers
{
    public class SinopticoController : Controller
    {
        string server = "SERCAUDAL2";
        List<Sinoptico> lst = new List<Sinoptico>();
 

        public ActionResult Index()
        {
            CargarDatos();
            return View(new Sinoptico() { itemModel = lst });
        }

        public void CargarDatos()
        {
            // cargar fatima
            SqlConnection cn = new SqlConnection("Data Source=" + server + ";Initial Catalog=Estaciones;User ID=sa;Password=Demase49");
            cn.Open();
            BuscarValores(cn, "Historico_FATIMA", 30, 31);
            BuscarValores(cn, "Historico_TAJUYA", 33, 34);
            BuscarValores(cn, "Historico_HERMOSILLA", 38, 39);
            BuscarValores(cn, "Historico_HERMOSILLA", 40, 41);
            BuscarValores(cn, "Historico_HERMOSILLA", 42, 43);
            BuscarValores(cn, "Historico_HERMOSILLA", 44, 45);
            BuscarValores(cn, "Historico_TAJUYA", 35, 36);
            BuscarValores(cn, "Historico_FATIMA", 32, 50);
            cn.Close();
        }

        private void BuscarValores(SqlConnection cn, string tabla, int tipo1, int tipo2)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select top 2 *  from " + tabla + " where (IdTipo = " + tipo1 + " or IdTipo = " + tipo2 + ") order by Fecha desc,idTipo", cn);
            da.Fill(dt);
            Sinoptico sn1 = new Sinoptico();
            DataRow dr0 = dt.Rows[0];
            DataRow dr1 = dt.Rows[1];
            sn1.Fecha = Convert.ToDateTime(dr0[1].ToString());
            sn1.Value1 = double.Parse(dr1[2].ToString());
            sn1.Value2 = double.Parse(dr0[2].ToString());

            DateTime fsistema = DateTime.Now;
            DateTime fbd = sn1.Fecha;
            TimeSpan tiempo = fsistema - fbd;
            double mins = tiempo.TotalMinutes;

            if (mins < 60)
                sn1.Color = "green";
            else
            {
                if (mins >= 60 && mins < 360)
                    sn1.Color = "orange";
                else
                {
                    if (mins >= 360 && mins < 1440)
                        sn1.Color = "red";
                    else
                        sn1.Color = "black";
                }
            }

            if (sn1.Value1 > 0)
                sn1.Color2 = "#007acc";
            else
                sn1.Color2 = "#bfbfbf";

            lst.Add(sn1);
        }

        public ActionResult GetItems()
        {
            CargarDatos();
            return PartialView("Partial1",lst);
        }
    }
}
