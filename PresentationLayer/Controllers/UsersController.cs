using DomainLayer.Common;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ServiceLayer.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;
        CustomMessages CustomMessage = new CustomMessages();

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<StatusMessages> Create(UserViewModel model)
        {
            try
            {
                var res = await _userServices.Create(model);
                if (res != null)
                    return new StatusMessages { IsSuccess = true, Messages = res.Messages };
                else
                    return new StatusMessages { IsSuccess = false, Messages = CustomMessage.Declined };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = false, Messages = err.Message };
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<LoginToken> Login(LoginViewModel model)
        {
            try
            {
                var res = await _userServices.Login(model);
                if (res != null)
                    return new LoginToken { Expiration = res.Expiration, Token = res.Token };
                else
                    return null;
            }
            catch (SqlException err)
            {
                throw;
            }
        }
    }
}
