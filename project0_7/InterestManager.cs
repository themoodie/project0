using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project0_7
{
    public class InterestManager
    {
        public decimal Checking = 1.06m;
        public decimal Loan = 1.10m;
        public Dictionary<int, decimal> TermDeposit = new Dictionary<int, decimal>();

        public InterestManager()
        {
            TermDeposit.Add(1, 1.01m);
            TermDeposit.Add(31, 1.0125m);
            TermDeposit.Add(61, 1.0150m);
            TermDeposit.Add(91, 1.0175m);
            TermDeposit.Add(365, 1.002m);
        }
        public decimal TermDepositInterest(int days)
        {

            var temp = TermDeposit.First();
            foreach (var duration in TermDeposit)
            {
                if (duration.Key >= days)
                {
                    temp = duration;
                }
                else
                {
                    break;
                }
            }
            return temp.Value;
        }
    }
}