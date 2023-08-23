using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount.Data.Model.AccountData
{
    public class AccountData
    {
        public AccountData(){
            HouseNo = string.Empty;
            StreetName = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
            MobileNo = string.Empty;
            Email = string.Empty;
            Name = string.Empty;
            AccountType= string.Empty;
            Status = string.Empty;
        }

        public int Id { get; set; }
        public long IdNo { get; set; }
        public string Name { get; set; }
        public string HouseNo { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Status { get; set; }
        public double AvailableBalance { get; set; }
        public double AmountTransaction { get; set; }
        public DateTime TimeStamp { get; set; }
        //public string TranactionType { get; set; }
    }
}
