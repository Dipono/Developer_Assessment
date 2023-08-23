using BankAccount.Data;
using BankAccount.Data.Model.AccountData;
using BankAccount.Data.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount.Service
{
    public interface IBankAccount
    {
        Task<List<AccountData>> InitialLoadAsync();
        IEnumerable<AccountInformation> GetAllBankAccountsAsync();
        AccountInformation GetBankAccount(int accountNo);
        string Withdrawal(Withdrawal withdraw);

    }
}
