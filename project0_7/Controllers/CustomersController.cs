using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using project0_7;

namespace project0_7.Controllers
{
    public class CustomersController : Controller
    {
        private BankingEntities1 db = new BankingEntities1();

        // GET: Customers
        public ActionResult Index()
        {
            Debug.WriteLine("Redirecting to /Details");
            return RedirectToAction("Details", new { id = Session["UserId"] });
            //return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if(Session["UserId"] != null)
            {
                var customers = db.Customers.ToList();
                id = customers.SingleOrDefault(c => c.Id == (int)Session["UserId"]).Id;
            }
            else
            {
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            Debug.WriteLine(Session["UserId"]);
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.InterestRate = new InterestManager().Checking;
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Email,Password,Address1,Address2,City,Zip,Phone,OpenDate,CloseDate,Active")]*/ Customer customer)
        {
            //var EmailVerify = db.Customers.Count(q => q.Email == customer.Email);
            var EmailVerify = db.Customers.SingleOrDefault(q => q.Email == customer.Email);
            if (EmailVerify == null)
            {
                if (ModelState.IsValid)
                {
                    //Form validation is good. Insert customer and set session var
                    customer.OpenDate = DateTime.Now;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    var Customers = db.Customers.ToList();
                    var Customer = Customers.Find(c => c.Id == customer.Id);
                    Session["UserId"] = Convert.ToString(Customer.Id);
                    Session["Password"] = Convert.ToString(Customer.Password);
                    return RedirectToAction("Details", Customer.Id);
                }
            }
            else
            {
                ViewBag.ValidationSummary = "Email is in use. Did you forget your password?";
            }
            return View();
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
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Password,Address1,Address2,City,Zip,Phone,OpenDate,CloseDate,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult ViewTest()
        {
            return View();
        }
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Models.CustomerLoginViewModel CLVM)
        {
            Debug.WriteLine("Reached POST: Customers/Login");
            var User = (from u in db.Customers
                       where u.Email == CLVM.Email
                       && u.Password == CLVM.Password
                       select u).SingleOrDefault();
            if (User != null)
            {
                Session["UserId"] = User.Id;
                Session["Username"] = User.Email;
                Debug.WriteLine("Routing to dets");
                //RedirectToRoute("Details", UserId);
                return RedirectToAction("Details", new { id = User.Id });
                //RedirectToAction("Details", UserId)
            }
            return View();
        }

        public ActionResult Logff()
        {
            Session["UserId"] = null;
            Session["Username"] = null;
            return RedirectToAction("Login");
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
