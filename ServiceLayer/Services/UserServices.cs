using DomainLayer.Common;
using DomainLayer.Models;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Implementations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserServices : IUserServices
    {

        private readonly ILogger<UserServices> _logger;
        private readonly IConfiguration _config;

        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        public CustomMessages CustomMessage = new CustomMessages();
        public UserServices(ILogger<UserServices> logger, IConfiguration config, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        public async Task<StatusMessages> Create(UserViewModel model)
        {
            try
            {
                UserModel userModel = new UserModel
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(userModel, model.Password);
                if (result.Succeeded)
                    return new StatusMessages { IsSuccess = true, Messages = "Account Created Successfully" };
                else
                {
                    foreach (IdentityError error in result.Errors)
                        return new StatusMessages { IsSuccess = true, Messages = error.Description };
                }
                return new StatusMessages { IsSuccess = false, Messages = CustomMessage.Declined };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = false, Messages = err.Message };
            }
        }

        public Task<IEnumerable<UserViewModel>> Fetch()
        {
            throw new NotImplementedException();
        }

        public async Task<string> FetchSpecificUser(string Fullname)
        {
            var res = await _userManager.Users.FirstOrDefaultAsync(p => p.FullName.Replace(" ", "").ToLower() == Fullname.Replace(" ", "").ToLower());
            if (res != null)
                return res.Id;
            else
                return null;
        }

        public async Task<LoginToken> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user != null)
            {
                var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (passwordCheck.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _config["Tokens:Issuer"],
                        _config["Tokens:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddHours(3),
                        signingCredentials: credentials
                        );
                    return new LoginToken
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    };
                }
            }
            else
            {
                return null;
            }
            return null;
        }

        public Task<StatusMessages> Modify(UserViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<StatusMessages> Remove(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<StatusMessages> Suspend(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
