using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount.Data.Model
{
    public class TransactionDb
    {
        public TransactionDb() { 
            TranactionType = string.Empty;
        }
        public int Id { get; set; }
        public double AmountTransaction { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TranactionType { get; set; }
        [ForeignKey("BankAccount")]
        public int BankAccountId { get; set; }
    }
}
