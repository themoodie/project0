using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace project0_7.Models
{
    public class CheckingAccountViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name ="Opening Date")]
        public System.DateTime OpenDate { get; set; }
        public decimal Balance { get; set; }
        [ReadOnly(true)]
        [Display(Name = "Interest Rate")]
        public decimal InterestRate = new InterestManager().Checking;
        public virtual Customer Customer { get; set; }

    }
    public class CTransactionsViewModel
    {
        [Required]
        public CheckingAccount CheckingAccount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public class CTransactionsDateViewModel
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
        public int CheckingAccountId { get; set; }
        public virtual CheckingAccount CheckingAccount { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }
    }

    public class CCreateTransactionViewModel
    {
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class CreateTransactionCheckingViewModel : CreateTransactionViewModel
    {
        public CheckingAccount CheckingAccount { get; set; }
    }

}