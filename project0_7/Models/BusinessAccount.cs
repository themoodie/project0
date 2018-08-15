using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using project0_7.Models;
namespace project0_7
{
    public partial class BusinessAccount
    {
        public const string Text = "banking";

        public bool Transaction(decimal Amount, string Description)
        {
            Debug.WriteLine("TRANSACTION");
            using (var db = new BankingEntities1())
            {
                Balance += Amount;

                var OverdraftFee = -30;
                bool OverDraft = false;
                var ODFee = new Transaction
                {
                    Amount = OverdraftFee,
                    AccountType = Text,
                    DateTime = DateTime.Now,
                    Description = "Overdraft charge",
                    AccountId = Id
                };
                var Transaction = new Transaction
                {
                    Amount = Amount,
                    AccountType = Text,
                    DateTime = DateTime.Now,
                    Description = Description,
                    AccountId = Id
                };
                db.Transactions.Add(Transaction);

                if (Balance < 0 && Amount < 0)
                {
                    // If the account balance is less than zero
                    // and the amount is negative it's a withdrawl
                    // apply a fee

                    OverDraft = true;
                    db.SaveChanges();
                }

                db.Transactions.Add(Transaction);
                if (OverDraft)
                {
                    db.Transactions.Add(ODFee);
                    Balance += OverdraftFee;
                }
                var BusAccount = db.BusinessAccounts.Find(Id);
                BusAccount.Balance = Balance;
                db.SaveChanges();
                return true;
                /*
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Amount is the amount of money being withdrawn or deposited
                        // Positive is a Deposit
                        // Negative is a Withdraw
                        Balance += Amount;

                        var Transaction = new Transaction
                        {
                            Amount = Amount,
                            AccountType = Text,
                            DateTime = DateTime.Now,
                            Description = Description,
                            AccountId = Id
                        };
                        db.Transactions.Add(Transaction);

                        if (Balance < 0 && Amount < 0)
                        {
                            // If the account balance is less than zero
                            // and the amount is negative it's a withdrawl
                            // apply a fee
                            var OverdraftFee = -30;

                            var ODFee = new Transaction
                            {
                                Amount = OverdraftFee,
                                AccountType = Text,
                                DateTime = DateTime.Now,
                                Description = "Overdraft charge",
                                AccountId = Id
                            };
                            Balance += OverdraftFee;
                            db.Transactions.Add(ODFee);
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        Debug.WriteLine("END TRANSACTION");
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }*/
            }
        }

        private void TransferHelper(dynamic obj, decimal Amount)
        {
            if(Amount > 0)
            {
                // Only transfer positive sums
                if(Transaction(Amount * -1, $"Internet transfer to {obj.Text}: {obj.Id}"))
                {
                    // If the withdrawal transaction is successful
                    if (!obj.Transfer(Amount, $"Internet transfer from {Text}: Id"))
                    {
                        // If the transfer to the transfer fails... then deposit the sum back to
                        // local account
                        Transaction(Amount, $"Error. Reconciling");
                    }
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


        public IEnumerable<Transaction> TransactionsDate(DateTime Start, DateTime Finish)
        {

            return DatabaseAccessor.db.Transactions.ToList().FindAll(
                t => t.AccountId == Id &&
                t.AccountType == Text &&
                t.DateTime >= Start &&
                t.DateTime <= Finish &&
                t.AccountType == Text
            );
        }
    }
}