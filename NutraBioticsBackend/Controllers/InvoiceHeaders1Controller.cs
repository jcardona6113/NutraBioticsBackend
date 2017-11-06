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

    public class InvoiceHeaders1Controller : Controller
    {


        int vendorid = Convert.ToInt32(System.Web.HttpContext.Current.Session["VendorId"]);
        int userid = Convert.ToInt32(System.Web.HttpContext.Current.Session["User"]);

        private DataContext db = new DataContext();

        // GET: InvoiceHeaders1
        public ActionResult Index()
        {
            return View(db.InvoiceHeaders.Where(i=> i.VendorId == vendorid).ToList());
        }

        // GET: InvoiceHeaders1

        public ActionResult ResultadosBusquedaFacturas(bool? pFacturaConSaldo, bool? pFiltroFechas, bool? pFiltroPeriodo, bool? pFiltroCliente, string pFechaInicial, string pFechaFinal, string pCalendarFInicial, string PCalendarFFinal)
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

                    ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                    ModelState.AddModelError("",
                    "No es posible filtrar por rango de fechas y periodo a la vez.");
                    return View();
                }

                //filtro x fact saldo
                if (pFacturaConSaldo == true && pFiltroPeriodo == false && pFiltroFechas == false)
                {

                    var facturasconsaldo = db
                        .InvoiceHeaders
                        .Where(s => s.InvoiceBal > 0 && s.VendorId == vendorid)
                        .ToList();


                    if (facturasconsaldo == null || facturasconsaldo.Count == 0)

                    {

                        ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                        TempData["CustomError"] = "No se encontraron facturas";
                        return View();
                    }

                    var facturasconsaldoretorno = new InvoiceHeaderView
                    {
                        InvoiceHeaders = facturasconsaldo,
                    };

                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                    return View(facturasconsaldoretorno);
                }

                //filtro x fechas
                if (pFiltroFechas == true && pFiltroPeriodo == false && pFacturaConSaldo == false)
                {
                    try
                    {
                        DateTime FInicial2, finicialfiltro=new DateTime().Date;
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
                        .InvoiceHeaders
                        .Where(s => s.InvoiceDate >= finicialfiltro && s.InvoiceDate <= ffinalfiltro && s.VendorId == vendorid)
                        .ToList();


                        if (invoicesxfechas == null || invoicesxfechas.Count == 0)
                        {

                            var invoicesxfechasretornonulo = new InvoiceHeaderView
                            {
                                InvoiceHeaders = invoicesxfechas,
                            };

                            ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                            ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                            TempData["CustomError"] = "No se encontraron facturas";

                            return View(invoicesxfechasretornonulo);
                        }


                        var invoicesxfechasretorno = new InvoiceHeaderView
                        {
                            InvoiceHeaders = invoicesxfechas,
                        };


                        ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                        ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

                        return View(invoicesxfechasretorno);
                    }
                    catch (Exception ex)
                    {
                        TempData["CustomError"] += ex.ToString();
                    }


                }

                //filtro x fechas y fact.con saldo
                if (pFiltroFechas == true && pFacturaConSaldo == false && pFiltroPeriodo == false)
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
                     .InvoiceHeaders
                     .Where(s => s.InvoiceDate >= finicialfiltro && s.InvoiceDate <= ffinalfiltro && s.InvoiceBal > 0)
                     .ToList();

                    if (invoicesxfechas == null || invoicesxfechas.Count == 0)

                    {

                        ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                        ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                        TempData["CustomError"] = "No se encontraron facturas";
                        return View();
                    }


                    var invoicesxfechasretorno = new InvoiceHeaderView
                    {
                        InvoiceHeaders = invoicesxfechas,
                    };

                    ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                    return View(invoicesxfechasretorno);
                }

                //filtro x periodo
                if (pFiltroPeriodo == true && pFiltroFechas == false && pFacturaConSaldo == false)
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
                     .InvoiceHeaders
                     .Where(s => s.InvoiceDate >= finicialfiltro && s.InvoiceDate <= ffinalfiltro && s.VendorId == vendorid)
                     .ToList();

                    if (facturasxperiodo == null || facturasxperiodo.Count == 0)

                    {

                        var invoicesxfechasretorno = new InvoiceHeaderView
                        {
                            InvoiceHeaders = facturasxperiodo,
                        };


                        ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                        TempData["CustomError"] = "No se encontraron facturas";

                        return View(invoicesxfechasretorno);
                    }


                    var facturasxperiodoretorno = new InvoiceHeaderView
                    {
                        InvoiceHeaders = facturasxperiodo,
                    };

                    ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

                    return View(facturasxperiodoretorno);
                }

                //filtro x periodo y fact. con saldo
                if (pFiltroPeriodo == true && pFacturaConSaldo == true && pFiltroFechas == false)
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


                    var facturasxpreiodo = db
                     .InvoiceHeaders
                     .Where(s => s.InvoiceDate >= finicialfiltro && s.InvoiceDate <= ffinalfiltro && s.VendorId == vendorid)
                     .ToList();

                    if (facturasxpreiodo == null || facturasxpreiodo.Count == 0)

                    {


                        var invoicesxfechasretorno = new InvoiceHeaderView
                        {
                            InvoiceHeaders = facturasxpreiodo,
                        };


                        TempData["CustomError"] = "No se encontraron facturas";
                        return View(invoicesxfechasretorno);
                    }

                    var facturasxperiodoretorno = new InvoiceHeaderView
                    {
                        InvoiceHeaders = facturasxpreiodo,
                    };

                    ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
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
                    ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                    ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
                    return View();

                }

                var facturasretorno = new InvoiceHeaderView
                {
                    InvoiceHeaders = facturas,
                };


                ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
                ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

                return View(facturasretorno);
            }

            ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
            ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");

            return View();
        }



        public ActionResult Search()
        {

            var facturasretorno = new InvoiceHeaderView
            {

            };



            ViewBag.CustNum = new SelectList(db.Customers.Where(i => i.VendorId == vendorid), "CustNum", "Names");
            ViewBag.PeriodId = new SelectList(db.Calendars.OrderBy(c => c.Description), "CalendarId", "Description");
            return View(facturasretorno);
        }



        public ActionResult DetalleFactura(int? id)
        {
            var detallefactura = db.InvoiceDetails.Where(i => i.InvoiceHeaderId == id && i.VendorId == vendorid).ToList();
            return View(detallefactura);
        }




        // GET: InvoiceHeaders1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            if (invoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoiceHeaders1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceHeaderId,Company,CustNum,CustID,Names,InvoiceNum,InvoiceType,InvoiceAmt,InvoiceBal,CheckSaldo,InvoiceDate,ClosedDate,DueDate,DiasVencimiento,OpenInvoice,VendorId")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceHeaders.Add(invoiceHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            if (invoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceHeaderId,Company,CustNum,CustID,Names,InvoiceNum,InvoiceType,InvoiceAmt,InvoiceBal,CheckSaldo,InvoiceDate,ClosedDate,DueDate,DiasVencimiento,OpenInvoice,VendorId")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            if (invoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceHeader invoiceHeader = db.InvoiceHeaders.Find(id);
            db.InvoiceHeaders.Remove(invoiceHeader);
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


        #region JS

        public JsonResult SearchCustomer(string pCustID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var customer = db.Customers.Where(s => s.CustId == pCustID && s.VendorId == vendorid).FirstOrDefault();

            if (customer == null)
            {
                var customernull = new Customer
                {
                    CustomerId = 99999,
                    CustNum = 99999,
                    Names = "NoEncontrado",
                };
                return Json(customernull);

            }
            return Json(customer);
        }


        public JsonResult CargarFechasXPeriodo(int CalendarId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var calendar = db.Calendars.Where(c => c.CalendarId == CalendarId).FirstOrDefault();
            //var formated_finicial = Convert.ToDateTime(calendar.StartDate.ToString(string.Format("MM-DD-YYYY HH:mm:ss")));
            //var formated_ffinal = Convert.ToDateTime(calendar.EndDate.ToString(string.Format("MM-DD-YYYY HH:mm:ss")));

            return Json(calendar);
        }


        //bool pFiltroFechas, bool pFiltroPeriodo, bool pFacturaConSaldo, int pCustID, DateTime pFechaI, DateTime pFechaFinal, string pCalendarFFinal, string pCalendarFinicial
        //public JsonResult SearchInvoices(InvoiceHeaderView view)
        //{
        //    bool pFacturaConSaldo = view.pFacturaConSaldo;
        //    bool pFiltroFechas = view.pFiltroFechas;
        //    bool pFiltroPeriodo = view.pFiltroPeriodo;
        //    bool pFiltroCliente = view.pFiltroCliente;
        //    bool HayFiltro = false;
        //    int pCustID = view.CustNum;
        //    DateTime pFechaInicial = Convert.ToDateTime(view.pFechaInicial);
        //    DateTime pFechaFinal = Convert.ToDateTime(view.pFechaFinal);

        //    DateTime pCalendarFInicial = Convert.ToDateTime(view.pCalendarFInicial);
        //    DateTime pCalendarFFinal = Convert.ToDateTime(view.pCalendarFFinal);

        //    if (pCustID != 0)
        //    {
        //        pFiltroCliente = true;
        //    }

        //    if (pFiltroFechas || pFiltroPeriodo || pFacturaConSaldo || pFiltroCliente) { HayFiltro = true; }

        //    if (HayFiltro)
        //    {

        //        if (pFiltroFechas && pFiltroPeriodo)
        //        {
        //            ModelState.AddModelError("",
        //            "No es posible filtrar por rango de fechas y periodo a la vez.");
        //            return Json(ViewData);
        //        }

        //        //filtro x fact saldo
        //        if (pFacturaConSaldo == true && pFiltroPeriodo == false && pFiltroFechas == false)
        //        {

        //            var facturasconsaldo = db
        //                .InvoiceHeaders
        //                .Where(s => s.InvoiceBal > 0)
        //                .ToList();


        //            if (facturasconsaldo == null || facturasconsaldo.Count == 0)

        //            {
        //                ModelState.AddModelError("",
        //                 "No se encontraron facturas");
        //                return Json(ViewData);
        //            }

        //            return Json(facturasconsaldo);
        //        }

        //        //filtro x fechas
        //        if (pFiltroFechas == true && pFiltroPeriodo == false && pFacturaConSaldo == false)
        //        {

        //            try
        //            {
        //                var finicialfiltro = pFechaInicial.Date;
        //                var ffinalfiltro = pFechaFinal.Date;

        //                var invoicesxfechas = db
        //                .InvoiceHeaders
        //                .Where(s => s.InvoiceDate >= finicialfiltro && s.InvoiceDate <= ffinalfiltro)
        //                .ToList();


        //                if (invoicesxfechas == null || invoicesxfechas.Count == 0)

        //                {

        //                    ModelState.AddModelError("",
        //                      "No se encontraron facturas");
        //                    return Json(ViewData);
        //                }


        //                return Json(invoicesxfechas);
        //            }
        //            catch (Exception ex)
        //            {

        //                TempData["CustomError"] += ex.ToString();
        //            }


        //        }

        //        //filtro x fechas y fact. con saldo
        //        if (pFiltroFechas == true && pFacturaConSaldo == true && pFiltroPeriodo == false)
        //        {
        //            var invoicesxfechas = db
        //             .InvoiceHeaders
        //             .Where(s => s.InvoiceDate.Value.Date >= pCalendarFInicial.Date && s.InvoiceDate.Value.Date <= pFechaFinal.Date && s.InvoiceBal > 0)
        //             .ToList();

        //            if (invoicesxfechas == null || invoicesxfechas.Count == 0)

        //            {
        //                ModelState.AddModelError("",
        //              "No se encontraron facturas");
        //                return Json(ViewData);
        //            }
        //            return Json(invoicesxfechas);
        //        }

        //        //filtro x periodo
        //        if (pFiltroPeriodo == true && pFiltroFechas == false && pFacturaConSaldo == false)
        //        {

        //            var facturasxperiodo = db
        //             .InvoiceHeaders
        //             .Where(s => Convert.ToDateTime(s.InvoiceDate) >= pCalendarFInicial && Convert.ToDateTime(s.InvoiceDate) <= pCalendarFFinal)
        //             .ToList();

        //            if (facturasxperiodo == null || facturasxperiodo.Count == 0)

        //            {
        //                ModelState.AddModelError("",
        //                  "No se encontraron facturas");
        //                return Json(ViewData);
        //            }

        //            return Json(facturasxperiodo);
        //        }

        //        //filtro x periodo y fact. con saldo
        //        if (pFiltroPeriodo == true && pFacturaConSaldo == true && pFiltroFechas == false)
        //        {

        //            var facturasxpreiodo = db
        //             .InvoiceHeaders
        //             .Where(s => Convert.ToDateTime(s.InvoiceDate) >= pCalendarFInicial && Convert.ToDateTime(s.InvoiceDate) <= pCalendarFFinal)
        //             .ToList();

        //            if (facturasxpreiodo == null || facturasxpreiodo.Count == 0)

        //            {
        //                TempData["CustomError"] = "No se encontraron facturas.";
        //                return Json(ViewData);
        //            }

        //            return Json(facturasxpreiodo);
        //        }

        //    }

        //    else

        //    {

        //        var facturas = db
        //               .InvoiceHeaders
        //               .ToList();

        //        if (facturas == null || facturas.Count == 0)

        //        {
        //            ModelState.AddModelError("",
        //       "No se encontraron facturas");
        //            return Json(ViewData);

        //        }

        //        return Json(facturas, JsonRequestBehavior.AllowGet);
        //    }

        //    ModelState.AddModelError("",
        //    "No se encontraron facturas");
        //    return Json(ViewData);

        //}


        #endregion


    }
}
