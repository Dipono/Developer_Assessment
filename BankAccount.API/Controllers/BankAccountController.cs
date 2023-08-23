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

        [HttpGet]
        public List<AccountInformation> BankAccountList()
        {
            var results = _bankAccount.GetAllBankAccountsAsync().ToList();

            return results;
        }

        [HttpPost]
        public AccountInformation GetAccount([FromBody] int accountNumber)
        {
            //var responseWrapper = new ResponseWrapper();
            //responseWrapper.success = false;

            var results = _bankAccount.GetBankAccount(accountNumber);
            if (results == null)
            {

                //responseWrapper.Message = "Wrong account number";
                return results;
            }

            // responseWrapper.success = true;
            //responseWrapper.Message = "Access granted";
            results.Token = GetToken(accountNumber);
            return results;
        }

        [HttpPost]
        public string makeWithdrawal([FromBody] Withdrawal withdraw)
        {
            return _bankAccount.Withdrawal(withdraw);
        }

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
