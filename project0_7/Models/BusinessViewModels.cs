using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace project0_7.Models
{
    public class BusinessAccountCreateViewModel
    {
        [Required]
        [Display(Name = "Opening Balance")]
        [Range(0.0, Double.MaxValue)]
        public decimal OpeningDeposit { get; set; }
    }

    public class TransactionsViewModel
    {
        [Required]
        public BusinessAccount BusinessAccount { get; set; }
        public List<Transaction> Transactions { get; set;}
    }

    public class TransactionsDateViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime Start { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Finish Date")]
        public DateTime End { get; set; }
        [Required]
        [Key]
        public int BusinessAccountId { get; set; }
        public virtual BusinessAccount BusinessAccount { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }
    }

    public class CreateTransactionViewModel
    {
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class CreateTransactionBusinessViewModel : CreateTransactionViewModel
    {
        public BusinessAccount BusinessAccount { get; set; }
    }

    public class CreateTransferViewModel
    {
        [Required]
        [Display(Name = "Amount")]
        [Range(0.0, Double.MaxValue)]
        public decimal Amount { get; set; }
        
        [Display(Name = "Target Business Account")]
        public virtual IEnumerable<project0_7.BusinessAccount> BusinessAccounts { get; set; }

        [Display(Name = "Target Customer Account")]
        public virtual IEnumerable<CheckingAccount> CheckingAccounts { get; set; }

        [Display(Name = "Target Loan")]
        public virtual IEnumerable<Loan> Loans { get; set; }
    }
}