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
    public class CheckingAccountsController : Controller
    {
        private BankingEntities1 db = new BankingEntities1();
        

        // GET: CheckingAccounts
        public ActionResult Index()
        {
            try
            {
                var UserId = Convert.ToInt32(Session["UserId"]);
                Debug.WriteLine($"{UserId}");
                if (UserId != 0)
                {
                    var checkingAccounts = db.CheckingAccounts.Include(c => c.Customer).Where(c => c.Customer.Id == UserId && c.Active == true);
                    return View(checkingAccounts.ToList());
                }
            }
            catch(NullReferenceException ex)
            {   
            }
            return RedirectToAction("Index", "Customers");
        }

        // GET: CheckingAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UserId = Convert.ToInt32(Session["UserId"]);
            CheckingAccount checkingAccount =
                db.CheckingAccounts.SingleOrDefault(
                    ca => ca.Customer.Id == UserId &&
                    ca.Id == (int)id
                );
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            return View(checkingAccount);
        }

        // GET: CheckingAccounts/Create
        public ActionResult Create()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            ViewBag.UserId = db.Customers.SingleOrDefault(Cust => Cust.Id == UserId);
            return View();
        }

        // POST: CheckingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,UserId,OpenDate,CloseDate,Active,Balance,InterestRate")]*/ Models.CheckingAccountViewModel cavm)
        {
            if (ModelState.IsValid)
            {
                var UserId = Convert.ToInt32(Session["UserId"]);
                var DT = DateTime.Now;
                var CheckingAccount = new CheckingAccount
                {
                    UserId = UserId,
                    Balance = cavm.Balance,
                    Customer = db.Customers.SingleOrDefault(cust=>cust.Id == UserId),
                    OpenDate = DT,
                    Active = true,
                    InterestRate = 1.05m
                };
                db.CheckingAccounts.Add(CheckingAccount);
                db.SaveChanges();
                db.Transactions.Add(
                    new Transaction
                    {
                        Amount = cavm.Balance,
                        DateTime = DT,
                        AccountType = CheckingAccount.Text,
                        Description = "Opening Deposit",
                        AccountId = CheckingAccount.Id
                    }
                );
                db.SaveChanges();
                return RedirectToAction("Details", new { id = CheckingAccount.Id });
            }

            //ViewBag.UserId = new SelectList(db.Customers, "Id", "Email", checkingAccount.UserId);
            return View(cavm);
        }

        // GET: CheckingAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Customers, "Id", "Email", checkingAccount.UserId);
            return View(checkingAccount);
        }

        // POST: CheckingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,OpenDate,CloseDate,Active,Balance,InterestRate")] CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Customers, "Id", "Email", checkingAccount.UserId);
            return View(checkingAccount);
        }

        // GET: CheckingAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            return View(checkingAccount);
        }

        // POST: CheckingAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            db.CheckingAccounts.Remove(checkingAccount);
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
        #region Transactions
        // GET: Transactions
        public ActionResult Transactions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CheckAccount = db.CheckingAccounts.ToList()
                            .Find(Account => Account.Id == id);
            var Transactions = (from t in db.Transactions
                                where t.AccountId == CheckAccount.Id &&
                                t.AccountType == CheckingAccount.Text
                                orderby t.DateTime descending
                                select t).Take(10).ToList();

            return View(
                new Models.CTransactionsViewModel
                {
                    CheckingAccount = CheckAccount,
                    Transactions = Transactions
                }
            );
        }

        // Get:
        public ActionResult CreateTransaction(int? id)
        {
            Debug.WriteLine("Create Transaction");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new Models.CreateTransactionCheckingViewModel
            {
                CheckingAccount =
                (
                    from b in db.CheckingAccounts
                    where b.Id == id
                    select b
                ).ToList().SingleOrDefault(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateTransaction(Models.CreateTransactionCheckingViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var id = Convert.ToInt32(RouteData.Values["id"]);
                Debug.Write($"Checking account: {id}");
                vm.CheckingAccount = (from ba in db.CheckingAccounts
                                      where ba.Id == id &&
                                      ba.Active == true
                                      select ba).SingleOrDefault();
                if (vm.Description == "")
                {
                    if (vm.Amount > 0)
                    {
                        vm.Description = "Online Deposit";
                    }
                    else
                    {
                        vm.Description = "Online Withdrawal";
                    }
                }

                vm.CheckingAccount.Transaction(vm.Amount, vm.Description);
                return RedirectToAction("Details", new { id = vm.CheckingAccount.Id });
            }
            return View(vm);
        }
        #region Transactions Dateview
        private Models.CTransactionsDateViewModel GenerateTransactionsDateViewModel(int id, DateTime start, DateTime end)
        {
            var CheckingAccount = db.CheckingAccounts.Single(ba => ba.Id == (int)id);
            var model = new Models.CTransactionsDateViewModel
            {
                End = end,
                Start = start,
                CheckingAccountId = (int)id,
                CheckingAccount = CheckingAccount,
                Transactions = CheckingAccount.TransactionsDate(start, end)
            };
            return model;
        }
        //Get: Transactions(Window of time)
        public ActionResult TransactionsDate(int? id)
        {
            if (id != null)
            {
                var start = DateTime.Now.AddDays(-30);
                var end = DateTime.Now;

                return View(GenerateTransactionsDateViewModel((int)id, start, end));
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult TransactionsDate(Models.CTransactionsDateViewModel vm)
        {
            var id = Convert.ToInt32(RouteData.Values["id"]);
            if (ModelState.IsValid)
            {
                var CheckingAccountId = Convert.ToInt32(RouteData.Values["id"]);
                return View(GenerateTransactionsDateViewModel(CheckingAccountId, vm.Start, vm.End));
            }
            return RedirectToAction("CheckingAccounts");
        }
        #endregion
        #endregion

    }
}
