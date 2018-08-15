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
using project0_7.Models;

namespace project0_7.Controllers
{
    public class BusinessAccountsController : Controller
    {
        private BankingEntities1 db = new BankingEntities1();
        private Customer _CUSTOMER;
        #region business accounts
        // GET: BusinessAccounts
        public ActionResult Index()
        {
            ValidateSession(Session["UserId"]);
            var businessAccounts = (from ba in db.BusinessAccounts
                                   where ba.Customer.Id == _CUSTOMER.Id &&
                                   ba.Active == true
                                   select ba).ToList();
            
            return View(businessAccounts);
        }

        // GET: BusinessAccounts/Details/5
        // Display balance.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValidateSession(Session["UserId"]);
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.Transactions = db.Transactions.
                ToList().Find(
                    x => x.AccountType == "banking" &&
                    x.Id == businessAccount.Id
                );
            return View(businessAccount);
        }

        
        // GET: BusinessAccounts/Create
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.Customers, "Id", "Email");
            return View();
        }

        // POST: BusinessAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,UserId,OpenDate,CloseDate,Active,Balance")]*/ Models.BusinessAccountCreateViewModel businessAccount)
        {
            Debug.WriteLine("Reached create post");
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    int BAId = 0;
                    try
                    {
                        Debug.WriteLine(businessAccount.OpeningDeposit);
                        var NewBusiness = new BusinessAccount
                        {
                            Balance = Convert.ToInt32(businessAccount.OpeningDeposit),
                            Active = true,
                            OpenDate = DateTime.Now,
                            UserId = Convert.ToInt32(Session["UserId"])
                        };
                        db.BusinessAccounts.Add(NewBusiness);
                        db.SaveChanges();
                        BAId = NewBusiness.Id;
                        db.Transactions.Add(
                            new Transaction
                            {
                                Description = "Opening Deposit",
                                AccountType = "banking",
                                Amount = NewBusiness.Balance,
                                DateTime = NewBusiness.OpenDate,
                                AccountId = BAId
                            });
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                    db.SaveChanges();
                    if (BAId != 0)
                    {
                        return RedirectToAction("Details", new { id = BAId });
                    }  
                    //return RedirectToAction("Index");
                }
            }
            //ViewBag.UserId = new SelectList(db.Customers, "Id", "Email", businessAccount.UserId);
            return View(businessAccount);
        }

        // GET: BusinessAccounts/Edit/5
        /*public ActionResult Edit(int? id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Customers, "Id", "Email", businessAccount.UserId);
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }*/
        /*
        // POST: BusinessAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,OpenDate,CloseDate,Active,Balance")] BusinessAccount businessAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Customers, "Id", "Email", businessAccount.UserId);
            return View(businessAccount);
        }
        */
        // GET: BusinessAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            ValidateSession(Session["UserId"]);
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.Customers.Find(_CUSTOMER)
                .BusinessAccounts.SingleOrDefault( ba => ba.Id == id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            return View(businessAccount);
        }

        // POST: BusinessAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if(businessAccount.Balance == 0)
            {
                businessAccount.Active = false;
                businessAccount.CloseDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Can't delete accounts with non-zero balance.";
            return View();
            
        }
        #endregion
        #region Transfer
        // GET: BusinessAccounts/Transfer/Action
        public ActionResult CreateTransferBusiness(int id)
        {
            Debug.WriteLine("CreateTransfer");
            var UserId = Convert.ToInt32(Session["UserId"]);
            Debug.WriteLine(UserId);
            if (UserId != 0)
            {
                var customer = db.Customers
                            .SingleOrDefault(
                                cust => cust.Id == UserId
                            );

                if (customer != null)
                {
                    //Get all business accounts that are not the target and belong to the customer
                    var ba = db.BusinessAccounts.ToList()
                    .FindAll(acc => acc.Active == true
                            && acc.Id != id
                            && acc.Customer.Id == UserId);
                    
                    ViewBag.BusinessAccounts = new SelectList(ba, "Id", "Id");
                    
                    //Get all checking accounts that are not the target and belong to the customer
                    var ca = db.CheckingAccounts.ToList()
                    .FindAll(acc => acc.Active == true
                            && acc.Id != id
                            && acc.Customer.Id == UserId);

                    ViewBag.CheckingAccounts = new SelectList(ca, "Id", "Id");

                    //Get all loans that are not the target and belong to the customer
                    var loans = db.Loans.ToList()
                    .FindAll(acc => acc.Active == true
                            && acc.Id != id
                            && acc.Customer.Id == UserId);
                    ViewBag.Loans = new SelectList(loans, "Id", "Id");
                    return View();
                }
            }
            return RedirectToAction("Login", "Customers");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: BusinessAccounts/Transfer/Action
        public ActionResult CreateTransferBusiness(Models.CreateTransferViewModel createTransfer)
        {           
            return View();
        }
        //GET Transfer to business accounts
        public ActionResult Transfer(int id)
        {
            Debug.Write("Transfer Business");
            ValidateSession(Session["UserId"]);

            var LocalAccount = db.BusinessAccounts.Find(id);
            var ForeignAccounts = _CUSTOMER.BusinessAccounts.ToList().Find(pkey => pkey.Id != id);
            return View();

        }
        #endregion

        #region Transactions
        // GET: Transactions
        public ActionResult Transactions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var BusAccount = db.BusinessAccounts.ToList()
                            .Find(Account => Account.Id == id);
            var Transactions = (from t in db.Transactions
                               where t.AccountId == BusAccount.Id &&
                               t.AccountType == BusinessAccount.Text
                               orderby t.DateTime descending
                               select t).Take(10).ToList();
                               
            return View(
                new Models.TransactionsViewModel {
                    BusinessAccount = BusAccount,
                    Transactions = Transactions
                }
            );
        }
        
        //Get:s
        public ActionResult CreateTransaction(int? id)
        {
            ValidateSession(Session["UserId"]);
            Debug.WriteLine("Create Transaction");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new Models.CreateTransactionBusinessViewModel
            {
                BusinessAccount = 
                (
                    from b in db.BusinessAccounts
                    where b.Id == id
                    select b
                ).ToList().SingleOrDefault(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateTransaction(Models.CreateTransactionBusinessViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var id = Convert.ToInt32(RouteData.Values["id"]);
                vm.BusinessAccount = (from ba in db.BusinessAccounts
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

                vm.BusinessAccount.Transaction(vm.Amount, vm.Description);
                return RedirectToAction("Details", new { id = vm.BusinessAccount.Id });
            }
            return View(vm);
        }
        #region Transactions Dateview
        private Models.TransactionsDateViewModel GenerateTransactionsDateViewModel(int id, DateTime start, DateTime end)
        {
            var BusinessAccount = db.BusinessAccounts.Single(ba => ba.Id == (int)id);
            var model = new Models.TransactionsDateViewModel
            {
                End = end,
                Start = start,
                BusinessAccountId = (int)id,
                BusinessAccount = BusinessAccount,
                Transactions = BusinessAccount.TransactionsDate(start, end)
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
        public ActionResult TransactionsDate(project0_7.Models.TransactionsDateViewModel vm)
        {
            var id = Convert.ToInt32(RouteData.Values["id"]);
            if (ModelState.IsValid)
            {
                var BusinessAccountId = Convert.ToInt32(RouteData.Values["id"]);
                return View(GenerateTransactionsDateViewModel(BusinessAccountId, vm.Start, vm.End));
            }
            return RedirectToAction("BusinessAccounts");
        }
        #endregion
        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private dynamic ValidateSession(Object User/*, SessionStateAttribute Password*/)
        {
            Debug.WriteLine("ValidateSEssion");
            var UserId = Convert.ToInt32(Session["UserId"]);
            Debug.WriteLine(UserId);
            if (UserId != 0)
            {
                _CUSTOMER = db.Customers
                            .SingleOrDefault(
                                cust => cust.Id == UserId
                            );
                return true;
            }
            else
            {
                //RedirectToRoute("Customers", "Login");
                return Redirect("Customers/Login");
            }
        }
    }
}
