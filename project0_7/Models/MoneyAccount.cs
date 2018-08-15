using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace project0_7.Models
{
    public static partial class MoneyAccount
    {
        private static BankingEntities1 db = new BankingEntities1();
        
        public static dynamic FindBusinessAccount(int Id)
        {
            var take = (from ca in db.BusinessAccounts
                        where ca.Id == Id
                        select ca).SingleOrDefault();
            if (take == null)
            {
                return false;
            }
            else
            {
                return take;
            }
        }

        public static dynamic FindCheckingAccount(int Id)
        {
            var take = (from ca in db.CheckingAccounts
                        where ca.UserId == Id
                        select ca).SingleOrDefault();
            if (take == null)
            {
                return false;
            }
            else
            {
                return take;
            }
        }
        public static dynamic FindLoan(int Id)
        {
            var take = (from loan in db.Loans
                        where loan.Id == Id
                        select loan).SingleOrDefault();
            if (take == null)
            {
                return false;
            }
            else
            {
                return take;
            }
        }

        public static dynamic FindTermDeposit(int Id)
        {
            var take = (from TD in db.TermDeposits
                        where TD.Id == Id
                        select TD).SingleOrDefault();
            if (take == null)
            {
                return false;
            }
            else
            {
                return take;
            }
        }

        public static dynamic FindMoneyAccount(int Id)
        {
            dynamic Account = FindCheckingAccount(Id);
            if(Account != null)
            {
                return Account as CheckingAccount;
            }
            else
            {
                Account = FindBusinessAccount(Id);
                if(Account != null)
                {
                    return Account as project0_7.BusinessAccount;
                }
                else
                {
                    Account = FindLoan(Id);
                    if (Account != null)
                    {
                        return Account as project0_7.BusinessAccount;
                    }
                }
            }
            
            var obj = new Object();
            return obj;
        }
    }
}
/*
    var BankAccounts = new List<Object>();
            BankAccounts.Add(db.BusinessAccounts.ToList());
            BankAccounts.Add(db.CheckingAccounts.ToList());
            BankAccounts.Add(db.Loans.ToList());
            BankAccounts.Add(db.TermDeposits.ToList());
                  
    var q = (from ba in db.BusinessAccounts
                     where ba.Id == Id
                     select ba).SingleOrDefault();
            if(q == null)
            {
                q = (from ca in db.CheckingAccounts
                    where ca.Id == Id
                    select ca).SingleOrDefault();
            }
                     .Union(

                        .Union(from l in db.Loans
                               where l.Id == Id);

            var Account = db.BusinessAccounts.ToList();
            if(Account.Find(a => a.Id == Id) == null)
            {
                
                //var Account = db.CheckingAccounts.ToList();

            }
            public dynamic ReturnDynAsClass(Type Class, dynamic obj)
        {
            
        }
            */
