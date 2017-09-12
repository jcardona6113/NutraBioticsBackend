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
    public class PersonContactsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: PersonContacts
        public ActionResult Index()
        {
            return View(db.PersonContacts.ToList());
        }

        // GET: PersonContacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonContact personContact = db.PersonContacts.Find(id);
            if (personContact == null)
            {
                return HttpNotFound();
            }
            return View(personContact);
        }

        // GET: PersonContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonContacts/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonContactId,PerConID,Company,Name,FirstName,LastName,PhoneNum,EMailAddress,CellPhoneNum,Address1,CountryId,State,City,CountryNum,SincronizadoEpicor")] PersonContact personContact)
        {
            if (ModelState.IsValid)
            {
                db.PersonContacts.Add(personContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personContact);
        }

        // GET: PersonContacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonContact personContact = db.PersonContacts.Find(id);
            if (personContact == null)
            {
                return HttpNotFound();
            }
            return View(personContact);
        }

        // POST: PersonContacts/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonContactId,PerConID,Company,Name,FirstName,LastName,PhoneNum,EMailAddress,CellPhoneNum,Address1,CountryId,State,City,CountryNum,SincronizadoEpicor")] PersonContact personContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personContact);
        }

        // GET: PersonContacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonContact personContact = db.PersonContacts.Find(id);
            if (personContact == null)
            {
                return HttpNotFound();
            }
            return View(personContact);
        }

        // POST: PersonContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonContact personContact = db.PersonContacts.Find(id);
            db.PersonContacts.Remove(personContact);
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
