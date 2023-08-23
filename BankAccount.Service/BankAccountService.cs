using BankAccount.Data;
using System.IO;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using IronXL;
using BankAccount.Data.Model.AccountData;
using BankAccount.Data.Model;
using DocumentFormat.OpenXml.Wordprocessing;
using BankAccount.Data.Model.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq;

namespace BankAccount.Service
{
    public class BankAccountService : IBankAccount
    {
        private readonly BankAccountDbContext _dbContext;

        public BankAccountService(BankAccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<List<AccountData>> InitialLoadAsync()
        {
            var accountHolder = new AccountHolderDb();
            var bankAccount = new BankAccountDb();
            
            var accountData = DataSource();
            
            foreach (var data in accountData)
            {
                var getExistingAccount = await _dbContext.AccountHolderDbs.FirstOrDefaultAsync(d => d.Email == data.Email);
                if(getExistingAccount != null)
                {
                    // nothing to code
                }
                else
                {
                    accountHolder = new AccountHolderDb() { 
                    StreetName = data.StreetName,
                    PostalCode = data.PostalCode,
                    Name = data.Name,
                    MobileNo = data.MobileNo,
                    IdNo = data.IdNo,
                    City = data.City,
                    Email = data.Email,
                    HouseNo = data.HouseNo,
                 };
                 //await _dbContext.AccountHolderDbs.AddAsync(accountHolder);
                // await _dbContext.SaveChangesAsync();

                bankAccount = new BankAccountDb()
                {
                    AccountHolderNo = accountHolder.Id,
                    AccountNumber = data.AccountNumber,
                    AccountType = data.AccountType,
                    Status = data.Status,
                    AvailableBalance = data.AvailableBalance,
                };
                //await _dbContext.BankAccountDbs.AddAsync(bankAccount);
                //await _dbContext.SaveChangesAsync();
                }
                
            }
            return accountData;            
        }


        // This function return all the accounts 
        public IEnumerable<AccountInformation> GetAllBankAccountsAsync()
        {
            var accountList = (from B in _dbContext.BankAccountDbs
                               join A in _dbContext.AccountHolderDbs on B.AccountHolderNo
                               equals A.Id
                               select new AccountInformation()
                               {
                                   AccountNumber = B.AccountNumber, AccountType = B.AccountType,
                                   Name = A.Name, Status = B.Status, AvailableBalance =  B.AvailableBalance
                               }).ToList();

            return accountList;
        }

        // Checking whether the account exist or not
        private bool CheckExistingAccount(int accNO)
        {
            return _dbContext.BankAccountDbs.Any(x => x.AccountNumber == accNO);
        }


        // Get account with speciic account number
        public  AccountInformation GetBankAccount(int accountNo)
        {
            if(CheckExistingAccount(accountNo) == false)
            {
                return null;
            }
            var accountList =  GetAccountDetails(accountNo);

            return   accountList;
        }


        private AccountInformation GetAccountDetails(int accountNo)
        {
            var accountList = (from B in _dbContext.BankAccountDbs
                                    join A in _dbContext.AccountHolderDbs on B.AccountHolderNo
                                    equals A.Id
                                    select new AccountInformation()
                                    {
                                        Id = B.Id,
                                        AccountNumber = B.AccountNumber,
                                        AccountType = B.AccountType,
                                        Name = A.Name,
                                        Status = B.Status,
                                        AvailableBalance = B.AvailableBalance
                                    }).Where(k => k.AccountNumber == accountNo).Single();

            return accountList;
        }

        public string Withdrawal(Withdrawal withdraw)
        {
            var accountDetails = GetAccountDetails(withdraw.AccountNo);

            if (CheckExistingAccount(withdraw.AccountNo) == false || accountDetails.Status == "inactive")
            {
                return "account does not exist";
            }


            if(withdrawalAmountLessZoro(withdraw.Amaount) == true)
            {
                return "Cannot withdraw the amount equal or less than R0";
            }

            if(accountDetails.AccountType.ToLower() != "fixed deposit")
            {
                if(amountIsGreaterThanBalance(withdraw.Amaount, accountDetails.AvailableBalance) == true)
                {
                    return "The amount withdrawing cannot be more than the available balance";
                }
            }
            else
            {
                if(fixedDepositAmount(withdraw.Amaount, accountDetails.AvailableBalance) == false)
                {
                    return "Withdrawal amount cammot be more than available balance but you may withdraw the total balance";
                }
            }

            var bankAccount = _dbContext.BankAccountDbs.SingleOrDefault(k => k.AccountNumber == withdraw.AccountNo);
            bankAccount.AvailableBalance = bankAccount.AvailableBalance - withdraw.Amaount;
            _dbContext.SaveChanges();

            DateTime currentDate = DateTime.Now;
            TransactionDb transaction = new TransactionDb()
            {
                AmountTransaction = withdraw.Amaount,
                BankAccountId = accountDetails.Id,
                TimeStamp  = currentDate,
                TranactionType = "Withdraw"
            };

            _dbContext.Add(transaction);
            _dbContext.SaveChanges();

            return "Transaction was successfully made, Payment R"+ withdraw.Amaount+" from "+
                accountDetails.AccountType+" Account, Available Balance R"+ bankAccount.AvailableBalance+
                " Date and Time "+ transaction.TimeStamp;
        }

        // The withdrawal amount cannot be less than or equal to 0
        private bool withdrawalAmountLessZoro(double withdrawalAmount)
        {
            if(withdrawalAmount < 1)
            {
                return true;
            }
            return false;
        }

        //The withdrawal amount cannot be greater than the available balance
        private bool amountIsGreaterThanBalance(double withdrawal, double balance)
        {
            if (withdrawal > balance)
            {
                return true;
            }
            return false;
        }

        //Fixed Deposit account type. withdraw 100%
        private bool fixedDepositAmount(double withdrawal, double balance)
        {
            if (withdrawal <= balance)
            {
                return true;
            }
            return false;
        }

        //List of data that will be populated into database
        private List<AccountData> DataSource()
        {
            return new List<AccountData> { 
                new AccountData(){Id=1,Name="L Nguyen",Email="Nguyen@gmail.com",MobileNo="0123456789", IdNo=01147896325, City="Pretoria", HouseNo="123", 
                    StreetName="Mongana", PostalCode="1234", AccountNumber=124578001, AccountType="Cheque", Status="active", AvailableBalance=1000.45},
                new AccountData(){Id=2,Name="DT Ntobe",Email="Ntobe@gmail.com",MobileNo="0145236987", IdNo=235689741235, City="Johannesburg", HouseNo="145",
                    StreetName="Lily", PostalCode="1596", AccountNumber=236478910, AccountType="Saving", Status="active", AvailableBalance=25000.85},
                new AccountData(){Id=3,Name="JK Lamola",Email="Lamola@gmail.com",MobileNo="0231456789", IdNo=01147897875, City="Johannesburg", HouseNo="3698",
                    StreetName="Vilakazi", PostalCode="0235", AccountNumber=456321789, AccountType="Cheque", Status="active", AvailableBalance=100503.45},
                new AccountData(){Id=4,Name="CK Scott",Email="Scott@gmail.com",MobileNo="0147896325", IdNo=2198689741365, City="Nelspruit", HouseNo="14569",
                    StreetName="Enoch", PostalCode="6589", AccountNumber=369852471, AccountType="Fixed Deposit", Status="active", AvailableBalance=36000.69},
                new AccountData(){Id=5,Name="JL James",Email="James@gmail.com",MobileNo="0189635325", IdNo=0123689741365, City="Pretoria", HouseNo="2354",
                    StreetName="Akhona", PostalCode="2578", AccountNumber=255852471, AccountType="Saving", Status="inactive", AvailableBalance=40000.09},
            };
        }
    }
}
