

namespace BankAccount.Data.Model.Data
{
    public class ResponseWrapper
    {

        public string Message { get; set; } = string.Empty;
        public object? Results { get;set; }
        public bool success { get; set; }
    }
}
