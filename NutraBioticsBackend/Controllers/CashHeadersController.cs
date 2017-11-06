using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NutraBioticsBackend.Models;

namespace NutraBioticsBackend.Controllers
{
    public class CashHeadersController : Controller
    {

        int vendorid = Convert.ToInt32(System.Web.HttpContext.Current.Session["VendorId"]);
        int userid = Convert.ToInt32(System.Web.HttpContext.Current.Session["User"]);
        private DataContext db = new DataContext();

        // GET: CashHeaders
        public ActionResult Index()
        {
            return View(db.CashHeaders.ToList());
        }

        // GET: CashHeaders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashHeader cashHeader = db.CashHeaders.Find(id);
            if (cashHeader == null)
            {
                return HttpNotFound();
            }
            return View(cashHeader);
        }

        // GET: CashHeaders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CashHeaders/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CashHeaderId,HeadNum,Posted,TranType,InvoiceNum,TranAmt,DocTranAmt,CustId,Custnum,Names,TranDate,AppliedAmt,DocAppliedAmt,VendorId")] CashHeader cashHeader)
        {
            if (ModelState.IsValid)
            {
                db.CashHeaders.Add(cashHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashHeader);
        }

        public ActionResult Search()
        {

            var pagosretorno = new ChashHeaderView
            {

            };



            ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
            ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
            return View(pagosretorno);
        }

        public ActionResult ResultadosBusquedaPagos(bool? pFiltroFechas, bool? pFiltroPeriodo, bool? pFiltroCliente, string pFechaInicial, string pFechaFinal, string pCalendarFInicial, string PCalendarFFinal)
        {

            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
            }

            bool HayFiltro = false;

            if (pFiltroFechas == true || pFiltroCliente == true || pFiltroPeriodo == true)
            {
                HayFiltro = true;
            }

            if (HayFiltro)
            {

                if (pFiltroFechas == true && pFiltroPeriodo == true)
                {
                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                    TempData["CustomError"] = "No se puede filtrar por periodo y rango de fechas a la vez";

                    var returnerror = new ChashHeaderView
                    {
                        CashHeaders = db.CashHeaders.Where(c => c.VendorId == vendorid).ToList(),
                    };



                    return View(returnerror);
                }


                //filtro x fechas
                if (pFiltroFechas == true && pFiltroPeriodo == false)
                {

                    DateTime FInicial2, finicialfiltro = new DateTime().Date;
                    if (pFechaInicial != "")
                    {
                        FInicial2 = Convert.ToDateTime(pFechaInicial);
                        finicialfiltro = FInicial2.Date;
                    }

                    DateTime FFinal2, ffinalfiltro = new DateTime().Date;
                    if (pFechaFinal != "")
                    {
                        FFinal2 = Convert.ToDateTime(pFechaFinal);
                        ffinalfiltro = FFinal2.Date;
                    }

                    var invoicesxfechas = db
                    .CashHeaders
                    .Where(s => s.TranDate >= finicialfiltro && s.TranDate <= ffinalfiltro && s.VendorId == vendorid)
                    .ToList();


                    if (invoicesxfechas == null || invoicesxfechas.Count == 0)
                    {

                        var invoicesxfechasretornonulo = new ChashHeaderView
                        {
                            CashHeaders = invoicesxfechas,
                        };

                        ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                        TempData["CustomError"] = "No se encontraron facturas";

                        return View(invoicesxfechasretornonulo);
                    }


                    var invoicesxfechasretorno = new ChashHeaderView
                    {
                        CashHeaders = invoicesxfechas,
                    };


                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

                    return View(invoicesxfechasretorno);
                }



                //filtro x periodo
                if (pFiltroPeriodo == true && pFiltroFechas == false)
                {


                    DateTime FInicial2, finicialfiltro = new DateTime().Date;
                    if (pCalendarFInicial != "")
                    {
                        FInicial2 = Convert.ToDateTime(pCalendarFInicial);
                        finicialfiltro = FInicial2.Date;
                    }

                    DateTime FFinal2, ffinalfiltro = new DateTime().Date;
                    if (PCalendarFFinal != "")
                    {
                        FFinal2 = Convert.ToDateTime(PCalendarFFinal);
                        ffinalfiltro = FFinal2.Date;
                    }


                    var facturasxperiodo = db
                     .CashHeaders
                     .Where(s => s.TranDate >= finicialfiltro && s.TranDate <= ffinalfiltro && s.VendorId == vendorid)
                     .ToList();

                    if (facturasxperiodo == null || facturasxperiodo.Count == 0)

                    {

                        var returnerror = new ChashHeaderView
                        {
                            CashHeaders = db.CashHeaders.Where(c => c.VendorId == vendorid).ToList(),
                        };




                        ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                        TempData["CustomError"] = "No se encontraron facturas";
                        return View(returnerror);
                    }


                    var facturasxperiodoretorno = new ChashHeaderView
                    {
                        CashHeaders = facturasxperiodo,
                    };

                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

                    return View(facturasxperiodoretorno);
                }
            }



            else

            {

                var facturas = db
                       .InvoiceHeaders
                       .ToList();

                if (facturas == null || facturas.Count == 0)

                {
                    TempData["CustomError"] = "No se encontraron facturas";


                    var facturasretorno2 = new InvoiceHeaderView
                    {
                        InvoiceHeaders = facturas,
                    };

                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                    return View(facturasretorno2);

                }

                var facturasretorno = new InvoiceHeaderView
                {
                    InvoiceHeaders = facturas,
                };

                ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

                return View(facturasretorno);
            }

                return View();
            }


        public JsonResult CargarFechasXPeriodo2(int CalendarId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var calendar = db.Calendars.Where(c => c.CalendarId == CalendarId).FirstOrDefault();
            //var formated_finicial = Convert.ToDateTime(calendar.StartDate.ToString(string.Format("MM-DD-YYYY HH:mm:ss")));
            //var formated_ffinal = Convert.ToDateTime(calendar.EndDate.ToString(string.Format("MM-DD-YYYY HH:mm:ss")));

            return Json(calendar);
        }



        // GET: CashHeaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashHeader cashHeader = db.CashHeaders.Find(id);
            if (cashHeader == null)
            {
                return HttpNotFound();
            }
            return View(cashHeader);
        }

        // POST: CashHeaders/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CashHeaderId,HeadNum,Posted,TranType,InvoiceNum,TranAmt,DocTranAmt,CustId,Custnum,Names,TranDate,AppliedAmt,DocAppliedAmt,VendorId")] CashHeader cashHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cashHeader);
        }

        // GET: CashHeaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashHeader cashHeader = db.CashHeaders.Find(id);
            if (cashHeader == null)
            {
                return HttpNotFound();
            }
            return View(cashHeader);
        }

        // POST: CashHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashHeader cashHeader = db.CashHeaders.Find(id);
            db.CashHeaders.Remove(cashHeader);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
