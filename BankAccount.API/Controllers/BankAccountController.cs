using BankAccount.Data.Model.Data;
using BankAccount.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankAccount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("CorsPolicy")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccount _bankAccount;
        IConfiguration _configuration;
        public BankAccountController(IBankAccount bankAccount, IConfiguration configuration)
        {
            _bankAccount = bankAccount;
            _configuration = configuration;
        }
        /*  Initialize Endpoint assign prepopulated with a list of account holders (firstname, lastname,
            date of birth, ID number, residential address, mobile number and e-mail address) and the 
            bank accounts linked to the account holders.
         */
        [HttpPost]
        public async Task<IActionResult> Initialize()
        {
            var responseWrapper = new ResponseWrapper();
            var results = await _bankAccount.InitialLoadAsync();
            if (results == null)
            {
                responseWrapper.success = false;
                responseWrapper.Message = "Unable to load data";
                return Ok(responseWrapper);
            }

            responseWrapper.success = true;
            responseWrapper.Message = "Successfully loaded data";
            responseWrapper.Results = results;

            return Ok(responseWrapper);
        }

        /*
         * Retrieve a list of bank accounts for a given account holder
         */

        [HttpGet]
        public List<AccountInformation> BankAccountList()
        {
            var results = _bankAccount.GetAllBankAccountsAsync().ToList();

            return results;
        }


        /*
         Retrieve a single bank account for a given account number
         */
        [HttpPost]
        public AccountInformation GetAccount([FromBody] int accountNumber)
        {
           

            var results = _bankAccount.GetBankAccount(accountNumber);
            if (results == null)
            {
                return results;
            }

            results.Token = GetToken(accountNumber);
            return results;
        }


        /*
         * withdrawal for a given bank account with amount
         */
        /// <summary>
        /// </summary>
        /// <param name="withdraw"></param>
        /// <returns>return the string message</returns>
        [HttpPost]
        public string makeWithdrawal([FromBody] Withdrawal withdraw)
        {
            return _bankAccount.Withdrawal(withdraw);
        }

        /// <summary>
        /// Defining the JWT
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        private string GetToken(int accountNumber)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("AccountNo", accountNumber.ToString())
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Audience"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signIn
                );

            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }

    }
}
