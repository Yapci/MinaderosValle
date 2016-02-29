using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AytoLosLLanos.Models;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Reporting.WebForms;
using WebMatrix.WebData;

namespace AytoLosLLanos.Controllers
{
    public class Item2Controller : Controller
    {
        SqlConnection cn = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=ESTACIONES;User ID=sa;Password=Demase49");
        SqlConnection cnt2 = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=ESTACIONES;User ID=sa;Password=Demase49");
        SqlConnection cnt3 = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=ESTACIONES;User ID=sa;Password=Demase49");
        SqlConnection cnt4 = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=ESTACIONES;User ID=sa;Password=Demase49");
        SqlDataAdapter da;
        ESTACIONESEntities context = new ESTACIONESEntities();

        public ActionResult Index(string f1, string f2)
        {
            if (f1 == null || f1 == "")
                f1 = DateTime.Now.ToShortDateString() + " 00:00";
            if (f2 == null || f2 == "")
                f2 = DateTime.Now.AddDays(1).ToShortDateString() + " 00:00";
            //SqlConnection cn = new SqlConnection("Data Source=SERCAUDAL2;Initial Catalog=ESTACIONES;User ID=sa;Password=Demase49");
            //SqlDataAdapter da = new SqlDataAdapter("SelectValues", cn);
            da = new SqlDataAdapter("SelectValues", cn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = "Historico_CANCAJOS";
            da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 19;
            da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
            da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
            DataTable dt = new DataTable();
            da.Fill(dt);
            String[] Categories1 = new string[dt.Rows.Count];
            object[] data1 = new object[dt.Rows.Count];
            object[] data2 = new object[dt.Rows.Count];
            object[] data3 = new object[dt.Rows.Count];
            ViewBag.Items = dt.Rows.Count * 3;
            for (int i = 0; i < Categories1.Count(); i++)
            {
                DataRow dr = dt.Rows[i];
                Categories1[i] = dr[0].ToString();
                data1[i] = dr[1].ToString().Replace(',', '.');
            }
            da = new SqlDataAdapter("SelectValues", cn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = "Historico_CANCAJOS";
            da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 21;
            da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
            da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
            dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < Categories1.Count(); i++)
            {
                DataRow dr = dt.Rows[i];
                double value = double.Parse(dr[1].ToString().Replace(',', '.')) * 0.01;
                data2[i] = value;
            }
            //da = new SqlDataAdapter("SelectValues", cn);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = "Historico_CANCAJOS";
            //da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 20;
            //da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
            //da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
            //dt = new DataTable();
            //da.Fill(dt);
            //for (int i = 0; i < Categories1.Count(); i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    double value = double.Parse(dr[1].ToString());
            //    data3[i] = value;
            //}

            int intervalo = (dt.Rows.Count * 30) / 841;

            DotNet.Highcharts.Highcharts chart1 = new DotNet.Highcharts.Highcharts("chart").InitChart(new Chart { Height = 650, ZoomType = ZoomTypes.Xy })
            .SetXAxis(new XAxis
            {
                Categories = Categories1,
                TickInterval = intervalo,
                Labels = new XAxisLabels
                {
                    Rotation = -45,
                    Align = HorizontalAligns.Right,
                    Style = "fontSize: '10px',fontFamily: 'Verdana, sans-serif'"
                }
            })
            .SetCredits(new Credits { Enabled = false })
            .SetTooltip(new Tooltip { Shared = false, Enabled = false })
            .SetExporting(new Exporting { Enabled = true, Buttons = new ExportingButtons { } })
            .SetSubtitle(new Subtitle { Text = "Pulsa y arrastra en el gráfico para hacer zoom " })
            .SetSeries(new Series[]
            {
                new Series {
                    Name = "Caudal",
                    Data = new Data(data1)
                },
                new Series {
                    Name = "Nivel",
                    Data = new Data(data2), YAxis = "1",
                    Color = System.Drawing.Color.Red
                },
                //new Series {
                //    Name = "Acumulado",
                //    Data = new Data(data3), YAxis = "2",
                //    Color = System.Drawing.Color.Green
                //}
            });
            chart1.SetTitle(new Title() { Text = "LOS CANCAJOS   ( " + f1 + " - " + f2 + " )", Style = "fontSize: '11px',fontFamily: 'Verdana, sans-serif'" });
            // un solo eje y
            //chart1.SetYAxis(new YAxis
            //{
            //    TickInterval = 2,
            //    Min = 0,
            //});
            // array de ejes y
            chart1.SetYAxis(new[]
            {
                new YAxis{
                    TickInterval = 2,
                    Min = 0,
                    Opposite = true,
                    Title = new YAxisTitle
                        {
                            Text = "Caudal (m3/h)",
                            //Style = "color: '#AA4643'"
                        },
            
                },
                new YAxis{
                    Title = new YAxisTitle
                        {
                            Text = "Nivel (m)",
                            //Style = "color: '#AA4623'"
                        },
                //},
                //new YAxis{
                //    Title = new YAxisTitle
                //        {
                //            Text = "Acumulado (m3)",
                //            //Style = "color: '#AA4623'"
                //        },
                }
            });
            ViewBag.Chart1Model = chart1;
            return View();
        }

        public ActionResult Index1(string f1, string f2, string Est, string check, string puntos, string fechaini, string fechafin, string idTipo, string idTipo1, string embalse, string canal, string kepware, string grupo)
        {

            Boolean tooltipOn = true;
            string fini = "00:00";
            string ffin = "00:00";
            if (fechaini != null && fechafin != null)
            {
                fini = fechaini + ":00";
                ffin = fechafin + ":00";
            }

            ViewBag.fini = f1;
            ViewBag.ffin = f2;
            ViewBag.hini = fini;
            ViewBag.hfin = ffin;
            ViewBag.puntos = puntos;
            ViewBag.check = check;
            ViewBag.canal = canal;
            ViewBag.kepware = kepware;

            if (WebSecurity.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");
            else
            {
                List<Item1> list = new List<Item1>();
                string nestacion = "";

                if (Est != null)
                    nestacion = Est;
                else
                    nestacion = Request["estacion"].ToString();


                if (f1 == null || f1 == "")
                    f1 = DateTime.Now.ToShortDateString() + " " + fini;
                else
                    f1 += " " + fini;
                if (f2 == null || f2 == "")
                    f2 = DateTime.Now.AddDays(1).ToShortDateString() + " " + ffin;
                else
                    f2 += " " + ffin;
                ViewBag.f1 = f1;
                ViewBag.f2 = f2;

                if (Est != null && check == null)
                {
                    tooltipOn = false;
                }
                double media = 0, pmedia = 0;
                string titulo = "<br/>";
                // SERIES IRREGULARES
                Estacion estacion = context.Estacions.FirstOrDefault(c => c.Nombre == nestacion);
                ViewBag.Estacion = nestacion;
                List<int> canales;
                List<string> strcanal;

                if (kepware == null)
                {
                    if (canal == null && grupo == null)
                    {
                        canales = estacion.Tipoes.Select(c => c.IdTipo).ToList();
                        strcanal = estacion.Tipoes.Select(c => c.Nombre).ToList();
                    }
                    else
                    {
                        if (canal != null)
                        {
                            int idc = int.Parse(canal);
                            canales = estacion.Tipoes.Where(c => c.IdTipo == idc).Select(c => c.IdTipo).ToList();
                            strcanal = estacion.Tipoes.Where(c => c.IdTipo == idc).Select(c => c.Nombre).ToList();
                        }
                        else
                        {
                            Tipo cc;
                            canales = estacion.Tipoes.Where(c => c.Nombre.Contains(grupo)).Select(c => c.IdTipo).ToList();
                            strcanal = estacion.Tipoes.Where(c => c.Nombre.Contains(grupo)).Select(c => c.Nombre).ToList();
                            switch (grupo)
                            {
                                case "depósito":
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Válvula Consigna"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Válvula Posición"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    break;
                                case "Vertido":
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Nivel"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Posición"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Consigna"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    break;
                                case "noroeste":
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Noroeste Caudal"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Noroeste Acumulado"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    break;
                                case "hermosilla":
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Hermosilla Caudal"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Hermosilla Acumulado"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    break;
                                case "balsa":
                                    cc = estacion.Tipoes.FirstOrDefault(c => c.Nombre.Contains("Balsa Nivel"));
                                    canales.Add(cc.IdTipo);
                                    strcanal.Add(cc.Nombre);
                                    break;

                            }
                        }
                    }
                }
                else
                {
                    if (grupo == null)
                    {
                        canales = new List<int>();
                        canales.Add(1);
                        strcanal = new List<string>();
                        strcanal.Add(kepware);
                    }
                    else
                    {
                        canales = new List<int>();
                        strcanal = new List<string>();
                        if (kepware == "bombeo")
                        {
                            for (int p = 1; p < 7; p++)
                            {
                                canales.Add(p);
                            }
                            strcanal.Add("automabombas.Analogicas.Contadorcaudalimetro2");
                            strcanal.Add("automabombas.Analogicas.Contadorcaudalimetro3");
                            strcanal.Add("automabombas.Analogicas.Contadorcaudalimetro1");
                            strcanal.Add("automabombas.Analogicas.caudalimetro1");
                            strcanal.Add("automabombas.Analogicas.caudalimetro2");
                            strcanal.Add("automabombas.Analogicas.caudalimetrointermunicipal");
                        }
                        else
                        {
                            if (kepware == "noroeste")
                            {
                                for (int p = 1; p < 3; p++)
                                {
                                    canales.Add(p);
                                }
                                strcanal.Add("2PINOS.MAG2.Qm3h");
                                strcanal.Add("2PINOS.MAG2.Totalizador1");
                            }
                            else
                            {
                                if (kepware == "hermosilla")
                                {
                                    for (int p = 1; p < 3; p++)
                                    {
                                        canales.Add(p);
                                    }
                                    strcanal.Add("2PINOS.TrasbaseHermocilla.Qm3h");
                                    strcanal.Add("2PINOS.TrasbaseHermocilla.Totalizador1");
                                }
                                else
                                {
                                    if (kepware == "balsa")
                                    {
                                        for (int p = 1; p < 2; p++)
                                        {
                                            canales.Add(p);
                                        }
                                        strcanal.Add("NivelBalsa.Nivel.altura_placa");
                                    }
                                    else
                                    {
                                        //if (kepware == "dospinos")
                                        //{
                                        //    for (int p = 1; p < 6; p++)
                                        //    {
                                        //        canales.Add(p);
                                        //    }
                                        //    strcanal.Add("2PINOS.MAG2.Qm3h");
                                        //    strcanal.Add("2PINOS.MAG2.Totalizador1");
                                        //    strcanal.Add("2PINOS.TrasbaseHermocilla.Qm3h");
                                        //    strcanal.Add("2PINOS.TrasbaseHermocilla.Totalizador1");
                                        //    strcanal.Add("NivelBalsa.Nivel.altura_placa");
                                        //}
                                        //else
                                        //{
                                        if (kepware == "balsacuatrocaminos")
                                        {
                                            for (int p = 1; p < 4; p++)
                                            {
                                                canales.Add(p);
                                            }
                                            strcanal.Add("Vertedero.GARAFIA.Qm3h");
                                            strcanal.Add("Vertedero.GARAFIA.Totalizador");
                                            strcanal.Add("LOOKOUT.TWIDO.telecontrol.Filtrado4CaminosSvr.PLCTwido.Nivel");
                                        }
                                        else
                                        {
                                            for (int p = 1; p < 4; p++)
                                            {
                                                canales.Add(p);
                                            }
                                            strcanal.Add("Tunel.analogicas.Sensor presión tubo 1");
                                            strcanal.Add("Tunel.analogicas.Sensor presion tubo 2");
                                            strcanal.Add("Tunel.analogicas.Sensor presión tubo 3");
                                        }
                                    }
                                    //}
                                }
                            }
                        }
                    }
                }

                Series[] series = new Series[canales.Count];
                double[] valores = new double[canales.Count];

                int tamaño = 0;

                for (int i = 0; i < canales.Count; i++)
                {
                    tamaño = canales.Count;
                    pmedia = 0;
                    media = 0;
                    if (kepware == null)
                    {
                        if (canal == null && grupo == null)
                        {
                            da = new SqlDataAdapter("SelectValues", cn);
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;
                            da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = estacion.Tabla.Nombre;
                            da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = canales[i];
                            da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                            da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        }
                        else
                        {
                            if (canal != null)
                            {
                                da = new SqlDataAdapter("select Fecha,Valor from " + estacion.Tabla.Nombre + " where Fecha >= '" + f1 + "' and Fecha <= '" + f2 + "' and idTipo = " + canal + " order by Fecha", cn);
                            }
                            else
                            {
                                da = new SqlDataAdapter("select Fecha,Valor from " + estacion.Tabla.Nombre + " where Fecha >= '" + f1 + "' and Fecha <= '" + f2 + "' and idTipo = " + canales[i] + " order by Fecha", cn);

                            }
                        }
                    }
                    else
                    {
                        if (estacion.Tabla.Nombre.Contains("ADUARES"))
                        {
                            if (grupo == null)
                            {
                                da = new SqlDataAdapter("select _TimeStamp,_Value from histo_web_aduares where _Timestamp >= '" + f1 + "' and _Timestamp <= '" + f2 + "' and _Name = '" + kepware + "' order by _TimeStamp", cn);
                            }
                            else
                            {
                                da = new SqlDataAdapter("select _TimeStamp,_Value from histo_web_aduares where _Timestamp >= '" + f1 + "' and _Timestamp <= '" + f2 + "' and _Name = '" + strcanal[i] + "' order by _TimeStamp", cn);
                            }
                        }
                        else
                        {
                            
                        }
                    }

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Series s = new Series();
                    object[,] obj;
                    if (puntos == null || puntos == "100")
                    {
                        obj = new object[dt.Rows.Count, 2];

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dr = dt.Rows[j];
                            obj[j, 0] = Convert.ToDateTime(dr[0].ToString());
                            if (strcanal[i].Contains("nivel") && kepware != null)
                                obj[j, 1] = Math.Round(double.Parse(dr[1].ToString().Replace('.', ',')) / 1000, 2);
                            else
                                obj[j, 1] = Math.Round(double.Parse(dr[1].ToString()), 2);
                            if (!strcanal[i].Contains("Acumulado") && !strcanal[i].Contains("Contadorcaudalimetro") && double.Parse(dr[1].ToString()) > 0 && !strcanal[i].Contains("Totalizador"))
                            {
                                pmedia++;
                                media += double.Parse(dr[1].ToString().Replace('.', ','));
                            }

                        }
                        if (media > 0)
                            valores[i] = Math.Round((media / pmedia), 2);
                        if (strcanal[i].Contains("Acumulado") || strcanal[i].Contains("Contadorcaudalimetro") || strcanal[i].Contains("Totalizador"))
                        {
                            DataRow dr1 = dt.Rows[0];
                            DataRow dr2 = dt.Rows[dt.Rows.Count - 1];
                            valores[i] = Math.Round(double.Parse(dr2[1].ToString().Replace('.', ',')) - double.Parse(dr1[1].ToString().Replace('.', ',')), 2);
                        }
                    }
                    else
                    {
                        obj = new object[dt.Rows.Count / 2, 2];
                        int x = 0;
                        for (int j = 0; j < dt.Rows.Count / 2; j++)
                        {
                            DataRow dr = dt.Rows[x];
                            obj[j, 0] = Convert.ToDateTime(dr[0].ToString());
                            if (strcanal[i].Contains("nivel") && kepware != null)
                                obj[j, 1] = Math.Round(double.Parse(dr[1].ToString().Replace('.', ',')) / 1000, 2);
                            else
                                obj[j, 1] = Math.Round(double.Parse(dr[1].ToString()), 2);
                            x += 2;
                            if (!strcanal[i].Contains("Acumulado") && !strcanal[i].Contains("Contadorcaudalimetro") && double.Parse(dr[1].ToString()) > 0 && !strcanal[i].Contains("Totalizador"))
                            {
                                pmedia++;
                                media += double.Parse(dr[1].ToString().Replace('.', ','));
                            }
                        }
                        if (media > 0)
                            valores[i] = Math.Round((media / pmedia), 2);
                        if (strcanal[i].Contains("Acumulado") || strcanal[i].Contains("Contadorcaudalimetro") || strcanal[i].Contains("Totalizador"))
                        {
                            DataRow dr1 = dt.Rows[0];
                            DataRow dr2 = dt.Rows[dt.Rows.Count - 1];
                            valores[i] = Math.Round(double.Parse(dr2[1].ToString()) - double.Parse(dr1[1].ToString()), 2);
                        }
                    }

                    s.Data = new Data(obj);
                    s.Name = strcanal[i];
                    s.YAxis = i.ToString();
                    series[i] = s;
                }




                int count = 0;
                for (int x = 0; x < valores.Count(); x++)
                {
                    if (strcanal[x].Contains("Acumulado") || strcanal[x].Contains("Contadorcaudalimetro"))
                        titulo += strcanal[x] + " total : " + valores[x].ToString() + " m3";
                    else
                    {
                        if (strcanal[x].Contains("Sensor"))
                            titulo += strcanal[x] + " Media : " + valores[x].ToString() + " m3/h";
                        if (strcanal[x].Contains("caudalimetro"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m3/h";
                        if (strcanal[x].ToUpper().Contains("CAUDAL"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m3/h";
                        if (strcanal[x].Contains("Nivel"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m";
                        if (strcanal[x].Contains("Batería"))
                            titulo += strcanal[x] + " Media : " + valores[x].ToString() + " v";
                        if (strcanal[x].Contains("Volumen"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m3";
                        if (strcanal[x].Contains("Cloro"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " mg/l";
                        if (strcanal[x].Contains("nivel"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m";
                        if (strcanal[x].Contains("Qm3h"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m3/h";
                        if (strcanal[x].Contains("Totalizador"))
                            titulo += strcanal[x] + " Medio : " + valores[x].ToString() + " m3";
                    }

                    if (x < valores.Count() - 1)
                        titulo += " , ";
                    count++;
                    if (count == 4)
                    {
                        titulo += "<br/>";
                        count = 0;
                    }
                }

                string[] dias = { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
                string[] smonth = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

                DotNet.Highcharts.Highcharts chart1 = new DotNet.Highcharts.Highcharts("chart").InitChart(new Chart { Height = 650, ZoomType = ZoomTypes.X, DefaultSeriesType = ChartTypes.Line })
                .SetXAxis(new XAxis
                {
                    Type = AxisTypes.Datetime,
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
                        Format = "{value:%d/%m/%Y %H:%M}",
                        Align = HorizontalAligns.Right,
                        Style = "fontSize: '10px',fontFamily: 'Verdana, sans-serif'"
                    },
                })
                .SetOptions(new GlobalOptions
                {
                    Global = new Global { UseUTC = false },
                    Lang = new DotNet.Highcharts.Helpers.Lang
                    {
                        Weekdays = dias,
                        ShortMonths = smonth
                    }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetTooltip(new Tooltip
                {
                    Shared = true,
                    Enabled = tooltipOn,
                    XDateFormat = "%d/%m/%Y %H:%M"
                })
                .SetExporting(new Exporting { Enabled = true, Buttons = new ExportingButtons { } })
                .SetSubtitle(new Subtitle { Text = "Pulsa y arrastra en el gráfico para hacer zoom " })
                .SetNavigation(new Navigation { ButtonOptions = new NavigationButtonOptions { Text = "Opciones" } })
                .SetSeries(series);
                chart1.SetTitle(new Title() { Text = " " + nestacion + "   ( " + f1 + " - " + f2 + " ) " + titulo, Style = "fontSize: '11px',fontFamily: 'Verdana, sans-serif',fontWeight: 'bold'" });
                YAxis[] ejes = new YAxis[canales.Count];
                Boolean opp = false;
                for (int i = 0; i < canales.Count; i++)
                {
                    if ((i % 2) == 0)
                        opp = false;
                    else
                        opp = true;
                    if (!strcanal[i].ToString().Contains("Garafia caudal"))
                    {
                        YAxis eje = new YAxis
                        {
                            Opposite = opp,
                            Title = new YAxisTitle
                            {
                                Text = strcanal[i].ToString(),
                                Style = "color: Highcharts.getOptions().colors[" + i + "]"
                            },
                            Labels = new YAxisLabels
                            {
                                Style = "color: Highcharts.getOptions().colors[" + i + "]"
                            }
                        };
                        ejes[i] = eje;
                    }
                    else
                    {
                        YAxis eje = new YAxis
                        {
                            Opposite = opp,
                            Max = 10,
                            Title = new YAxisTitle
                            {
                                Text = strcanal[i].ToString(),
                                Style = "color: Highcharts.getOptions().colors[" + i + "]"
                            },
                            Labels = new YAxisLabels
                            {
                                Style = "color: Highcharts.getOptions().colors[" + i + "]"
                            }
                        };
                        ejes[i] = eje;
                    }
                }

                chart1.SetYAxis(ejes);
                ViewBag.Chart1Model = chart1;

                return View(list);
            }
        }


        public ActionResult Index2()
        {
            //DotNet.Highcharts.Highcharts chart1 = new DotNet.Highcharts.Highcharts("chart").
            //    InitChart(new Chart
            //    {
            //        Height = 650,
            //        ZoomType = ZoomTypes.Xy,
            //        Events = new ChartEvents 
            //        { 
            //            Load = "ChartLoadEvent"
            //        }
            //    })
            //    .AddJavascripFunction("ChartLoadEvent","var ren = this.renderer,colors = Highcharts.getOptions().colors, rightArrow = ['M', 0, 0, 'L', 100, 0, 'L', 95, 5, 'M', 100, 0, 'L', 95, -5],leftArrow = ['M', 100, 0, 'L', 0, 0, 'L', 5, 5, 'M', 0, 0, 'L', 5, -5]; ren.path(['M', 120, 40, 'L', 120, 330]).attr({'stroke-width': 2,stroke: 'silver',dashstyle: 'dash'}).add();");

            return View();
        }

        public ActionResult Report(string id, string f1, string f2, string estacion)
        {
            if (WebSecurity.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");
            else
            {
                string tb = "", path = "";
                switch (estacion)
                {
                    case "FATIMA":
                        tb = "Historico_FATIMA";
                        path = Path.Combine(Server.MapPath("/AytoLosLLanos/Report"), "ReportFatima.rdlc");
                        break;
                    case "TAJUYA":
                        tb = "Historico_TAJUYA";
                        path = Path.Combine(Server.MapPath("/AytoLosLLanos/Report"), "ReportTajuya.rdlc");
                        break;
                    case "HERMOSILLA":
                        tb = "Historico_HERMOSILLA";
                        path = Path.Combine(Server.MapPath("/AytoLosLLanos/Report"), "ReportHermosilla.rdlc");
                        break;
                }

                LocalReport lr = new LocalReport();
                if (System.IO.File.Exists(path))
                    lr.ReportPath = path;

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataAdapter da = new SqlDataAdapter("select Fecha,Valor,Nombre from " + tb + " inner join Tipo on Tipo.IdTipo = " + tb + ".IdTipo where Fecha between '" + f1 + "' and '" + f2 + "' order by Fecha,Nombre", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ReportDataSource rd = new ReportDataSource();

                switch (estacion)
                {
                    case "FATIMA":
                        DataSet1.DataTable1DataTable tabla = new DataSet1.DataTable1DataTable();
                        int c = 0;
                        while (c < dt.Rows.Count)
                        {
                            DataRow dr = dt.Rows[c];
                            DataRow dr1 = dt.Rows[c + 1];
                            DataRow dr2 = dt.Rows[c + 2];
                            tabla.Rows.Add(dr[0].ToString(), double.Parse(dr1[1].ToString()), double.Parse(dr[1].ToString()), double.Parse(dr2[1].ToString()));
                            c += 4;
                        }
                        rd = new ReportDataSource("DataSet1", tabla.ToList());
                        break;
                    case "TAJUYA":
                        DataSet2.DataTable1DataTable tabla1 = new DataSet2.DataTable1DataTable();
                        int c1 = 0;
                        while (c1 < dt.Rows.Count)
                        {
                            DataRow dr = dt.Rows[c1];
                            DataRow dr1 = dt.Rows[c1 + 1];
                            DataRow dr2 = dt.Rows[c1 + 2];
                            DataRow dr3 = dt.Rows[c1 + 3];
                            DataRow dr4 = dt.Rows[c1 + 4];
                            tabla1.Rows.Add(dr[0].ToString(),double.Parse(dr1[1].ToString()), double.Parse(dr[1].ToString()), double.Parse(dr2[1].ToString()), double.Parse(dr4[1].ToString()), double.Parse(dr3[1].ToString()));
                            c1 += 5;
                        }
                        rd = new ReportDataSource("DataSet2", tabla1.ToList());
                        break;
                    case "HERMOSILLA":
                        DataSet3.DataTable1DataTable tabla2 = new DataSet3.DataTable1DataTable();
                        int c2 = 0;
                        while (c2 < dt.Rows.Count)
                        {
                            DataRow dr = dt.Rows[c2];
                            DataRow dr1 = dt.Rows[c2 + 1];
                            DataRow dr2 = dt.Rows[c2 + 2];
                            DataRow dr3 = dt.Rows[c2 + 3];
                            DataRow dr4 = dt.Rows[c2 + 4];
                            DataRow dr5 = dt.Rows[c2 + 5];
                            DataRow dr6 = dt.Rows[c2 + 6];
                            DataRow dr7 = dt.Rows[c2 + 7];
                            tabla2.Rows.Add(dr[0].ToString(), double.Parse(dr[1].ToString()), double.Parse(dr1[1].ToString()), double.Parse(dr3[1].ToString()), double.Parse(dr2[1].ToString()), double.Parse(dr5[1].ToString()), double.Parse(dr4[1].ToString()), double.Parse(dr7[1].ToString()), double.Parse(dr6[1].ToString()));
                            c2 += 8;
                        }
                        rd = new ReportDataSource("DataSet3", tabla2.ToList());
                        break;
                }

                
                ReportParameter rp = new ReportParameter("ReportParameter1", "Desde " + f1 + " hasta " + f2);
                ReportParameter rp1 = new ReportParameter("Fecha", DateTime.Now.ToString());
                lr.SetParameters(new ReportParameter[] { rp, rp1 });
                lr.DataSources.Add(rd);

                string reportType = id;
                string mimeType;
                string encoding;
                string fileNameExtension;

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    null,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);


                return File(renderedBytes, mimeType);
            }
        }

        public ActionResult Export(string id, string f1, string f2, string estacion)
        {
            string tb = "";
            int valor = 0;

            Estacion e = context.Estacions.FirstOrDefault(c => c.Nombre == estacion);
            var t = e.Tipoes.OrderBy(c => c.Nombre).ToList();
            SqlDataAdapter da;
            DataTable dtaux1 = new DataTable();
            DataTable dtaux2 = new DataTable();
            DataTable dtaux3 = new DataTable();
            DataTable dtaux4 = new DataTable();
            SqlDataAdapter da2;
            SqlDataAdapter da3;
            SqlDataAdapter da4;

            if (cn.State == ConnectionState.Closed)
                cn.Open();

            switch (estacion)
            {
                case "FATIMA":
                    tb = "Historico_FATIMA";
                    valor = 4;
                    cnt2.Open();
                    da2 = new SqlDataAdapter();
                    Task t1_1 = new Task(() =>
                    {
                        da = new SqlDataAdapter("SelectValuesXLSH1", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 30;
                        da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 31;
                        da.Fill(dtaux1); 
                    });
                    t1_1.Start();
                    Task t2_1 = new Task(() =>
                    {
                        da2 = new SqlDataAdapter("SelectValuesXLSH2", cnt2);
                        da2.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da2.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da2.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 32;
                        da2.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da2.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da2.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da2.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 50;
                        da2.Fill(dtaux2);
                    });
                    t2_1.Start();
                    Task[] tasks1 = new Task[2];
                    tasks1[0] = t1_1;
                    tasks1[1] = t2_1;

                    Task.WaitAll(tasks1);
                    cnt2.Close();
                    break;

                case "TAJUYA":
                    tb = "Historico_TAJUYA";
                    valor = 5;
                    cnt2.Open();
                    cnt3.Open();
                    da2 = new SqlDataAdapter();
                    da3 = new SqlDataAdapter();

                    Task t1_2 = new Task(() =>
                    {
                        da = new SqlDataAdapter("SelectValuesXLSH1", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 33;
                        da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 34;
                        da.Fill(dtaux1); 
                    });
                    t1_2.Start();
                    Task t2_2 = new Task(() =>
                    {
                        da2 = new SqlDataAdapter("SelectValuesXLSH2", cnt2);
                        da2.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da2.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da2.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 35;
                        da2.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da2.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da2.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da2.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 36;
                        da2.Fill(dtaux2);
                    });
                    t2_2.Start();
                    Task t3_2 = new Task(() =>
                    {
                        da3 = new SqlDataAdapter("SelectValuesXLSH3", cnt3);
                        da3.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da3.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da3.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 37;
                        da3.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da3.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da3.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da3.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 0;
                        da3.Fill(dtaux3);
                    });
                    t3_2.Start();

                    Task[] tasks2 = new Task[3];
                    tasks2[0] = t1_2;
                    tasks2[1] = t2_2;
                    tasks2[2] = t3_2;

                    Task.WaitAll(tasks2);

                    cnt2.Close();
                    cnt3.Close();
                    break;

                case "HERMOSILLA":

                    cnt2.Open();
                    cnt3.Open();
                    cnt4.Open();
                    da2 = new SqlDataAdapter();
                    da3 = new SqlDataAdapter();
                    da4 = new SqlDataAdapter();
                    tb = "Historico_HERMOSILLA";
                    valor = 8;
                    Task t1 = new Task(() =>
                    {
                        da = new SqlDataAdapter("SelectValuesXLSH1", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 38;
                        da.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 39;
                        da.Fill(dtaux1); 
                    });
                    t1.Start();
                    Task t2 = new Task(() =>
                    {
                        da2 = new SqlDataAdapter("SelectValuesXLSH2", cnt2);
                        da2.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da2.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da2.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 40;
                        da2.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da2.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da2.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da2.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 41;
                        da2.Fill(dtaux2);
                    });
                    t2.Start();
                    Task t3 = new Task(() =>
                    {
                        da3 = new SqlDataAdapter("SelectValuesXLSH3", cnt3);
                        da3.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da3.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da3.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 42;
                        da3.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da3.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da3.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da3.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 43;
                        da3.Fill(dtaux3);
                    });
                    t3.Start();
                    Task t4 = new Task(() =>
                    {
                        da4 = new SqlDataAdapter("SelectValuesXLSH4", cnt4);
                        da4.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da4.SelectCommand.Parameters.Add("@tabla", SqlDbType.NVarChar).Value = tb;
                        da4.SelectCommand.Parameters.Add("@tipo", SqlDbType.Int).Value = 44;
                        da4.SelectCommand.Parameters.Add("@fecha1", SqlDbType.NVarChar).Value = f1;
                        da4.SelectCommand.Parameters.Add("@fecha2", SqlDbType.NVarChar).Value = f2;
                        da4.SelectCommand.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                        da4.SelectCommand.Parameters.Add("@tipo1", SqlDbType.NVarChar).Value = 45;
                        da4.Fill(dtaux4);
                    });
                    t4.Start();
                    Task[] tasks = new Task[4];
                    tasks[0] = t1;
                    tasks[1] = t2;
                    tasks[2] = t3;
                    tasks[3] = t4;

                    Task.WaitAll(tasks);

                    cnt2.Close();
                    cnt3.Close();
                    cnt4.Close();

                    break;
            }
            if (estacion == "HERMOSILLA")
            {
                dtaux1.Merge(dtaux2);
                dtaux1.Merge(dtaux3);
                dtaux1.Merge(dtaux4);
            }
            if (estacion == "FATIMA")
            {
                dtaux1.Merge(dtaux2);
            }
            if (estacion == "TAJUYA")
            {
                dtaux1.Merge(dtaux2);
                dtaux1.Merge(dtaux3);
            }

            DataView dv = dtaux1.DefaultView;
            dv.Sort = "Fecha,Nombre";
            DataTable dt = dv.ToTable();

            ReportDataSource rd = new ReportDataSource();
            DataTable dtaux = new DataTable();
            System.Web.UI.WebControls.GridView gridvw = new System.Web.UI.WebControls.GridView();

            switch (estacion)
            {
                case "FATIMA":
                    DataSet1.DataTable1DataTable tabla = new DataSet1.DataTable1DataTable();
                    int c = 0;
                    while (c < dt.Rows.Count)
                    {
                        DataRow dr = dt.Rows[c];
                        DataRow dr1 = dt.Rows[c + 1];
                        DataRow dr2 = dt.Rows[c + 2];
                        tabla.Rows.Add(dr[0].ToString(), double.Parse(dr1[1].ToString()), double.Parse(dr[1].ToString()), double.Parse(dr2[1].ToString()));

                        c += 4;
                    }
                    rd = new ReportDataSource("DataSet1", tabla.ToList());
                    gridvw.DataSource = tabla; //bind the datatable to the gridview    
                    break;
                case "TAJUYA":
                    DataSet2.DataTable1DataTable tabla1 = new DataSet2.DataTable1DataTable();
                    int c1 = 0;
                    while (c1 < dt.Rows.Count)
                    {
                        try
                        {
                            DataRow dr = dt.Rows[c1];
                            DataRow dr1 = dt.Rows[c1 + 1];
                            DataRow dr2 = dt.Rows[c1 + 2];
                            DataRow dr3 = dt.Rows[c1 + 3];
                            DataRow dr4 = dt.Rows[c1 + 4];
                            tabla1.Rows.Add(dr[0].ToString(), double.Parse(dr1[1].ToString()), double.Parse(dr[1].ToString()), double.Parse(dr2[1].ToString()), double.Parse(dr4[1].ToString()), double.Parse(dr3[1].ToString()));
                            c1 += 5;
                        }
                        catch (Exception ex)
                        {
                            string a = "A";
                        }
                    }
                    rd = new ReportDataSource("DataSet2", tabla1.ToList());
                    gridvw.DataSource = tabla1; //bind the datatable to the gridview    
                    break;
                case "HERMOSILLA":
                    DataSet3.DataTable1DataTable tabla2 = new DataSet3.DataTable1DataTable();
                    int c2 = 0;
                    while (c2 < dt.Rows.Count)
                    {
                        try
                        {
                            DataRow dr = dt.Rows[c2];
                            DataRow dr1 = dt.Rows[c2 + 1];
                            DataRow dr2 = dt.Rows[c2 + 2];
                            DataRow dr3 = dt.Rows[c2 + 3];
                            DataRow dr4 = dt.Rows[c2 + 4];
                            DataRow dr5 = dt.Rows[c2 + 5];
                            DataRow dr6 = dt.Rows[c2 + 6];
                            DataRow dr7 = dt.Rows[c2 + 7];
                            tabla2.Rows.Add(dr[0].ToString(), double.Parse(dr[1].ToString()), double.Parse(dr1[1].ToString()), double.Parse(dr3[1].ToString()), double.Parse(dr2[1].ToString()), double.Parse(dr5[1].ToString()), double.Parse(dr4[1].ToString()), double.Parse(dr7[1].ToString()), double.Parse(dr6[1].ToString()));
                            c2 += 8;
                        }
                        catch (Exception ex)
                        {
                            string a = "A";
                        }
                        c2 += 8;
                    }
                    rd = new ReportDataSource("DataSet3", tabla2.ToList());
                    gridvw.DataSource = tabla2; //bind the datatable to the gridview   
                    break;
            }

            gridvw.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + estacion + ".xls");
            Response.ContentType = "application/excel";
            StringWriter swr = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(swr);
            gridvw.RenderControl(tw);
            Response.Write(swr.ToString());

            Response.End();


            return RedirectToAction("Index", "Home");
        }
    }
}
