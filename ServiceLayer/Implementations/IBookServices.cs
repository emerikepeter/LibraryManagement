using DomainLayer.ViewModels;
using ServiceLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementations
{
    public interface IBookServices: IRepository<BookViewModel>
    {
        Task<IEnumerable<BookViewModel>> SearchBooks(string Title, string ISBN, string Status);
        Task<IEnumerable<BookViewModel>> FetchBookDetails(string BootTitle);
    }
}
