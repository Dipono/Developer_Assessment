using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Data
{
    public class BankAccountDb
    {
        public BankAccountDb()
        {
            AccountType = string.Empty;
            Status = string.Empty;
        }
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Status { get; set; }
        public double AvailableBalance { get; set; }

        [ForeignKey("AccountHolderDb")]
        public int AccountHolderNo { get; set; }
    }
}