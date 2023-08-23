using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount.Data.Model.Data
{
    public class AccountInformation
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }  = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double AvailableBalance { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
