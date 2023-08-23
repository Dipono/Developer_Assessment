using BankAccount.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace BankAccount.Data
{
    public class BankAccountDbContext : DbContext
    {
        public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options) : base(options) 
        { 
        }
        public DbSet<AccountHolderDb> AccountHolderDbs { get; set; }
        public DbSet<BankAccountDb> BankAccountDbs { get; set; }
        public DbSet<TransactionDb> TransactionDbs { get; set; }
    }
}
