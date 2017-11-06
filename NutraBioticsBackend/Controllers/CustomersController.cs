using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NutraBioticsBackend.Models;
using PagedList;

namespace NutraBioticsBackend.Controllers
{
    //Authorize(Roles = "User")]
    public class CustomersController : Controller
    {
        public DataContext db = new DataContext();

        int sessionVendorID = Convert.ToInt32(System.Web.HttpContext.Current.Session["VendorId"]);

        // GET: Customers
        public ActionResult Index()
        {

            var customers = db.Customers.Where(c => c.VendorId == sessionVendorID).ToList();


            //int? page = null)
            //page = (page ?? 1);

            //return View(db.Customers
            //    .OrderBy(c => c.Names)
            //    .ThenBy(c => c.LastNames)
            //    .ToPagedList((int)page, 5));

            ////Sin paginacion
            //var customers = db.Customers.Where(c => c.VendorId == sessionVendorID);

                return View(customers.ToList());
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ViewResult Index(string btnSubmit)
        //{
        //    if (btnSubmit == null)
        //    {
        //        var customers = db.Customers.Where(c => c.VendorId == sessionVendorID).ToList();
        //        return View(customers);
        //    }
        //    else
        //    {
        //        var customers = db.Customers.Where(c => c.VendorId == sessionVendorID).ToList();
        //        var customers2 = customers.Where(c => c.Names.Contains(btnSubmit)).ToList();
        //        return View(customers2);
        //    }
        //}


        //[HttpPost]
        //public ActionResult Index(Customer view)
        //{
        //    var customers = db.Customers
        //        .Include(c => c.ResaleId)
        //        .OrderBy(c => c.ResaleId)
        //        .ThenBy(c => c.Names)
        //        .ToList();

        //    if (!string.IsNullOrEmpty(view.Names))
        //    {
        //        customers = customers.Where(c => c.Names.ToUpper().Contains(view.Names.ToUpper())).ToList();
        //    }

        //    view = customers;

        //    return View(view);
        //}




        //// GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
            }

            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }


            //return View(db.CustomerViews
            //    .Where(cv=> cv.CustomerId == customer.CustomerId)
            //    .OrderBy(c => c.Names)
            //    .ThenBy(c => c.LastNames)
            //    .ToPagedList((int)page, 5));

            var customerview = new CustomerView
            {
                CustomerId = customer.CustomerId,
                Terms = customer.Terms,
                TermsCode = customer.TermsCode,
                CustId = customer.CustId,
                Company = customer.Company,
                Address = customer.Address,
                Country = customer.Country,
                CountryId = customer.CountryId,
                State = customer.State,
                CreditHold = customer.CreditHold,
                TerritoryId = customer.TerritoryId,
                City = customer.City,
                ShipViaCode = customer.ShipViaCode,
                LastNames = customer.LastNames,
                Names = customer.Names,
                PhoneNum = customer.PhoneNum,
                ResaleId = customer.ResaleId,
                CustNum = customer.CustNum,
                TerritoryEpicorId = customer.TerritoryId,

                //cargo shiptoes diferentes de rowmod U
                ShipTos = customer.ShipTos.Where(s => s.RowMod != "D").ToList(),
            };

            return View(customerview);
        }




        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorEpicorId");
            return View();
        }

        // POST: Customers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,CustId,CustNum,Company,ResaleId,TerritoryId,ShipViaCode,CountryId,State,City,Address,PhoneNum,Names,LastNames,CreditHold,TermsCode,Terms,VendorId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorEpicorId", customer.VendorId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorEpicorId", customer.VendorId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,CustId,CustNum,Company,ResaleId,TerritoryId,ShipViaCode,CountryId,State,City,Address,PhoneNum,Names,LastNames,CreditHold,TermsCode,Terms,VendorId")] CustomerView customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "VendorEpicorId", customer.VendorId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
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


        public JsonResult GetCustomerList(int CustomerId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var customers = db.Customers.Where(c => c.CustomerId == CustomerId);
            return Json(customers);
        }


        public JsonResult GetShipToList(int ShipToId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var shiptoes = db.ShipToes.Where(c => c.ShipToId == ShipToId);
            return Json(shiptoes);
        }

        #region Sucursales




        // GET: Customers/CreateShipTo
        [SessionExpireFilter]
        public ActionResult CreateShipTo(int id)
        {
            ViewBag.CustomerId = new SelectList(db.Customers.Where(c => c.VendorId == sessionVendorID && c.CustomerId == id), "CustomerId", "Names", id);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountry(), "CountryId", "Description");
            ViewBag.TerritoryEpicorId = new SelectList(CombosHelper.GetTerritory(), "TerritoryEpicorId", "TerritoryDesc");

            return View();
        }

        // POST: ShipToes/CreateShipTo
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult CreateShipTo(ShipTo shipTo)
        {
            var country = db.Countries.Find(shipTo.CountryId);
            shipTo.Country = country.Description;

            var customer = db.Customers.Where(c => c.CustomerId == shipTo.CustomerId).FirstOrDefault();
            shipTo.CustNum = customer.CustNum;

            //Valido si el shipto ya existe para el cliente
            var shipto = db.ShipToes.Where(s => s.ShipToNum == shipTo.ShipToNum && s.CustNum == shipTo.CustNum).FirstOrDefault();

            if (shipto != null)
            {
                TempData["CustomError"] = "El numero del paciente : " + shipto.ShipToNum.ToString() + " , ya se encuentra registrado.";
                return RedirectToAction("Details" + "/" + shipTo.CustomerId);
            }



            //Uso de variables de Session.
            shipTo.VendorId = sessionVendorID;
            shipTo.Company = System.Web.HttpContext.Current.Session["Company"].ToString();
            shipTo.RowMod = "C";



            if (ModelState.IsValid)
            {
                db.ShipToes.Add(shipTo);
                db.SaveChanges();

                return RedirectToAction("Details" + "/" + shipTo.CustomerId);
            }

            return View(shipTo);
        }


        public JsonResult Get(int CustomerId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var shiptoes = db.ShipToes.Where(s => s.CustomerId == CustomerId);
            return Json(shiptoes);
        }


        public ActionResult EditShipTo(int? id)
        {
            ShipTo shipTo = db.ShipToes.Find(id);
            if (shipTo == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerId = new SelectList(db.Customers.Where(c => c.CustomerId == shipTo.CustomerId), "CustomerId", "Names", shipTo.CustomerId);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountry(), "CountryId", "Description", shipTo.CountryId);
            ViewBag.TerritoryEpicorId = new SelectList(CombosHelper.GetTerritory(), "TerritoryEpicorId", "TerritoryDesc", shipTo.TerritoryEpicorId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var shiptoview = new ShipToView
            {
                ShipToId = shipTo.ShipToId,
                ShipToNum = shipTo.ShipToNum,
                ShipToName = shipTo.ShipToName,
                CustomerId = shipTo.CustomerId,
                CustNum = shipTo.CustNum,
                Company = shipTo.Company,
                Address = shipTo.Address,
                Address2 = shipTo.Address2,
                Address3 = shipTo.Address3,
                State = shipTo.State,
                Email = shipTo.Email,
                Country = shipTo.Country,
                CountryId = shipTo.CountryId,
                SincronizadoEpicor = shipTo.SincronizadoEpicor,
                VendorId = shipTo.VendorId,
                City = shipTo.City,
                PhoneNum = shipTo.PhoneNum,
                Contacts = shipTo.Contacts.Where(c => c.RowMod != "D").ToList(),
            };
            return View(shiptoview);
        }

        // POST: ShipToes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult EditShipTo([Bind(Include = "ShipToId,CustomerId,ShipToNum,CustNum,Company,ShipToName,TerritoryEpicorId,CountryId,Country,State,City,Address,PhoneNum,Email,VendorId,SincronizadoEpicor")] ShipTo shipTo)
        public ActionResult EditShipTo(ShipTo shipTo)
        {
            if (ModelState.IsValid)
            {
                var country = db.Countries.Find(shipTo.CountryId);
                shipTo.Country = country.Description;

                var customer = db.Customers.Where(c => c.CustomerId == shipTo.CustomerId).FirstOrDefault();
                shipTo.CustNum = customer.CustNum;

                shipTo.SincronizadoEpicor = false;

                //Uso de variables de Session
                shipTo.VendorId = sessionVendorID;
                shipTo.Company = System.Web.HttpContext.Current.Session["Company"].ToString();
                shipTo.RowMod = "U";

                db.Entry(shipTo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details" + "/" + shipTo.CustomerId);
            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(c => c.VendorId == sessionVendorID), "CustomerId", "Names", shipTo.CustomerId);
            return View(shipTo);
        }

        public ActionResult BuscarCliente(string pFilter)
        {
            var customers = from s in db.Customers select s;
            if (!string.IsNullOrEmpty(pFilter))
            {
                customers = customers.Where(c => c.Names.Contains(pFilter));
            }
            return View(customers);
        }

        // GET: ShipToes/DeleteShipTo/5
        public ActionResult DeleteShipTo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var contacts = db.Contacts.Where(c => c.ShipToId == id).FirstOrDefault();

            ////Contact contact = db.Contacts.Find(contacts.ShipToId);

            //if (contacts != null)
            //{

            //}

            ShipTo shipTo = db.ShipToes.Find(id);
            if (shipTo == null)
            {
                return HttpNotFound();
            }
            return View(shipTo);
        }

        // POST: ShipToes/DeleteShipTo/5
        [HttpPost, ActionName("DeleteShipTo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSHConfirmed(int id)
        {
            ShipTo shipTo = db.ShipToes.Find(id);
            shipTo.RowMod = "D";
            shipTo.SincronizadoEpicor = false;
            db.Entry(shipTo).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                TempData["CustomError"] = "No se puede eliminar la sucursal.";
                return RedirectToAction("Details" + "/" + shipTo.CustomerId);
            }
            return RedirectToAction("Details" + "/" + shipTo.CustomerId);


        }

        #endregion


        #region Contactos

        // GET: Contacts/Create
        [SessionExpireFilter]
        public ActionResult CreateContact(int? id)
        {
       
            ViewBag.PerConID = new SelectList(db.PersonContacts, "PerConID", "Name");
            ViewBag.ShipToId = new SelectList(db.ShipToes.Where(s => s.ShipToId == id), "ShipToId", "ShipToName", id);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountry(), "CountryId", "Description");

            
            return View();
        }

        // POST: Contacts/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        public ActionResult CreateContact(Contact contact)
        {
            if (ModelState.IsValid)
            {

                if (contact.Country != null)
                {
                    var country = db.Countries.Find(contact.CountryId);
                    contact.Country = country.Description;
                }
                else
                {
                    contact.Country = "Colombia";
                }

                var percon = db.PersonContacts.Where(s => s.PerConID == contact.PerConID).FirstOrDefault();
                if (string.IsNullOrEmpty(contact.PagerNum))
                {
                    contact.PagerNum = "0";
                }
                if (contact.Name == null) { contact.Name = string.Empty; }
                if (contact.PerConID == 0 && contact.Name == string.Empty) { contact.Name = "Sin Nombre"; }
                if (contact.PerConID != 0 && contact.Name == string.Empty) { contact.Name = percon.Name; }

              

                    //contact.Name = percon.Name;
                    //contact.Address = percon.Address1;
                    //contact.Email = percon.EMailAddress;
                    //contact.City = percon.City;
                    //contact.State = percon.State;
                

                var shipto = db.ShipToes.Where(s => s.ShipToId == contact.ShipToId).FirstOrDefault();
                contact.CustNum = shipto.CustNum;
                contact.ShipToNum = shipto.ShipToNum;


                var UltimoRegistro = db
                .Contacts
                .OrderByDescending(c => c.ConNum)
                .Where(c => c.ShipToId == contact.ShipToId)
                .FirstOrDefault();

                if (UltimoRegistro == null)
                {
                    var UltimoContacto = new Contact
                    {
                        ConNum = 1,
                    };

                    contact.ConNum = UltimoContacto.ConNum;
                }
                else
                {
                    contact.ConNum = UltimoRegistro.ConNum + 1;
                }

                //Uso de variables de Session
                contact.VendorId = sessionVendorID;
                contact.Company = System.Web.HttpContext.Current.Session["Company"].ToString();
                contact.RowMod = "C";
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("EditShipTo" + "/" + contact.ShipToId); ;
            }
            ViewBag.ShipToId = new SelectList(db.ShipToes.Where(s => s.ShipToId == contact.ShipToId), "ShipToId", "ShipToNum", contact.ShipToId);
            return View(contact);
        }



        // GET: Contacts/Edit/5
        public ActionResult EditContact(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            ViewBag.PerConID = new SelectList(db.PersonContacts, "PerConID", "Name", contact.PerConID);
            ViewBag.ShipToId = new SelectList(db.ShipToes.Where(s => s.ShipToId == contact.ShipToId), "ShipToId", "ShipToName", contact.ShipToId);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountry(), "CountryId", "Description", contact.CountryId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContact(Contact contact)
        {
            if (ModelState.IsValid)
            {

                if (contact.Country != null)
                {
                    var country = db.Countries.Find(contact.CountryId);
                    contact.Country = country.Description;
                }
                else
                {
                    contact.Country = "Colombia";
                }

                if (contact.Name == null) { contact.Name = string.Empty; }

                if(contact.PerConID != 0) {

                    var percon = db.PersonContacts.Where(s => s.PerConID == contact.PerConID).FirstOrDefault();
                    if (contact.Name == string.Empty)
                    {
                        contact.Name = percon.Name;
                    }
                }

              
                if (contact.PerConID == 0 && contact.Name == string.Empty) { contact.Name = "Sin Nombre"; }



                //contact.Name = percon.Name;

                // var UltimoRegistro = db
                //.Contacts
                //.OrderByDescending(c => c.ConNum)
                //.Where(c => c.ShipToId == contact.ShipToId)
                //.FirstOrDefault();

                //var contactNUM = db.Contacts.Where(c => c.ContactId == contact.ContactId).FirstOrDefault();

                //contact.ConNum = contactNUM.ConNum;
                //contact.Name = percon.Name;
                //contact.Address = percon.Address1;
                //contact.Email = percon.EMailAddress;
                //contact.City = percon.City;
                //contact.State = percon.State;

                contact.SincronizadoEpicor = false;

                //Uso de variables de Session
                contact.VendorId = sessionVendorID;
                contact.Company = System.Web.HttpContext.Current.Session["Company"].ToString();


                var shipto = db.ShipToes.Where(s => s.ShipToId == contact.ShipToId).FirstOrDefault();
                contact.CustNum = shipto.CustNum;
                contact.ShipToNum = shipto.ShipToNum;

                contact.RowMod = "U";
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditShipTo" + "/" + contact.ShipToId);
            }

            ViewBag.ShipToId = new SelectList(db.ShipToes.Where(s => s.ShipToId == contact.ShipToId), "ShipToId", "ShipToNum", contact.ShipToId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult DeleteContact(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("DeleteContact")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            contact.RowMod = "D";
            contact.SincronizadoEpicor = false;
            db.Entry(contact).State = EntityState.Modified;
            // db.Contacts.Remove(contact);
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("EditShipTo" + "/" + contact.ShipToId);
        }

        #endregion

        public JsonResult PerConIDAutoComplete(int Prefix)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var personcontacts = db.PersonContacts.ToList();


            //Searching records from list using LINQ query  
            var PersonContact = (from N in personcontacts
                                 where N.PerConID.ToString().StartsWith(Prefix.ToString())
                                 select new { N.PerConID });
            return Json(PersonContact, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SelectPerConIDFromTextBox(int PerConId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var perconid = db.PersonContacts.Where(s => s.PerConID == PerConId).FirstOrDefault();

            if (perconid == null)
            {

                var perconnull = new PersonContact
                {
                    PerConID = 999,
                };
                return Json(perconnull, JsonRequestBehavior.AllowGet);

            }
            return Json(perconid, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPersonContactData(int? PerConID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var perconid = db.PersonContacts.Where(s => s.PerConID == PerConID);
            return Json(perconid, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarSh(string ShipToNum, int? CustNum)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var custnum = db.Customers.Where(c => c.CustomerId == CustNum).FirstOrDefault();
            var shipto = db.ShipToes.Where(s => s.CustNum == custnum.CustNum && s.ShipToNum == ShipToNum).FirstOrDefault();
            return Json(shipto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContactName(int? PerConID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var name = db.PersonContacts.Where(s => s.PerConID == PerConID).FirstOrDefault();
            return Json(name.Name, JsonRequestBehavior.AllowGet);
        }





    }
}
