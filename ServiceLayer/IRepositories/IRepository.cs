using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<StatusMessages> Create(T model);
        Task<IEnumerable<T>> Fetch();
        Task<StatusMessages> Modify(T model);
        Task<StatusMessages> Remove(string Id);
        Task<StatusMessages> Suspend(string Id);
    }
}
