using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AytoLosLLanos.Models;
using WebMatrix.WebData;

namespace AytoLosLLanos.Controllers
{
    public class HomeController : Controller
    {
        ESTACIONESEntities context = new ESTACIONESEntities();
        List<Sinoptico> lst = new List<Sinoptico>();
        string server = "SERCAUDAL2";

        public ActionResult Index()
        {

            if (WebSecurity.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");
            else
            {
                EstacionSinoptico estsnp = new EstacionSinoptico();
                estsnp.lstEstaciones = new List<Estacion>();
                estsnp.lstEstaciones = context.Estacions.Where(c => c.Alias == "ELLANZE").ToList();
                CargarDatos();
                estsnp.lstSinoptico = new List<Sinoptico>();
                estsnp.lstSinoptico = lst;
                return View(estsnp);
            }
        }

        public void CargarDatos()
        {
            // cargar fatima
            SqlConnection cn = new SqlConnection("Data Source=" + server + ";Initial Catalog=Estaciones;User ID=sa;Password=Demase49");
            cn.Open();
            BuscarValores(cn, "Historico_ELLANZE", 281, 0);
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
            EstacionSinoptico estsnp = new EstacionSinoptico();
            estsnp.lstEstaciones = new List<Estacion>();
            estsnp.lstEstaciones = context.Estacions.Where(c => c.Alias == "ELLANZE").ToList();
            CargarDatos();
            estsnp.lstSinoptico = new List<Sinoptico>();
            estsnp.lstSinoptico = lst;

            return PartialView("Partial2", estsnp);
        }

        public ActionResult Contact()
        {
            if (WebSecurity.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add("yapcibp@gmail.com");
                    mail.From = new MailAddress("EstacionesContacto@gmail.com");
                    mail.Subject = "ELLANZE - Comentarios";
                    string Body = c.Apellidos + ", " + c.Nombre + ", " + c.Telefono + "   : ";
                    Body += c.Comentario;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("EstacionesContacto", "demase7151");// Enter seders User name and password 
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    string a = ex.ToString();
                    ViewBag.Error = "No se ha podido enviar el comentario.";
                    ViewBag.Error1 = "Compruebe que todos los datos son correctos.";
                    return View("Contact");
                }
            }
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        public ActionResult Location(string estacion)
        {
            if (WebSecurity.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");
            else
            {
                Estacion est = context.Estacions.FirstOrDefault(c => c.Alias == estacion);
                return View(est);
            }
        }
    }
}
