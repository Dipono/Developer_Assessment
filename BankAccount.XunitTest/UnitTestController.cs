using BankAccount.API.Controllers;
using BankAccount.Data.Model.Data;
using BankAccount.Service;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BankAccount.XunitTest
{
    public class UnitTestController
    {
        private readonly Mock<IBankAccount> bankAccount;
        IConfiguration _configuration;
        public UnitTestController(IConfiguration configuration)
        {
            bankAccount = new Mock<IBankAccount>();
            _configuration = configuration;
        }

        [Fact]
        public void GetAccountList()
        {
            //arrange
            var accountList = GetAccountsData();
            bankAccount.Setup(k=> k.GetAllBankAccountsAsync()).Returns(accountList);
            var bankAccountController = new BankAccountController(bankAccount.Object, _configuration);

            //act
            var accountResult = bankAccountController.BankAccountList();

            //assert
            Assert.NotNull(accountResult);
            Assert.Equal(GetAccountsData().Count(), accountResult.Count());
            Assert.Equal(GetAccountsData().ToString(), accountResult.ToString());
            Assert.True(accountList.Equals(accountResult));
        }

        [Fact]
        public void GetAccoundByNumber()
        {
            //arrange
            var accountList = GetAccountsData();
            bankAccount.Setup(k => k.GetBankAccount(369852471)).Returns(accountList[2]);
            var bankAccountController = new BankAccountController(bankAccount.Object, _configuration);

            //act
            var accountResult = bankAccountController.GetAccount(369852471);

            //assert
            Assert.NotNull(accountResult);
            Assert.Equal(accountList[1].AccountNumber, accountResult.AccountNumber);
            Assert.True(accountList.Equals(accountResult));
        }




        private List<AccountInformation> GetAccountsData()
        {
            List<AccountInformation> acountsData = new List<AccountInformation>
            {
                /*new AccountInformation
                {
                    AccountNumber = 1,
                    AccountType = "Savings",
                    AvailableBalance = 100,
                    Name = "Test",
                    Status = "active"
                },
                 new AccountInformation
                {
                    AccountNumber = 2,
                    AccountType = "Savings",
                    AvailableBalance = 200,
                    Name = "K Scott",
                    Status = "inactive"
                },
                  new AccountInformation
                {
                    AccountNumber = 3,
                    AccountType = "Savings",
                    AvailableBalance = 5000,
                    Name = "L maula",
                    Status = "active"
                },
                   new AccountInformation
                {
                    AccountNumber = 4,
                    AccountType = "Savings",
                    AvailableBalance = 4000,
                    Name = "Dk Mamba",
                    Status = "active"
                },
                    new AccountInformation
                {
                    AccountNumber = 5,
                    AccountType = "Savings",
                    AvailableBalance = 20000,
                    Name = "N Khona",
                    Status = "active"
                },*/
                     new AccountInformation
                       {
                        AccountNumber = 124578001,
                        AccountType =  "Cheque",
                        Name = "L Nguyen",
                        AvailableBalance = 1000.45,
                        Status = "active"
                    },
                     new AccountInformation
                    {
                        AccountNumber =  236478910,
                        AccountType = "Saving",
                        Name = "DT Ntobe",
                        AvailableBalance = 25000.85,
                        Status = "active"
                    },

            };


            return acountsData;
        }
    }
}
