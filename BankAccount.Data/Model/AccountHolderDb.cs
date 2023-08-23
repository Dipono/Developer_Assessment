


using System.ComponentModel.DataAnnotations;

namespace BankAccount.Data.Model
{
    public class AccountHolderDb
    {
        public AccountHolderDb() { 
        HouseNo = string.Empty;
            StreetName = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
            MobileNo = string.Empty;
            Email = string.Empty;
            Name = string.Empty;
        }
        [Key]
        public int Id { get; set; }
        public long IdNo { get; set; }
        public string Name { get; set; }
        public string HouseNo { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }
}
