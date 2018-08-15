using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using project0_7.Models;
namespace project0_7
{
    public partial class CheckingAccount
    {
        public const string Text = "checking";

        public bool Transaction(decimal Amount, string Description)
        {
            if (Balance + Amount > 0)
            {
                using (var db = new BankingEntities1())
                {

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Amount is the amount of money being withdrawn or deposited
                            // Positive is a Deposit
                            // Negative is a Withdraw

                            if (Balance + Amount >= 0)
                            {
                                var Transaction = new Transaction
                                {
                                    Amount = Amount,
                                    AccountType = Text,
                                    DateTime = DateTime.Now,
                                    Description = Description,
                                    AccountId = Id
                                };
                                Debug.WriteLine("Transaction insertion.");
                                db.Transactions.Add(Transaction);
                                db.CheckingAccounts.Find(Id).Balance += Amount;
                            }
                            Debug.WriteLine("CheckingAccounts balance updated. Saving changes.s");
                            db.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                        {
                            Debug.WriteLine(e.InnerException);
                            Debug.WriteLine(e.Message);
                            Debug.WriteLine(e.Source);
                            foreach (var eve in e.Data)
                            {
                                Debug.WriteLine(eve);
                            }
                            throw;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private void TransferHelper(dynamic obj, decimal Amount)
        {
            if (Amount > 0 && Transaction(Amount * -1, $"Internet transfer to {obj.Text}: {obj.Id}"))
            {
                if (!obj.Transfer(Amount, $"Internet transfer from {Text}: Id"))
                {
                    Transaction(Amount, $"Error. Reconciling");
                }
            }
        }

        public void Transfer(BusinessAccount foreign, decimal Amount)
        {
            TransferHelper(foreign, Amount);
        }

        public void Transfer(CheckingAccount foreign, decimal Amount)
        {
            TransferHelper(foreign, Amount);
        }

        public void Transfer(Loan foreign, decimal Amount)
        {
            TransferHelper(foreign, Amount);
        }

        public decimal CalculateInterest()
        {
            return Balance * InterestRate;
        }

        public IEnumerable<Transaction> TransactionsDate(DateTime Start, DateTime Finish)
        {
            var testList = from t in DatabaseAccessor.db.Transactions
                           where t.AccountId == Id &&
                           t.AccountType == Text
                           select t;

            var list = (from t in DatabaseAccessor.db.Transactions
                       where t.AccountId == Id &&
                       t.AccountType == Text &&
                       t.DateTime >= Start &&
                       t.DateTime < Finish
                       select t).ToList();
            Debug.WriteLine(list.Count);

            return list;
        }
    }

}