using DomainLayer.ViewModels;
using ServiceLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementations
{
    public interface IUserServices: IRepository<UserViewModel>
    {
        Task<string> FetchSpecificUser(string Fullname);

        Task<LoginToken> Login(LoginViewModel model);
    }
}
